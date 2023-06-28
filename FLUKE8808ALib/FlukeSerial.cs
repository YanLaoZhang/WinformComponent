using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static FLUKE8808ALib.FlukeForm;

namespace FLUKE8808ALib
{
    public class FlukeSerial
    {
        System.IO.Ports.SerialPort _serialPort;
        string _portName;
        System.Windows.Forms.RichTextBox _richTextBox;

        // FLUKE 8808A万用表串口
        string[] RangeMap = new string[6] { "200uA", "2000uA", "20mA", "200mA", "2A", "10A" };
        bool bool_exit_current_1 = false;
        string str_Receive_current = "";

        /// <summary>
        /// 非阻塞式等待
        /// </summary>
        /// <param name="t"></param>
        public void Delay(int t)
        {
            int numa = Environment.TickCount;
            while (true)
            {
                Application.DoEvents();
                if (Environment.TickCount - numa > t)
                {
                    break;
                }
            }
        }

        public FlukeSerial(System.IO.Ports.SerialPort serialPort, string portName, System.Windows.Forms.RichTextBox richTextBox)
        {
            _serialPort = serialPort;
            _portName = portName;
            _serialPort.PortName = _portName;
            _serialPort.BaudRate = 9600;
            _serialPort.DataBits = 8;
            _serialPort.Parity = System.IO.Ports.Parity.None;
            _serialPort.StopBits = System.IO.Ports.StopBits.One;
            _richTextBox = richTextBox;
            _richTextBox.Text = "";
            str_Receive_current = "";
            // 绑定 DataReceived 事件处理程序
            _serialPort.DataReceived += SerialPort_DataReceived;
        }

        private void SerialPort_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            string indata = _serialPort.ReadExisting();
            str_Receive_current += indata;
            _richTextBox.Invoke((Action)(() =>
            {
                _richTextBox.Text += indata;
                _richTextBox.SelectionStart = _richTextBox.TextLength;
                _richTextBox.ScrollToCaret();
            }));
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

                // 打开串口
                _serialPort.Open();
                _richTextBox.Text = string.Empty;
                _serialPort.DataReceived += SerialPort_DataReceived;

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 打开串口
        /// </summary>
        /// <returns></returns>
        public bool OpenPort()
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

        /// <summary>
        /// 发送指令到万用表，并获取返回值
        /// </summary>
        /// <param name="str_send">发送的指令字符串</param>
        /// <param name="t">超时时间</param>
        /// <param name="Respone">是否接收响应信息</param>
        /// <param name="str_value">返回信息</param>
        /// <returns></returns>
        private bool SendAndWaitCurrent(string str_send, int t, Boolean Respone, ref string str_value)
        {
            try
            {
                OpenPort();

                bool_exit_current_1 = false;
                str_Receive_current = "";
                _serialPort.DiscardInBuffer();
                _serialPort.DiscardOutBuffer();

                //str_Send_X代表要发送的字符串,指令后需添加结束符
                string str_Send_X = str_send + "\r";

                _serialPort.Write(str_Send_X);
                if (Respone == false)
                {
                    return true;
                }
                int numa = Environment.TickCount;
                while (str_Receive_current.IndexOf("=>\r\n") < 0 && str_Receive_current.IndexOf("!>\r\n") < 0 && str_Receive_current.IndexOf("?>\r\n") < 0)
                {
                    Application.DoEvents();
                    if (Environment.TickCount - numa > t)
                    {
                        return false;
                    }
                    if (bool_exit_current_1)
                    {
                        return false;
                    }
                }
                if (str_Receive_current.Contains("!>"))
                {
                    MessageBox.Show("执行错误或检测到与设备相关的错误。命令能够被正确解析，但是不能执行。");
                }
                if (str_Receive_current.Contains("?>"))
                {
                    MessageBox.Show("检测到命令错误。由于不能解析命令，所以未执行。");
                }
                //rich_run_log($"+{str_Receive_current}+");
                str_value = str_Receive_current.Trim().Replace("=>", "").Replace(@"\r\n", "");
                return true;
            }
            catch (Exception ee)
            {
                MessageBox.Show($"向万用表发送指令{str_send},发生异常：{ee.Message}");
                return false;
            }
            //finally { _serialPort.Close(); }
        }

