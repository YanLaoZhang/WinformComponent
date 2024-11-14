using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace DS18B20_Temp_Sensor
{
    /*
产品名称：DS18B20温度传感器模块
1、尺寸：25mm X宽12mm
2、工作电压：DC3.3~5V
3、分辨率调整范围：9-12位
4、温度测量范围：-55~+125℃
5、温度测量精度：0.5℃
6、串口信号输出：
串口波特率：115200 8 N 1

防水型钢管6*50mm

* 此传感器模式有多种方式，下面一一列举
* 此程序使用ds18b20，下面的数据中传感器名称为0x01，保留位都为0x00
* 0：ds18b20默认情况下为主动发送数据，时间间隔1秒
* 1：ds18b20设置为问答模式，通过命令读取(具体的命令命令通过文档讲解)
* 艾特森科技协议说明：0xff 传感器名称 数据命令 数据分量 数据符号 数据高位 数据低位 保留位 保留位 校验位
* 主动上传：0xff 传感器名称 0x00 数据分量 数据符号 数据高位 数据低位 保留位 保留位 校验位
* 问答模式：0xff 传感器名称 0x86 数据分量 数据符号 数据高位 数据低位 保留位 保留位 校验位
* 切换到主动：0xff 传感器名称 0xcc 0x88 保留位 保留位 保留位 保留位 保留位 校验位
* 切换到问答：0xff 传感器名称 0xcc 0x82 保留位 保留位 保留位 保留位 保留位 校验位
* 读取温度： 0xff 传感器名称 0x86 保留位 保留位 保留位 保留位 保留位 保留位 校验位

注：在问答读取模式下，由于传感器转换读取需要一定的时间，更为温度精度等，所以在问答模式下的数据读取周期大于0.05秒。推荐使用不超过0.05秒的数据采集频率。

例如 ：
切换到问答发送模式： 0xff 0x01 0xcc 0x82 0x00 0x00 0x00 0x00 0x00 0xB1
切换到主动发送模式： 0xff 0x01 0xcc 0x88 0x00 0x00 0x00 0x00 0x00 0xAB
问答模式温湿度读取： 0xff 0x01 0x86 0x00 0x00 0x00 0x00 0x00 0x00 0x79
问答发送模式下返回的数据：FF 01 86 02 00 09 C9 00 00 A5
主动发送模式下返回的数据：FF 01 00 02 00 09 51 00 00 A3
第一位：起始位
第二位：0x01表示DS18B20温度传感器
第三位：表示为问答模式下的数据返回格式(0x86)；主动模式下的数据返回格式(0x00)
第四位：0x02表示数据分量为2，数据计算出来后除以100
第五位：温度符号位 0：正温度 1：负温度
第六位：0x09表示温度数据高位
第七位：0xC9表示温度数据低位
第八位：0x00（保留位）
第九位：0x00（保留位）
第十位：0xA5表示数据校验位
数据分量=1时 温度=（温度高位*256+温度低位）/10；
数据分量=2时 温度=（温度高位*256+温度低位）/100；

*/
    public class TempSensorSerial
    {
        [DllImport("user32.dll")]
        private static extern int MessageBoxTimeoutA(IntPtr hWnd, string msg, string caption, int type, int id, int time);

        byte[] CMD_PREFIX = new byte[2] { 0xFF, 0x01}; // 帧头
        byte[] CMD_SUFFIX = new byte[2] { 0x00, 0x79}; // 帧尾
        byte[] CMD_Active = new byte[2] { 0xCC, 0x88}; // 主动发送模式
        byte[] CMD_QandA = new byte[2] { 0xCC, 0x82}; // 问答发送模式
        byte[] CMD_Read = new byte[2] { 0x86, 0x00}; // 主动模式读取温度
        byte[] CMD_Reserve = new byte[2] { 0x86, 0x00}; // 保留位

        string STR_CMD_SWITCH_QandAMode = "FF01CC820000000000B1"; // 切换到问答发送模式
        string STR_CMD_SWITCH_ActiveMode = "FF01CC880000000000AB"; // 切换到主动发送模式
        string STR_CMD_QandAMode_Read = "FF018600000000000079"; // 问答模式温湿度读取

        public int[] int_relay_status = new int[16];
        public byte[] byte_Receive = new byte[256];
        public List<byte> Recive_buffer = new List<byte>(256);
        bool bool_exit_relay = false;
        public byte[] cur_bytes = new byte[10];
        System.IO.Ports.SerialPort _serialPort;
        string _portName;

        RichTextBox _richTextBox;

        public TempSensorSerial(string portName, RichTextBox richTextBox)
        {
           /* _serialPort = serialPort;
            _portName = portName;
            _serialPort.PortName = _portName;
            _serialPort.BaudRate = 115200;
            _serialPort.DataBits = 8;
            _serialPort.Parity = System.IO.Ports.Parity.None;
            _serialPort.StopBits = System.IO.Ports.StopBits.One;
            // 绑定 DataReceived 事件处理程序
            _serialPort.DataReceived += SerialPort_DataReceived;*/

            _serialPort = new SerialPort(portName, 115200)
            {
                DataBits = 8,
                StopBits = StopBits.One,
                Parity = Parity.None,
                ReadTimeout = 500, // 设置读取超时时间
                WriteTimeout = 500
            };

            _serialPort.DataReceived += SerialPortDataReceived;
            _richTextBox = richTextBox;
        }

        // DataReceived 事件处理程序
        private void SerialPortDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                int requiredLength = 10;
                int len = _serialPort.BytesToRead;
                if (len >= requiredLength)
                {
                    byte[] buffer = new byte[requiredLength];
                    _serialPort.Read(buffer, 0, requiredLength);
                    cur_bytes = buffer;
                    Recive_buffer.AddRange(buffer);
                    AppendTextToRichTextBox(ByteArrayToHexString(buffer));
                }
                // 休眠一小段时间，避免占用过多 CPU
                Thread.Sleep(50);
            }
            catch (InvalidOperationException ex)
            {
                MessageBoxTimeoutA(IntPtr.Zero, ex.Message, "", 16, 0, 3000);
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

        public static string ByteArrayToHexString(byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", " ");
        }

        #region "Change16_10 and Change10_16"
        public int Change16_10(string x16)//OK
        {
            try
            {
                return Convert.ToInt32(x16, 16);
            }
            catch //(Exception ee)
            {
                return 0;
            }
        }

        public string Change10_16(int x10)//
        {
            //return Convert.ToChar(x10).ToString();
            string str_16 = x10.ToString("x1").ToUpper();
            switch (str_16.Length)
            {
                case 1:
                    str_16 = "0" + str_16;
                    break;
            }
            return str_16;
        }
        #endregion

        public bool CheckStatus()
        {
            return _serialPort.IsOpen;
        }

        public bool OpenRelayPort()
        {
            try
            {
                if (!_serialPort.IsOpen)
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

        /// <summary>
        /// 关闭串口
        /// </summary>
        /// <returns></returns>
        public bool ClosePort()
        {
            try
            {
                if (_serialPort.IsOpen)
                {
                    _serialPort.Close();
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

        /// <summary>
        /// 发送指令到传感器
        /// </summary>
        /// <param name="str_send">指令内容</param>
        /// <param name="t">超时时间</param>
        /// <param name="Respone">是否需要解析返回数据</param>
        /// <param name="byte_ret_value">存储返回数据，</param>
        /// <returns></returns>
        private bool SendCMDToSensor(string str_send, int t, Boolean Respone, ref byte[] byte_ret_value)
        {
            try
            {
                OpenRelayPort();
                bool_exit_relay = false;
                _serialPort.DiscardInBuffer();
                _serialPort.DiscardOutBuffer();
                Recive_buffer.Clear();

                byte[] byte_send = new byte[(int)(str_send.Length / 2)];
                for (int i = 0; i < byte_send.Length; i++)
                {
                    byte_send[i] = (byte)Change16_10(str_send.Substring(i * 2, 2));
                }
                _serialPort.Write(byte_send, 0, byte_send.Length);
                if (Respone == false)
                {
                    return true;
                }
                System.Threading.Thread.Sleep(500);
                int numa = Environment.TickCount;
                int startIndex = -1;
                // 条件是真的时执行循环，条件是假的时候跳出循环
                while (true)
                {
                    if (Environment.TickCount - numa > t)
                    {
                        break;
                    }
                    if (bool_exit_relay)
                    {
                        break;
                    }

                    if (Recive_buffer.Count >= 10)
                    {
                        // 数据接收完整
                        Console.WriteLine($"Recive_buffer.Count:[{Recive_buffer.Count}]");
                        Console.WriteLine($"copy count:[{byte_ret_value.Length}]");
                        //Recive_buffer.CopyTo(0, byte_Receive, 0, byte_ret_value.Length);
                        byte_ret_value = Recive_buffer.GetRange(0, byte_ret_value.Length).ToArray();
                        Recive_buffer.Clear();

                        break;
                    }
                }
                //for (int i = 0; i < byte_ret_value.Length; i++)
                //{
                //    byte_ret_value[i] = byte_Receive[i];
                //}
                return true;

            }
            catch (Exception ee)
            {
                return false;
            }
            //finally { _serialPort.Close(); }
        }

        /// <summary>
        /// 切换到问答模式
        /// </summary>
        /// <returns></returns>
        public bool TriggerQandAMode()
        {
            try
            {
                byte[] byte_ret_value = new byte[10];
                if (SendCMDToSensor(STR_CMD_SWITCH_QandAMode, 200, false, ref byte_ret_value) == false)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ee)
            {
                return false;
            }
        }

        /// <summary>
        /// 切换到主动发送模式
        /// </summary>
        /// <returns></returns>
        public bool TriggerActiveMode()
        {
            try
            {
                byte[] byte_ret_value = new byte[10];
                if (SendCMDToSensor(STR_CMD_SWITCH_ActiveMode, 200, false, ref byte_ret_value) == false)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ee)
            {
                return false;
            }
        }

        /// <summary>
        /// 主动模式下获取返回值
        /// </summary>
        /// <param name="str_error_log"></param>
        /// <param name="int_bytes_data"></param>
        /// <returns></returns>
        public bool GetActiveData(ref string str_error_log, ref byte[] int_bytes_data, out double temp)
        {
            temp = -1;
            try
            {
                // 先切换到主动发送模式
                TriggerActiveMode();

                Thread.Sleep(50);

                int_bytes_data = cur_bytes;
                temp = ParseTemperature(int_bytes_data);
                return true;
            }
            catch (Exception ee)
            {
                str_error_log = ee.Message;
                return false;
            }
        }

        /// <summary>
        /// 问答模式下获取返回值
        /// </summary>
        /// <param name="str_error_log"></param>
        /// <param name="int_bytes_data"></param>
        /// <returns></returns>
        public bool GetQandAData(ref string str_error_log, ref byte[] int_bytes_data, out double temp)
        {
            temp = -1;  // 默认值为-1，表示异常
            try
            {
                // 先切换到主动发送模式
                TriggerQandAMode();
                Thread.Sleep(50);

                byte[] byte_ret_value = new byte[10];  // 返回数据，长度为10
                if (SendCMDToSensor(STR_CMD_QandAMode_Read, 500, true, ref byte_ret_value) == false)
                {
                    str_error_log = "问答模式读取温湿度指令异常";
                    return false;
                }
                int_bytes_data = byte_ret_value;
                temp = ParseTemperature(int_bytes_data);
                return true;
            }
            catch (Exception ee)
            {
                str_error_log = ee.Message;
                return false;
            }
        }

        /// <summary>
        /// 解析返回值数据为温度值
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static double ParseTemperature(byte[] data)
        {
            // 验证数据包的长度是否满足基本要求
            if (data.Length < 10)
            {
                throw new ArgumentException("数据包长度不足");
            }

            // 数据分量（决定除数是10还是100）
            byte dataFactor = data[3];
            int divisor = dataFactor == 1 ? 10 : 100;

            // 温度符号位
            bool isNegative = data[4] == 1;

            // 温度数据高位和低位
            int highByte = data[5];
            int lowByte = data[6];

            // 计算温度
            int temperatureRaw = (highByte << 8) | lowByte;
            double temperature = temperatureRaw / (double)divisor;

            // 如果是负温度，温度取负值
            if (isNegative)
            {
                temperature = -temperature;
            }

            return temperature;
        }
    }
}
