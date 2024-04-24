using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace RelaySerialLib
{
    public class RelaySerial
    {
        [DllImport("user32.dll")]
        private static extern int MessageBoxTimeoutA(IntPtr hWnd, string msg, string caption, int type, int id, int time);
        //------继电器控制指令 "AA5A00 + 开关编号 + 1|0 + 00FF"
        byte[] CMD_PREFIX = new byte[3] { 0xAA, 0x5A, 0x00};
        byte[] CMD_SUFFIX = new byte[2] { 0x00, 0xFF};
        byte[] CMD_OPEN_ALL = new byte[6] { 0xAA, 0x5A, 0x00, 0xFF, 0x00, 0xFF };
        byte[] CMD_CLOSE_ALL = new byte[6] { 0xAA, 0x5A, 0x00, 0xFE, 0x00, 0xFF };
        byte[] CMD_GET_STATUS = new byte[6] { 0xAA, 0x5A, 0x00, 0xFC, 0x00, 0xFF };

        string[] STR_CMD_OPEN_SINGLE = new string[16] { "AA5A000100FF", "AA5A001100FF", "AA5A002100FF", "AA5A003100FF", "AA5A004100FF", "AA5A005100FF", 
            "AA5A006100FF", "AA5A007100FF", "AA5A008100FF", "AA5A009100FF", "AA5A00A100FF", "AA5A00B100FF", "AA5A00C100FF", "AA5A00D100FF", "AA5A00E100FF", "AA5A00F100FF" };
        string[] STR_CMD_CLOSE_SINGLE = new string[16] { "AA5A000000FF", "AA5A001000FF", "AA5A002000FF", "AA5A003000FF", "AA5A004000FF", "AA5A005000FF", 
            "AA5A006000FF", "AA5A007000FF", "AA5A008000FF", "AA5A009000FF", "AA5A00A000FF", "AA5A00B000FF", "AA5A00C000FF", "AA5A00D000FF", "AA5A00E000FF", "AA5A00F000FF" };
        string STR_CMD_OPEN_ALL = "AA5A00FF00FF";
        string STR_CMD_CLOSE_ALL = "AA5A00FE00FF";
        string STR_CMD_GET_STATUS = "AA5A00FC00FF";
        public int[] int_relay_status = new int[16];
        public byte[] byte_Receive = new byte[256];
        public List<byte> Recive_buffer = new List<byte>(256);
        bool bool_exit_relay = false;

        System.IO.Ports.SerialPort _serialPort;
        string _portName;

        public RelaySerial(System.IO.Ports.SerialPort serialPort, string portName)
        {
            _serialPort = serialPort;
            _portName = portName;
            _serialPort.PortName = _portName;
            _serialPort.BaudRate = 9600;
            _serialPort.DataBits = 8;
            _serialPort.Parity = System.IO.Ports.Parity.None;
            _serialPort.StopBits = System.IO.Ports.StopBits.One;
            // 绑定 DataReceived 事件处理程序
            _serialPort.DataReceived += SerialPort_DataReceived;
        }

        // DataReceived 事件处理程序
        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                int len = _serialPort.BytesToRead;
                byte[] bytes = new byte[len];
                _serialPort.Read(bytes, 0, len);
                Recive_buffer.AddRange(bytes);
            }
            catch (InvalidOperationException ex)
            {
                MessageBoxTimeoutA(IntPtr.Zero, ex.Message, "", 16, 0, 3000);
            }
            catch (TimeoutException)
            {
                // 忽略异常
            }
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
        /// 发送指令到继电器
        /// </summary>
        /// <param name="str_send">指令内容</param>
        /// <param name="t">超时时间</param>
        /// <param name="Respone">是否需要解析返回数据</param>
        /// <param name="byte_ret_value">存储返回数据，</param>
        /// <returns></returns>
        private bool SendCMDToRelay(string str_send, int t, Boolean Respone, ref byte[] byte_ret_value)
        {
            try
            {
                OpenRelayPort();
                bool_exit_relay = false;
                _serialPort.DiscardInBuffer();
                _serialPort.DiscardOutBuffer();
                Recive_buffer.Clear();
                // string str_Send_X = str_send + "\r\n"; //str_Send_X代表要发送的字符串
                //---字符分解成字节;
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
                    
                    // 有些继电器会有多余信息干扰，添加AA起始标识位检查来消除干扰
                    if (Recive_buffer.Contains(0xAA) && Recive_buffer.Contains(0x5A)){
                        startIndex = Recive_buffer.IndexOf(0xAA);
                        Console.WriteLine($"startIndex:{startIndex}");
                        if(startIndex < 0)
                        {
                            continue;
                        }
                        else
                        {
                            if (Recive_buffer.Count >= byte_ret_value.Length+startIndex)
                            {
                                // 数据接收完整
                                Console.WriteLine($"Recive_buffer.Count:[{Recive_buffer.Count}]");
                                Console.WriteLine($"copy count:[{byte_ret_value.Length}]");
                                Recive_buffer.CopyTo(startIndex, byte_Receive, 0, byte_ret_value.Length);
                                Recive_buffer.Clear();
                                break;
                            }
                        }
                    }
                    
                }
                for (int i = 0; i < byte_ret_value.Length; i++)
                {
                    byte_ret_value[i] = byte_Receive[i];
                }
                return true;

            }
            catch (Exception ee)
            {
                return false;
            }
            //finally { _serialPort.Close(); }
        }

        #region 继电器
        public bool OpenAllRelay()
        {
            try
            {
                byte[] byte_ret_value = new byte[6];
                if (SendCMDToRelay(STR_CMD_OPEN_ALL, 200, false, ref byte_ret_value) == false)
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

        public bool CloseAllRelay()
        {
            try
            {
                byte[] byte_ret_value = new byte[6];
                if (SendCMDToRelay(STR_CMD_CLOSE_ALL, 200, false, ref byte_ret_value) == false)
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

        public bool TriggerRelaySingle(ushort relay_no, bool trigger)
        {
            try
            {
                string str_command = null;
                if(trigger)
                {
                    switch (relay_no)
                    {
                        case 1:
                            str_command = STR_CMD_OPEN_SINGLE[0];
                            break;
                        case 2:
                            str_command = STR_CMD_OPEN_SINGLE[1];
                            break;
                        case 3:
                            str_command = STR_CMD_OPEN_SINGLE[2];
                            break;
                        case 4:
                            str_command = STR_CMD_OPEN_SINGLE[3];
                            break;
                        case 5:
                            str_command = STR_CMD_OPEN_SINGLE[4];
                            break;
                        case 6:
                            str_command = STR_CMD_OPEN_SINGLE[5];
                            break;
                        case 7:
                            str_command = STR_CMD_OPEN_SINGLE[6];
                            break;
                        case 8:
                            str_command = STR_CMD_OPEN_SINGLE[7];
                            break;
                        case 9:
                            str_command = STR_CMD_OPEN_SINGLE[8];
                            break;
                        case 10:
                            str_command = STR_CMD_OPEN_SINGLE[9];
                            break;
                        case 11:
                            str_command = STR_CMD_OPEN_SINGLE[10];
                            break; ;
                        case 12:
                            str_command = STR_CMD_OPEN_SINGLE[11];
                            break;
                        case 13:
                            str_command = STR_CMD_OPEN_SINGLE[12];
                            break;
                        case 14:
                            str_command = STR_CMD_OPEN_SINGLE[13];
                            break;
                        case 15:
                            str_command = STR_CMD_OPEN_SINGLE[14];
                            break;
                        case 16:
                            str_command = STR_CMD_OPEN_SINGLE[15];
                            break;
                    }
                }
                else
                {
                    switch (relay_no)
                    {
                        case 1:
                            str_command = STR_CMD_CLOSE_SINGLE[0];
                            break;
                        case 2:
                            str_command = STR_CMD_CLOSE_SINGLE[1];
                            break;
                        case 3:
                            str_command = STR_CMD_CLOSE_SINGLE[2];
                            break;
                        case 4:
                            str_command = STR_CMD_CLOSE_SINGLE[3];
                            break;
                        case 5:
                            str_command = STR_CMD_CLOSE_SINGLE[4];
                            break;
                        case 6:
                            str_command = STR_CMD_CLOSE_SINGLE[5];
                            break;
                        case 7:
                            str_command = STR_CMD_CLOSE_SINGLE[6];
                            break;
                        case 8:
                            str_command = STR_CMD_CLOSE_SINGLE[7];
                            break;
                        case 9:
                            str_command = STR_CMD_CLOSE_SINGLE[8];
                            break;
                        case 10:
                            str_command = STR_CMD_CLOSE_SINGLE[9];
                            break;
                        case 11:
                            str_command = STR_CMD_CLOSE_SINGLE[10];
                            break; ;
                        case 12:
                            str_command = STR_CMD_CLOSE_SINGLE[11];
                            break;
                        case 13:
                            str_command = STR_CMD_CLOSE_SINGLE[12];
                            break;
                        case 14:
                            str_command = STR_CMD_CLOSE_SINGLE[13];
                            break;
                        case 15:
                            str_command = STR_CMD_CLOSE_SINGLE[14];
                            break;
                        case 16:
                            str_command = STR_CMD_CLOSE_SINGLE[15];
                            break;
                    }
                }

                byte[] byte_ret_value = new byte[6];
                if (SendCMDToRelay(str_command, 200, false, ref byte_ret_value) == false)
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

        public bool GetRelayStatus(ref string str_error_log, ref int[] int_relay_status)
        {
            try
            {
                byte[] byte_ret_value = new byte[6];  // 返回数据，长度为6
                /*
                 * 查询继电器状态指令：AA5A00FC00FF  
                 * 返回  AA5A00FC0000  (FC后面的两个十六进制对应的两组8位二进制为十六个继电器的状态  1为打开，0为关闭)
                 */
                if (SendCMDToRelay(STR_CMD_GET_STATUS, 500, true, ref byte_ret_value) == false)
                {
                    str_error_log = "查询继电器状态指令异常";
                    return false;
                }
                //MessageBoxTimeoutA(IntPtr.Zero, BitConverter.ToString(byte_ret_value)/*.Replace("-", "")*/, "", 16, 0, 3000);
                // aa 5a 00 fc 00 ff aa 5a 00 fc 80 00 通道1打开
                // aa 5a 00 fc 00 ff aa 5a 00 fc 40 00 通道2打开
                int int_ret_value = (byte_ret_value[4] << 8) + byte_ret_value[5];
                int_relay_status[15] = int_ret_value & 0x1;
                int_relay_status[14] = (int_ret_value & 0x2) >> 1;
                int_relay_status[13] = (int_ret_value & 0x4) >> 2;
                int_relay_status[12] = (int_ret_value & 0x8) >> 3;
                int_relay_status[11] = (int_ret_value & 0x10) >> 4;
                int_relay_status[10] = (int_ret_value & 0x20) >> 5;
                int_relay_status[9] = (int_ret_value & 0x40) >> 6;
                int_relay_status[8] = (int_ret_value & 0x80) >> 7;
                int_relay_status[7] = (int_ret_value & 0x100) >> 8;
                int_relay_status[6] = (int_ret_value & 0x200) >> 9;
                int_relay_status[5] = (int_ret_value & 0x400) >> 10;
                int_relay_status[4] = (int_ret_value & 0x800) >> 11;
                int_relay_status[3] = (int_ret_value & 0x1000) >> 12;
                int_relay_status[2] = (int_ret_value & 0x2000) >> 13;
                int_relay_status[1] = (int_ret_value & 0x4000) >> 14;
                int_relay_status[0] = (int_ret_value & 0x8000) >> 15;
                return true;
            }
            catch (Exception ee)
            {
                str_error_log = ee.Message;
                return false;
            }
        }
        #endregion
    }
}