        #region FLUKE 8808A万用表
        /// <summary>
        /// 锁定万用表面板：RWLS
        /// </summary>
        /// <returns></returns>
        public bool Lock(ref string error_log)
        {
            try
            {
                string cmd_lock = "RWLS";
                string str_ret_value = "";
                if (SendAndWaitCurrent(cmd_lock, 2000, false, ref str_ret_value) == false)
                {
                    error_log = "锁定面板失败";
                    return false;
                }
                //// 验证
                //if (str_ret_value.Contains("") == false)
                //{
                //    return false;
                //}
                return true;
            }
            catch (Exception ee)
            {
                error_log = $"锁定万用表面板，发生异常:[{ee.Message}]";
                return false;
            }
        }

        /// <summary>
        /// 设置万用表功能为直流电流测试：ADC
        /// </summary>
        /// <returns></returns>
        public bool SetFunction(ref string error_log)
        {
            try
            {
                string cmd_model = "ADC";
                string str_ret_value = "";
                if (SendAndWaitCurrent(cmd_model, 2000, false, ref str_ret_value) == false)
                {
                    error_log = "设置功能为ADC失败";
                    return false;
                }
                Delay(300);
                // 验证
                string cmd_get_model = "FUNC1?";  // 万用表返回所选功能的命令助记符
                str_ret_value = "";
                if (SendAndWaitCurrent(cmd_get_model, 2000, true, ref str_ret_value) == false)
                {
                    error_log = "读取当前功能失败";
                    return false;
                }
                if (str_ret_value.Contains(cmd_model) == false)
                {
                    error_log = "设置功能为ADC未生效";
                    return false;
                }
                return true;
            }
            catch (Exception ee)
            {
                error_log = $"设置万用表直流电流测试，发生异常:[{ee.Message}]";
                return false;
            }
        }

        /// <summary>
        /// 设置万用表量程：RANGE <value>
        /// 量程编号-直流电流对应关系：1-200uA | 2-2000uA | 3-20mA | 4-200mA | 5-2A | 6-10A
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public bool SetRange(string val, ref string error_log)
        {
            try
            {
                // 先退出自动量程模式
                string cmd_fixed = "FIXED";
                string str_ret_value = "";
                if (SendAndWaitCurrent(cmd_fixed, 2000, true, ref str_ret_value) == false)
                {
                    error_log = "退出自动量程模式失败";
                    return false;
                }
                Delay(300);
                string cmd_range = $"RANGE {val}";
                if (SendAndWaitCurrent(cmd_range, 2000, true, ref str_ret_value) == false)
                {
                    error_log = "设置量程失败";
                    return false;
                }
                Delay(300);
                // 验证
                string cmd_get_range = "RANGE1?";  // 返回主屏上当前所选的量程
                str_ret_value = "";
                if (SendAndWaitCurrent(cmd_get_range, 2000, true, ref str_ret_value) == false)
                {
                    error_log = "读取当前量程失败";
                    return false;
                }
                if (str_ret_value.Contains(val) == false)
                {
                    error_log = "设置量程未生效";
                    return false;
                }
                return true;
            }
            catch (Exception ee)
            {
                error_log = $"设置万用表量程，发生异常:[{ee.Message}]";
                return false;
            }
        }

        /// <summary>
        /// 获取当前万用表量程：RANGE <value>
        /// 量程编号-直流电流对应关系：1-200uA | 2-2000uA | 3-20mA | 4-200mA | 5-2A | 6-10A
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool GetRange(ref string result, ref string error_log)
        {
            try
            {
                string cmd_get_range = "RANGE1?";  // 返回主屏上当前所选的量程
                //string str_ret_value = "";
                if (SendAndWaitCurrent(cmd_get_range, 2000, true, ref result) == false)
                {
                    error_log = "读取当前量程失败";
                    return false;
                }
                //int index = int.Parse(str_ret_value);
                //result = str_ret_value + "-" + RangeMap[index-1];
                return true;
            }
            catch (Exception ee)
            {
                error_log = $"读取当前万用表量程，发生异常:[{ee.Message}]";
                return false;
            }
        }

