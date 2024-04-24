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
            _serialPort.BaudRate = 19200;
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

        public bool GetCurrentData(ref float current, ref string errorInfo)
        {
            OpenSerialPort();
            _serialPort.DiscardInBuffer();
            _serialPort.DiscardOutBuffer();
            currentReceive = "";
            // 定义正则表达式模式
            string pattern = @"^\d{10}80$";

            // 创建 Regex 对象
            Regex regex = new Regex(pattern);

            int numa = Environment.TickCount;
            while (true)
            {
                /* 表显示：1.092 串口输出：029010000080 */
                Trace.WriteLine("电流表输出：" + currentReceive);
                Console.WriteLine("电流表输出：" + currentReceive);
                if (currentReceive.Contains("?") || currentReceive.Contains("=") || currentReceive.Contains(";"))
                {
                    errorInfo = "电流表量程设置错误";
                    return false;
                }
                if (currentReceive.Contains("80"))
                {
                    string[] lines = currentReceive.Split('\n');
                    foreach (string line in lines)
                    {
                        Trace.WriteLine($"raw:[{line}]");
                        //if (line.Contains("80"))
                        if (line.Contains("80"))
                        {
                            string temp = line.TrimEnd('\r', '\n');
                            Trace.WriteLine($"temp:[{temp}]");
                            if (regex.IsMatch(temp))
                            {
                                char[] chars = temp.Substring(0, temp.Length - 2).ToCharArray();
                                Array.Reverse(chars);
                                current = int.Parse(new string(chars)) / 10.0f;
                                //current = int.Parse(line.Substring(3, 1) + line.Substring(2, 1) + line.Substring(1, 1));
                                Trace.WriteLine("电流：" + current);
                                break;
                            }
                        }
                    }
                    return true;
                }
                if (Environment.TickCount - numa > 3000)
                {
                    errorInfo = "读取电流表示数超时（3s）";
                    return false;
                }
                Thread.Sleep(200);
            }
        }
    }
}
