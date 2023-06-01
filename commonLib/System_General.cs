using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace System_General // inifile--operation,skybell_command,Accept and processing results
{
    public class General_ver
    {
        public string ver()
        {
            return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
    }

    public class bell_coap_command
    {
        public string str_coap_get_mac = "coap get coap://192.168.1.1:5683/system/mac";
        public string str_coap_get_battery = "coap get coap://192.168.1.1:5683/sensors/battery";
        public string str_coap_get_light = "coap get coap://192.168.1.1:5683/sensors/light";
        public string str_coap_get_temperature = "coap get coap://192.168.1.1:5683/sensors/temperature";
        public string str_coap_get_network = "coap get coap://192.168.1.1:5683/network";
        public string str_coap_get_firmware = "coap get coap://192.168.1.1:5683/system/firmware";

        //--------------------------------------------------------------------------------------------------------------------------
        public string str_coap_post_mic = "echo -n '{\"duration\":3}' | coap post coap://192.168.1.1/audio/capture/start";
        public string str_coap_post_Button = "echo button call | coap post coap://192.168.1.1/system/skybell";
        public string str_coap_post_Motion = "echo sta_motion_event | coap post coap://192.168.1.1/system/skybell";

        public string str_coap_post_led_r = "echo -n '{\"led\" : \"setrgb 255 0 0\"}' |coap post coap://192.168.1.1:5683/led";
        public string str_coap_post_led_g = "echo -n '{\"led\" : \"setrgb 0 255 0\"}' |coap post coap://192.168.1.1:5683/led";
        public string str_coap_post_led_b = "echo -n '{\"led\" : \"setrgb 0 0 255\"}' |coap post coap://192.168.1.1:5683/led";
        public string str_coap_post_led_w = "echo -n '{\"led\" : \"setrgb 255 255 255\"}' |coap post coap://192.168.1.1:5683/led";

        //------------------------------------------------------------------------------------------------------------------------------
        public string str_coap_post_led_brightness_100 = "echo -n '{\"led\" : \"dac 100\"}' |coap post coap://192.168.1.1:5683/led";
        public string str_coap_post_led_brightness_50 = "echo -n '{\"led\" : \"dac 50\"}' |coap post coap://192.168.1.1:5683/led";
        public string str_coap_post_led_brightness_0 = "echo -n '{\"led\" : \"dac 0\"}' |coap post coap://192.168.1.1:5683/led";
        
    }
  
    public class System_Inifile
    {
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        //声明读写INI文件的API函数     

        //类的构造函数，传递INI文件名
        //public string Path_ini;
        public void IniWriteValue(string Section, string Key, string Value, string Path_ini)
        {
            WritePrivateProfileString(Section, Key, Value, Path_ini);
        }

        //读INI文件         
        public string IniReadValue(string Section, string Key, string Path_ini)
        {
            StringBuilder temp = new StringBuilder(256);
            int i = GetPrivateProfileString(Section, Key, "", temp, 256, Path_ini);
            return temp.ToString();
        }
    }

    public class bell_send_command
    {
        ///////////////////// Linux指令
        // 写入RN号
        // 1.写入Rn:echo 123123123 > /mnt/diskc/factorytest_rn
        // 2.保存：ubus send d3_sys_setting  '{"sync_disk":1}'
        public string str_write_rn = "echo #RN# > /mnt/diskc/factorytest_rn && ubus send d3_sys_setting  '{\"sync_disk\":1}'";
        public string str_save_rn = "ubus send d3_sys_setting  '{\"sync_disk\":1}'";
        // 读取RN号
        public string str_read_rn = "rn=`cat /mnt/diskc/factorytest_rn` && echo \"Rn=\"$rn";

        // 写入测试工序号 如工序号为**** 代替0000
        public string str_write_tagnumber = "echo #TAG# > /mnt/diskc/test_station";
        // 读取测试工序号
        public string str_read_tagnumber = "cat /mnt/diskc/test_station";//cat /skybell/config/PT_NUM   ---读工序号 S100

        // 读取FW版本号,返回内容：D3_FW_APP_VER:1001
        public string str_read_FW_ver = "strings /usr/bin/d3_client | grep D3_FW_APP_VER";
        // 读取产品MAC
        public string str_read_mac = "ubus send d3_sys_setting  '{\"GetMacAddr\":1}'";
        /*
         root@XDC03-c344a:/# #0 2022-11-18 10:19:32
        16:10:36[1559319036.588581] DBUG0: [SystemParam.FwVersion = V5003]
        16:10:36[1559319036.595004] DBUG0: [SystemParam.RtosVersion = 220913]
        16:10:36[1559319036.595241] DBUG0: [SystemParam.McuBootloaderV = 21]
        16:10:36[1559319036.595356] DBUG0: [SystemParam.McuAppV = 0.7]
        16:10:36[1559319036.595461] DBUG0: [SystemParam.HwVersion = 0]
        16:10:36[1559319036.595557] DBUG0: [SystemParam.Battery = 420]
        16:10:36[1559319036.595654] DBUG0: [SystemParam.BatteryPerCent = 100%]
        16:10:36[1559319036.595747] DBUG0: [SystemParam.Temp = 0]
        16:10:36[1559319036.595840] DBUG0: [SystemParam.McuRfType = 4]
        16:10:36[1559319036.595932] DBUG0: [SystemParam.Backend = http://test.redbeecloud.com/RedbeeBackend/]
        16:10:36[1559319036.596030] DBUG0: [SystemParam.WifiVer = MT7682-v1.1]
        16:10:36[1559319036.597071] DBUG0: [info print end]
         */
        // 读取产品Hardware版本,返回结果中过滤SystemParam.HwVersion
        public string str_read_HW_ver = "ubus send \"d3_sys_info\" '{}'";
        // 读取产品MCU版本号，返回结果中过滤SystemParam.McuAppV
        public string str_read_MCU_ver = "ubus send \"d3_sys_info\" '{}'";
        // 读取产品电池电压。返回结果中过滤SystemParam.Battery
        public string str_read_battery_vol = "ubus send \"d3_sys_info\" '{}'";
        // 读取产品电池电量。返回结果中过滤SystemParam.BatteryPerCent
        public string str_read_battery_level = "ubus send \"d3_sys_info\" '{}'";
        // 读取温度。返回结果中过滤SystemParam.Temp
        public string str_read_temperature = "ubus send \"d3_sys_info\" '{}'";

        // 读取WIFi信息
        public string str_read_wifi_ssid = "cat /mnt/diskb/UDF/DOORBELL.CFG | grep ESSID";
        public string str_read_wifi_pwd = "cat /mnt/diskb/UDF/DOORBELL.CFG | grep PWD";
        //public string str_wifi_ssid_get = "uci -c /mnt/diskc/etc/config get wireless.radio0_sta.ssid";
        //public string str_wifi_password_get = "uci -c /mnt/diskc/etc/config get wireless.radio0_sta.key";
        // 设置WIFI信息
        public string str_set_wifi = "ubus send \"d3_sys_setting\" '{\"wifiset\": \"#SSID#\",\"password\":\"#PWD#\"}'";

        public string str_killall_ping = "killall ping ?";
        public string str_ping_route_ip = "ping 192.168.1.1 &";//ping 192.168.98.201 &

        // 设置Button LED灯颜色
        public string str_button_led_r = "ubus send \"d3_product_test\" '{\"button-LED\": \"red\" }'";
        public string str_button_led_g = "ubus send \"d3_product_test\" '{\"button-LED\": \"green\" }'";
        public string str_button_led_b = "ubus send \"d3_product_test\" '{\"button-LED\": \"blue\" }'";
        public string str_button_led_w = "ubus send \"d3_product_test\" '{\"button-LED\": \"white\" }'";
        public string str_button_led_off = "ubus send \"d3_product_test\" '{\"button-LED\": \"off\" }'";

        // lightsensor数据读取
        /*
         设备读数需要适配产测环境光源保证精确度，该指令不能连续读取，中间需要插入非lightsensor读取命令，比如
        先调用 ubus send "d3_product_test" '{"Lightsensor": "get" }'
        然后 ubus send "d3_product_test" '{"PIR": "off" }'
        再调用 ubus send "d3_product_test" '{"Lightsensor": "get" }'

        示例打印：
        02:22:47[1659666167.243958] DBUG0: [Lightsensor value = 16383]
         */
        public string str_read_lightsensor_val = "ubus send \"d3_product_test\" '{\"Lightsensor\": \"get\" }'";

        // 按键测试
        /*
         * 首先关闭PIR，以避免PIR影响，测试者按键则可以看到日志输出[SystemParam.ButtonStatus = 1]
         */
        public string str_button_test = "ubus send \"d3_product_test\" '{\"PIR\": \"off\" }'";
        // motion pir测试
        /*
         * 打开PIR功能，设备PIR镜头前挥手,则可以看到日志输出[SystemParam.MotionTriggerEvent = 1 service_up 0]
         */
        public string str_motion_pir_test = "ubus send \"d3_product_test\" '{\"PIR\": \"on\" }'";

        // 喇叭功放测试
        /* 
         * 产测软件选择需要播放的wav文件，如果需要反复播放，则反复调用，需要加上电池确保每一个声音文件都正常播放 
         */
        public string str_speak_1wav_test = "omfWavPlayer_ -n /etc/ringtone/1.wav";
        public string str_speak_2wav_test = "omfWavPlayer_ -n /etc/ringtone/2.wav";
        public string str_speak_3wav_test = "omfWavPlayer_ -n /etc/ringtone/3.wav";
        public string str_speak_4wav_test = "omfWavPlayer_ -n /etc/ringtone/4.wav";
        public string str_speak_5wav_test = "omfWavPlayer_ -n /etc/ringtone/5.wav";
        public string str_speak_8wav_test = "omfWavPlayer_ -n /etc/ringtone/8.wav";
        public string str_speak_9wav_test = "omfWavPlayer_ -n /etc/ringtone/9.wav";

        // 麦克风测试
        /*
         * PC端播放特定音频文件，D3提供
         * 日志返回{"max_abs":77,"delta":299,"result":True}
         */
        //麦克风录音指令
        public string str_mic_test_1 = "omfWavRecord_ -n /tmp/audio_cap.wav -d10";
        public string str_mic_test_2 = "mic_test-openwrt.arm /tmp/audio_cap.wav";
        public string str_mic_test_3 = "omfWavPlayer_ -n /tmp/audio_cap.wav";

        // WIFI吞吐量测试
        // PC端跑吞吐量测试服务器
        public string str_console_server = "iperf3 -s -i 1";
        // 串口发送
        public string str_downrate_test = "iperf3 -c #IP# -u -i1 -t10 -b 100M";  //下行吞吐量测试;
        public string str_uprate_test = "iperf3 -c #IP# -u -i1 -t10 -b 100M -R"; //上行吞吐量测试;

        // 实时影像测试
        // DUT推流
        public string str_rtsp_start = "omfRtspServer_ &";
        // PC端的VLC流地址
        public string str_vlp_test = "rtsp://192.168.1.1/";

        // USB/YUV出图 设备会模拟USB Camera，请检查电脑的设备管理器中的iCatch Vi37摄像头
        public string str_camera_on = "ubus send \"d3_sys_setting\" '{\"pc_camera_mode\": \"on\" }'";

        // IR-CUT打开,IR-CUT切换并进入黑白模式
        public string str_IR_cut_open = "ubus send \"d3_product_test\" '{\"IR-CUT\": \"on\" }'";
        // IR-CUT关闭,IR-CUT切换并退出黑白模式
        public string str_IR_cut_close = "ubus send \"d3_product_test\" '{\"IR-CUT\": \"off\" }'";
        // IR-LED打开,打开IR灯
        public string str_IR_led_open = "ubus send \"d3_product_test\" '{\"IR-LED\": \"on\" }'";
        // IR-LED关闭,关闭IR灯
        public string str_IR_led_close = "ubus send \"d3_product_test\" '{\"IR-LED\": \"off\" }'";

        public string str_mic_close = "ubus send \"d3_product_test\" '{\"CloseMic\":\"\"}'";  //ubus send "d3_product_test" '{"CloseMic":""}' 
        public string str_mic_open = "ubus send \"d3_product_test\" '{\"OpenMic\":\"\"}'";  //ubus send "d3_product_test" '{"CloseMic":""}' 

        // PCBA-RF433测试 接收端播放音频1，需要有接收设备, 参数可填1/2/3/4
        public string str_pcba_rf433 = "ubus send \"d3_product_test\" '{\"PCBA-RFTEST\":1}'";

        // RF433性能测试
        // 设置MCU发送RF数据测试 设置成功打印信息[RF-Send-test:send]。需要从频谱仪看结果
        public string str_rf_test_send = "ubus send \"d3_product_test\" '{\"RF-Test\": \"send\" }'";
        public string str_rf_test_accept = "[RF-Send-test:send]";
        // 设置MCU进入接收RF测试 设置成功打印信息[RF-Send-test:rcv]
        // 接收成功打印信息[RF Receive test successful = 3]
        public string str_Sensitivity_test_rx = "ubus send \"d3_product_test\" '{\"RF-Test\": \"rcv\" }'";
        public string str_Sensitivity_accept_1 = "[RF-Send-test:rcv]";
        public string str_Sensitivity_accept_2 = "[RF Receive test successful = 3]";
        public string str_Sensitivity_test_rx_close = "ubus send \"d3_product_test\" '{\"RF-Test\": \"send\" }'";

        // 写入DUT SN（20位）及UID（10位）
        // burn_UID_SN.sh JT9WV9A56C1KYZRK111A A1AAB00001
        // {"result": 0}
        public string str_write_uid_sn = "burn_UID_SN.sh "; //JT9WV9A56C1KYZRK111A  指令后面追加20位UID  10位SN A1AAB00001
        public string str_read_sn = "cat /mnt/diskc/camera_setting.json | jsonfilter -e \"@.reg_data.cameraSn\"";
        public string str_read_uid = "cat /mnt/diskc/camera_setting.json | jsonfilter -e \"@.reg_data.cameraUid\"";
        // 进入产测模式
        public string str_factory_mode = "npiReset.sh && ubus send d3_sys_setting  '{\"rebootall\":1}'";

        // 恢复出厂设置
        public string str_FactoryReset = "FactoryReset.sh";

        // 设置设备进入充电模式
        /* 设置设备进入充电模式，充电达到目标时停止充电
            设置成功打印[Enter Charge Test --> Target = 80]
            80表示充电截止目标电量80%，可以修改0-100
            此时LED指示含义：
            红蓝间隔2秒交替闪烁，表示充电中
            红+蓝同时亮，表示充电达到设置值
         */
        public string str_set_charging_mode = "ubus send \"d3_product_test\" '{\"CHARGE-MODE-TEST\": #LEVEL# }'";
        // 控制设备停止充电 设备启动默认状态为充电状态 设置充电IC停止充电，设置成功打印[ChargeIC Disable]
        public string str_stop_charging = "ubus send \"d3_product_test\" '{\"CHARGE-IC\": \"disable\"}'";
        // 控制设备开始充电 设备启动默认状态为充电状态 设置充电IC开始充电，设置成功打印[ChargeIC Enable]
        public string str_start_charging = "ubus send \"d3_product_test\" '{\"CHARGE-IC\": \"enable\"}'";

        // XDC03 MAC地址通过WiFi模组厂烧录,产线不写入
        //public string str_write_mac = "rtwpriv wlan0 efuse_set wmap,11A,";

        ///////////////////////// RTOS指令
        public string str_rtos_powerstatus = "factorytest powerstatus";     //设备正常时会返回  return:power start the normal
        public string str_rtos_targnumber = "factorytest read";        //读取test station, return:read test station=S160 
        public string str_rtos_write_targnumber = "factorytest write S160"; //写入test station, return:read test station=S160
        public string str_rtos_open_camera = "pccam";     //进入usb取图模式
        public string str_rtos_IR_cut_open = "factorytest ircut-on";      // 打开夜视模式
        public string str_rtos_IR_cut_close = "factorytest ircut-off";      // 打开夜视模式
        public string str_rtos_read_rn = "factorytest readrn";   // 读RN号
        public string str_ifconfig = "ifconfig";
        // old cmd
        /*
        public string str_read_sn = "cat /mnt/diskc/camera_setting.json  | jsonfilter -e \"@.reg_data.cameraSn\"";  //读取产品SN A1AAB00001  SN：10位数
        public string str_read_uid = "cat /mnt/diskc/camera_setting.json  | jsonfilter -e \"@.reg_data.cameraUid\"";//读取产品UID  JT9WV9A56C1KYZRK111A  UID：20位数
        public string str_read_mac_1 = "rtwpriv wlan0 efuse_get mac";   //返回  wlan0    efuse_get:80:9F:9B:08:D1:12
        public string str_read_mac_2 = "cat /sys/class/net/wlan0/address";   //返回  80:9f:9b:08:d1:12;

        public string str_set_wifi_SSID_PASSWORD = "uci -c /mnt/diskc/etc/config set wireless.radio0_sta.ssid='wifissid'" + "\r\n" +
                                                   "uci -c /mnt/diskc/etc/config set wireless.radio0_sta.key='password'" + "\r\n" +
                                                   "uci -c /mnt/diskc/etc/config set wireless.radio0_sta.encryption='psk2'";

        public string str_set_wifi_SSID_PASSWORD_0 = "uci -c /mnt/diskc/etc/config set wireless.radio0_sta.ssid='wifissid'" + "\r\n" +
                                                   "uci -c /mnt/diskc/etc/config set wireless.radio0_sta.key='password'" + "\r\n" +
                                                   "uci -c /mnt/diskc/etc/config set wireless.radio0_sta.encryption='psk2'" + "\r\n" +
                                                   "staSwitch.sh";//设置wifissid Password

        public string str_set_wifi_SSID_PASSWORD_save = "uci -c /mnt/diskc/etc/config commit wireless" + "\r\n";

        public string str_set_wifi_SSID_PASSWORD_remove = "rm /mnt/diskc/etc/config/wireless.crc && sync";
        public string str_set_wifi_SSID_PASSWORD_check = "ls /mnt/diskc/etc/config";

        public string str_ifconfig = "ifconfig";
        
        public string str_get_gain_exposure = "ubus send \"d3_product_test\" '{\"ES-gain\": \"get\" }' ";//get Gain值
        
        public string str_killall_ping = "killall ping ?";
        public string str_ping_route_ip = "ping 192.168.1.1 &";//ping 192.168.98.201 &

        public bool bool_console_server;
        //factory_mode = 1(产品测试模式)  factory_mode = 0（产品正常模式）
        public string str_factory_1 = "npiReset.sh";
        public string str_factory_2 = "ubus send \"d3_sys_setting\" '{\"rebootall\": 1 }'";

        public string str_after_factory_1 = "npiReset.sh";
        public string str_after_factory_2 = "ubus send \"d3_sys_setting\" '{\"reboot\": \"run\" }'";

        public string str_mic_close = "ubus send \"d3_product_test\" '{\"CloseMic\":\"\"}'";  //ubus send "d3_product_test" '{"CloseMic":""}' 
        public string str_mic_open = "ubus send \"d3_product_test\" '{\"OpenMic\":\"\"}'";  //ubus send "d3_product_test" '{"CloseMic":""}' 
        */
    }

    public class System_Command
    {
        public bool pcba_rn_check_0_9_a_z(string str_get_rn,bool bool_0_9_a_z)
        {
            try
            {
                if (bool_0_9_a_z)
                {
                    string pattern_0_9_a_z_A_Z = "^[0-9a-zA-Z]+$"; // @"^\d+$"
                    if (!System.Text.RegularExpressions.Regex.IsMatch(str_get_rn, pattern_0_9_a_z_A_Z))// @"^[+-]?\d*[.]?\d*$"))
                    {
                        return false;
                    }
                }
                else
                {
                    string pattern_0_9 = @"^\d+$"; // @"^\d+$"
                    if (!System.Text.RegularExpressions.Regex.IsMatch(str_get_rn, pattern_0_9))// @"^[+-]?\d*[.]?\d*$"))
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ee)
            {
                return false;
            }
        }

        public bool power_on_check_0(string str_get_value, ref short short_power_on_finish, ref short short_go_here)
        {
            try
            {
                if (str_get_value.ToLower().Contains("power on finish"))
                {
                    short_power_on_finish = 0; // 正常开机
                }
                else
                {
                    short_power_on_finish = 1; // 异常
                }
                int int_count = System.Text.RegularExpressions.Regex.Matches(str_get_value, "goto here").Count;// 就是你要的次数
                if (int_count == 4)//正常开机   no wifi to connect
                {
                    short_go_here = 0;//wifi to connect normal
                }
                if (int_count == 2)//正常开机   no wifi to connect
                {
                    short_go_here = 1;//wifi to connect error
                }
                return true;
            }
            catch (Exception ee)
            {
                return false;
            }
        }

        /// <summary>
        /// 解析串口日志，获取1、开机状态short_power_on_finish；2、wifi状态short_go_here；3、是否为测试模式short_factory_mode；
        /// </summary>
        /// <param name="str_get_value"></param>
        /// <param name="short_power_on_finish"></param>
        /// <param name="short_go_here"></param>
        /// <param name="short_factory_mode"></param>
        /// <returns></returns>
        public bool power_on_check(string str_get_value, ref short short_power_on_finish,ref short short_go_here,ref short short_factory_mode)
        {
            try
            {
                //factory_mode = 1(产品测试模式)  factory_mode = 0（产品正常模式）
                if (str_get_value.ToLower().Contains("power on finish"))
                {
                    short_power_on_finish = 0; // power normal
                }
                else
                {
                    short_power_on_finish = 1; // power error
                }
                if (str_get_value.ToLower().Contains("mtwlan0 ip ready"))
                {
                    short_go_here = 0; //wifi to connect normal
                }
                else
                {
                    short_go_here = 1; //wifi to connect error
                }
                //if (str_get_value.ToLower().Contains("3335") && str_get_value.ToLower().Contains("3343") || str_get_value.ToLower().Contains("wifi_down"))
                //{
                //    short_go_here = 1;//wifi to connect error
                //}
                //if (str_get_value.ToLower().Contains("3347") && str_get_value.ToLower().Contains("3357") || str_get_value.ToLower().Contains("wifi_up"))
                //{
                //    short_go_here = 0;//wifi to connect normal
                //}
                if(str_get_value.ToLower().Contains("factory_mode = 1"))
                {
                    short_factory_mode = 1;//测试模式，正常
                }
                if (str_get_value.ToLower().Contains("factory_mode = 0"))
                {
                    short_factory_mode = 0;//产品正常模式...
                }
                return true;
            }
            catch (Exception ee)
            {
                return false;
            }
        }

        /// <summary>
        /// RTOS 从串口日志中解析设备开机状态
        /// </summary>
        /// <param name="str_get_value"></param>
        /// <param name="str_error"></param>
        /// <returns></returns>
        public bool GetPowerStatusByRTOS(string str_get_value, ref string str_error)
        {
            try
            {
                if (!str_get_value.Contains("power start the normal"))
                {
                    return false;
                }
                return true;
            }
            catch (Exception ee)
            {
                str_error = "ee:" + ee.Message.Replace("\r\n", "");
                return false;
            }
        }

        /// <summary>
        /// RTOS 从串口日志中解析测试工序号
        /// </summary>
        /// <param name="str_get_value"></param>
        /// <param name="str_targnumber"></param>
        /// <param name="str_error"></param>
        /// <returns></returns>
        public bool GetTargNumberByRTOS(string str_get_value, ref string str_targnumber,ref string str_error)
        {
            try
            {
                //read test_station =S160
                if (!str_get_value.Contains("read test_station"))
                {
                    return false;
                }
                string[] striparr = str_get_value.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                string str_targnumber_temp = "";
                for (int i = 0; i < striparr.Length; i++)
                {
                    Application.DoEvents();
                    if (striparr[i].Contains("read test_station"))
                    {
                        str_targnumber_temp = striparr[i].Replace("\r\n","").Replace(">cmd","").Replace("#", "").TrimEnd();
                        break;
                    }
                }
                str_targnumber = str_targnumber_temp.Substring(str_targnumber_temp.Length - 4, 4);
                return true;
            }
            catch (Exception ee)
            {
                str_error = "ee:" + ee.Message.Replace("\r\n", "");
                return false;
            }
        }

        /// <summary>
        /// RTOS 从串口日志中解析RN号
        /// </summary>
        /// <param name="str_get_value"></param>
        /// <param name="rn"></param>
        /// <param name="str_error"></param>
        /// <returns></returns>
        public bool GetRnByRTOS(string str_get_value, ref string rn, ref string str_error)
        {
            try
            {
                //factorytest readrn
                //read factorytest_rn = XDC032242200013
                //
                //
                //cmd >

                if (!str_get_value.Contains("read factorytest_rn"))
                {
                    return false;
                }
                string[] striparr = str_get_value.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                string str_rn_temp = "";
                for (int i = 0; i < striparr.Length; i++)
                {
                    Application.DoEvents();
                    if (striparr[i].Contains("read factorytest_rn"))
                    {
                        str_rn_temp = striparr[i].Replace("\r\n", "").Replace(">cmd", "").TrimEnd();
                    }
                }
                rn = str_rn_temp.Replace("read factorytest_rn =", "").Trim();
                if (rn.ToLower().Contains("error"))
                {
                    str_error = "异常:" + rn;
                    return false;
                }
                return true;
            }
            catch (Exception ee)
            {
                str_error = "异常:" + ee.Message.Replace("\r\n", "");
                return false;
            }
        }

        /// <summary>
        /// 从串口日志中解析wifi名称
        /// </summary>
        /// <param name="str_get_value"></param>
        /// <param name="str_wifi_ssid"></param>
        /// <param name="str_error"></param>
        /// <returns></returns>
        public bool GetWiFiSSID(string str_get_value,ref string str_wifi_ssid,ref string str_error)
        {
            try
            {
                if (!str_get_value.Contains("ESSID="))
                {
                    return false;
                }
                string[] striparr = str_get_value.Split(new string[] { "\r\n" }, StringSplitOptions.None);
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
            catch(Exception  ee)
            {
                str_error = ee.Message;
                return false;
            }
        }

        /// <summary>
        /// 从串口日志中解析wifi密码
        /// </summary>
        /// <param name="str_get_value"></param>
        /// <param name="str_wifi_password"></param>
        /// <param name="str_error"></param>
        /// <returns></returns>
        public bool GetWiFiPassword(string str_get_value, ref string str_wifi_password, ref string str_error)
        {
            try
            {
                if (!str_get_value.Contains("PWD="))
                {
                    return false;
                }
                string[] striparr = str_get_value.Split(new string[] { "\r\n" }, StringSplitOptions.None);
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
                str_error = ee.Message;
                return false;
            }
        }
        
        /// <summary>
        /// 从串口日志中解析Firmware版本号
        /// </summary>
        /// <param name="str_get_value"></param>
        /// <param name="str_ver"></param>
        /// <param name="str_error"></param>
        /// <returns></returns>
        public bool GetFwVersion(string str_get_value, ref string str_ver, ref string str_error)
        {
            try
            {
                if (!str_get_value.Contains("D3_FW_APP_VER:"))
                {
                    return false;
                }
                string[] striparr = str_get_value.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                string str_fw_temp = "";
                for (int i = 0; i<striparr.Length; i++)
                {
                    if (striparr[i].Contains("D3_FW_APP_VER:"))
                    {
                        str_fw_temp = striparr[i];
                        break;
                    }
                }
                str_ver = str_fw_temp.Replace("D3_FW_APP_VER:", "").Trim();
                if (check_int_double(str_ver) == false)
                {
                    str_error = "ver";
                    return false;
                }
                return true;
            }
            catch (Exception ee)
            {
                str_error = "ee:" + ee.Message.Replace("\r\n", "");
                return false;
            }
        }  

        /// <summary>
        /// 判断字符串是否包含测站字段
        /// </summary>
        /// <param name="str_get_value"></param>
        /// <returns></returns>
        public bool isContainTagNumber(string str_get_value)
        {
            try
            {
                string[] tagnumbers = new string[11] {"T010", "T020", "T030", "T040", "T050", "T060", "T070", "T080", "T090", "T100", "T110" };
                bool isContains = false;
                for(int i=0; i<tagnumbers.Length; i++)
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
            catch(Exception ee)
            {
                return false;
            }
        }

        /// <summary>
        /// 从串口日志中解析测试工序号
        /// </summary>
        /// <param name="str_get_value"></param>
        /// <param name="str_tagNumber"></param>
        /// <param name="str_error"></param>
        /// <returns></returns>
        public bool GetTagNumber(string str_get_value, ref string str_tagNumber, ref string str_error)
        {
            try
            {
                if (!isContainTagNumber(str_get_value))
                {
                    return false;
                }
                string[] striparr = str_get_value.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                string str_tagnumber_temp = "";
                for (int i = 0; i < striparr.Length; i++)
                {
                    if (isContainTagNumber(striparr[i]))
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
                str_error = "异常:" + ee.Message.Replace("\r\n", "");
                return false;
            }
        }

        /// <summary>
        /// 从串口日志中解析Rn号
        /// </summary>
        /// <param name="str_get_value"></param>
        /// <param name="rn"></param>
        /// <param name="str_error"></param>
        /// <returns></returns>
        public bool GetRn(string str_get_value, ref string rn, ref string str_error)
        {
            try
            {
                if (!str_get_value.Contains("Rn=XDC"))
                {
                    return false;
                }
                string[] striparr = str_get_value.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                string str_rn_temp = "";
                for(int i = 0;i< striparr.Length; i++)
                {
                    if (striparr[i].Contains("Rn=XDC"))
                    {
                        str_rn_temp = striparr[i];
                        break;
                    }
                }
                int start = str_rn_temp.IndexOf("XDC");
                str_rn_temp = str_rn_temp.Substring(start, 15);
                rn = str_rn_temp.Replace("Rn=", "").Trim();
                return true;
            }
            catch (Exception ee)
            {
                str_error = "异常:" + ee.Message.Replace("\r\n", "");
                return false;
            }
        }

        /// <summary>
        /// 从串口日志中解析SN号
        /// </summary>
        /// <param name="str_get_value"></param>
        /// <param name="str_sn"></param>
        /// <param name="str_error"></param>
        /// <returns></returns>
        public bool GetSn(string str_get_value,ref string str_sn,ref string str_error)
        {
            /*
            /# cat /mnt/diskc/camera_setting.json  | jsonfilter -e "@.reg_d

                ata.cameraSn"
                1200000017412031
                root@XDC01-f9694:/#


                root@XDC01-f9694:/# sn=`cat /mnt/diskc/camera_setting.json  | jsonfilter -e "@.r

                eg_data.cameraSn"` && echo "cameraSn="$sn
                cameraSn=1200000017412031
                root@XDC01-f9694:/# 
             */
            try
            {
                string[] striparr = str_get_value.Split(new string[] { "\r\n" }, StringSplitOptions.None);
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
                return false;
            }
        }

        public bool GetUid(string str_get_value, ref string str_uid, ref string str_error)
        {
            try
            {
                string[] striparr = str_get_value.Split(new string[] { "\r\n" }, StringSplitOptions.None);
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
                return false;
            }
        }

        /// <summary>
        /// 从串口日志中解析温度值
        /// </summary>
        /// <param name="str_get_value"></param>
        /// <param name="str_temperature"></param>
        /// <param name="str_error"></param>
        /// <returns></returns>
        public bool GetTemperature(string str_get_value, ref string str_temperature, ref string str_error)
        {
            try
            {
                if(str_get_value.Contains("SystemParam.Temp") == false)
                {
                    str_error = "No found SystemParam.Temp";
                    return false;
                }
                string[] striparr = str_get_value.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                string str_temperature_temp = "";// striparr[7];
                for (int i = 0; i < striparr.Length; i++)
                {
                    if(striparr[i].Contains("SystemParam.Temp"))
                    {
                        str_temperature_temp = striparr[i];
                    }
                }
                int int_deng = str_temperature_temp.IndexOf("=");
                str_temperature = str_temperature_temp.Substring(int_deng, str_temperature_temp.Length - int_deng).Replace("]", "").Replace("=","").Trim();

                if (check_int_double(str_temperature) == false)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ee)
            {
                str_error = "ee:" + ee.Message.Replace("\r\n", "");
                return false;
            }
        }

        /// <summary>
        /// 从串口日志中解析MCU版本号
        /// </summary>
        /// <param name="str_get_value"></param>
        /// <param name="str_mcu_ver"></param>
        /// <param name="str_error"></param>
        /// <returns></returns>
        public bool GetMCUVersion(string str_get_value, ref string str_mcu_ver, ref string str_error)
        {
            try
            {
                if (str_get_value.Contains("SystemParam.McuAppV") == false)
                {
                    str_error = "No found SystemParam.McuAppV";
                    return false;
                }
                string[] striparr = str_get_value.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                string str_mcu_ver_temp = "";// = striparr[4];
                for (int i = 0; i < striparr.Length; i++)
                {
                    if (striparr[i].Contains("SystemParam.McuAppV"))
                    {
                        str_mcu_ver_temp = striparr[i];
                    }
                }
                int int_deng = str_mcu_ver_temp.IndexOf("=");
                str_mcu_ver = str_mcu_ver_temp.Substring(int_deng, str_mcu_ver_temp.Length - int_deng).Replace("]", "").Replace("=", "").Trim();

                if (check_int_double(str_mcu_ver) == false)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ee)
            {
                str_error = "ee:" + ee.Message.Replace("\r\n", "");
                return false;
            }
        }

        /// <summary>
        /// 从串口日志中解析亮度值
        /// </summary>
        /// <param name="str_get_value"></param>
        /// <param name="str_light_sensor_value"></param>
        /// <param name="str_error"></param>
        /// <returns></returns>
        public bool GetLightSensorValue(string str_get_value, ref string str_light_sensor_value, ref string str_error)
        {
            try
            {
                if (str_get_value.Contains("Lightsensor value") == false)
                {
                    str_error = "No found Lightsensor value";
                    return false;
                }
                string[] striparr = str_get_value.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                string str_light_temp = "";
                for(int i=0; i<striparr.Length; i++)
                {
                    if (striparr[i].Contains("Lightsensor value"))
                    {
                        str_light_temp = striparr[i];
                        break;
                    }
                }
                int int_deng = str_light_temp.IndexOf("=");
                str_light_sensor_value = str_light_temp.Substring(int_deng, str_light_temp.Length - int_deng).Replace("=", "").Replace("]", "").Trim();
                if (check_int_double(str_light_sensor_value) == false)
                {
                    str_error = "light_sensor";
                    return false;
                }
                return true;
            }
            catch (Exception ee)
            {
                str_error = "ee:" + ee.Message.Replace("\r\n", "");
                return false;
            }
        }
        
        /// <summary>
        /// 从串口日志中解析电池电量
        /// </summary>
        /// <param name="str_get_value"></param>
        /// <param name="str_battery_level"></param>
        /// <param name="str_error"></param>
        /// <returns></returns>
        public bool GetBatteryLevel(string str_get_value, ref string str_battery_level, ref string str_error)
        {
            try
            {
                if (str_get_value.Contains("SystemParam.BatteryPerCent") == false)
                {
                    str_error = "No found SystemParam.BatteryPerCent";
                    return false;
                }
                string[] striparr = str_get_value.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                string str_battery_level_temp = "";
                for (int i = 0; i < striparr.Length; i++)
                {
                    if (striparr[i].Contains("SystemParam.BatteryPerCent"))
                    {
                        str_battery_level_temp = striparr[i];
                        break;
                    }
                }
                int int_deng = str_battery_level_temp.IndexOf("=");
                str_battery_level = str_battery_level_temp.Substring(int_deng, str_battery_level_temp.Length - int_deng).Replace("]","").Replace("=","").Trim();
                str_battery_level = str_battery_level.Replace("%", "").Trim();
                if (check_int_double(str_battery_level) == false)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ee)
            {
                str_error = "ee:" + ee.Message.Replace("\r\n", "");
                return false;
            }
        }

        /// <summary>
        /// 从串口日志中解析电池电压
        /// </summary>
        /// <param name="str_get_value"></param>
        /// <param name="str_battery_level"></param>
        /// <param name="str_error"></param>
        /// <returns></returns>
        public bool GetBatteryVoltage(string str_get_value, ref string str_battery_level, ref string str_error)
        {
            try
            {
                if (str_get_value.Contains("SystemParam.Battery") == false)
                {
                    str_error = "No found SystemParam.Battery";
                    return false;
                }
                string[] striparr = str_get_value.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                string str_battery_level_temp = "";
                for (int i = 0; i < striparr.Length; i++)
                {
                    if (striparr[i].Contains("SystemParam.Battery =")) 
                    {
                        str_battery_level_temp = striparr[i];
                        break;
                    }
                }
                int int_deng = str_battery_level_temp.IndexOf("=");
                str_battery_level = str_battery_level_temp.Substring(int_deng, str_battery_level_temp.Length - int_deng).Replace("]", "").Replace("=", "").Trim();

                if (check_int_double(str_battery_level) == false)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ee)
            {
                str_error = "ee:" + ee.Message.Replace("\r\n", "");
                return false;
            }
        }

        /// <summary>
        /// 从串口日志中解析硬件版本号
        /// </summary>
        /// <param name="str_get_value"></param>
        /// <param name="str_hw_version"></param>
        /// <param name="str_error"></param>
        /// <returns></returns>
        public bool GetHWVersion(string str_get_value, ref string str_hw_version, ref string str_error)
        {
            try
            {
                if (str_get_value.Contains("SystemParam.HwVersion") == false)
                {
                    str_error = "No found SystemParam.HwVersion";
                    return false;
                }
                string[] striparr = str_get_value.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                string str_HwVersion_temp = "";//striparr[5];
                for (int i = 0; i < striparr.Length; i++)
                {
                    if (striparr[i].Contains("SystemParam.HwVersion"))
                    {
                        str_HwVersion_temp = striparr[i];
                    }
                }
                int int_deng = str_HwVersion_temp.IndexOf("=");
                str_hw_version = str_HwVersion_temp.Substring(int_deng, str_HwVersion_temp.Length - int_deng).Replace("]", "").Replace("=", "").Trim();

                if (check_int_double(str_hw_version) == false)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ee)
            {
                str_error = "ee:" + ee.Message.Replace("\r\n", "");
                return false;
            }
        }

        /// <summary>
        /// 从串口日志中读取mac地址
        /// </summary>
        /// <param name="str_get_value"></param>
        /// <param name="str_mac_address"></param>
        /// <param name="str_error"></param>
        /// <returns></returns>
        public bool GetMac(string str_get_value, ref string str_mac_address, ref string str_error)
        {
            try
            {
                /*
                root@XDC03-de84e:/# ubus send d3_sys_setting  '{"GetMacAddr":1}'
                09:06:25[1669799185.219012] DBUG0: [ubus_probe_config_event { "d3_sys_setting": {"GetMacAddr":1} }]
                root@XDC03-de84e:/# 09:06:26[1669799186.230431] DBUG0: [Get Mac Addr = F0:C8:14:7D:E8:4E]
                09:06:26[1669799186.236411] DBUG0: [info print end]
                 */
                if (str_get_value.Contains("Get Mac Addr =") == false)
                {
                    str_error = "No found Get Mac Addr";
                    return false;
                }
                string[] striparr = str_get_value.Split(new string[] { "\r\n" }, StringSplitOptions.None);
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
                str_mac_address = str_mac_temp.Substring(int_deng, str_mac_temp.Length - int_deng).Replace("]", "").Replace("=", "").Trim();

                if (mac_check(str_mac_address, 17) == false)
                {
                    str_error = "mac address format error";
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
        /// 从串口日志中解析麦克风测试结果数据
        /// </summary>
        /// <param name="str_get_value"></param>
        /// <param name="str_mic_value"></param>
        /// <param name="str_error"></param>
        /// <returns></returns>
        public bool GetMicValue(string str_get_value, ref string[] str_mic_value, ref string str_error)
        {
            try
            {
                //{"max_abs":383,"delta":450,"result":True}
                //str_get_value = str_get_value.Replace("\"", "").Replace("{", "").Replace("}", "").Trim();
                string str_temp_mic = "";
                string[] striparr = str_get_value.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0; i < striparr.Length; i++)
                {
                    if (striparr[i].ToLower().Contains("max_abs"))
                    {
                        str_temp_mic = striparr[i];
                        break;
                    }
                }
                str_temp_mic = str_temp_mic.Replace("\"", "").Replace("{","").Replace("}","");
                string[] striparr_1 = str_temp_mic.Split(new string[] { "," }, StringSplitOptions.None);

                str_mic_value[0] = striparr_1[0].Replace("max_abs:", "").Trim();
                str_mic_value[1] = striparr_1[1].Replace("delta:", "").Trim();
                str_mic_value[2] = striparr_1[2].Replace("result:", "").Trim();
                if (check_int_double(str_mic_value[1]) == false || check_int_double(str_mic_value[0]) == false)
                {
                    str_error = "micAMP or micPower";
                    return false;
                }
                return true;
            }
            catch (Exception ee)
            {
                str_error = "ee:" + ee.Message.Replace("\r\n", "");
                return false;
            }
        }

        /// <summary>
        /// 从串口日志中获取设备IP
        /// </summary>
        /// <param name="str_get_value"></param>
        /// <param name="str_bell_ip"></param>
        /// <param name="str_error"></param>
        /// <returns></returns>
        public bool GetIP(string str_get_value, ref string str_bell_ip, ref string str_error)
        {
            try
            {
                /*
                ifconfig            0
                lo        Link encap:Local Loopback      1
                          inet addr:127.0.0.1  Mask:255.0.0.0       2
                          UP LOOPBACK RUNNING  MTU:65536  Metric:1
                          RX packets:32 errors:0 dropped:0 overruns:0 frame:0
                          TX packets:32 errors:0 dropped:0 overruns:0 carrier:0
                          collisions:0 txqueuelen:1 
                          RX bytes:2624 (2.5 KiB)  TX bytes:2624 (2.5 KiB)

                wlan0     Link encap:Ethernet  HWaddr 80:9F:9B:BF:9B:E7                       --9
                          inet addr:192.168.31.24  Bcast:192.168.31.255  Mask:255.255.255.0   --10
                          UP BROADCAST RUNNING MULTICAST  MTU:1500  Metric:1
                          RX packets:25352 errors:0 dropped:226 overruns:0 frame:0
                          TX packets:300 errors:0 dropped:1 overruns:0 carrier:0
                          collisions:0 txqueuelen:1000 
                          RX bytes:2400098 (2.2 MiB)  TX bytes:27956 (27.3 KiB)

                root@(none):/# 
                 */
                if (!str_get_value.Contains("inet addr:") && str_get_value.Contains("Bcast:"))
                {
                    str_error = "No found Get IP";
                    return false;
                }
                string[] striparr = str_get_value.Split(new string[] { "\r\n" }, StringSplitOptions.None);
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
                str_bell_ip = str_ip_temp.Substring(int_inet+10, int_Bcast-int_inet-10).Trim();

                System.Net.IPAddress ipAddress;
                if (System.Net.IPAddress.TryParse(str_bell_ip, out ipAddress) == false)
                {
                    str_error = "Error IP address";
                    return false;
                }
                return true;
            }
            catch (Exception ee)
            {
                str_error = "ee:" + ee.Message.Replace("\r\n", "");
                return false;
            }
        }

        //----获取无线网卡的IP
        public bool local_ip_wifi(ref string str_ip, ref string str_gate_ip,ref string str_error_log)
        {
            try
            {
                foreach (System.Net.NetworkInformation.NetworkInterface f in System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (f.OperationalStatus == System.Net.NetworkInformation.OperationalStatus.Unknown)
                    {
                        str_error_log = "Not connect wifi";
                        return false;
                    }
                }
                System.Net.NetworkInformation.NetworkInterface[] nics = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();
                foreach (System.Net.NetworkInformation.NetworkInterface adapter in nics)
                {
                    //Wireless80211 无线网卡     Ethernet 以太网卡   
                    if (adapter.NetworkInterfaceType == System.Net.NetworkInformation.NetworkInterfaceType.Wireless80211)
                    {
                        
                        //网卡Mac地址
                        System.Net.NetworkInformation.PhysicalAddress mac = adapter.GetPhysicalAddress();
                        //TODO:根据mac 地址 匹配目标网卡

                        //获取以太网卡网络接口信息
                        System.Net.NetworkInformation.IPInterfaceProperties ip = adapter.GetIPProperties();
                        //获取单播地址集
                        System.Net.NetworkInformation.UnicastIPAddressInformationCollection ipCollection = ip.UnicastAddresses;
                        foreach (System.Net.NetworkInformation.UnicastIPAddressInformation ipadd in ipCollection)
                        {
                            if (ipadd.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                            {
                                str_ip = ipadd.Address.ToString();//获取ip
                            }
                            else if (ipadd.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                            {
                                //本机IPV6 地址
                            }
                        }
                        if (ip.GatewayAddresses != null && ip.GatewayAddresses.Count != 0)
                        {
                            str_gate_ip = ((System.Net.NetworkInformation.GatewayIPAddressInformation)(ip.GatewayAddresses[0])).Address.ToString();
                        }
                    }
                }
                System.Net.IPAddress ipAddress;
                // --- 判断获取的IP--是否正常的IP地址;
                if (!System.Net.IPAddress.TryParse(str_ip, out ipAddress) || !System.Net.IPAddress.TryParse(str_gate_ip, out ipAddress))
                {
                    str_error_log = "IP address";
                    return false;
                }
              
                return true;
            }
            catch (Exception ee)
            {
                str_error_log = "ee:" + ee.Message.Replace("\r\n", "");
                return false;
            }
        }

        //----获取有线网卡的IP
        public bool local_ip_Ethernet(ref string str_ip, ref string str_gate_ip, ref string str_error_log)
        {
            try
            {
                foreach (System.Net.NetworkInformation.NetworkInterface f in System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (f.OperationalStatus == System.Net.NetworkInformation.OperationalStatus.Unknown)
                    {
                        str_error_log = "Not connect Ethernet";
                        return false;
                    }
                }
                System.Net.NetworkInformation.NetworkInterface[] nics = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();
                foreach (System.Net.NetworkInformation.NetworkInterface adapter in nics)
                {
                    //Wireless80211 无线网卡     Ethernet 以太网卡   
                    if (adapter.NetworkInterfaceType == System.Net.NetworkInformation.NetworkInterfaceType.Ethernet)
                    {
                        //网卡Mac地址
                        System.Net.NetworkInformation.PhysicalAddress mac = adapter.GetPhysicalAddress();
                        //TODO:根据mac 地址 匹配目标网卡

                        //获取以太网卡网络接口信息
                        System.Net.NetworkInformation.IPInterfaceProperties ip = adapter.GetIPProperties();
                        //获取单播地址集
                        System.Net.NetworkInformation.UnicastIPAddressInformationCollection ipCollection = ip.UnicastAddresses;
                        foreach (System.Net.NetworkInformation.UnicastIPAddressInformation ipadd in ipCollection)
                        {   //System.Net.NetworkInformation.OperationalStatus
                            if (ipadd.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                            {
                                str_ip = ipadd.Address.ToString();//获取ip
                            }
                            else if (ipadd.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                            {
                                //本机IPV6 地址
                            }
                        }
                        if (ip.GatewayAddresses != null && ip.GatewayAddresses.Count != 0)
                        {
                            str_gate_ip = ((System.Net.NetworkInformation.GatewayIPAddressInformation)(ip.GatewayAddresses[0])).Address.ToString();
                        }
                    }
                }
                System.Net.IPAddress ipAddress;
                // --- 判断获取的IP--是否正常的IP地址;
                if (!System.Net.IPAddress.TryParse(str_ip, out ipAddress) || !System.Net.IPAddress.TryParse(str_gate_ip, out ipAddress))
                {
                    str_error_log = "IP address";
                    return false;
                }
                return true;
            }
            catch (Exception ee)
            {
                str_error_log = "ee:" + ee.Message.Replace("\r\n", "");
                return false;
            }
        }

        public bool wifi_up_open(string str_get_value,ref string str_error_log)
        {
            try
            {
                if (str_get_value.Contains("wlan0: link becomes ready") || str_get_value.Contains("deinit ifname"))//wlan0: link becomes ready
                {
                   
                }
                else
                {
                    str_error_log = "Cann't connect Router";
                    return false;
                }
                return true;
            }
            catch(Exception ee)
            {
                str_error_log = "ee:" + ee.Message.Replace("\r\n", "");
                return false;
            }
        }

        public bool wifi_up_load_get(string str_get_value, ref string[] str_wifi_up_value, ref string str_error)
        {
            try
            {
                //[ID]    Interval          Transfer       Bitrate         Jitter    Lost/Total        Datagrams
                //[  5]   0.00-10.00  sec   34.9 MBytes    29.3 Mbits/sec  0.257 ms  1/25071 (0.004%)  receiver
                string[] striparr = str_get_value.Split(new string[] { "\r\n" }, StringSplitOptions.None);
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
                string str_rate = result[5];
                // 丢包率
                string str_loss = result[7].Substring(result[7].IndexOf("(")+1).Replace(")", "");

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
                str_wifi_up_value[0] = str_rate;

                str_wifi_up_value[1] = str_loss.Replace("%", "").Trim();

                if (check_int_double(str_wifi_up_value[0]) == false || check_int_double(str_wifi_up_value[1]) == false)
                {
                    str_error = "up_rate or packetloss";
                    return false;
                }
                
                return true;
            }
            catch (Exception ee)
            {
                str_error = "ee:" + ee.Message.Replace("\r\n", "");
                return false;
            }
        }

        public bool wifi_down_load_get(string str_get_value, ref string[] str_wifi_down_value, ref string str_error)
        {//0 -- str_down_rate    1 -- down_Packet_Loss
            try
            {
                string[] striparr = str_get_value.Split(new string[] { "\r\n" }, StringSplitOptions.None);
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

                // 速率
                string str_rate = result[5];
                // 丢包率
                string str_loss = result[7].Substring(result[7].IndexOf("(")+1).Replace(")", "");

                str_rate = str_rate.Split()[0].Trim();
                //if (check_int_double(str_rate) == false)
                //{
                //    str_error = "up_rate";
                //    return false;
                //}
                //else
                //{
                //    // MBytes换算成MBits 1MBytes = 8MBits
                //    str_rate = (double.Parse(str_rate) * 8.0).ToString();
                //}
                str_wifi_down_value[0] = str_rate;

                str_wifi_down_value[1] = str_loss.Replace("%", "").Trim();

                if (check_int_double(str_wifi_down_value[0]) == false || check_int_double(str_wifi_down_value[1]) == false)
                {
                    str_error = "down_rate or packetloss";
                    return false;
                }
                return true;
            }
            catch (Exception ee)
            {
                str_error = "ee:" + ee.Message.Replace("\r\n", "");
                return false;
            }
        }

        public bool button_get(string str_get_value, ref string str_button, ref string str_error)
        {
            try
            {
                /*
               ubus send "d3_product_test" '{"PIR": "off" }'
16:12:11[1559319131.15844] DBUG0: [ubus_probe_config_event { "d3_product_test": {"PIR":"off"} }]
16:12:11[1559319131.24693] DBUG0: [PIR:off]
root@XDC01-f9be1:/# 16:12:11[1559319131.35252] DBUG0: [info print end]
16:12:55[1559319175.593284] DBUG0: [SystemParam.ButtonStatus = 1]
16:12:55[1559319175.976722] DBUG0: [SystemParam.ButtonRelease Trigger = 1]
                 */
                if (str_get_value.Contains("SystemParam.ButtonStatus = 1") && str_get_value.Contains("SystemParam.ButtonRelease Trigger = 1"))
                {
                    str_button = "OK";
                }
                else
                {
                    str_button = "NG";
                }
                return true;
            }
            catch (Exception ee)
            {
                str_error = "ee:" + ee.Message.Replace("\r\n", "");
                return false;
            }
        }

        public bool motion_get(string str_get_value, ref string str_motion, ref string str_error)
        {
            try
            {
                /*
             ubus send "d3_product_test" '{"PIR": "on" }'
16:20:38[1559319638.115465] DBUG0: [ubus_probe_config_event { "d3_product_test": {"PIR":"on"} }]
16:20:38[1559319638.124335] DBUG0: [PIR:on]
root@XDC01-f9be1:/# 16:20:38[1559319638.136521] DBUG0: [info print end]
16:20:43[1559319643.115583] DBUG0: [SystemParam.MotionTriggerEvent = 1 service_up 0]
                */
                if (str_get_value.Contains("SystemParam.MotionTriggerEvent = 1 service_up 0")) 
                { 
                    str_motion = "OK";
                }
                else
                {
                    str_motion = "NG";
                }
                return true;
            }
            catch (Exception ee)
            {
                str_error = "ee:" + ee.Message.Replace("\r\n", "");
                return false;
            }
        }

        public bool mac_address_generate(string str_mac_1_6, string str_mac_12, string str_mac_range_min, string str_mac_range_max,ref string str_mac, ref string str_mac_error)
        {
            try
            {   //mac---前6位数字固定,  后6位累加...
                if (mac_check(str_mac_1_6, str_mac_1_6.Length) == false)
                {
                    str_mac_error = "mac_1_6_[Not hex]";
                    return false;
                }
                if (mac_check(str_mac_12, str_mac_12.Length) == false)
                {
                    str_mac_error = "mac_7_12_[Not hex]";
                    return false;
                }
                if (mac_check(str_mac_range_min, str_mac_range_min.Length) == false)
                {
                    str_mac_error = "mac_min_[Not hex]";
                    return false;
                }
                if (mac_check(str_mac_range_max, str_mac_range_max.Length) == false)
                {
                    str_mac_error = "mac_max_[Not hex]";
                    return false;
                }
                int int_mac_min = 0;
                int int_mac_max = 0;
                int int_mac_7_12_now = 0;
                Change16_10(str_mac_12, ref int_mac_7_12_now);
                Change16_10(str_mac_range_min, ref int_mac_min);
                Change16_10(str_mac_range_max, ref int_mac_max);
                if (int_mac_7_12_now < int_mac_min || int_mac_7_12_now > int_mac_max)
                {
                    str_mac_error = "mac_out_range";
                    return false;
                }
                string str_write_mac_1_12 = str_mac_1_6 + str_mac_12;
                str_mac = str_write_mac_1_12.Substring(0, 2) + ":" + str_write_mac_1_12.Substring(2, 2) + ":" +
                          str_write_mac_1_12.Substring(4, 2) + ":" + str_write_mac_1_12.Substring(6, 2) + ":" +
                          str_write_mac_1_12.Substring(8, 2) + ":" + str_write_mac_1_12.Substring(10, 2);
                return true;
            }
            catch (Exception ee)
            {
                str_mac_error = "ee:" + ee.Message.Replace("\r\n", "");
                return false;
            }
        }

        public bool mac_write_get(string str_get_value, ref string str_mac, ref string str_error)
        {
            try
            {
                if (str_get_value.Contains("WiFi write map compare OK"))
                {
                    str_mac = "OK";
                }
                else
                {
                    str_mac = "NG";
                }
                return true;
            }
            catch (Exception ee)
            {
                str_error = "ee:" + ee.Message.Replace("\r\n", "");
                return false;
            }
        }

        //--------OK
        
        public bool mac_read_x(string str_get_value, ref string str_mac_address, ref string str_error)
        {
            try
            {
                /*
                1. # rtwpriv wlan0 efuse_get mac
wlan0    efuse_get:80:9F:9B:08:D1:12
2. # cat /sys/class/net/wlan0/address
80:9f:9b:08:d1:12
                 */
                string[] striparr = str_get_value.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                string str_mac_temp = striparr[1];
                str_mac_address = str_mac_temp.Replace("wlan0", "").Replace("efuse_get:", "").Trim();
                if (mac_check(str_mac_address, 17) == false)
                {
                    str_error = "mac address format error";
                    return false;
                }
                return true;
            }
            catch (Exception ee)
            {
                return false;
            }
        }

        /*
         ping 192.168.31.1 &
root@(none):/# PING 192.168.31.1 (192.168.31.1): 56 data bytes
64 bytes from 192.168.31.1: seq=0 ttl=64 time=9.953 ms
64 bytes from 192.168.31.1: seq=1 ttl=64 time=2.503 ms
64 bytes from 192.168.31.1: seq=2 ttl=64 time=14.379 ms
64 bytes from 192.168.31.1: seq=3 ttl=64 time=1.730 ms
64 bytes from 192.168.31.1: seq=4 ttl=64 time=6.557 ms
64 bytes from 192.168.31.1: seq=5 ttl=64 time=1.515 ms
64 bytes from 192.168.31.1: seq=6 ttl=64 time=2.666 ms
64 bytes from 192.168.31.1: seq=7 ttl=64 time=1.657 ms
64 bytes from 192.168.31.1: seq=8 ttl=64 time=1.912 ms
64 bytes from 192.168.31.1: seq=9 ttl=64 time=2.107 ms
64 bytes from 192.168.31.1: seq=10 ttl=64 time=7.478 ms
64 bytes from 192.168.31.1: seq=11 ttl=64 time=1.592 ms
64 bytes from 192.168.31.1: seq=12 ttl=64 time=6.326 ms
killall ping ?
killall: ?: no process killed
[1]+  Terminated                 ping 192.168.31.1
root@(none):/# 
         */
        public bool ping_result_get(string str_get_value, ref string str_ping, ref string str_error)
        {
            try
            {
                /*
                ping 192.168.31.1 &
[1]-  Done                       wpa_supplicant -B -D nl80211 -i wlan0 -c /skybell/config/net/wpa_supplicant.conf && udhcpc -iwlan0
# PING 192.168.31.1 (192.168.31.1): 56 data bytes
64 bytes from 192.168.31.1: seq=0 ttl=64 time=7.488 ms
E/[VMTK_VI] VMTK_VI_GetBuffs() wait frame time out !!! (2)
[VIC] Error!! device 0 No Signal !!
64 bytes from 192.168.31.1: seq=1 ttl=64 time=2.333 ms
64 bytes from 192.168.31.1: seq=2 ttl=64 time=2.386 ms
64 bytes from 192.168.31.1: seq=3 ttl=64 time=2.340 ms
64 bytes from 192.168.31.1: seq=4 ttl=64 time=2.407 ms
                */
                //先确认"64 bytes from"的次数
                int int_count = System.Text.RegularExpressions.Regex.Matches(str_get_value, "64 bytes from").Count;
                if (int_count <= 0)
                {
                    str_error = "Not ping any data";
                    return false;
                }
                string[] striparr = str_get_value.Split(new string[] { "\r\n" }, StringSplitOptions.None);//--- 分离出来的总行数
                string str_time = "";
                List<string> list_str_time = new List<string>();
                for (int i = 0; i < striparr.Length; i++)
                {
                    Application.DoEvents();
                    str_time = "";
                    if (striparr[i].Contains("64 bytes from"))
                    {
                        if (ping_separation(striparr[i], ref str_time) == false)
                        {
                            str_error = "ping time out";
                            return false;
                        }
                        list_str_time.Add(str_time);
                    }
                }
                double double_sum = 0;
                for (int i = 0; i < list_str_time.Count; i++)
                {
                    Application.DoEvents();
                    double_sum += Convert.ToDouble(list_str_time[i]);
                }
                double avg = double_sum / list_str_time.Count;
                str_ping = avg.ToString("0.000");
                return true;
            }
            catch (Exception ee)
            {
                str_error = "ee:" + ee.Message.Replace("\r\n", "");
                return false;
            }
        }

        private bool ping_separation(string str_separation_befor,ref string str_time)
        {
            try
            {   // 64 bytes from 192.168.31.1: seq=0 ttl=64 time=4.049 ms
                int int_time = str_separation_befor.IndexOf("time=");
                int int_ms = str_separation_befor.IndexOf("ms");
                str_time = str_separation_befor.Substring(int_time + 5, int_ms - int_time - 5).Trim();
                if (check_int_double(str_time) == false)
                {
                    str_time = "0";
                    return false;
                }
                return true;
            }
            catch (Exception ee)
            {
                return false;
            }
        }

        public bool mac_check(string str_0_9_A_F, int int_length)
        {
            try
            {
                string pattrn_06 = @"(^[0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F]$)";
                string pattrn_12 = @"(^[0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F]$)";
                string pattrn_17 = @"(^[0-9a-fA-F][0-9a-fA-F]:[0-9a-fA-F][0-9a-fA-F]:[0-9a-fA-F][0-9a-fA-F]:[0-9a-fA-F][0-9a-fA-F]:[0-9a-fA-F][0-9a-fA-F]:[0-9a-fA-F][0-9a-fA-F]$)";
                if (int_length == 6)
                {
                    string pattrn = pattrn_06;
                    if (!System.Text.RegularExpressions.Regex.IsMatch(str_0_9_A_F, pattrn))
                    {
                        return false;
                    }
                }
                if (int_length == 12)
                {
                    string pattrn = pattrn_12;
                    if (!System.Text.RegularExpressions.Regex.IsMatch(str_0_9_A_F, pattrn))
                    {
                        return false;
                    }
                }
                if (int_length == 17)//0E:0F:12:A5:B6:05
                {
                    string pattrn = pattrn_17;
                    if (!System.Text.RegularExpressions.Regex.IsMatch(str_0_9_A_F, pattrn))
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ee)
            {
                return false;
            }
        }

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
                if(System.Text.RegularExpressions.Regex.IsMatch(str_int_double, @"^[+-]?\d*[.]?\d*$") == true)
                {
                    bool_double = true;
                }
                if(!bool_int && !bool_double)
                {
                    return false;
                }
                return true;
            }
            catch(Exception ee)
            {
                return false;
            }
        }

        public bool txt_file_save(string str_file)
        {
            try
            {
                string str_path_write = System.Windows.Forms.Application.StartupPath + @"\Log_file" + @"\power_on_error_" + string.Format("{0:yyyy.MM.dd_HH.mm.ss}", DateTime.Now) + ".txt";
                if (System.IO.File.Exists(str_path_write) == false)
                {
                    System.IO.FileStream filest = new System.IO.FileStream(str_path_write, System.IO.FileMode.Append, System.IO.FileAccess.Write, System.IO.FileShare.None);
                    System.IO.StreamWriter sw = new System.IO.StreamWriter(filest);
                    //sw.Write("Log file" + "\r\n");
                    sw.WriteLine(str_file);//要写入的信息。 
                    sw.Close();
                    filest.Close();
                }
                else
                {
                    System.IO.FileStream filest = new System.IO.FileStream(str_path_write, System.IO.FileMode.Append, System.IO.FileAccess.Write, System.IO.FileShare.None);
                    System.IO.StreamWriter sw = new System.IO.StreamWriter(filest);
                    sw.WriteLine(str_file);//要写入的信息。 
                    sw.Close();
                    filest.Close();
                }
                return true;
            }
            catch(Exception ee)
            {
                return false;
            }
        }

        public bool txt_file_save(string str_file_path,string str_file_content)
        {
            try
            {
                //string str_path_write = System.Windows.Forms.Application.StartupPath + @"\Log_file" + @"\power_on_error_" + string.Format("{0:yyyy.MM.dd_HH.mm.ss}", DateTime.Now) + ".txt";

                if (System.IO.File.Exists(str_file_path) == false)
                {
                    System.IO.FileStream filest = new System.IO.FileStream(str_file_path, System.IO.FileMode.Append, System.IO.FileAccess.Write, System.IO.FileShare.None);
                    System.IO.StreamWriter sw = new System.IO.StreamWriter(filest);
                    //sw.Write("Log file" + "\r\n");
                    sw.WriteLine(str_file_content);//要写入的信息。 
                    sw.Close();
                    filest.Close();
                }
                else
                {
                    System.IO.FileStream filest = new System.IO.FileStream(str_file_path, System.IO.FileMode.Append, System.IO.FileAccess.Write, System.IO.FileShare.None);
                    System.IO.StreamWriter sw = new System.IO.StreamWriter(filest);
                    sw.WriteLine(str_file_content);//要写入的信息。 
                    sw.Close();
                    filest.Close();
                }
                return true;
            }
            catch (Exception ee)
            {
                return false;
            }
        }

        #region "Change16_10 and Change10_16"
        private bool Change16_10(string str_x16, ref int int_x10)//OK
        {
            try
            {
                int_x10 = Convert.ToInt32(str_x16, 16);
                return true;
            }
            catch (Exception ee)
            {
                return false;
            }
        }

        private bool Change10_16(int x10, ref string str_x16)//
        {
            try
            {
                //return Convert.ToChar(x10).ToString();
                str_x16 = x10.ToString("x1").ToUpper();
                switch (str_x16.Length)
                {
                    case 1:
                        str_x16 = "00000" + str_x16;
                        break;
                    case 2:
                        str_x16 = "0000" + str_x16;
                        break;
                    case 3:
                        str_x16 = "000" + str_x16;
                        break;
                    case 4:
                        str_x16 = "00" + str_x16;
                        break;
                    case 5:
                        str_x16 = "0" + str_x16;
                        break;
                }
                return true;
            }
            catch (Exception ee)
            {
                return false;
            }

        }
        #endregion
    }

    public class System_TranslateValue
    {
        public bool TranslateValue_voltage(byte[] byte_in, ref double vol_out)
        {
            try
            {
                int _vol_value = byte_in[5] * 256 + byte_in[4];
                if (TranslateValue(_vol_value, 3, ref vol_out) == false)
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

        public bool TranslateValue_current(byte[] byte_in, ref double current_out)
        {
            try
            {
                int _vol_value = byte_in[5] * 256 + byte_in[4];
                if (TranslateValue(_vol_value, 4, ref current_out) == false)
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

        public bool TranslateValue_current_1(byte[] byte_in, int _N, ref double current_out)
        {
            try
            {
                int _vol_value = byte_in[5] * 256 + byte_in[4];
                if (TranslateValue(_vol_value, _N, ref current_out) == false)
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

        private bool TranslateValue(int in_data, int _N, ref double ret_value)
        {
            try
            {
                if (in_data == 32768)
                {
                    return false;
                }
                if (in_data > 32768)
                {
                    in_data = in_data - 65536;
                }
                ret_value = (double)((double)in_data / Math.Pow(10, _N));
                return true;
            }
            catch (Exception ee)
            {
                return false;
            }
        }
    }
}
   
   
