using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO.Ports;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
/*
 按压传感器随着力度不断增加无源蜂鸣器会根据压力大小按音乐音符变化发出鸣响，蜂鸣器有六档，跟LED点亮数是同步的
 该模块可以通过串口可以打印出来AD采样值和按压状态
波特率选9600
HEX码“AA 03 03 C3”
第一个03代表打印速度,数值越大打印速度越慢；
第二个03代表按压状态设定值；03代表300；以此类推04代表400；02代表200；
“AA”“C3”是数据包的包头和包尾,是固定的。

点击发送HEX码“AA 03 03 C3”后软件会接收如：“adc1:0000_0adc2:0511_1”的数字， 
adc1代表第1通道,adc2代表第2通道; 
0000/0511代表的是AD采样值，
取值范围：0~1023；
后面的“_0/_1”代表按压状态；如果按压力量较大, AD值超过300就会变成“_1”。
按压状态根据AD值分段显示“声光提示”；蜂鸣器有六档，LED也有六档；根据按压力量会有不同的变化。
 */
namespace RP_C_MK06
{
    public class AdcData
    {
        // AD采样值
        public int Adc1Value { get; set; }
        public int Adc2Value { get; set; }
        // 按压状态
        public bool Adc1Pressed { get; set; }
        public bool Adc2Pressed { get; set; }
    }

    public class PressureResult
    {
        public int Adc1Min { get; set; }  // ADC1 最小值
        public int Adc1Max { get; set; }  // ADC1 最大值
        public int Adc1Hfv { get; set; }  // ADC1 频率最高值
        public int Adc2Min { get; set; }
        public int Adc2Max { get; set; }
        public int Adc2Hfv { get; set; }
    }

    public class PressureSensor : IDisposable
    {
        private SerialPort _serialPort;
        private string _buffer = string.Empty;
        RichTextBox _richTextBox;

        private StringBuilder _dataBuffer;

        public PressureSensor(string portName, RichTextBox richTextBox)
        {
            _serialPort = new SerialPort(portName, 9600)
            {
                DataBits = 8,
                StopBits = StopBits.One,
                Parity = Parity.None,
                ReadTimeout = 500, // 设置读取超时时间
                WriteTimeout = 500
            };

            _serialPort.DataReceived += SerialPortDataReceived;
            _richTextBox = richTextBox;
            _dataBuffer = new StringBuilder();
        }

        public void Open()
        {
            if (!_serialPort.IsOpen)
            {
                _serialPort.Open();
                AppendTextToRichTextBox("串口已打开");
            }
        }

        public void Close()
        {
            if (_serialPort.IsOpen)
            {
                _serialPort.Close();
                AppendTextToRichTextBox("串口已关闭");
            }
        }

        public bool ChangePort(string newPort)
        {
            try
            {
                if (_serialPort != null && _serialPort.IsOpen)
                {
                    _serialPort.DataReceived -= SerialPortDataReceived;
                }
                if (_serialPort != null && _serialPort.IsOpen)
                {
                    _serialPort.Close();
                }

                // 切换串口号
                _serialPort.PortName = newPort;

                //_richTextBox.Text = string.Empty;
                _serialPort.DataReceived += SerialPortDataReceived;
                // 打开串口
                _serialPort.Open();

                return true;
            }
            catch
            {
                return false;
            }
        }

