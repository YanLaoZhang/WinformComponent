using System;
using System.IO.Ports;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using System.Linq;

namespace UsbPowerMeter
{
    public class UsbPowerMeter : IDisposable
    {
        private SerialPort _serialPort;
        private readonly StringBuilder _receiveBuffer = new StringBuilder();
        private readonly object _lock = new object();

        // 事件声明
        public event EventHandler<WaveformDataEventArgs> WaveformDataReceived;
        public event EventHandler<SyncDataEventArgs> SyncDataReceived;

        // 在类内添加以下字段
        private List<double> _powerReadings;
        private readonly object _statsLock = new object();
        private bool _isCollectingData;

        public UsbPowerMeter(string portName)
        {
            _serialPort = new SerialPort(portName)
            {
                BaudRate = 921600,
                Parity = Parity.None,
                DataBits = 8,
                StopBits = StopBits.One,
                Handshake = Handshake.None,
                ReadTimeout = 3000,
                WriteTimeout = 1000
            };

            _powerReadings = new List<double>();
            _serialPort.DataReceived += SerialPort_DataReceived;
        }

        public void Open()
        {
            if (!_serialPort.IsOpen)
            {
                _serialPort.Open();
            }
        }

        public void Close()
        {
            if (_serialPort.IsOpen)
            {
                _serialPort.Close();
            }
        }

        // 设置单个页面参数
        public void SetPageParameter(char page, int frequency, double offset)
        {
            ValidatePage(page);
            ValidateFrequency(frequency);
            var command = $"{page}{frequency:0000}{offset:+#0.0;-#0.0}";
            SendCommand(command);
        }

        // 一键设置所有页面
        public void SetAllPages(IEnumerable<PageParameter> parameters)
        {
            var command = new StringBuilder();
            char currentPage = 'A';

            foreach (var param in parameters)
            {
                ValidatePage(currentPage);
                ValidateFrequency(param.Frequency);
                command.Append($"{currentPage++}{param.Frequency:0000}{param.Offset:+#0.0;-#0.0}");
            }

            SendCommand(command.ToString());
        }

        // 设置采样速率
        public void SetSamplingRate(SamplingSpeed speed)
        {
            SendCommand($"S{(int)speed}");
        }

        // 请求同步数据:获取设置的页面参数
        public void RequestSyncData()
        {
            SendCommand("Read");
        }

        private void SendCommand(string command)
        {
            if (!_serialPort.IsOpen)
                throw new InvalidOperationException("Serial port is not open");

            var fullCommand = $"{command}\r\n";
            Console.WriteLine($"Sending: {fullCommand}");
            _serialPort.Write(fullCommand);
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            lock (_lock)
            {
                var data = _serialPort.ReadExisting();
                _receiveBuffer.Append(data);

                ProcessBuffer();
            }
        }

        private void ProcessBuffer()
        {
            // 防止缓冲区溢出（参考搜索结果中的错误处理）
            /*if (_receiveBuffer.Length > 1024)
            {
                _receiveBuffer.Clear();
                throw new InvalidDataException("Buffer overflow detected");
            }*/
            Console.WriteLine(_receiveBuffer.ToString());
            // 处理同步数据
            ProcessSyncData();
            // 处理波形数据
            ProcessWaveformData();
        }

        // 修改同步数据正则表达式，增加对结尾符A的匹配
        private void ProcessSyncData()
        {
            // 更新正则表达式匹配模式：R开头 + 9组数据 + 结尾符A
            var pattern = @"^R((\d{4}[+-]\d{2}\.\d){9})A";
            var match = Regex.Match(_receiveBuffer.ToString(), pattern);

            if (match.Success)
            {
                var rawData = match.Groups[1].Value;
                _receiveBuffer.Remove(0, match.Index + match.Length);
                ParseSyncData(rawData);
            }
        }

        // 改进解析方法（处理9组数据）
        private void ParseSyncData(string data)
        {
            var results = new List<PageParameter>();
            var matches = Regex.Matches(data, @"([A-I])(\d{4})([+-]\d{2}\.\d)");

            if (matches.Count != 9)
                throw new FormatException("Invalid sync data format");

            foreach (Match match in matches)
            {
                results.Add(new PageParameter(
                    int.Parse(match.Groups[2].Value),
                    double.Parse(match.Groups[3].Value)
                ));
            }

            SyncDataReceived?.Invoke(this, new SyncDataEventArgs(results));
        }

