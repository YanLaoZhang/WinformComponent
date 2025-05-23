﻿using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace commonLib
{
    public class CameraInfo
    {
        public string FwVersion { get; set; } = "";
        public string RtosVersion { get; set; }="";
        public string McuBootloaderV { get; set; }="";
        public string McuAppV { get; set; }="";
        public string HwVersion { get; set; }="";
        public string Battery { get; set; }="";
        public string BatteryPerCent { get; set; }="";
        public string Temp { get; set; }="";
        public string McuRfType { get; set; }="";
        public string Backend { get; set; }="";
        public string WifiVer { get; set; }="";
    }

    public class XDC03Serial
    {
        [DllImport("user32.dll")]
        private static extern int MessageBoxTimeoutA(IntPtr hWnd, string msg, string caption, int type, int id, int time);

        System.IO.Ports.SerialPort _serialPort;
        string _portName;
        System.Windows.Forms.RichTextBox _richTextBox;
        string str_Receive_skybell = "";

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

        public XDC03Serial(System.IO.Ports.SerialPort serialPort, string portName, System.Windows.Forms.RichTextBox richTextBox)
        {
            _serialPort = serialPort;
            _portName = portName;
            _richTextBox = richTextBox;
            // 绑定 DataReceived 事件处理程序
            serialPort.DataReceived += SerialPort_DataReceived;
        }

        ~XDC03Serial()
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
                str_Receive_skybell += indata;
                _richTextBox.Text += indata;
                _richTextBox.SelectionStart = _richTextBox.TextLength;
                _richTextBox.ScrollToCaret();
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

        /// <summary>
        /// 打开串口
        /// </summary>
        /// <returns></returns>
        public bool OpenPort()
        {
            try
            {
                _serialPort.PortName = _portName;
                _serialPort.BaudRate = 9600;
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

        // 关闭串口
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
        public bool SendCMDToXDC03(string str_send_cmd, int t, Boolean Respone, ref string str_receive_data, string endFlag=ENDFLAG_1)
        {
            try
            {
                if(_serialPort.IsOpen == false)
                {
                    _serialPort.Open();
                }
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
                if (SendCMDToXDC03(CMD_SYS_INFO, 2000, true, ref str_ret_value, ENDFLAG_1) == false)
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
                    }
                    if (str_temp.Contains("SystemParam.RtosVersion ="))
                    {
                        int int_deng = str_temp.IndexOf("=");
                        cameraInfo.RtosVersion = str_temp.Substring(int_deng, str_temp.Length - int_deng).Replace("]", "").Replace("=", "").Trim();
                    }
                    if (str_temp.Contains("SystemParam.McuBootloaderV ="))
                    {
                        int int_deng = str_temp.IndexOf("=");
                        cameraInfo.McuBootloaderV = str_temp.Substring(int_deng, str_temp.Length - int_deng).Replace("]", "").Replace("=", "").Trim();
                    }
                    if (str_temp.Contains("SystemParam.McuAppV ="))
                    {
                        int int_deng = str_temp.IndexOf("=");
                        cameraInfo.McuAppV = str_temp.Substring(int_deng, str_temp.Length - int_deng).Replace("]", "").Replace("=", "").Trim();
                    }
                    if (str_temp.Contains("SystemParam.HwVersion ="))
                    {
                        int int_deng = str_temp.IndexOf("=");
                        cameraInfo.HwVersion = str_temp.Substring(int_deng, str_temp.Length - int_deng).Replace("]", "").Replace("=", "").Trim();
                    }
                    if (str_temp.Contains("SystemParam.Battery  ="))
                    {
                        int int_deng = str_temp.IndexOf("=");
                        cameraInfo.Battery = str_temp.Substring(int_deng, str_temp.Length - int_deng).Replace("]", "").Replace("=", "").Trim();
                    }
                    if (str_temp.Contains("SystemParam.BatteryPerCent  ="))
                    {
                        int int_deng = str_temp.IndexOf("=");
                        cameraInfo.BatteryPerCent = str_temp.Substring(int_deng, str_temp.Length - int_deng).Replace("]", "").Replace("=", "").Trim();
                    }
                    if (str_temp.Contains("SystemParam.Temp  ="))
                    {
                        int int_deng = str_temp.IndexOf("=");
                        cameraInfo.Temp = str_temp.Substring(int_deng, str_temp.Length - int_deng).Replace("]", "").Replace("=", "").Trim();
                    }
                    if (str_temp.Contains("SystemParam.McuRfType ="))
                    {
                        int int_deng = str_temp.IndexOf("=");
                        cameraInfo.McuRfType = str_temp.Substring(int_deng, str_temp.Length - int_deng).Replace("]", "").Replace("=", "").Trim();
                    }
                    if (str_temp.Contains("SystemParam.Backend ="))
                    {
                        int int_deng = str_temp.IndexOf("=");
                        cameraInfo.Backend = str_temp.Substring(int_deng, str_temp.Length - int_deng).Replace("]", "").Replace("=", "").Trim();
                    }
                    if (str_temp.Contains("SystemParam.WifiVer ="))
                    {
                        int int_deng = str_temp.IndexOf("=");
                        cameraInfo.WifiVer = str_temp.Substring(int_deng, str_temp.Length - int_deng).Replace("]", "").Replace("=", "").Trim();
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
                string CMD_SYS_MAC = "ubus send d3_sys_setting  '{\"GetMacAddr\":1}'";
                string str_ret_value = "";
                if (SendCMDToXDC03(CMD_SYS_MAC, 2000, true, ref str_ret_value, ENDFLAG_1) == false)
                {
                    str_error_log = $"发送获取MAC信息指令[{CMD_SYS_MAC}]失败";
                    return false;
                }
                string[] striparr = str_ret_value.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                string str_mac_temp = "";
                for (int i = 0; i < striparr.Length; i++)
                {
                    if (striparr[i].Contains("Get Mac Addr ="))
                    {
                        str_mac_temp = striparr[i];
                        break;
                    }
                }
                int int_deng = str_mac_temp.IndexOf("=");
                str_mac = str_mac_temp.Substring(int_deng, str_mac_temp.Length - int_deng).Replace("]", "").Replace("=", "").Trim();
                return true;
            }
            catch (Exception ee)
            {
                str_error_log = $"GetSystemMac发生异常：[{ee.Message}]";
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
                string CMD_SYS_IP = "ifconfig";
                string str_ret_value = "";
                if (SendCMDToXDC03(CMD_SYS_IP, 2000, true, ref str_ret_value, ENDFLAG_2) == false)
                {
                    str_error_log = $"发送获取IP信息指令[{CMD_SYS_IP}]失败";
                    return false;
                }
                if (!str_ret_value.Contains("inet addr:") && str_ret_value.Contains("Bcast:"))
                {
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
                if (SendCMDToXDC03(CMD_LIGHT_SENSOR, 2000, true, ref str_ret_value, ENDFLAG_1) == false)
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
                if (SendCMDToXDC03(CMD_GET_TAGNUMBER, 2000, true, ref str_ret_value, ENDFLAG_1) == false)
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
                if (SendCMDToXDC03(CMD_SET_TAGNUMBER, 2000, true, ref str_ret_value, ENDFLAG_2) == false)
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
                string CMD_GET_RN = "rn=`cat /mnt/diskc/factorytest_rn` && echo \"Rn=\"$rn";
                string str_ret_value = "";
                if (SendCMDToXDC03(CMD_GET_RN, 2000, true, ref str_ret_value, ENDFLAG_2) == false)
                {
                    str_error_log = $"发送获取RN号信息指令[{CMD_GET_RN}]失败";
                    return false;
                }
                if (!str_ret_value.Contains("Rn=XDC"))
                {
                    return false;
                }
                string[] striparr = str_ret_value.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                string str_rn_temp = "";
                for (int i = 0; i < striparr.Length; i++)
                {
                    if (striparr[i].Contains("Rn=XDC"))
                    {
                        str_rn_temp = striparr[i];
                        break;
                    }
                }
                int start = str_rn_temp.IndexOf("XDC");
                str_rn_temp = str_rn_temp.Substring(start, 15);
                str_rn = str_rn_temp.Replace("Rn=", "").Trim();
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
                string CMD_SET_RN = $"echo {str_rn} > /mnt/diskc/factorytest_rn && ubus send d3_sys_setting  '{{\"sync_disk\":1}}'";
                string str_ret_value = "";
                if (SendCMDToXDC03(CMD_SET_RN, 2000, true, ref str_ret_value, ENDFLAG_2) == false)
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
                string CMD_GET_WIFI_SSID = "cat /mnt/diskb/UDF/DOORBELL.CFG | grep ESSID";
                string str_ret_value = "";
                if (SendCMDToXDC03(CMD_GET_WIFI_SSID, 2000, true, ref str_ret_value, ENDFLAG_2) == false)
                {
                    str_error_log = $"发送获取Wifi SSID信息指令[{CMD_GET_WIFI_SSID}]失败";
                    return false;
                }
                if (!str_ret_value.Contains("ESSID="))
                {
                    return false;
                }
                string[] striparr = str_ret_value.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                string str_wifi_ssid_temp = "";
                for (int i = 0; i < striparr.Length; i++)
                {
                    if (striparr[i].Contains("ESSID="))
                    {
                        str_wifi_ssid_temp = striparr[i];
                        break;
                    }
                }
                str_wifi_ssid = str_wifi_ssid_temp.Replace("ESSID=", "").Trim();
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
                string CMD_GET_WIFI_PASSWORD = "cat /mnt/diskb/UDF/DOORBELL.CFG | grep PWD";
                string str_ret_value = "";
                if (SendCMDToXDC03(CMD_GET_WIFI_PASSWORD, 2000, true, ref str_ret_value, ENDFLAG_2) == false)
                {
                    str_error_log = $"发送获取Wifi密码信息指令[{CMD_GET_WIFI_PASSWORD}]失败";
                    return false;
                }
                if (!str_ret_value.Contains("PWD="))
                {
                    return false;
                }
                string[] striparr = str_ret_value.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                string str_wifi_pwd_temp = "";
                for (int i = 0; i < striparr.Length; i++)
                {
                    if (striparr[i].Contains("PWD="))
                    {
                        str_wifi_pwd_temp = striparr[i];
                        break;
                    }
                }
                str_wifi_password = str_wifi_pwd_temp.Replace("PWD=", "").Trim();
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
                string CMD_SET_WIFI = $"ubus send \"d3_sys_setting\" '{{\"wifiset\": \"{str_wifi_ssid}\",\"password\":\"{str_wifi_password}\"}}'";
                string str_ret_value = "";
                if (SendCMDToXDC03(CMD_SET_WIFI, 2000, true, ref str_ret_value, ENDFLAG_1) == false)
                {
                    str_error_log = $"发送设置wifi信息指令[{CMD_SET_WIFI}]失败";
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
                if (SendCMDToXDC03(CMD_ENTER_FACTORY_MODE, 2000, true, ref str_ret_value, ENDFLAG_1) == false)
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
                if (SendCMDToXDC03(CMD_RESET_FACTORY, 2000, true, ref str_ret_value, ENDFLAG_1) == false)
                {
                    str_error_log = $"发送恢复出厂设置指令[{CMD_RESET_FACTORY}]失败";
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
                if (SendCMDToXDC03(CMD_SET_COLOR, 2000, true, ref str_ret_value, ENDFLAG_1) == false)
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
                if (SendCMDToXDC03(CMD_SET_PIR, 2000, true, ref str_ret_value, ENDFLAG_2) == false)
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
        /// <param name="str_index"></param>
        /// <param name="str_error_log"></param>
        /// <returns></returns>
        public bool PlayWav(string str_index, ref string str_error_log)
        {
            try
            {
                string CMD_PLAY_WAV = $"omfWavPlayer_ -n /etc/ringtone/{str_index}.wav";
                string str_ret_value = "";
                if (SendCMDToXDC03(CMD_PLAY_WAV, 2000, true, ref str_ret_value, ENDFLAG_2) == false)
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
                if (SendCMDToXDC03(CMD_OPEN_MIC, 2000, true, ref str_ret_value, ENDFLAG_1) == false)
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
                if (SendCMDToXDC03(CMD_CLOSE_MIC, 2000, true, ref str_ret_value, ENDFLAG_1) == false)
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
                if (SendCMDToXDC03(CMD_RECORD_MIC, 2000, true, ref str_ret_value, ENDFLAG_1) == false)
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
        public bool TestRecordWav(string str_max_abs, string str_delta, string str_result, ref string str_error_log)
        {
            try
            {
                string CMD_TEST_RECORD = $"mic_test-openwrt.arm /tmp/audio_cap.wav";
                string str_ret_value = "";
                if (SendCMDToXDC03(CMD_TEST_RECORD, 2000, true, ref str_ret_value, "}") == false)
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
                if (SendCMDToXDC03(CMD_PLAY_RECORD_WAV, 2000, true, ref str_ret_value, ENDFLAG_2) == false)
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
        public bool TestWifiUpThroughput(ref string str_rate, ref string str_loss, ref string str_error_log, string str_ip, string str_duration="10", string str_bandwidth="100M")
        {
            try
            {
                // -i指定每隔多少秒生成一次性能报告
                string CMD_IPERF3_UP = $"iperf3 -c {str_ip} -u -i1 -t{str_duration} -b {str_bandwidth} -R";
                string str_ret_value = "";
                if (SendCMDToXDC03(CMD_IPERF3_UP, 2000, true, ref str_ret_value, "iperf Done") == false)
                {
                    str_error_log = $"发送iperf3上行吞吐量测试指令[{CMD_IPERF3_UP}]失败";
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
                if (SendCMDToXDC03(CMD_IPERF3_DOWN, 2000, true, ref str_ret_value, "iperf Done") == false)
                {
                    str_error_log = $"发送iperf3下行吞吐量测试指令[{CMD_IPERF3_DOWN}]失败";
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
                if (SendCMDToXDC03(CMD_OPEN_RTSP, 2000, true, ref str_ret_value, ENDFLAG_1) == false)
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
                string CMD_SWITCH_IRCUT = $"ubus send \"d3_product_test\" '{{\"IR-CUT\": \"{str_state}\" }}'";
                string str_ret_value = "";
                if (SendCMDToXDC03(CMD_SWITCH_IRCUT, 2000, true, ref str_ret_value, ENDFLAG_1) == false)
                {
                    str_error_log = $"发送切换IR CUT指令[{CMD_SWITCH_IRCUT}]失败";
                    return false;
                }
                return true;
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
                if (SendCMDToXDC03(CMD_SWITCH_IRLED, 2000, true, ref str_ret_value, ENDFLAG_1) == false)
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
                if (SendCMDToXDC03(CMD_PCBA_RFTEST, 2000, true, ref str_ret_value, ENDFLAG_1) == false)
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
        /// PCBA指令发送RF信号
        /// PCBA-RF433测试 接收端播放音频1，需要有接收设备, 
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
                string endFlag=ENDFLAG_1;
                if (str_state == RF_TX)
                {
                    endFlag = "[RF-Send-test:send]";
                }
                else if (str_state == RF_RX)
                {
                    endFlag = "[RF-Send-test:rcv]";
                }
                if (SendCMDToXDC03(CMD_RF_MODE, 2000, true, ref str_ret_value, endFlag) == false)
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
                if (SendCMDToXDC03(CMD_GET_SN, 2000, true, ref str_ret_value, ENDFLAG_2) == false)
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
                if (SendCMDToXDC03(CMD_GET_UID, 2000, true, ref str_ret_value, ENDFLAG_2) == false)
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
                if (SendCMDToXDC03(CMD_SET_SN_UID, 2000, true, ref str_ret_value, ENDFLAG_1) == false)
                {
                    str_error_log = $"发送写入UID和SN指令[{CMD_SET_SN_UID}]失败";
                    return false;
                }
                Delay(200);
                string str_cur_sn = "";
                GetSN(ref str_cur_sn, ref str_error_log);
                if(str_cur_sn == str_sn)
                {
                    Delay(200);
                    string str_cur_uid = "";
                    GetUID(ref str_cur_uid, ref str_error_log);
                    if(str_cur_uid == str_uid)
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
                if (SendCMDToXDC03(CMD_SET_CHARGE_MODE, 2000, true, ref str_ret_value, $"Enter Charge Test --> Target = {str_battery_level}") == false)
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
                if (SendCMDToXDC03(CMD_CHARGE_IC, 2000, true, ref str_ret_value, endFlag) == false)
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

    }
}