        /// <summary>
        /// 设置万用表触发方式：TRIGGER <value>
        /// value: 可选值
        /// 1-内触发（连续触发测量）
        /// 2~5-外触发（以用户方向触发测量）
        /// 外触发如下：
        /// • 后面板触发禁用的外触发。这包括触发类型 2 和 3
        /// • 后面板触发允许的外触发。这包括触发类型 4 和 5
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public bool SetTriggerModel(string val, ref string error_log)
        {
            try
            {
                string cmd_trigger = $"TRIGGER {val}";
                string str_ret_value = "";
                if (SendAndWaitCurrent(cmd_trigger, 2000, false, ref str_ret_value) == false)
                {
                    error_log = "设置触发失败";
                    return false;
                }
                Delay(300);
                // 验证
                string cmd_get_trigger = "TRIGGER?";  // 返回通过 TRIGGER 命令设置的触发类型
                str_ret_value = "";
                if (SendAndWaitCurrent(cmd_get_trigger, 2000, true, ref str_ret_value) == false)
                {
                    error_log = "读取当前触发方式失败";
                    return false;
                }
                if (str_ret_value.Contains(val) == false)
                {
                    error_log = "设置触发未生效";
                    return false;
                }
                return true;
            }
            catch (Exception ee)
            {
                error_log = $"设置万用表触发方式，发生异常:[{ee.Message}]";
                return false;
            }
        }

        /// <summary>
        /// 设置万用表测量速度：RATE <value>
        /// value可选值：
        /// S-慢速（2.5 读数/秒）
        /// M-中速（20 读数/秒）
        /// F-快速（100 读数/秒）
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public bool SetRate(string val, ref string error_log)
        {
            try
            {
                string cmd_rate = $"RATE {val}";
                string str_ret_value = "";
                if (SendAndWaitCurrent(cmd_rate, 2000, false, ref str_ret_value) == false)
                {
                    error_log = "设置测量速度失败";
                    return false;
                }
                Delay(300);
                // 验证
                string cmd_get_rate = "RATE?";  // 返回 <speed>：慢速 S（2.5 读数/秒）、中速 M（20 读数/秒）或快速 F（100 读数 / 秒）
                str_ret_value = "";
                if (SendAndWaitCurrent(cmd_get_rate, 2000, true, ref str_ret_value) == false)
                {
                    error_log = "读取当前测量速度失败";
                    return false;
                }
                if (str_ret_value.Contains(val) == false)
                {
                    error_log = "设置测量速度未生效";
                    return false;
                }
                return true;
            }
            catch (Exception ee)
            {
                error_log = $"设置测量速度，发生异常:[{ee.Message}]";
                return false;
            }
        }

        /// <summary>
        /// 读取万用表测量速度：RATE <value>
        /// value可选值：
        /// S-慢速（2.5 读数/秒）
        /// M-中速（20 读数/秒）
        /// F-快速（100 读数/秒）
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool GetRate(ref string result, ref string error_log)
        {
            try
            {
                string cmd_get_rate = "RATE?";  // 返回 <speed>：慢速 S（2.5 读数/秒）、中速 M（20 读数/秒）或快速 F（100 读数 / 秒）
                if (SendAndWaitCurrent(cmd_get_rate, 2000, true, ref result) == false)
                {
                    error_log = "读取当前测量速度失败";
                    return false;
                }
                return true;
            }
            catch (Exception ee)
            {
                error_log = $"读取当前测量速度，发生异常:[{ee.Message}]";
                return false;
            }
        }