        // 处理波形数据
        private void ProcessWaveformData()
        {
            var pattern = @"a[+-]\d{8,9}[umw]A";
            var match = Regex.Match(_receiveBuffer.ToString(), pattern);

            if (match.Success)
            {
                var data = match.Value;
                _receiveBuffer.Remove(0, match.Index + match.Length);
                ParseAndRaiseWaveformData(data);
            }
        }

        private void ParseAndRaiseWaveformData(string data)
        {
            // 格式验证（参考搜索结果中的通信协议）
            if (data.Length != 12 || !data.EndsWith("A"))
                throw new FormatException("Invalid waveform format");

            // 解析示例：a-42100006uA
            var sign = data[1] == '+' ? 1 : -1;

            // 分解dBm值：-42.1（第2-4位为整数，第5位为小数）
            var dBmInteger = double.Parse(data.Substring(2, 2));
            var dBmDecimal = double.Parse(data.Substring(4, 1)) * 0.1;
            var dBmValue = sign * (dBmInteger + dBmDecimal);

            // 分解功率值：00006 -> 0.06（根据单位换算）
            var powerValue = double.Parse(data.Substring(5, 5)) / 100;

            // 单位处理（兼容C# 7.3）
            PowerUnit unit;
            switch (data[10])
            {
                case 'u':
                    unit = PowerUnit.Microwatt;
                    break;
                case 'm':
                    unit = PowerUnit.Milliwatt;
                    break;
                case 'w':
                    unit = PowerUnit.Watt;
                    break;
                default:
                    throw new InvalidOperationException("Unknown power unit");
            }
            _powerReadings.Add(dBmValue);
            WaveformDataReceived?.Invoke(this, new WaveformDataEventArgs(
                dBmValue,
                powerValue,
                unit
            ));
        }

        #region Validation
        private void ValidatePage(char page)
        {
            if (page < 'A' || page > 'I')
                throw new ArgumentException("Invalid page character (A-I)");
        }

        private void ValidateFrequency(int frequency)
        {
            if (frequency < 0 || frequency > 9999)
                throw new ArgumentException("Frequency must be between 0000 and 9999");
        }
        #endregion

        #region IDisposable
        public void Dispose()
        {
            if (_serialPort != null)
            {
                _serialPort.DataReceived -= SerialPort_DataReceived;
                _serialPort.Dispose();
                _serialPort = null;
            }
        }
        #endregion

        // 新增统计方法
        public PowerStatistics GetPowerStatistics(TimeSpan duration)
        {
            if (!_serialPort.IsOpen)
                throw new InvalidOperationException("Serial port is not open");

            _powerReadings.Clear();
            _isCollectingData = true;

            var timer = new System.Timers.Timer(duration.TotalMilliseconds)
            {
                AutoReset = false
            };

            timer.Elapsed += (s, e) => _isCollectingData = false;
            timer.Start();

            // 等待采集完成
            while (_isCollectingData)
            {
                System.Threading.Thread.Sleep(50);
            }

            timer.Stop();
            timer.Dispose();

            lock (_statsLock)
            {
                if (_powerReadings.Count == 0)
                    return new PowerStatistics(0, 0, 0, 0);

                var max = _powerReadings.Max();
                var min = _powerReadings.Min();
                var avg = _powerReadings.Average();

                return new PowerStatistics(
                    max,
                    min,
                    avg,
                    _powerReadings.Count
                );
            }
        }
    }

    // 支持类型定义
    public enum SamplingSpeed { Low = 0, Medium = 1, High = 2 }
    public enum PowerUnit { Microwatt, Milliwatt, Watt }

    public class PageParameter
    {
        public int Frequency { get; }
        public double Offset { get; }

        public PageParameter(int frequency, double offset)
        {
            Frequency = frequency;
            Offset = offset;
        }
    }

    public class SyncDataEventArgs : EventArgs
    {
        public IReadOnlyList<PageParameter> Parameters { get; }

        public SyncDataEventArgs(IEnumerable<PageParameter> parameters)
        {
            Parameters = new List<PageParameter>(parameters);
        }
    }

    public class WaveformDataEventArgs : EventArgs
    {
        public double DbmValue { get; }
        public double PowerValue { get; }
        public PowerUnit Unit { get; }

        public WaveformDataEventArgs(double dbmValue, double powerValue, PowerUnit unit)
        {
            DbmValue = dbmValue;
            PowerValue = powerValue;
            Unit = unit;
        }
    }

    // 新增统计数据结构
    public struct PowerStatistics
    {
        public double Max { get; }
        public double Min { get; }
        public double Avg { get; }
        public int Samples { get; }

        public PowerStatistics(double max, double min, double avg, int samples)
        {
            Max = max;
            Min = min;
            Avg = avg;
            Samples = samples;
        }
    }
}
