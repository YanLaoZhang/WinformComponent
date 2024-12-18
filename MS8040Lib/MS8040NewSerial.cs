using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS8040Lib
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO.Ports;
    using System.Linq;
    using System.Runtime.ConstrainedExecution;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Xml.Schema;

    public class SerialDataProcessor
    {
        private List<float> dataList; // 存储数据
        private string unit;
        private SerialPort serialPort;
        private bool isCollecting;

        /// <summary>
        /// 构造函数，初始化串口。
        /// </summary>
        /// <param name="portName">串口号 (例如 "COM1")</param>
        /// <param name="baudRate">波特率</param>
        public SerialDataProcessor(string portName)
        {
            dataList = new List<float>();
            unit = string.Empty;
            serialPort = new SerialPort(portName, 19230, Parity.Odd, 7, StopBits.One);
            serialPort.DtrEnable = true;
            serialPort.RtsEnable = true;
            serialPort.DataReceived += SerialPort_DataReceived;
        }

        /// <summary>
        /// 停止数据采集
        /// </summary>
        public void StopDataCollection()
        {
            isCollecting = false;
            if (serialPort.IsOpen)
            {
                serialPort.Close();
            }
            Console.WriteLine("数据采集已停止");
        }

        /// <summary>
        /// 处理串口数据接收
        /// </summary>
        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string value = serialPort.ReadLine();
                Console.WriteLine($"接收数据: {value}");
                lock (dataList)
                {
                    ProcessData(value.TrimEnd(new char[] { '\r', '\n' }));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"接收数据时出错: {ex.Message}");
            }
        }

        /// <summary>
        /// 处理一帧完整的串口数据
        /// </summary>
        /// <param name="frameData">一帧数据</param>
        private void ProcessData(string data)
        {
            Console.WriteLine($"解析的数据{data}, {data.Length}");
            // 这里可以将数据进一步解析
            if (string.IsNullOrEmpty(data) || data.Length != 12)
            {
                Console.WriteLine("输入数据长度不为12字节");
                //throw new ArgumentException("输入数据长度不为12字节");
                return;
            }
            /* HOLD 时，最后两位变为82
             * 当表档位在A档时，
             * 表显示： 1.092 串口输出：029010000080
             * 表显示：-0.736 串口输出：063700040080
             * 当表档位在mA档时，
             * 表显示： 0.001 串口输出：010000?000:0
             * 表显示：-0.001 串口输出：010000?400:0
             * 当表档位在uA档时，
             * 表显示： 0.01 串口输出： 010000=000:0
             * 表显示：-0.01 串口输出： 010000=400:0
             * 当表档位在V档时，
             * 表显示： 0.0004 串口输出：040000;000:0
             * 表显示：-0.0004 串口输出：040000;400:0
             */
            if(!Regex.IsMatch(data, @"^(0|4)\d{5}[0\?;](0|4)(0080|0082|00:0)$"))
            {
                Console.WriteLine("不符合指定规则");
                return;
            }
            try
            {
                string index_1 = data.Substring(0,1);
                string index_2_6 = data.Substring(1,5);
                string index_7 = data.Substring(6,1);
                string index_8 = data.Substring(7,1);
                string index_9_12 = data.Substring(8,4);

                bool isNegative = index_8 == "4"; // 例如 "4000" 表示负数

                int point_station = 0;
                switch (index_7)
                {
                    case "0": // A档
                        unit = "A";
                        point_station = 3;
                        break;
                    case "?": // mA档
                        unit = "mA";
                        point_station = 3;
                        break;
                    case "=": // uA档
                        unit = "uA";
                        point_station = 2;
                        break;
                    case ";": // V档
                        unit = "V";
                        point_station = 4;
                        break;
                }

                // 将前5个字符如"0637000"转换为"0637.00"格式
                string formattedString = index_2_6.Insert(point_station, ".");
                // 反转并转换
                string reversedString = ReverseString(formattedString);
                float displayValue = float.Parse(reversedString);

                // 如果是负数，则值取负
                if (isNegative)
                {
                    displayValue = -displayValue;
                }
                //Trace.WriteLine($"解析后的测量值为[{displayValue}]{unit}");
                Console.WriteLine($"解析后的测量值为[{displayValue}]{unit}");
                dataList.Add(displayValue);
            }
            catch (Exception ex)
            {
                throw new Exception($"数据解析错误: {ex.Message}");
            }
        }

        static string ReverseString(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        /// <summary>
        /// 计算最大值
        /// </summary>
        /// <returns>返回最大值</returns>
        public float GetMaxValue()
        {
            lock (dataList)
            {
                return dataList.Count > 0 ? dataList.Max() : 0;
            }
        }

        /// <summary>
        /// 计算最小值
        /// </summary>
        /// <returns>返回最小值</returns>
        public float GetMinValue()
        {
            lock (dataList)
            {
                return dataList.Count > 0 ? dataList.Min() : 0;
            }
        }

        /// <summary>
        /// 计算去除最大最小值后的平均值
        /// </summary>
        /// <returns>去除最大和最小后的平均值</returns>
        public float GetAverageExcludingMinMax()
        {
            lock (dataList)
            {
                if (dataList.Count < 0)
                    return 0; // 数据小于0条时，无法去除最大和最小
                if (dataList.Count == 1)
                    return dataList[0]; // 数据1条时，当前值为平均值
                var sortedData = dataList.OrderBy(x => x).ToList();
                if (dataList.Count == 2) // 数据2条时，不去除最大和最小，直接算平均
                    return sortedData.Average(); 

                sortedData.RemoveAt(0); // 删除最小值
                sortedData.RemoveAt(sortedData.Count - 1); // 删除最大值

                return sortedData.Average();
            }
        }

        public DataResult GetCurrentData(int durationSeconds, ref string str_error_log)
        {
            try
            {
                if (!serialPort.IsOpen)
                {
                    serialPort.Open();
                }

                isCollecting = true;
                dataList.Clear();
                unit = string.Empty;
                Console.WriteLine($"开始数据采集，持续时间：{durationSeconds} 秒");

                // 在指定时间后停止采集
                Thread.Sleep(durationSeconds * 1000);
                StopDataCollection();

                DataResult dataResult = new DataResult();
                // 打印数据和统计信息
                lock (dataList)
                {
                    if (dataList.Count == 0)
                    {
                        Console.WriteLine("没有接收到任何数据。");
                        str_error_log = $"No Recevie Data";
                        return null;
                    }

                    dataResult.DataList = dataList;
                    dataResult.Count = dataList.Count;
                    dataResult.Max = GetMaxValue();
                    dataResult.Min = GetMinValue();
                    dataResult.AverageExcludingMinMax = GetAverageExcludingMinMax();
                    dataResult.Unit = unit;

                    Console.WriteLine(dataResult.ToString());
                    return dataResult;
                }
            }
            catch (Exception ee)
            {
                str_error_log = $"{ee.Message}";
                return null;
            }
        }

    }

    public class DataResult
    {
        public int Count;
        public List<float> DataList;
        public float Max;
        public float Min;
        public float AverageExcludingMinMax;
        public string Unit;

        public override string ToString()
        {
            return $"---------- 数据统计 ----------\r\n" +
                $"数据总数: {Count}\r\n" +
                $"数据: {string.Join(", ", DataList)}\r\n" +
                $"最大值: {Max}\r\n" +
                $"最小值: {Min}\r\n" +
                $"去除最大最小后的平均值: {AverageExcludingMinMax}\r\n" +
                $"单位: {Unit}\r\n" +
                $"--------------------------------\r\n";
        }
    }

}
