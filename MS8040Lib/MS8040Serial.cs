using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace MS8040Lib
{
    public class MS8040Serial
    {
        [DllImport("user32.dll")]
        private static extern int MessageBoxTimeoutA(IntPtr hWnd, string msg, string caption, int type, int id, int time);

        private string currentReceive;
        System.IO.Ports.SerialPort _serialPort;
        string _portName;
        public MS8040Serial(System.IO.Ports.SerialPort serialPort, string portName)
        {
            _serialPort = serialPort;
            _portName = portName;
            _serialPort.PortName = _portName;
            _serialPort.BaudRate = 19230;
            _serialPort.DataBits = 7;
            _serialPort.Parity = System.IO.Ports.Parity.None;
            _serialPort.StopBits = System.IO.Ports.StopBits.One;
            _serialPort.Encoding = Encoding.ASCII;
            _serialPort.ReadBufferSize = 4096;
            _serialPort.DtrEnable = true;
            _serialPort.RtsEnable = true;

            // 绑定 DataReceived 事件处理程序
            _serialPort.DataReceived += SerialPort_DataReceived;
        }

        private void SerialPort_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            try
            {
                // Trace.WriteLine("串口收到数据");
                currentReceive += _serialPort.ReadExisting();
                // Trace.WriteLine("串口：" + currentReceive);
            }
            catch (Exception ex)
            {
                MessageBoxTimeoutA(IntPtr.Zero, ex.Message, "", 16, 0, 3000);
            }
        }

        public bool OpenSerialPort()
        {
            try
            {
                if (_serialPort.IsOpen == false)
                {
                    _serialPort.Open();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool ChangePort(string newPort)
        {
            try
            {
                if (_serialPort != null && _serialPort.IsOpen)
                {
                    _serialPort.DataReceived -= SerialPort_DataReceived;
                }
                if (_serialPort != null && _serialPort.IsOpen)
                {
                    _serialPort.Close();
                }

                // 切换串口号
                _serialPort.PortName = newPort;

                //_richTextBox.Text = string.Empty;
                _serialPort.DataReceived += SerialPort_DataReceived;
                //// 打开串口
                //_serialPort.Open();

                return true;
            }
            catch
            {
                return false;
            }
        }

        static string ReverseString(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        /// <summary>
        /// </summary>
        /// <param name="current">返回mA</param>
        /// <param name="errorInfo"></param>
        /// <returns></returns>
        public bool GetCurrentData(ref float current, ref string errorInfo)
        {
            OpenSerialPort();
            _serialPort.DiscardInBuffer();
            _serialPort.DiscardOutBuffer();
            currentReceive = "";
            /* 当表档位在A档时，
             * 表显示： 1.092 串口输出：029010000080
             * 表显示：-0.736 串口输出：063700040080
             * 当表档位在mA档时，
             * 表显示： 0.001 串口输出：010000?000:0
             * 表显示：-0.001 串口输出：010000?400:0
             * 当表档位在uA档时，
             * 表显示： 0.01 串口输出： 010000=000:0
             * 表显示：-0.01 串口输出： 010000=400:0
             */
            // 创建 Regex 对象
            Regex regex_A = new Regex(@"^\d{6}0(000|400)80$");
            Regex regex_mA = new Regex(@"^\d{6}\?(000|400):0$");
            Regex regex_uA = new Regex(@"^\d{6}=(000|400):0$");

            int numa = Environment.TickCount;
            bool alwaysEmpty = true;
            while (true)
            {
                Trace.WriteLine("MS8040串口输出：" + currentReceive);
                string cur = currentReceive.Trim('\r', '\n');
                if (Environment.TickCount - numa > 3000)
                {
                    if (alwaysEmpty)
                    {
                        errorInfo += $"未接收到串口信息，请检查PC-Link是否打开(表显示屏左下角出现RS232表示连接正常)";
                    }
                    errorInfo += "; 读取电流表显示数值超时（3s）";
                    return false;
                }
                if (regex_A.IsMatch(cur))
                {
                    Trace.WriteLine($"当前量程为A档，数据为[{cur}]");
                    // 将前6个字符如"0637000"转换为"0637.00"格式
                    string formattedString = cur.Substring(0, 6).Insert(4, ".");
                    // 反转并转换为"0.7360"格式
                    string reversedString = ReverseString(formattedString);
                    current = float.Parse(reversedString) * 1000.0f;
                    Trace.WriteLine($"解析后的电流值为[{current}]mA");
                    return true;
                }
                else if (regex_mA.IsMatch(cur))
                {
                    Trace.WriteLine($"当前量程为mA档，数据为[{cur}]");
                    // 将前6个字符如"0637000"转换为"0637.00"格式
                    string formattedString = cur.Substring(0, 6).Insert(4, ".");
                    // 反转并转换为"0.7360"格式
                    string reversedString = ReverseString(formattedString);
                    current = float.Parse(reversedString);
                    Trace.WriteLine($"解析后的电流值为[{current}]mA");
                    return true;
                }
                else if (regex_uA.IsMatch(cur))
                {
                    Trace.WriteLine($"当前量程为uA档，数据为[{cur}]");
                    // 将前6个字符如"0637000"转换为"063.700"格式
                    string formattedString = cur.Substring(0, 6).Insert(3, ".");
                    // 反转并转换为"0.7360"格式
                    string reversedString = ReverseString(formattedString);
                    current = float.Parse(reversedString) / 1000.0f;
                    Trace.WriteLine($"解析后的电流值为[{current}]mA");
                    return true;
                }
                else
                {
                    if(cur.Length != 0)
                    {
                        alwaysEmpty = false;
                        errorInfo += $"串口信息[{cur}]异常";
                    }
                    Thread.Sleep(200);
                    continue;
                }
            }
        }
    }
}