        /// <summary>
        /// 设置万用表输出格式：FORMAT <value>
        /// value可选值：
        /// 1-输出没有测量单位（VDC、ADC、OHMS，等）的测量值
        /// 2-可输出包括测量单位的测量值
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public bool SetFormat(string val, ref string error_log)
        {
            try
            {
                string cmd_format = $"FORMAT {val}";
                string str_ret_value = "";
                if (SendAndWaitCurrent(cmd_format, 2000, false, ref str_ret_value) == false)
                {
                    error_log = "设置输出格式失败";
                    return false;
                }
                Delay(300);
                // 验证
                string cmd_get_format = "FORMAT?";  // 返回当前使用的格式（1 或 2）
                str_ret_value = "";
                if (SendAndWaitCurrent(cmd_get_format, 2000, true, ref str_ret_value) == false)
                {
                    error_log = "读取当前输出格式失败";
                    return false;
                }
                if (str_ret_value.Contains(val) == false)
                {
                    error_log = "设置输出格式未生效";
                    return false;
                }
                return true;
            }
            catch (Exception ee)
            {
                error_log = $"设置万用表输出格式，发生异常:[{ee.Message}]";
                return false;
            }
        }

        /// <summary>
        /// 读取万用表输出格式：FORMAT <value>
        /// value可选值：
        /// 1-输出没有测量单位（VDC、ADC、OHMS，等）的测量值
        /// 2-可输出包括测量单位的测量值
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool GetFormat(ref string result, ref string error_log)
        {
            try
            {
                string cmd_get_format = "FORMAT?";  // 返回当前使用的格式（1 或 2）
                if (SendAndWaitCurrent(cmd_get_format, 2000, true, ref result) == false)
                {
                    error_log = "读取当前输出格式失败";
                    return false;
                }
                return true;
            }
            catch (Exception ee)
            {
                error_log = $"读取万用表输出格式，发生异常:[{ee.Message}]";
                return false;
            }
        }

        /// <summary>
        /// 获取主屏显示的电流值：VAL1?
        /// 万用表的数文输出如下例所示：
        ///    +1.2345E+0（格式 1） 测量值 1.2345 
        ///    +1.2345E+6（格式 1） 测量值 1.2345M 
        ///    +12.345E+6 OHM（格式 2） 测量值 12.345MΩ
        ///    +/- 1.0E+9 过载（屏幕上显示 0L）
        /// 微安：
        /// 量程为200uA，返回值为+0.000E-6
        /// 量程为2000uA，返回值为+0.00E-6
        /// 毫安：
        /// 量程为20mA，返回值为-0.0003E-3
        /// 量程为200mA，返回值为-0.001E-3
        /// 安：
        /// 量程为2A，返回值为+0.00001E+0
        /// 量程为10A，返回值为-0.0003E+0
        /// </summary>
        /// <param name="result">读数</param>
        /// <param name="unit">单位：uA、mA、A</param>
        /// <returns></returns>
        public bool GetCurrent(ref string result, ref string unit, ref string error_log)
        {
            try
            {
                string cmd_get_current = "VAL1?";  // 万用表返回主屏上显示的值。如果主屏为空白，则返回下次触发测量的值。
                if (SendAndWaitCurrent(cmd_get_current, 2000, true, ref result) == false)
                {
                    error_log = "读取主屏的电流值失败";
                    return false;
                }
                try
                {
                    if (result.Contains("E+0"))  // 直流 安档
                    {
                        result = result.Trim().Replace("E+0", "");
                        unit = "A";
                        float.Parse(result);
                    }
                    else if (result.Contains("E-3"))  // 直流 毫安档
                    {
                        result = result.Trim().Replace("E-3", "");
                        unit = "mA";
                        float.Parse(result);
                    }
                    else if (result.Contains("E-6"))  // 直流 微安档
                    {
                        result = result.Trim().Replace("E-6", "");
                        unit = "uA";
                        float.Parse(result);
                    }
                    else if (result.Contains("E+9"))
                    {
                        error_log = "测试过载，请选择合适连接端子和量程！";
                        return false;
                    }
                }
                catch (Exception ee)
                {
                    error_log = $"读取到的值无法转换为float类型: [{ee.Message}]";
                    return false;
                }
                return true;
            }
            catch (Exception ee)
            {
                error_log = $"读取万用表的电流值，发生异常:[{ee.Message}]";
                return false;
            }
        }
        #endregion


    }
}
