using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace TDMSerialLib
{
    public class TDMSerial
    {
        [DllImport("user32.dll")]
        private static extern int MessageBoxTimeoutA(IntPtr hWnd, string msg, string caption, int type, int id, int time);

        // 请求连续数据 指令
        byte[] CMD_REQUEST_CONTINUOUS_DATA = new byte[6] { 0xAA, 0x55, 0x2, 0xF1, 0x0, 0xF4 };
        // 停止连续数据 指令
        byte[] CMD_STOP_CONTINUOUS_DATA = new byte[6] { 0xAA, 0x55, 0x2, 0xF2, 0x0, 0xF4 };
        // 单次读取测量值 指令
        // 发送内容：AA 55 02 FE 01 00
        // 应答内容：AA 55 04 F6 v0 v1 sumH sumL
        /*
         v0 v1 为测量值 0xv1v0，即测量值 value = v1 * 256 + v0；
        value 为双字节有符号整型数据(SIGNED SHORT)，
        四位半取值范围 -19999 ~ 19999，
        三位半取值范围为-1999 ~ 1999，
        当 value = 0x8000[-32768] 时，表示超量程；
        value 与实际值的关系：
        实际值 = value 除以 10 的 N 次方，N 根据具体量程的小数位数而定，
        如
        四位半 2mA[1.9999 N = 4]、20mA[19.999 N = 3]、200mA[199.99 N = 2]、400V[402.0 N = 1]、
        三位半 400V[400 N = 0]、20mA[19.99 N = 2]
        关于正负值的问题：
        signed short 类型本身包含正负的，如 0x0002 = 2，0xFFFE = -2，部分用户把返回
        值全做为正数处理，那么负数 0xFFFE 就会被判断为 65534；一种解决办法就是，当 value > 32768 时，返
        回值为负值，实际值为 value - 65536，如 0xFFFE 为 65534 – 65536 = -2，否则为正值。
         */
        byte[] CMD_GET_DATA = new byte[6] { 0xAA, 0x55, 0x2, 0xFE, 0x01, 0x00 };

        public byte[] byte_Receive = new byte[256];
        public List<byte> Recive_buffer = new List<byte>(256);
        bool bool_exit_voltage_1 = false;

        System.IO.Ports.SerialPort _serialPort;
        string _portName;
        int _N;    // 小数位数

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serialPort"></param>
        /// <param name="portName"></param>
        /// <param name="N">数显表的小数位数</param>
        public TDMSerial(System.IO.Ports.SerialPort serialPort, string portName, int N=3)
        {
            _serialPort = serialPort;
            _portName = portName;
            _N = N;

            // 绑定 DataReceived 事件处理程序
            serialPort.DataReceived += SerialPort_DataReceived;
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
                //MessageBoxTimeoutA(IntPtr.Zero, $"DataReceived Error:{ex.Message}", "", 16, 0, 3000);
            }
            catch (TimeoutException)
            {
                // 忽略异常
            }
        }

        public bool CheckStatus()
        {
            return _serialPort.IsOpen;
        }

        public bool OpenSerialPort()
        {
            try
            {
                if(_serialPort.IsOpen)
                {
                    _serialPort.Close();
                }
                _serialPort.PortName = _portName;
                _serialPort.BaudRate = 115200;
                _serialPort.DataBits = 8;
                _serialPort.Parity = System.IO.Ports.Parity.None;
                _serialPort.StopBits = System.IO.Ports.StopBits.One;
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

        private bool SendCMDToTDM(byte[] byte_send, int t, Boolean Respone, ref byte[] byte_ret_value, int ret_Length)
        {
            try
            {
                OpenSerialPort();
                bool_exit_voltage_1 = false;
                _serialPort.DiscardInBuffer();
                _serialPort.DiscardOutBuffer();
                Recive_buffer.Clear();
                _serialPort.Write(byte_send, 0, byte_send.Length);

                if (Respone == false)
                {
                    return true;
                }
                int numa = Environment.TickCount;
                while (true)
                {
                    if (Environment.TickCount - numa > t)
                    {
                        //break;
                        return false;
                    }
                    if (Recive_buffer.Count >= ret_Length)
                    {
                        Recive_buffer.CopyTo(0, byte_Receive, 0, Recive_buffer.Count);//数据接收完整
                        Recive_buffer.Clear();
                        break;
                    }
                    if (bool_exit_voltage_1)
                    {
                        break;
                    }
                }
                for (int i = 0; i < ret_Length; i++)
                {
                    byte_ret_value[i] = byte_Receive[i];
                }
                return true;
            }
            catch (Exception ee)
            {
                return false;
            }
            finally { _serialPort.Close(); }
        }

        public bool _vol_stop_continuous()
        {
            try
            {
                byte[] byte_ret_value = new byte[8];
                if (SendCMDToTDM(CMD_STOP_CONTINUOUS_DATA, 200, false, ref byte_ret_value, byte_ret_value.Length) == false)
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

        public bool _voltage_value(ref float ret_value)
        {
            try
            {
                byte[] byte_ret_value = new byte[8];
                if (SendCMDToTDM(CMD_GET_DATA, 3000, true, ref byte_ret_value, 8) == false)
                {
                    return false;
                }
                if (TranslateValue_voltage(byte_ret_value, _N, ref ret_value) == false)
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

        public bool TranslateValue_voltage(byte[] byte_in, int _N, ref float vol_out)
        {
            try
            {
                int _vol_value = byte_in[5] * 256 + byte_in[4];
                if (TranslateValue(_vol_value, _N, ref vol_out) == false)
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

        private bool TranslateValue(int in_data, int _N, ref float ret_value)
        {
            try
            {
                if (in_data == 32768)
                {
                    return false;
                }
                if (in_data > 32768)
                {
                    in_data -= 65536;
                }
                ret_value = (float)((float)in_data / Math.Pow(10, _N));
                return true;
            }
            catch (Exception ee)
            {
                return false;
            }
        }
    }

}
