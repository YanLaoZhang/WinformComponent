using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XDC01SerialLib
{
    public class CameraInfo
    {
        public string FwVersion { get; set; } = "";
        public string RtosVersion { get; set; } = "";
        public string McuBootloaderV { get; set; } = "";
        public string McuAppV { get; set; } = "";
        public string HwVersion { get; set; } = "";
        public string Battery { get; set; } = "";
        public string BatteryPerCent { get; set; } = "";
        public string Temp { get; set; } = "";
        public string McuRfType { get; set; } = "";
        public string Backend { get; set; } = "";
        public string WifiVer { get; set; } = "";

        public override string ToString()
        {
            return $"固件版本：[{this.FwVersion}]；\r\n硬件版本：[{this.HwVersion}]；\r\nMCU版本：[{this.McuAppV}]；\r\n" +
                $"电池电压：[{this.Battery}]；\r\n电池电量百分比：[{this.BatteryPerCent}]；\r\nCPU温度：[{this.Temp}]；\r\n" +
                $"WiFi版本：[{this.WifiVer}]；";
        }
    }

    public class XDC01Serial
    {
        [DllImport("user32.dll")]
        private static extern int MessageBoxTimeoutA(IntPtr hWnd, string msg, string caption, int type, int id, int time);

        System.IO.Ports.SerialPort _serialPort;
        string _portName;
        System.Windows.Forms.RichTextBox _richTextBox;
        public string str_Receive_skybell = "";

        // 结束字符串
        const string ENDFLAG_1 = "info print end";
        const string ENDFLAG_2 = "#";

        // LED灯颜色组
        const string LED_RED = "red";
        const string LED_GREEN = "green";
        const string LED_BLUE = "blue";
        const string LED_WHITE = "white";
        const string LED_OFF = "off";

        // PIR开关
        const string PIR_ON = "on";
        const string PIR_OFF = "off";

        // RF模式
        const string RF_TX = "send";
        const string RF_RX = "rcv";

        // IC充电使能
        const string CHARGE_ENABLE = "enable";
        const string CHARGE_DISABLE = "disable";

        // IR_CUT开关
        const string IR_CUT_ON = "on";
        const string IR_CUT_OFF = "off";

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

        public XDC01Serial(System.IO.Ports.SerialPort serialPort, string portName, System.Windows.Forms.RichTextBox richTextBox)
        {
            _serialPort = serialPort;
            _portName = portName;
            _serialPort.PortName = _portName;
            _serialPort.BaudRate = 115200;
            _serialPort.DataBits = 8;
            _serialPort.Parity = System.IO.Ports.Parity.None;
            _serialPort.StopBits = System.IO.Ports.StopBits.One;
            _richTextBox = richTextBox;
            str_Receive_skybell = "";
            _richTextBox.Text = "";
            // 绑定 DataReceived 事件处理程序
            _serialPort.DataReceived += SerialPort_DataReceived;
        }

        ~XDC01Serial()
        {
            if (_serialPort.IsOpen)
            {
                _serialPort.Close();
            }
        }

        // DataReceived 事件处理程序
        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string indata = _serialPort.ReadExisting();
                //Console.WriteLine($"串口信息：{indata}");
                str_Receive_skybell += indata;
                _richTextBox.Invoke((Action)(() =>
                {
                    _richTextBox.Text += indata;
                    _richTextBox.SelectionStart = _richTextBox.TextLength;
                    _richTextBox.ScrollToCaret();
                }));
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
                _richTextBox.Text = string.Empty;
                _serialPort.DataReceived += SerialPort_DataReceived;
                _serialPort.Open();

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
        /// 向XDC03发送串口指令
        /// </summary>
        /// <param name="str_send_cmd">串口指令</param>
        /// <param name="t">等待返回内容的超时时间</param>
        /// <param name="Respone">是否等待返回内容</param>
        /// <param name="str_receive_data">返回内容</param>
        /// <param name="endFlag">返回内容结束标识</param>
        /// <returns></returns>
        public bool SendCMDToXDC01(string str_send_cmd, int t, Boolean Respone, ref string str_receive_data, string endFlag = ENDFLAG_1)
        {
            try
            {
                OpenPort();
                str_Receive_skybell = "";
                _serialPort.DiscardInBuffer();
                _serialPort.DiscardOutBuffer(); // \r回车 \n换行
                string str_Send_X = str_send_cmd + "\r"; //str_Send_X代表要发送的字符串
                _serialPort.Write(str_Send_X);

                if (Respone == false)
                {
                    return true;
                }
                int numa = Environment.TickCount;
                while (str_Receive_skybell.IndexOf(endFlag) < 0)//条件是真的时执行循环，条件是假的时候跳出循环
                {
                    //Console.WriteLine($"信息：{str_Receive_skybell}");
                    Application.DoEvents();
                    if (Environment.TickCount - numa > t)
                    {
                        return false;
                    }
                }
                str_receive_data = str_Receive_skybell;
                return true;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
                return false;
            }
            finally
            {
                ClosePort();
            }
        }

        #region Unix指令
        /// <summary>
        /// ifcoonfig指令来判断是否已开机
        /// </summary>
        /// <param name="str_bell_ip_address"></param>
        /// <param name="str_error_log"></param>
        /// <returns></returns>
        public bool CheckIfconfig(ref string str_error_log)
        {
            try
            {
                string CMD_IFCOONFIG = "ifconfig";
                string str_ret_value = "";
                if (SendCMDToXDC01(CMD_IFCOONFIG, 3000, true, ref str_ret_value, ENDFLAG_2) == false)
                {
                    str_error_log = $"发送获取网卡信息指令[{CMD_IFCOONFIG}]失败";
                    return false;
                }
                if (!str_ret_value.Contains("127.0.0.1"))
                {
                    return false;
                }
                return true;
            }
            catch (Exception ee)
            {
                str_error_log = $"CheckIfconfig发生异常：[{ee.Message}]";
                return false;
            }
        }

        /// <summary>
        /// 获取系统信息：FwVersion、RtosVersion、McuBootloaderV、McuAppV、HwVersion、Battery、BatteryPerCent、Temp、McuRfType、Backend、WifiVer
        /// </summary>
        /// <param name="cameraInfo">存储系统信息的实例</param>
        /// <param name="str_error_log">错误信息</param>
        /// <returns></returns>
        public bool GetSystemParam(ref CameraInfo cameraInfo, ref string str_error_log)
        {
            /* 以下为XDC03串口日志输出
             ubus send "d3_sys_info" '{}'
            09:21:28[1681723288.691861] DBUG0: [ubus_probe_config_event { "d3_sys_info": {} }]
            09:21:28[1681723288.699978] DBUG0: [SystemParam.Release_build =]
            root@XDC03-60710:/# #0 2023-03-31 01:16:02
            09:21:28[1681723288.902909] DBUG0: [SystemParam.FwVersion = V5015]
            09:21:28[1681723288.908662] DBUG0: [SystemParam.RtosVersion = 230308]
            09:21:28[1681723288.908812] DBUG0: [SystemParam.McuBootloaderV = 21]
            09:21:28[1681723288.908917] DBUG0: [SystemParam.McuAppV = 0.15]
            09:21:28[1681723288.909060] DBUG0: [SystemParam.HwVersion = 0]
            09:21:28[1681723288.909162] DBUG0: [SystemParam.Battery = 359]
            09:21:28[1681723288.909258] DBUG0: [SystemParam.BatteryPerCent = 37%]
            09:21:28[1681723288.909350] DBUG0: [SystemParam.Temp = 332]
            09:21:28[1681723288.909446] DBUG0: [SystemParam.McuRfType = 4]
            09:21:28[1681723288.909666] DBUG0: [SystemParam.Backend=https://ecosystem.redbeecloud.com/RedbeeBackend/]
            09:21:28[1681723288.909788] DBUG0: [SystemParam.WifiVer = MT7682-v1.9]
            09:21:28[1681723288.909905] DBUG0: [info print end]
             */
            try
            {
                string CMD_SYS_INFO = "ubus send \"d3_sys_info\" '{}'";
                string str_ret_value = "";
                if (SendCMDToXDC01(CMD_SYS_INFO, 3000, true, ref str_ret_value, "SystemParam.Backend =") == false)
                {
                    str_error_log = $"发送获取系统信息指令[{CMD_SYS_INFO}]失败";
                    return false;
                }
                string[] striparr = str_ret_value.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                string str_temp = "";
                for (int i = 0; i < striparr.Length; i++)
                {
                    str_temp = striparr[i];
                    if (str_temp.Contains("SystemParam.FwVersion ="))
                    {
                        int int_deng = str_temp.IndexOf("=");
                        cameraInfo.FwVersion = str_temp.Substring(int_deng, str_temp.Length - int_deng).Replace("]", "").Replace("=", "").Replace("V", "").Trim();
                        continue;
                    }
                    if (str_temp.Contains("SystemParam.RtosVersion ="))
                    {
                        int int_deng = str_temp.IndexOf("=");
                        cameraInfo.RtosVersion = str_temp.Substring(int_deng, str_temp.Length - int_deng).Replace("]", "").Replace("=", "").Trim();
                        continue;
                    }
                    if (str_temp.Contains("SystemParam.McuBootloaderV ="))
                    {
                        int int_deng = str_temp.IndexOf("=");
                        cameraInfo.McuBootloaderV = str_temp.Substring(int_deng, str_temp.Length - int_deng).Replace("]", "").Replace("=", "").Trim();
                        continue;
                    }
                    if (str_temp.Contains("SystemParam.McuAppV ="))
                    {
                        int int_deng = str_temp.IndexOf("=");
                        cameraInfo.McuAppV = str_temp.Substring(int_deng, str_temp.Length - int_deng).Replace("]", "").Replace("=", "").Trim();
                        continue;
                    }
                    if (str_temp.Contains("SystemParam.HwVersion ="))
                    {
                        int int_deng = str_temp.IndexOf("=");
                        cameraInfo.HwVersion = str_temp.Substring(int_deng, str_temp.Length - int_deng).Replace("]", "").Replace("=", "").Trim();
                        continue;
                    }
                    if (str_temp.Contains("SystemParam.Battery ="))
                    {
                        int int_deng = str_temp.IndexOf("=");
                        cameraInfo.Battery = str_temp.Substring(int_deng, str_temp.Length - int_deng).Replace("]", "").Replace("=", "").Trim();
                        continue;
                    }
                    if (str_temp.Contains("SystemParam.BatteryPerCent ="))
                    {
                        int int_deng = str_temp.IndexOf("=");
                        cameraInfo.BatteryPerCent = str_temp.Substring(int_deng, str_temp.Length - int_deng).Replace("]", "").Replace("=", "").Trim();
                        continue;
                    }
                    if (str_temp.Contains("SystemParam.Temp ="))
                    {
                        int int_deng = str_temp.IndexOf("=");
                        cameraInfo.Temp = str_temp.Substring(int_deng, str_temp.Length - int_deng).Replace("]", "").Replace("=", "").Trim();
                        continue;
                    }
                    if (str_temp.Contains("SystemParam.McuRfType ="))
                    {
                        int int_deng = str_temp.IndexOf("=");
                        cameraInfo.McuRfType = str_temp.Substring(int_deng, str_temp.Length - int_deng).Replace("]", "").Replace("=", "").Trim();
                        continue;
                    }
                    if (str_temp.Contains("SystemParam.Backend ="))
                    {
                        int int_deng = str_temp.IndexOf("=");
                        cameraInfo.Backend = str_temp.Substring(int_deng, str_temp.Length - int_deng).Replace("]", "").Replace("=", "").Trim();
                        continue;
                    }
                    if (str_temp.Contains("SystemParam.WifiVer ="))
                    {
                        int int_deng = str_temp.IndexOf("=");
                        cameraInfo.WifiVer = str_temp.Substring(int_deng, str_temp.Length - int_deng).Replace("]", "").Replace("=", "").Trim();
                        continue;
                    }
                }
                return true;
            }
            catch (Exception ee)
            {
                str_error_log = $"GetSystemParam发生异常：[{ee.Message}]";
                return false;
            }
        }

        public bool GetFirmwareVersion(ref string str_fw, ref string str_error_log)
        {
            try
            {
                string CMD_GET_FW = "strings /usr/bin/d3_client | grep D3_FW_APP_VER";
                string str_ret_value = "";
                if (SendCMDToXDC01(CMD_GET_FW, 5000, true, ref str_ret_value, ENDFLAG_2) == false)
                {
                    str_error_log = $"发送获取FW版本信息指令[{CMD_GET_FW}]失败";
                    return false;
                }
                string[] array = str_ret_value.Split(new string[1] { "\r\n" }, StringSplitOptions.None);
                str_fw = array[1].Replace("D3_FW_APP_VER:", "").Trim();
                if (!check_int_double(str_fw))
                {
                    str_error_log = "ver";
                    return false;
                }

                return true;
            }
            catch (Exception ee)
            {
                str_error_log = $"GetTagNumber发生异常：[{ee.Message}]";
                return false;
            }
        }

        public bool mac_check(string str_0_9_A_F, int int_length)
        {
            try
            {
                string text = "(^[0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F]$)";
                string text2 = "(^[0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F]$)";
                string text3 = "(^[0-9a-fA-F][0-9a-fA-F]:[0-9a-fA-F][0-9a-fA-F]:[0-9a-fA-F][0-9a-fA-F]:[0-9a-fA-F][0-9a-fA-F]:[0-9a-fA-F][0-9a-fA-F]:[0-9a-fA-F][0-9a-fA-F]$)";
                if (int_length == 6)
                {
                    string pattern = text;
                    if (!Regex.IsMatch(str_0_9_A_F, pattern))
                    {
                        return false;
                    }
                }

                if (int_length == 12)
                {
                    string pattern2 = text2;
                    if (!Regex.IsMatch(str_0_9_A_F, pattern2))
                    {
                        return false;
                    }
                }

                if (int_length == 17)
                {
                    string pattern3 = text3;
                    if (!Regex.IsMatch(str_0_9_A_F, pattern3))
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 获取系统MAC地址
        /// </summary>
        /// <param name="str_mac"></param>
        /// <param name="str_error_log"></param>
        /// <returns></returns>
        public bool GetSystemMac(ref string str_mac, ref string str_error_log)
        {
            try
            {
                //string CMD_SYS_MAC = "cat /sys/class/net/wlan0/address";
                string CMD_SYS_MAC = "rtwpriv wlan0 efuse_get mac";
                string str_ret_value = "";
                if (SendCMDToXDC01(CMD_SYS_MAC, 5000, true, ref str_ret_value, ENDFLAG_2) == false)
                {
                    str_error_log = $"发送获取MAC信息指令[{CMD_SYS_MAC}]失败";
                    return false;
                }
                string[] array = str_ret_value.Split(new string[1] { "\r\n" }, StringSplitOptions.None);
                string text = array[1];
                str_mac = text.Replace("wlan0", "").Replace("efuse_get:", "").Trim();
                if (!mac_check(str_mac, 17))
                {
                    str_error_log = $"mac格式错误:[{str_mac}]";
                    str_mac = "error";
                    return false;
                }

                return true;
            }
            catch (Exception ee)
            {
                str_error_log = $"GetSystemMac发生异常：[{ee.Message}]";
                return false;
            }
        }

        /// <summary>
        /// 写入MAC
        /// </summary>
        /// <param name="str_mac"></param>
        /// <param name="str_error_log"></param>
        /// <returns></returns>
        public bool SetSystemMac(string str_mac, ref string str_error_log)
        {
            try
            {
                if (str_mac.Length != 17)
                {
                    str_error_log = "mac address length error";
                    return false;
                }
                if (mac_check(str_mac, 17) == false)
                {
                    str_error_log = "mac address format error";
                    return false;
                }
                string CMD_SET_MAC = $"rtwpriv wlan0 efuse_set wmap,16A,{str_mac.Replace(":", "")}";
                string str_ret_value = "";
                if (SendCMDToXDC01(CMD_SET_MAC, 5000, true, ref str_ret_value, ENDFLAG_2) == false)
                {
                    str_error_log = $"发送写入MAC指令[{CMD_SET_MAC}]失败";
                    return false;
                }

                return true;
            }
            catch (Exception ee)
            {
                str_error_log = $"SetSystemMac发生异常：[{ee.Message}]";
                return false;
            }
        }

        /// <summary>
        /// 获取系统IP地址
        /// </summary>
        /// <param name="str_mac"></param>
        /// <param name="str_error_log"></param>
        /// <returns></returns>
        public bool GetSystemIP(ref string str_ip, ref string str_error_log)
        {
            try
            {
                string CMD_SYS_IP = "ifconfig wlan0";
                string str_ret_value = "";
                if (SendCMDToXDC01(CMD_SYS_IP, 5000, true, ref str_ret_value, "UP BROADCAST") == false)
                {
                    str_error_log = $"发送获取IP信息指令[{CMD_SYS_IP}]失败";
                    return false;
                }
                if (!str_ret_value.Contains("inet addr:") && !str_ret_value.Contains("Bcast:"))
                {
                    str_error_log = $"当前WiFi连接未分配IP";
                    return false;
                }
                if (str_ret_value.Contains("192.168.77.1"))
                {
                    str_error_log = $"当前WiFi连接的IP为默认IP192.168.77.1, 请进行检查WiFi配置";
                    return false;
                }
                string[] striparr = str_ret_value.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                string str_ip_temp = "";
                for (int i = 0; i < striparr.Length; i++)
                {
                    if (striparr[i].Contains("inet addr:") && striparr[i].Contains("Bcast:"))
                    {
                        str_ip_temp = striparr[i];
                        break;
                    }
                }
                int int_inet = str_ip_temp.IndexOf("inet addr:");
                int int_Bcast = str_ip_temp.IndexOf("Bcast");
                str_ip = str_ip_temp.Substring(int_inet + 10, int_Bcast - int_inet - 10).Trim();

                System.Net.IPAddress ipAddress;
                if (System.Net.IPAddress.TryParse(str_ip, out ipAddress) == false)
                {
                    str_error_log = "IP地址格式错误";
                    return false;
                }
                return true;
            }
            catch (Exception ee)
            {
                str_error_log = $"GetSystemIP发生异常：[{ee.Message}]";
                return false;
            }
        }

        /// <summary>
        /// 检查字符串是否可转换为int或double类型
        /// </summary>
        /// <param name="str_int_double"></param>
        /// <returns></returns>
        private bool check_int_double(string str_int_double)
        {
            bool bool_int = false;
            bool bool_double = false;
            try
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(str_int_double, @"^[+-]?\d*$") == true)
                {
                    bool_int = true;
                }
                if (System.Text.RegularExpressions.Regex.IsMatch(str_int_double, @"^[+-]?\d*[.]?\d*$") == true)
                {
                    bool_double = true;
                }
                if (!bool_int && !bool_double)
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
        /// 获取亮度值
        /// </summary>
        /// <param name="str_lightsensor"></param>
        /// <param name="str_error_log"></param>
        /// <returns></returns>
        public bool GetLightSensor(ref string str_lightsensor, ref string str_error_log)
        {
            try
            {
                string CMD_LIGHT_SENSOR = "ubus send \"d3_product_test\" '{\"Lightsensor\": \"get\" }'";
                string str_ret_value = "";
                if (SendCMDToXDC01(CMD_LIGHT_SENSOR, 5000, true, ref str_ret_value, ENDFLAG_1) == false)
                {
                    str_error_log = $"发送获取亮度值信息指令[{CMD_LIGHT_SENSOR}]失败";
                    return false;
                }
                if (str_ret_value.Contains("Lightsensor value") == false)
                {
                    return false;
                }
                string[] striparr = str_ret_value.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                string str_light_temp = "";
                for (int i = 0; i < striparr.Length; i++)
                {
                    if (striparr[i].Contains("Lightsensor value"))
                    {
                        str_light_temp = striparr[i];
                        break;
                    }
                }
                int int_deng = str_light_temp.IndexOf("=");
                str_lightsensor = str_light_temp.Substring(int_deng, str_light_temp.Length - int_deng).Replace("=", "").Replace("]", "").Trim();
                if (check_int_double(str_lightsensor) == false)
                {
                    str_error_log = "亮度值格式错误";
                    return false;
                }
                return true;
            }
            catch (Exception ee)
            {
                str_error_log = $"GetLightSensor发生异常：[{ee.Message}]";
                return false;
            }
        }

        /// <summary>
        /// 判断字符串是否包含测站字段
        /// </summary>
        /// <param name="str_get_value"></param>
        /// <returns></returns>
        public bool IsContainTagNumber(string str_get_value)
        {
            try
            {
                string[] tagnumbers = new string[11] { "T010", "T020", "T030", "T040", "T050", "T060", "T070", "T080", "T090", "T100", "T110" };
                bool isContains = false;
                for (int i = 0; i < tagnumbers.Length; i++)
                {
                    string temp = tagnumbers[i];
                    if (str_get_value.Contains(temp))
                    {
                        isContains = true;
                        break;
                    }
                }
                return isContains;
            }
            catch (Exception ee)
            {
                return false;
            }
        }

        /// <summary>
        /// 获取工序号
        /// </summary>
        /// <param name="str_tagNumber"></param>
        /// <param name="str_error_log"></param>
        /// <returns></returns>
        public bool GetTagNumber(ref string str_tagNumber, ref string str_error_log)
        {
            try
            {
                string CMD_GET_TAGNUMBER = "cat /mnt/diskc/test_station";
                string str_ret_value = "";
                if (SendCMDToXDC01(CMD_GET_TAGNUMBER, 5000, true, ref str_ret_value, ENDFLAG_2) == false)
                {
                    str_error_log = $"发送获取工序号信息指令[{CMD_GET_TAGNUMBER}]失败";
                    return false;
                }
                if (!IsContainTagNumber(str_ret_value))
                {
                    return false;
                }
                string[] striparr = str_ret_value.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                string str_tagnumber_temp = "";
                for (int i = 0; i < striparr.Length; i++)
                {
                    if (IsContainTagNumber(striparr[i]))
                    {
                        str_tagnumber_temp = striparr[i];
                        break;
                    }
                }
                int startIndex = str_tagnumber_temp.IndexOf("T");
                str_tagNumber = str_tagnumber_temp.Replace("\r\n", "").Trim().Substring(startIndex, 4);
                return true;
            }
            catch (Exception ee)
            {
                str_error_log = $"GetTagNumber发生异常：[{ee.Message}]";
                return false;
            }
        }

        /// <summary>
        /// 写入工序号
        /// </summary>
        /// <param name="str_tagNumber"></param>
        /// <param name="str_error_log"></param>
        /// <returns></returns>
        public bool SetTagNumber(string str_tagNumber, ref string str_error_log)
        {
            try
            {
                string CMD_SET_TAGNUMBER = $"echo {str_tagNumber} > /mnt/diskc/test_station";
                string str_ret_value = "";
                if (SendCMDToXDC01(CMD_SET_TAGNUMBER, 5000, true, ref str_ret_value, ENDFLAG_2) == false)
                {
                    str_error_log = $"发送写入工序号信息指令[{CMD_SET_TAGNUMBER}]失败";
                    return false;
                }
                // 再次读取
                string str_cur_tagNumber = "";
                GetTagNumber(ref str_cur_tagNumber, ref str_error_log);
                if (str_cur_tagNumber == str_tagNumber)
                {
                    return true;
                }
                else
                {
                    str_error_log = $"写入工序号[{str_tagNumber}]后，读取到工序号[{str_cur_tagNumber}],写入未生效";
                    return false;
                }
            }
            catch (Exception ee)
            {
                str_error_log = $"SetTagNumber发生异常：[{ee.Message}]";
                return false;
            }
        }

        /// <summary>
        /// 获取RN号
        /// </summary>
        /// <param name="str_rn"></param>
        /// <param name="str_error_log"></param>
        /// <returns></returns>
        public bool GetRN(ref string str_rn, ref string str_error_log)
        {
            try
            {
                string CMD_GET_RN = "cat /mnt/diskc/factorytest_rn";
                string str_ret_value = "";
                if (SendCMDToXDC01(CMD_GET_RN, 5000, true, ref str_ret_value, ENDFLAG_2) == false)
                {
                    str_error_log = $"发送获取RN号信息指令[{CMD_GET_RN}]失败";
                    return false;
                }

                if (!str_ret_value.Contains("XDC01"))
                {
                    return false;
                }
                string[] striparr = str_ret_value.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                string str_rn_temp = "";
                for (int i = 0; i < striparr.Length; i++)
                {
                    if (striparr[i].Contains("XDC01"))
                    {
                        str_rn_temp = striparr[i];
                        break;
                    }
                }
                int start = str_rn_temp.IndexOf("XDC01");
                str_rn_temp = str_rn_temp.Substring(start, 15);
                str_rn = str_rn_temp.Replace("\r\n", "").Trim();

                return true;
            }
            catch (Exception ee)
            {
                str_error_log = $"GetRN发生异常：[{ee.Message}]";
                return false;
            }
        }

        /// <summary>
        /// 写入工序号
        /// </summary>
        /// <param name="str_rn"></param>
        /// <param name="str_error_log"></param>
        /// <returns></returns>
        public bool SetRN(string str_rn, ref string str_error_log)
        {
            try
            {
                string CMD_SET_RN = $"echo {str_rn} > /mnt/diskc/factorytest_rn && sync";
                string str_ret_value = "";
                if (SendCMDToXDC01(CMD_SET_RN, 5000, true, ref str_ret_value, ENDFLAG_2) == false)
                {
                    str_error_log = $"发送写入RN号信息指令[{CMD_SET_RN}]失败";
                    return false;
                }
                // 再次读取
                string str_cur_rn = "";
                GetRN(ref str_cur_rn, ref str_error_log);
                if (str_cur_rn == str_rn)
                {
                    return true;
                }
                else
                {
                    str_error_log = $"写入RN号[{str_rn}]后，读取到RN号[{str_cur_rn}],写入未生效";
                    return false;
                }
            }
            catch (Exception ee)
            {
                str_error_log = $"SetRN发生异常：[{ee.Message}]";
                return false;
            }
        }

        /// <summary>
        /// 获取Wifi SSID
        /// </summary>
        /// <param name="str_wifi_ssid"></param>
        /// <param name="str_error_log"></param>
        /// <returns></returns>
        public bool GetWiFiSSID(ref string str_wifi_ssid, ref string str_error_log)
        {
            try
            {
                string CMD_GET_WIFI_SSID = "uci -c /mnt/diskc/etc/config get wireless.radio0_sta.ssid";
                string str_ret_value = "";
                if (SendCMDToXDC01(CMD_GET_WIFI_SSID, 5000, true, ref str_ret_value, ENDFLAG_2) == false)
                {
                    str_error_log = $"发送获取Wifi SSID信息指令[{CMD_GET_WIFI_SSID}]失败";
                    return false;
                }
                string[] array = str_ret_value.Split(new string[1] { "\r\n" }, StringSplitOptions.None);
                str_wifi_ssid = array[1].Replace("\r\n", "").Trim();
                return true;
            }
            catch (Exception ee)
            {
                str_error_log = $"GetWiFiSSID发生异常：[{ee.Message}]";
                return false;
            }
        }

        /// <summary>
        /// 获取Wifi Password
        /// </summary>
        /// <param name="str_wifi_password"></param>
        /// <param name="str_error_log"></param>
        /// <returns></returns>
        public bool GetWiFiPassword(ref string str_wifi_password, ref string str_error_log)
        {
            try
            {
                string CMD_GET_WIFI_PASSWORD = "uci -c /mnt/diskc/etc/config get wireless.radio0_sta.key";
                string str_ret_value = "";
                if (SendCMDToXDC01(CMD_GET_WIFI_PASSWORD, 5000, true, ref str_ret_value, ENDFLAG_2) == false)
                {
                    str_error_log = $"发送获取Wifi密码信息指令[{CMD_GET_WIFI_PASSWORD}]失败";
                    return false;
                }
                string[] array = str_ret_value.Split(new string[1] { "\r\n" }, StringSplitOptions.None);
                str_wifi_password = array[1].Replace("\r\n", "").Trim();
                return true;
            }
            catch (Exception ee)
            {
                str_error_log = $"GetWiFiPassword发生异常：[{ee.Message}]";
                return false;
            }
        }

        /// <summary>
        /// 设置wifi(ssid+密码)
        /// </summary>
        /// <param name="str_wifi_ssid"></param>
        /// <param name="str_wifi_password"></param>
        /// <param name="str_error_log"></param>
        /// <returns></returns>
        public bool SetWIFIConfig(string str_wifi_ssid, string str_wifi_password, ref string str_error_log)
        {
            try
            {
                string CMD_SET_WIFI = $"uci -c /mnt/diskc/etc/config set wireless.radio0_sta.ssid='{str_wifi_ssid}'\r\nuci -c /mnt/diskc/etc/config set wireless.radio0_sta.key='{str_wifi_password}'";
                string str_ret_value = "";
                if (SendCMDToXDC01(CMD_SET_WIFI, 5000, true, ref str_ret_value, ENDFLAG_2) == false)
                {
                    str_error_log = $"发送设置wifi信息指令[{CMD_SET_WIFI}]失败";
                    return false;
                }

                string CMD_SET_WIFI_SAVE = "uci -c /mnt/diskc/etc/config commit wireless";
                if (SendCMDToXDC01(CMD_SET_WIFI_SAVE, 5000, true, ref str_ret_value, ENDFLAG_2) == false)
                {
                    str_error_log = $"发送设置wifi信息指令[{CMD_SET_WIFI_SAVE}]失败";
                    return false;
                }

                string CMD_SET_WIFI_REMOVE = "rm /mnt/diskc/etc/config/wireless.crc && sync";
                if (SendCMDToXDC01(CMD_SET_WIFI_REMOVE, 5000, true, ref str_ret_value, ENDFLAG_2) == false)
                {
                    str_error_log = $"发送设置wifi信息指令[{CMD_SET_WIFI_REMOVE}]失败";
                    return false;
                }
                Delay(500);
                str_ret_value = "";
                string CMD_SET_WIFI_CHECK = "ls /mnt/diskc/etc/config";
                if (SendCMDToXDC01(CMD_SET_WIFI_CHECK, 5000, true, ref str_ret_value, ENDFLAG_2) == false)
                {
                    str_error_log = $"发送设置wifi信息指令[{CMD_SET_WIFI_CHECK}]失败";
                    return false;
                }

                Delay(500);
                // 再次读取
                string str_cur_wifi_ssid = "";
                GetWiFiSSID(ref str_cur_wifi_ssid, ref str_error_log);
                if (str_cur_wifi_ssid == str_wifi_ssid)
                {
                    Delay(200);
                    string str_cur_wifi_pwd = "";
                    GetWiFiPassword(ref str_cur_wifi_pwd, ref str_error_log);
                    if (str_cur_wifi_pwd == str_wifi_password)
                    {
                        return true;
                    }
                    else
                    {
                        str_error_log = $"写入wifi密码[{str_wifi_password}]后，读取到wifi密码[{str_cur_wifi_pwd}],写入未生效";
                        return false;
                    }
                }
                else
                {
                    str_error_log = $"写入wifi SSID[{str_wifi_ssid}]后，读取到wifi SSID[{str_cur_wifi_ssid}],写入未生效";
                    return false;
                }
            }
            catch (Exception ee)
            {
                str_error_log = $"SetWIFIConfig发生异常：[{ee.Message}]";
                return false;
            }
        }

        /// <summary>
        /// 进入产测模式
        /// </summary>
        /// <param name="str_error_log"></param>
        /// <returns></returns>
        public bool ChangeFactoryMode(ref string str_error_log)
        {
            try
            {
                string CMD_ENTER_FACTORY_MODE = "npiReset.sh && ubus send d3_sys_setting  '{\"rebootall\":1}'";
                string str_ret_value = "";
                if (SendCMDToXDC01(CMD_ENTER_FACTORY_MODE, 2000, true, ref str_ret_value, ENDFLAG_1) == false)
                {
                    str_error_log = $"发送进入产测模式指令[{CMD_ENTER_FACTORY_MODE}]失败";
                    return false;
                }
                return true;
            }
            catch (Exception ee)
            {
                str_error_log = $"ChangeFactoryMode发生异常：[{ee.Message}]";
                return false;
            }
        }

        /// <summary>
        /// 重启设备
        /// </summary>
        /// <param name="str_error_log"></param>
        /// <returns></returns>
        public bool Reboot(ref string str_error_log)
        {
            try
            {
                string CMD_REBOOT = "ubus send d3_sys_setting  '{\"rebootall\":1}'";
                string str_ret_value = "";
                if (SendCMDToXDC01(CMD_REBOOT, 5000, true, ref str_ret_value, ENDFLAG_1) == false)
                {
                    str_error_log = $"发送进入产测模式指令[{CMD_REBOOT}]失败";
                    return false;
                }
                return true;
            }
            catch (Exception ee)
            {
                str_error_log = $"Reboot发生异常：[{ee.Message}]";
                return false;
            }
        }

        /// <summary>
        /// 检查wifi是否进入产测模式
        /// </summary>
        /// <param name="str_error_log"></param>
        /// <returns></returns>
        public bool CheckWiFiFactoryMode(ref string str_error_log)
        {
            try
            {
                string CMD_CHECK_WIFI_MODE = "cat /mnt/diskc/etc/config/wireless";
                string str_ret_value = "";
                if (SendCMDToXDC01(CMD_CHECK_WIFI_MODE, 3000, true, ref str_ret_value, ENDFLAG_2) == false)
                {
                    str_error_log = $"发送查询wifi模式指令[{CMD_CHECK_WIFI_MODE}]失败";
                    return false;
                }
                // STA是产测模式，AP模式是出货模式 0是开启，1是关闭
                /*config wifi-iface 'radio0_ap'
                    option device 'radio0'
                    option network 'wlan'
                    option mode 'ap'
                    option encryption 'null'
                    option ssid 'XDC01-00e7d'
                    option disabled '1'

                config wifi-iface 'radio0_sta'
                    option device 'radio0'
                    option network 'wlan'
                    option mode 'sta'
                    option encryption 'psk2'
                    option ssid 'SKC1'
                    option key '01233210'
                    option disabled '0'
                */
                string[] striparr = str_ret_value.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                string str_ap_temp = "";
                string str_sta_temp = "";
                int index = 0;
                while(index < striparr.Length)
                {
                    string cur_row = striparr[index];
                    if (cur_row.Contains("config wifi-iface 'radio0_ap'"))
                    {
                        index++;
                        while (!striparr[index].Contains("option disabled"))
                        {
                            index++;
                        }
                        str_ap_temp = striparr[index];
                    }
                    if (cur_row.Contains("config wifi-iface 'radio0_sta'"))
                    {
                        index++;
                        while (!striparr[index].Contains("option disabled"))
                        {
                            index++;
                        }
                        str_sta_temp = striparr[index];
                    }
                    index++;
                }
                if(str_ap_temp.Contains("option disabled '1'") && str_sta_temp.Contains("option disabled '0'")) {
                    return true;
                }
                if(str_ap_temp.Contains("option disabled '0'") && str_sta_temp.Contains("option disabled '1'")) {
                    str_error_log = "wifi为出货模式";
                    return false;
                }
                str_error_log = $"解析异常ap:[{str_ap_temp}],sta[{str_sta_temp}]";
                return false;
            }
            catch (Exception e)
            {
                str_error_log = $"CheckFactoryTestMode发生异常：[{e.Message}]";
                return false;
            }
        }

        /// <summary>
        /// 检查产测模式文件是否存在
        /// </summary>
        /// <param name="str_error_log"></param>
        /// <returns></returns>
        public bool CheckFactoryTestFile(ref string str_error_log)
        {
            try
            {
                string CMD_LS_FILE = "ls /mnt/diskc/factory_test";
                string str_ret_value = "";
                if (SendCMDToXDC01(CMD_LS_FILE, 5000, true, ref str_ret_value, ENDFLAG_2) == false)
                {
                    str_error_log = $"发送检查产测模式文件指令[{CMD_LS_FILE}]失败";
                    return false;
                }

                if (str_ret_value.Contains("No such file or directory"))
                {
                    str_error_log = "未找到文件/mnt/diskc/factory_test";
                    return false;
                }
                return true;
            }
            catch (Exception ee)
            {
                str_error_log = $"CheckFactoryTestFile发生异常：[{ee.Message}]";
                return false;
            }
        }

        /// <summary>
        /// 恢复出厂模式
        /// </summary>
        /// <param name="str_error_log"></param>
        /// <returns></returns>
        public bool ResetFactory(ref string str_error_log)
        {
            try
            {
                string CMD_RESET_FACTORY = "FactoryReset.sh";
                string str_ret_value = "";
                if (SendCMDToXDC01(CMD_RESET_FACTORY, 5000, true, ref str_ret_value, ENDFLAG_1) == false)
                {
                    str_error_log = $"发送恢复出厂设置指令[{CMD_RESET_FACTORY}]失败";
                    return false;
                }
                Delay(5000);
                string CMD_CHECK_FACTORY = "ls /mnt/diskc/apSwitch.sh";
                if (SendCMDToXDC01(CMD_CHECK_FACTORY, 5000, true, ref str_ret_value, ENDFLAG_2) == false)
                {
                    str_error_log = $"发送检查恢复出厂是否成功指令[{CMD_CHECK_FACTORY}]失败";
                    return false;
                }
                if (str_ret_value.Contains("/mnt/diskc/apSwitch.sh") && str_ret_value.Contains("No such file or directory"))
                {
                    // apSwitch.sh文件不存在，说明仍旧为产测模式
                    str_error_log = str_ret_value.Replace("\r", "").Replace("\n", "");
                    return false;
                }
                return true;
            }
            catch (Exception ee)
            {
                str_error_log = $"ResetFactory发生异常：[{ee.Message}]";
                return false;
            }
        }

        /// <summary>
        /// 设置按键LED灯的颜色
        /// </summary>
        /// <param name="str_error_log"></param>
        /// <returns></returns>
        public bool SetBtnLEDColor(string str_color, ref string str_error_log)
        {
            try
            {
                string CMD_SET_COLOR = $"ubus send \"d3_product_test\" '{{\"button-LED\": \"{str_color}\" }}'";
                string str_ret_value = "";
                if (SendCMDToXDC01(CMD_SET_COLOR, 5000, true, ref str_ret_value, ENDFLAG_1) == false)
                {
                    str_error_log = $"发送设置LED灯颜色指令[{CMD_SET_COLOR}]失败";
                    return false;
                }
                return true;
            }
            catch (Exception ee)
            {
                str_error_log = $"SetBtnLEDColor发生异常：[{ee.Message}]";
                return false;
            }
        }

        /// <summary>
        /// 设置PIR的状态
        /// </summary>
        /// <param name="str_error_log"></param>
        /// <returns></returns>
        public bool SetPIR(string str_state, ref string str_error_log)
        {
            try
            {
                string CMD_SET_PIR = $"ubus send \"d3_product_test\" '{{\"PIR\": \"{str_state}\" }}'";
                string str_ret_value = "";
                if (SendCMDToXDC01(CMD_SET_PIR, 5000, true, ref str_ret_value, ENDFLAG_2) == false)
                {
                    str_error_log = $"发送设置PIR指令[{CMD_SET_PIR}]失败";
                    return false;
                }
                return true;
            }
            catch (Exception ee)
            {
                str_error_log = $"SetPIR发生异常：[{ee.Message}]";
                return false;
            }
        }

        /// <summary>
        /// 播放wav音频文件
        /// </summary>
        /// <param name="str_index">可选1,2,3,4,5,8,9</param>
        /// <param name="str_error_log"></param>
        /// <returns></returns>
        public bool PlayWav(string str_index, ref string str_error_log)
        {
            try
            {
                string CMD_PLAY_WAV = $"omfWavPlayer_ -n /etc/ringtone/{str_index}.wav";
                string str_ret_value = "";
                if (SendCMDToXDC01(CMD_PLAY_WAV, 5000, true, ref str_ret_value, ENDFLAG_2) == false)
                {
                    str_error_log = $"发送播放wav音频文件指令[{CMD_PLAY_WAV}]失败";
                    return false;
                }
                return true;
            }
            catch (Exception ee)
            {
                str_error_log = $"PlayWav发生异常：[{ee.Message}]";
                return false;
            }
        }

        /// <summary>
        /// 打开麦克风
        /// </summary>
        /// <param name="str_error_log"></param>
        /// <returns></returns>
        public bool OpenMic(ref string str_error_log)
        {
            try
            {
                string CMD_OPEN_MIC = $"ubus send \"d3_product_test\" '{{\"OpenMic\":\"\"}}'";
                string str_ret_value = "";
                if (SendCMDToXDC01(CMD_OPEN_MIC, 5000, true, ref str_ret_value, ENDFLAG_1) == false)
                {
                    str_error_log = $"发送打开麦克风指令[{CMD_OPEN_MIC}]失败";
                    return false;
                }
                return true;
            }
            catch (Exception ee)
            {
                str_error_log = $"OpenMic发生异常：[{ee.Message}]";
                return false;
            }
        }

        /// <summary>
        /// 关闭麦克风
        /// </summary>
        /// <param name="str_error_log"></param>
        /// <returns></returns>
        public bool CloseMic(ref string str_error_log)
        {
            try
            {
                string CMD_CLOSE_MIC = $"ubus send \"d3_product_test\" '{{\"CloseMic\":\"\"}}'";
                string str_ret_value = "";
                if (SendCMDToXDC01(CMD_CLOSE_MIC, 5000, true, ref str_ret_value, ENDFLAG_1) == false)
                {
                    str_error_log = $"发送关闭麦克风指令[{CMD_CLOSE_MIC}]失败";
                    return false;
                }
                return true;
            }
            catch (Exception ee)
            {
                str_error_log = $"CloseMic发生异常：[{ee.Message}]";
                return false;
            }
        }

        /// <summary>
        /// 麦克风录音
        /// </summary>
        /// <param name="str_duration"></param>
        /// <param name="str_error_log"></param>
        /// <returns></returns>
        public bool RecordMic(string str_duration, ref string str_error_log)
        {
            try
            {
                string CMD_RECORD_MIC = $"omfWavRecord_ -n /tmp/audio_cap.wav -d{str_duration}";
                string str_ret_value = "";
                if (SendCMDToXDC01(CMD_RECORD_MIC, 5000, true, ref str_ret_value, ENDFLAG_1) == false)
                {
                    str_error_log = $"发送录音指令[{CMD_RECORD_MIC}]失败";
                    return false;
                }
                return true;
            }
            catch (Exception ee)
            {
                str_error_log = $"RecordMic发生异常：[{ee.Message}]";
                return false;
            }
        }

        /// <summary>
        /// 测试录音文件
        /// </summary>
        /// <param name="str_error_log"></param>
        /// <returns></returns>
        public bool TestRecordWav(ref string str_max_abs, ref string str_delta, ref string str_result, ref string str_error_log)
        {
            try
            {
                string CMD_TEST_RECORD = $"mic_test-openwrt.arm /tmp/audio_cap.wav";
                string str_ret_value = "";
                if (SendCMDToXDC01(CMD_TEST_RECORD, 30000, true, ref str_ret_value, "{\"max_abs\":") == false)
                {
                    str_error_log = $"发送测试录音文件指令[{CMD_TEST_RECORD}]失败";
                    return false;
                }

                string str_temp_mic = "";
                string[] striparr = str_ret_value.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0; i < striparr.Length; i++)
                {
                    if (striparr[i].ToLower().Contains("max_abs"))
                    {
                        str_temp_mic = striparr[i];
                        break;
                    }
                }
                str_temp_mic = str_temp_mic.Replace("\"", "").Replace("{", "").Replace("}", "");
                string[] striparr_1 = str_temp_mic.Split(new string[] { "," }, StringSplitOptions.None);

                str_max_abs = striparr_1[0].Replace("max_abs:", "").Trim();
                str_delta = striparr_1[1].Replace("delta:", "").Trim();
                str_result = striparr_1[2].Replace("result:", "").Trim();
                if (check_int_double(str_max_abs) == false || check_int_double(str_delta) == false)
                {
                    str_error_log = $"max_abs[{str_max_abs}]或delta[{str_delta}]格式错误";
                    return false;
                }
                return true;
            }
            catch (Exception ee)
            {
                str_error_log = $"TestRecordWav发生异常：[{ee.Message}]";
                return false;
            }
        }

        /// <summary>
        /// 播放录音文件
        /// </summary>
        /// <param name="str_error_log"></param>
        /// <returns></returns>
        public bool PlayRecordWav(ref string str_error_log)
        {
            try
            {
                string CMD_PLAY_RECORD_WAV = $"omfWavPlayer_ -n /tmp/audio_cap.wav";
                string str_ret_value = "";
                if (SendCMDToXDC01(CMD_PLAY_RECORD_WAV, 5000, true, ref str_ret_value, ENDFLAG_2) == false)
                {
                    str_error_log = $"发送播放录音wav文件指令[{CMD_PLAY_RECORD_WAV}]失败";
                    return false;
                }
                return true;
            }
            catch (Exception ee)
            {
                str_error_log = $"PlayRecordWav发生异常：[{ee.Message}]";
                return false;
            }
        }

        /// <summary>
        /// 测试wifi上行吞吐量（速率和丢包率）
        /// </summary>
        /// <param name="str_rate">速率</param>
        /// <param name="str_loss">丢包率</param>
        /// <param name="str_error_log"></param>
        /// <param name="str_ip">服务器IP</param>
        /// <param name="str_duration">测试持续时间</param>
        /// <param name="str_bandwidth">测试带宽</param>
        /// <returns></returns>
        public bool TestWifiUpThroughput(ref string str_rate, ref string str_loss, ref string str_error_log, string str_ip, string str_duration = "10", string str_bandwidth = "100M")
        {
            try
            {
                // -i指定每隔多少秒生成一次性能报告
                string CMD_IPERF3_UP = $"iperf3 -c {str_ip} -u -i1 -t{str_duration} -b {str_bandwidth}";
                string str_ret_value = "";
                if (SendCMDToXDC01(CMD_IPERF3_UP, 38000, true, ref str_ret_value, ENDFLAG_2) == false)
                {
                    str_error_log = $"发送iperf3上行吞吐量测试指令[{CMD_IPERF3_UP}]失败";
                    return false;
                }
                if (str_ret_value.Contains("error"))
                {
                    str_error_log = $"指令发送异常:[{str_ret_value}]";
                    return false;
                }
                //[ID]    Interval          Transfer       Bitrate         Jitter    Lost/Total        Datagrams
                //[  5]   0.00-10.00  sec   34.9 MBytes    29.3 Mbits/sec  0.257 ms  1/25071 (0.004%)  receiver
                string[] striparr = str_ret_value.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                string str_upload_temp = "";
                for (int i = 0; i < striparr.Length; i++)
                {
                    if (striparr[i].Contains("receiver"))
                    {
                        str_upload_temp = striparr[i];
                        break;
                    }
                }
                string[] result = System.Text.RegularExpressions.Regex.Split(str_upload_temp, @"\s{2,}");

                // 速率 MBytes/sec
                str_rate = result[5];
                // 丢包率
                str_loss = result[7].Substring(result[7].IndexOf("(") + 1).Replace(")", "");

                str_rate = str_rate.Split()[0].Trim();
                //if(check_int_double(str_rate) == false)
                //{
                //    str_error = "up_rate";
                //    return false;
                //}
                //else
                //{
                //    // MBytes换算成MBits 1MBytes = 8MBits
                //    str_rate = (double.Parse(str_rate) * 8.0).ToString();
                //}

                str_loss = str_loss.Replace("%", "").Trim();

                if (check_int_double(str_rate) == false || check_int_double(str_loss) == false)
                {
                    str_error_log = $"rate[{str_rate}]或loss[{str_loss}]格式错误";
                    return false;
                }

                return true;
            }
            catch (Exception ee)
            {
                str_error_log = $"TestWifiUpThroughput发生异常：[{ee.Message}]";
                return false;
            }
        }

        /// <summary>
        /// 测试wifi下行吞吐量（速率和丢包率）
        /// </summary>
        /// <param name="str_rate">速率</param>
        /// <param name="str_loss">丢包率</param>
        /// <param name="str_error_log"></param>
        /// <param name="str_ip">服务器IP</param>
        /// <param name="str_duration">测试持续时间</param>
        /// <param name="str_bandwidth">测试带宽</param>
        /// <returns></returns>
        public bool TestWifiDownThroughput(ref string str_rate, ref string str_loss, ref string str_error_log, string str_ip, string str_duration = "10", string str_bandwidth = "100M")
        {
            try
            {
                // -i指定每隔多少秒生成一次性能报告
                string CMD_IPERF3_DOWN = $"iperf3 -c {str_ip} -u -i1 -t{str_duration} -b {str_bandwidth} -R";
                string str_ret_value = "";
                if (SendCMDToXDC01(CMD_IPERF3_DOWN, 38000, true, ref str_ret_value, ENDFLAG_2) == false)
                {
                    str_error_log = $"发送iperf3下行吞吐量测试指令[{CMD_IPERF3_DOWN}]失败";
                    return false;
                }
                if (str_ret_value.Contains("error"))
                {
                    str_error_log = $"指令发送异常:[{str_ret_value}]";
                    return false;
                }
                //[ID]    Interval          Transfer       Bitrate         Jitter    Lost/Total        Datagrams
                //[  5]   0.00-10.00  sec   34.9 MBytes    29.3 Mbits/sec  0.257 ms  1/25071 (0.004%)  receiver
                string[] striparr = str_ret_value.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                string str_upload_temp = "";
                for (int i = 0; i < striparr.Length; i++)
                {
                    if (striparr[i].Contains("receiver"))
                    {
                        str_upload_temp = striparr[i];
                        break;
                    }
                }
                string[] result = System.Text.RegularExpressions.Regex.Split(str_upload_temp, @"\s{2,}");

                // 速率 MBytes/sec
                str_rate = result[5];
                // 丢包率
                str_loss = result[7].Substring(result[7].IndexOf("(") + 1).Replace(")", "");

                str_rate = str_rate.Split()[0].Trim();
                //if(check_int_double(str_rate) == false)
                //{
                //    str_error = "up_rate";
                //    return false;
                //}
                //else
                //{
                //    // MBytes换算成MBits 1MBytes = 8MBits
                //    str_rate = (double.Parse(str_rate) * 8.0).ToString();
                //}

                str_loss = str_loss.Replace("%", "").Trim();

                if (check_int_double(str_rate) == false || check_int_double(str_loss) == false)
                {
                    str_error_log = $"rate[{str_rate}]或loss[{str_loss}]格式错误";
                    return false;
                }

                return true;
            }
            catch (Exception ee)
            {
                str_error_log = $"TestWifiDownThroughput发生异常：[{ee.Message}]";
                return false;
            }
        }

        public bool TestPingDelay(ref string str_rtt, string str_ip, ref string str_error_log, int int_count = 10)
        {
            try
            {
                string CMD_PING = $"ping -c {int_count} {str_ip}";
                string str_ret_value = "";
                if (SendCMDToXDC01(CMD_PING, int_count*1000+1000, true, ref str_ret_value, ENDFLAG_2) == false)
                {
                    str_error_log = $"发送ping指令[{CMD_PING}]失败";
                    return false;
                }
                /*
                 --- 127.0.0.1 ping statistics ---
                10 packets transmitted, 10 packets received, 0% packet loss
                round-trip min/avg/max = 0.236/0.252/0.297 ms
                 */
                string[] striparr = str_ret_value.Split(new string[1] { "\r\n" }, StringSplitOptions.None);
                string str_rtt_temp = "";
                for (int i = 0; i < striparr.Length; i++)
                {
                    if (striparr[i].Contains("round-trip"))
                    {
                        str_rtt_temp = striparr[i];
                        break;
                    }
                }
                str_rtt_temp = str_rtt_temp.Replace("round-trip min/avg/max = ", "");

                int startIndex = str_rtt_temp.IndexOf('/') + 1; // 加1是为了跳过第一个斜杠
                int endIndex = str_rtt_temp.IndexOf('/', startIndex);

                if (startIndex >= 0 && endIndex > startIndex)
                {
                    str_rtt = str_rtt_temp.Substring(startIndex, endIndex - startIndex);
                    return true;
                }
                else
                {
                    str_error_log = $"解析[{str_rtt_temp}]错误";
                    return false;
                }
            }
            catch (Exception ee)
            {
                str_error_log = $"TestPingDelay发生异常：[{ee.Message}]";
                return false;
            }
        }

        /// <summary>
        /// 打开RTSP流
        /// </summary>
        /// <param name="str_error_log"></param>
        /// <returns></returns>
        public bool OpenRTSP(ref string str_error_log)
        {
            try
            {
                string CMD_OPEN_RTSP = $"omfRtspServer_ &";
                string str_ret_value = "";
                if (SendCMDToXDC01(CMD_OPEN_RTSP, 5000, true, ref str_ret_value, "RtspServer()") == false)
                {
                    str_error_log = $"发送打开RTSP流指令[{CMD_OPEN_RTSP}]失败";
                    return false;
                }
                return true;
            }
            catch (Exception ee)
            {
                str_error_log = $"OpenRTSP发生异常：[{ee.Message}]";
                return false;
            }
        }

        /// <summary>
        /// 切换IR_CUT
        /// </summary>
        /// <param name="str_state"></param>
        /// <param name="str_error_log"></param>
        /// <returns></returns>
        public bool SwitchIR_CUT(string str_state, ref string str_error_log)
        {
            try
            {
                if (str_state == IR_CUT_ON || str_state == IR_CUT_OFF)
                {
                    string CMD_SWITCH_IRCUT = $"ubus send \"d3_product_test\" '{{\"IR-CUT\": \"{str_state}\" }}'";
                    string str_ret_value = "";
                    if (SendCMDToXDC01(CMD_SWITCH_IRCUT, 5000, true, ref str_ret_value, ENDFLAG_1) == false)
                    {
                        str_error_log = $"发送切换IR CUT指令[{CMD_SWITCH_IRCUT}]失败";
                        return false;
                    }
                    return true;
                }
                else
                {
                    str_error_log = $"参数str_state{str_state}错误,仅支持{IR_CUT_ON}和{IR_CUT_OFF}";
                    return false;
                }
            }
            catch (Exception ee)
            {
                str_error_log = $"SwitchIR_CUT发生异常：[{ee.Message}]";
                return false;
            }
        }

        /// <summary>
        /// 切换IR_LED
        /// </summary>
        /// <param name="str_state"></param>
        /// <param name="str_error_log"></param>
        /// <returns></returns>
        public bool SwitchIR_LED(string str_state, ref string str_error_log)
        {
            try
            {
                string CMD_SWITCH_IRLED = $"ubus send \"d3_product_test\" '{{\"IR-LED\": \"{str_state}\" }}'";
                string str_ret_value = "";
                if (SendCMDToXDC01(CMD_SWITCH_IRLED, 5000, true, ref str_ret_value, ENDFLAG_1) == false)
                {
                    str_error_log = $"发送切换IR LED指令[{CMD_SWITCH_IRLED}]失败";
                    return false;
                }
                return true;
            }
            catch (Exception ee)
            {
                str_error_log = $"SwitchIR_LED发生异常：[{ee.Message}]";
                return false;
            }
        }

        /// <summary>
        /// PCBA指令发送RF信号
        /// PCBA-RF433测试 接收端播放音频1，需要有接收设备, 
        /// </summary>
        /// <param name="str_index">参数可填1/2/3/4</param>
        /// <param name="str_error_log"></param>
        /// <returns></returns>
        public bool PCBA_RFTX(string str_index, ref string str_error_log)
        {
            try
            {
                string CMD_PCBA_RFTEST = $"ubus send \"d3_product_test\" '{{\"PCBA-RFTEST\":{str_index}}}'";
                string str_ret_value = "";
                if (SendCMDToXDC01(CMD_PCBA_RFTEST, 5000, true, ref str_ret_value, ENDFLAG_1) == false)
                {
                    str_error_log = $"发送PCBA RF TX指令[{CMD_PCBA_RFTEST}]失败";
                    return false;
                }
                return true;
            }
            catch (Exception ee)
            {
                str_error_log = $"PCBA_RFTX发生异常：[{ee.Message}]";
                return false;
            }
        }

        /// <summary>
        /// RF433性能测试
        /// </summary>
        /// <param name="str_state">RF_TX: RF发送; RF_RX: RF接收</param>
        /// <param name="str_error_log"></param>
        /// <returns></returns>
        public bool SetRFMode(string str_state, ref string str_error_log)
        {
            try
            {
                string CMD_RF_MODE = $"ubus send \"d3_product_test\" '{{\"RF-Test\": \"{str_state}\" }}'";
                string str_ret_value = "";
                string endFlag = ENDFLAG_1;
                if (str_state == RF_TX)
                {
                    endFlag = "[RF-Send-test:send]";
                }
                else if (str_state == RF_RX)
                {
                    endFlag = "[RF-Send-test:rcv]";
                }
                if (SendCMDToXDC01(CMD_RF_MODE, 5000, true, ref str_ret_value, endFlag) == false)
                {
                    str_error_log = $"发送设置RF测试模式指令[{CMD_RF_MODE}]失败";
                    return false;
                }
                return true;
            }
            catch (Exception ee)
            {
                str_error_log = $"SetRFMode发生异常：[{ee.Message}]";
                return false;
            }
        }

        /// <summary>
        /// 获取SN号
        /// </summary>
        /// <param name="str_sn"></param>
        /// <param name="str_error_log"></param>
        /// <returns></returns>
        public bool GetSN(ref string str_sn, ref string str_error_log)
        {
            try
            {
                string CMD_GET_SN = "cat /mnt/diskc/camera_setting.json | jsonfilter -e \"@.reg_data.cameraSn\"";
                string str_ret_value = "";
                if (SendCMDToXDC01(CMD_GET_SN, 5000, true, ref str_ret_value, ENDFLAG_2) == false)
                {
                    str_error_log = $"发送获取SN号信息指令[{CMD_GET_SN}]失败";
                    return false;
                }
                string[] striparr = str_ret_value.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                string str_temp_value = "";
                Regex regex = new Regex(@"\d{16}");
                for (int i = 0; i < striparr.Length; i++)
                {
                    if (regex.IsMatch(striparr[i]))
                    {
                        str_temp_value = striparr[i];
                        break;
                    }
                }
                str_sn = str_temp_value.Trim();
                return true;
            }
            catch (Exception ee)
            {
                str_error_log = $"GetSN发生异常：[{ee.Message}]";
                return false;
            }
        }

        /// <summary>
        /// 获取UID
        /// </summary>
        /// <param name="str_uid"></param>
        /// <param name="str_error_log"></param>
        /// <returns></returns>
        public bool GetUID(ref string str_uid, ref string str_error_log)
        {
            try
            {
                string CMD_GET_UID = "cat /mnt/diskc/camera_setting.json | jsonfilter -e \"@.reg_data.cameraUid\"";
                string str_ret_value = "";
                if (SendCMDToXDC01(CMD_GET_UID, 5000, true, ref str_ret_value, ENDFLAG_2) == false)
                {
                    str_error_log = $"发送获取UID信息指令[{CMD_GET_UID}]失败";
                    return false;
                }
                string[] striparr = str_ret_value.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                string str_temp_value = "";
                Regex regex = new Regex(@"[A-Z0-9]{17}");
                for (int i = 0; i < striparr.Length; i++)
                {
                    if (regex.IsMatch(striparr[i]))
                    {
                        str_temp_value = striparr[i];
                        break;
                    }
                }
                str_uid = str_temp_value.Trim();
                return true;
            }
            catch (Exception ee)
            {
                str_error_log = $"GetUID发生异常：[{ee.Message}]";
                return false;
            }
        }

        /// <summary>
        /// 写入UID和SN
        /// </summary>
        /// <param name="str_uid"></param>
        /// <param name="str_sn"></param>
        /// <param name="str_error_log"></param>
        /// <returns></returns>
        public bool SetUIDandSN(string str_uid, string str_sn, ref string str_error_log)
        {
            try
            {
                string CMD_SET_SN_UID = $"burn_UID_SN.sh {str_uid} {str_sn}";
                string str_ret_value = "";
                if (SendCMDToXDC01(CMD_SET_SN_UID, 5000, true, ref str_ret_value, "{\"reg_data\":{") == false)
                {
                    str_error_log = $"发送写入UID和SN指令[{CMD_SET_SN_UID}]失败";
                    return false;
                }
                Delay(3000);
                string str_cur_sn = "";
                GetSN(ref str_cur_sn, ref str_error_log);
                if (str_cur_sn == str_sn)
                {
                    Delay(200);
                    string str_cur_uid = "";
                    GetUID(ref str_cur_uid, ref str_error_log);
                    if (str_cur_uid == str_uid)
                    {
                        return true;
                    }
                    else
                    {
                        str_error_log = $"写入UID[{str_uid}]后，读取到SN[{str_cur_uid}],写入未生效";
                        return false;
                    }
                }
                else
                {
                    str_error_log = $"写入SN[{str_sn}]后，读取到SN[{str_cur_sn}],写入未生效";
                    return false;
                }
            }
            catch (Exception ee)
            {
                str_error_log = $"SetUIDandSN发生异常：[{ee.Message}]";
                return false;
            }
        }

        /// <summary>
        /// 设置设备进入充电模式
        /// </summary>
        /// <param name="str_battery_level"></param>
        /// <param name="str_error_log"></param>
        /// <returns></returns>
        public bool SetChargeMode(string str_battery_level, ref string str_error_log)
        {
            try
            {
                string CMD_SET_CHARGE_MODE = $"ubus send \"d3_product_test\" '{{\"CHARGE-MODE-TEST\": {str_battery_level} }}'";
                string str_ret_value = "";
                if (SendCMDToXDC01(CMD_SET_CHARGE_MODE, 5000, true, ref str_ret_value, $"Enter Charge Test --> Target = {str_battery_level}") == false)
                {
                    str_error_log = $"发送设置充电模式指令[{CMD_SET_CHARGE_MODE}]失败";
                    return false;
                }
                return true;
            }
            catch (Exception ee)
            {
                str_error_log = $"SetChargeMode发生异常：[{ee.Message}]";
                return false;
            }
        }

        /// <summary>
        /// 控制设备开始或停止充电
        /// </summary>
        /// <param name="str_state">CHARGE_ENABLE: 开始充电; CHARGE_DISABLE: 停止充电</param>
        /// <param name="str_error_log"></param>
        /// <returns></returns>
        public bool SetChargeIC(string str_state, ref string str_error_log)
        {
            try
            {
                string CMD_CHARGE_IC = $"ubus send \"d3_product_test\" '{{\"CHARGE-IC\": \"{str_state}\"}}'";
                string str_ret_value = "";
                string endFlag = ENDFLAG_2;
                if (str_state == CHARGE_ENABLE)
                {
                    endFlag = "[ChargeIC Enable]";
                }
                else if (str_state == CHARGE_DISABLE)
                {
                    endFlag = "[ChargeIC Disable]";
                }
                if (SendCMDToXDC01(CMD_CHARGE_IC, 5000, true, ref str_ret_value, endFlag) == false)
                {
                    str_error_log = $"发送设置IC充电指令[{CMD_CHARGE_IC}]失败";
                    return false;
                }
                return true;
            }
            catch (Exception ee)
            {
                str_error_log = $"SetChargeIC发生异常：[{ee.Message}]";
                return false;
            }
        }

        #endregion

        #region RTOS指令
        /// <summary>
        /// RTOS指令：检查开机情况
        /// </summary>
        /// <param name="str_error_log"></param>
        /// <returns></returns>
        public bool CheckPowerStatusRTOS(ref string str_error_log)
        {
            try
            {
                string CMD_CHECK_POWER_STATUS_RTOS = $"factorytest powerstatus";
                string str_ret_value = "";
                if (SendCMDToXDC01(CMD_CHECK_POWER_STATUS_RTOS, 5000, true, ref str_ret_value, $"power start the normal") == false)
                {
                    str_error_log = $"RTOS发送查询上电情况指令[{CMD_CHECK_POWER_STATUS_RTOS}]失败";
                    return false;
                }
                return true;
            }
            catch (Exception ee)
            {
                str_error_log = $"CheckPowerStatusRTOS发生异常：[{ee.Message}]";
                return false;
            }
        }

        /// <summary>
        /// RTOS指令：读取RN号
        /// </summary>
        /// <param name="str_rn"></param>
        /// <param name="str_error_log"></param>
        /// <returns></returns>
        public bool GetRnRTOS(ref string str_rn, ref string str_error_log)
        {
            try
            {
                string CMD_GET_RN_RTOS = $"factorytest ReadRn";
                string str_ret_value = "";
                if (SendCMDToXDC01(CMD_GET_RN_RTOS, 5000, true, ref str_ret_value, $"cmd>") == false)
                {
                    str_error_log = $"RTOS发送读取Rn号指令[{CMD_GET_RN_RTOS}]失败";
                    return false;
                }
                if (!str_ret_value.Contains("read Rn ="))
                {
                    return false;
                }
                string[] striparr = str_ret_value.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                string str_rn_temp = "";
                for (int i = 0; i < striparr.Length; i++)
                {
                    Application.DoEvents();
                    if (striparr[i].Contains("read Rn ="))
                    {
                        str_rn_temp = striparr[i].Replace("\r\n", "").Replace(">cmd", "").TrimEnd();
                    }
                }
                str_rn = str_rn_temp.Replace("read Rn =", "").Trim();
                if (str_rn.ToLower().Contains("error"))
                {
                    str_error_log = "异常:" + str_rn;
                    return false;
                }
                return true;
            }
            catch (Exception ee)
            {
                str_error_log = $"GetRnRTO发生异常：[{ee.Message}]";
                return false;
            }
        }

        /// <summary>
        /// RTOS指令：读取工序号
        /// </summary>
        /// <param name="str_tagnumber"></param>
        /// <param name="str_error_log"></param>
        /// <returns></returns>
        public bool GetTagNumberRTOS(ref string str_tagnumber, ref string str_error_log)
        {
            try
            {
                string CMD_GET_TAGNUMBER_RTOS = $"factorytest read";
                string str_ret_value = "";
                if (SendCMDToXDC01(CMD_GET_TAGNUMBER_RTOS, 5000, true, ref str_ret_value, $"cmd>") == false)
                {
                    str_error_log = $"RTOS发送读取工序号指令[{CMD_GET_TAGNUMBER_RTOS}]失败";
                    return false;
                }
                if (!str_ret_value.Contains("read test_station"))
                {
                    return false;
                }
                string[] striparr = str_ret_value.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                string str_tagnumber_temp = "";
                for (int i = 0; i < striparr.Length; i++)
                {
                    Application.DoEvents();
                    if (striparr[i].Contains("read test_station"))
                    {
                        str_tagnumber_temp = striparr[i].Replace("\r\n", "").Replace(">cmd", "").Replace("#", "").TrimEnd();
                        break;
                    }
                }
                str_tagnumber = str_tagnumber_temp.Substring(str_tagnumber_temp.Length - 4, 4);
                return true;
            }
            catch (Exception ee)
            {
                str_error_log = $"GetTagNumberRTOS发生异常：[{ee.Message}]";
                return false;
            }
        }

        /// <summary>
        /// RTOS指令：写入工序号
        /// </summary>
        /// <param name="str_tagNumber"></param>
        /// <param name="str_error_log"></param>
        /// <returns></returns>
        public bool SetTagNumberRTOS(string str_tagNumber, ref string str_error_log)
        {
            try
            {
                string CMD_SET_TAGNUMBER_RTOS = $"factorytest write {str_tagNumber}";
                string str_ret_value = "";
                if (SendCMDToXDC01(CMD_SET_TAGNUMBER_RTOS, 5000, true, ref str_ret_value, "cmd>") == false)
                {
                    str_error_log = $"RTOS发送写入工序号信息指令[{CMD_SET_TAGNUMBER_RTOS}]失败";
                    return false;
                }
                // 再次读取
                string str_cur_tagNumber = "";
                GetTagNumberRTOS(ref str_cur_tagNumber, ref str_error_log);
                if (str_cur_tagNumber == str_tagNumber)
                {
                    return true;
                }
                else
                {
                    str_error_log = $"写入工序号[{str_tagNumber}]后，读取到工序号[{str_cur_tagNumber}],写入未生效";
                    return false;
                }
            }
            catch (Exception ee)
            {
                str_error_log = $"SetTagNumberRTOS发生异常：[{ee.Message}]";
                return false;
            }
        }

        /// <summary>
        /// RTOS指令：打开USB摄像头
        /// </summary>
        /// <param name="str_error_log"></param>
        /// <returns></returns>
        public bool OpenCameraRTOS(ref string str_error_log)
        {
            try
            {
                string CMD_OPEN_CAMERA = $"usbon 1";
                string str_ret_value = "";
                if (SendCMDToXDC01(CMD_OPEN_CAMERA, 15000, true, ref str_ret_value, $"cmd") == false)
                {
                    str_error_log = $"RTOS发送打开摄像头指令[{CMD_OPEN_CAMERA}]失败";
                    return false;
                }
                return true;
            }
            catch (Exception ee)
            {
                str_error_log = $"OpenCameraRTOS发生异常：[{ee.Message}]";
                return false;
            }
        }

        /// <summary>
        /// RTOS指令：切换夜视模式
        /// </summary>
        /// <param name="str_state">可选参数，on: 表示打开； off: 表示关闭</param>
        /// <param name="str_error_log"></param>
        /// <returns></returns>
        public bool SwitchIR_CUT_RTOS(string str_state, ref string str_error_log)
        {
            try
            {
                //string str_check_1 = "";
                string str_check_2 = "";
                string CMD_SWITCH_IRCUT_RTOS = "";
                if (str_state == IR_CUT_ON)
                {
                    //str_check_1 = "rcv system statu = 1";
                    str_check_2 = "open led lv2";
                    CMD_SWITCH_IRCUT_RTOS = $"xdc02_cmd day_night 1";
                }
                else if (str_state == IR_CUT_OFF)
                {
                    //str_check_1 = "rcv system statu = 2";
                    str_check_2 = "close led";
                    CMD_SWITCH_IRCUT_RTOS = $"xdc02_cmd day_night 0";
                }
                else
                {
                    str_error_log = $"参数str_state{str_state}错误,仅支持{IR_CUT_ON}和{IR_CUT_OFF}";
                    return false;
                }
                string str_ret_value = "";
                if (SendCMDToXDC01(CMD_SWITCH_IRCUT_RTOS, 5000, true, ref str_ret_value, "Digital Effect") == false)
                {
                    str_error_log = $"RTOS发送切换IR CUT指令[{CMD_SWITCH_IRCUT_RTOS}]失败";
                    return false;
                }

                if (/*!str_ret_value.Contains(str_check_1) || */!str_ret_value.Contains(str_check_2))
                {
                    str_error_log = $"摄像头{str_state}夜视模式失败";
                    return false;
                }
                return true;
            }
            catch (Exception ee)
            {
                str_error_log = $"SwitchIR_CUT_RTOS发生异常：[{ee.Message}]";
                return false;
            }
        }

        #endregion
    }
}