        private void SerialPortDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string data = _serialPort.ReadLine(); 
                _dataBuffer.Append(data+ "\r\n");  // 将数据添加到 StringBuilder
                AppendTextToRichTextBox(data);
            }
            catch (TimeoutException)
            {
                AppendTextToRichTextBox("读取超时");
            }
        }

        private void AppendTextToRichTextBox(string text)
        {
            if (_richTextBox.InvokeRequired)
            {
                _richTextBox.BeginInvoke(new Action(() => AppendTextToRichTextBox(text)));
            }
            else
            {
                _richTextBox.AppendText(text + Environment.NewLine);
                _richTextBox.ScrollToCaret();
            }
        }

        private bool TryParseAdcData(string input, out AdcData adcData)
        {
            adcData = null;

            // 使用正则表达式解析数据
            var match = Regex.Match(input, @"adc1:(\d{4})_(\d)adc2:(\d{4})_(\d)");

            if (match.Success)
            {
                adcData = new AdcData
                {
                    Adc1Value = int.Parse(match.Groups[1].Value),
                    Adc1Pressed = match.Groups[2].Value == "1",
                    Adc2Value = int.Parse(match.Groups[3].Value),
                    Adc2Pressed = match.Groups[4].Value == "1"
                };
                return true;
            }

            return false;
        }

        public async Task<List<AdcData>> ReadForDuration(TimeSpan duration)
        {
            _dataBuffer.Clear();  // 清空数据缓冲区

            using (var cts = new CancellationTokenSource(duration))
            {
                try
                {
                    await Task.Delay(duration, cts.Token);  // 等待指定时间
                }
                catch (TaskCanceledException)
                {
                    // 超时后自动取消
                }
            }

            List<AdcData> dataList = new List<AdcData>();
            // 尝试解析每一行
            string[] lines = _dataBuffer.ToString().Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < lines.Length - 1; i++)
            {
                Console.WriteLine(lines[i]);
                if (TryParseAdcData(lines[i], out AdcData adcData))
                {
                    dataList.Add(adcData);
                }
            }

            return dataList;  // 返回在指定时间内接收到的数据
        }

        public async Task<PressureResult> GetResult(double duration)
        {
            try
            {
                PressureResult pressureResult = null;
                TimeSpan _duration = TimeSpan.FromSeconds((double)duration);

                List<AdcData> dataList = await ReadForDuration(_duration);
                if (dataList == null || dataList.Count == 0)
                {
                    Console.WriteLine("ADC 数据列表为空。");
                    return pressureResult;
                }
                pressureResult = new PressureResult();
                pressureResult.Adc1Min = dataList.Min(data => data.Adc1Value);
                pressureResult.Adc1Max = dataList.Max(data => data.Adc1Value);

                int adc1_hfv = -1;
                // 获取出现次数最多的 Adc1Value
                var mostFrequentAdc1Value = dataList
                    .GroupBy(data => data.Adc1Value) // 按 Adc1Value 分组
                    .OrderByDescending(group => group.Count()) // 按出现次数降序排列
                    .Select(group => new
                    {
                        Value = group.Key,
                        Count = group.Count()
                    })
                    .FirstOrDefault(); // 获取出现次数最多的值

                if (mostFrequentAdc1Value != null)
                {
                    Console.WriteLine($"出现次数最多的 Adc1Value: {mostFrequentAdc1Value.Value}, 次数: {mostFrequentAdc1Value.Count}");
                    adc1_hfv = mostFrequentAdc1Value.Value;
                }
                else
                {
                    Console.WriteLine("列表为空，没有值。");
                }
                pressureResult.Adc1Hfv = adc1_hfv;

                pressureResult.Adc2Min = dataList.Min(data => data.Adc2Value);
                pressureResult.Adc2Max = dataList.Max(data => data.Adc2Value);

                int adc2_hfv = -1;
                // 获取出现次数最多的 Adc1Value
                var mostFrequentAdc2Value = dataList
                    .GroupBy(data => data.Adc2Value) // 按 Adc1Value 分组
                    .OrderByDescending(group => group.Count()) // 按出现次数降序排列
                    .Select(group => new
                    {
                        Value = group.Key,
                        Count = group.Count()
                    })
                    .FirstOrDefault(); // 获取出现次数最多的值

                if (mostFrequentAdc2Value != null)
                {
                    Console.WriteLine($"出现次数最多的 Adc1Value: {mostFrequentAdc2Value.Value}, 次数: {mostFrequentAdc2Value.Count}");
                    adc2_hfv = mostFrequentAdc2Value.Value;
                }
                else
                {
                    Console.WriteLine("列表为空，没有值。");
                }
                pressureResult.Adc2Hfv = adc2_hfv;

                return pressureResult;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void Dispose()
        {
            if (_serialPort != null)
            {
                _serialPort.DataReceived -= SerialPortDataReceived;
                _serialPort.Close();
                _serialPort.Dispose();
                _serialPort = null;
            }
        }
    }

}
