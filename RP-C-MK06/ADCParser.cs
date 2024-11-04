using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO.Ports;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace RP_C_MK06
{
    public class AdcData
    {
        public int Adc1Value { get; set; }
        public int Adc2Value { get; set; }
        public bool Adc1Pressed { get; set; }
        public bool Adc2Pressed { get; set; }
    }

    public class AdcParser : IDisposable
    {
        private SerialPort _serialPort;
        private string _buffer = string.Empty;
        RichTextBox _richTextBox;

        private StringBuilder _dataBuffer;

        public AdcParser(string portName, int baudRate, RichTextBox richTextBox)
        {
            _serialPort = new SerialPort(portName, baudRate)
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
