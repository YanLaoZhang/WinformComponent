using CameraRTOSLib;
using CloudAPILib;
using ConfigFileLib;
using FLUKE8808ALib;
using LogLib;
using MySql.Data.MySqlClient;
using PCCommandLib;
using RelaySerialLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TDMSerialLib;
using TestDataLib;
using XDC01Action;
using XDC01Debug;
using XDC01SerialLib;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace XDC01_TestAction
{
    public partial class Form1 : Form
    {
        // 调试界面
        XDC01DebugForm xDC01DebugForm;
        // 测试记录查询
        Data_information data_InfoForm;

        // 配置文件路径
        private string Path_ini = System.Windows.Forms.Application.StartupPath + @"\MyConfig.config";

        // 日志
        Logger logger;
        public string LogPath = System.Windows.Forms.Application.StartupPath + @"\Log_file";

        // 测试规格
        TestSpecification testSpecification;
        TestSpecMax specMax;
        TestSpecMin specMin;
        TestStandard specStandard;

        // 运行参数
        TestParam testParam;

        // XDC01串口
        private XDC01Serial serial;

        // PC端操作
        private PCCommand pcCommand;

        // 麦克风测试音频文件
        public string wavePath = System.Windows.Forms.Application.StartupPath + @"\wav";

        // rtos截图
        public string Path_iamge_document = System.Windows.Forms.Application.StartupPath + @"\Image_file" + @"\" + DateTime.Now.ToString("yyyy.MM.dd"); //文件夹路径
        public string Path_image_file = "";//当前保存文件路径

        #region XDC01调试界面
        private void OpenXDC01DebugForm()
        {
            xDC01DebugForm = new XDC01DebugForm();
            xDC01DebugForm.FormClosed += XDC01DebugForm_FormClosed;  // 订阅XDC03DebugForm的关闭事件
            xDC01DebugForm.Show();
        }

        private void XDC01DebugForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            xDC01DebugForm = null;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (serial != null)
            {
                serial.ClosePort();
            }
            if (xDC01DebugForm == null)
            {
                OpenXDC01DebugForm();
            }
            else
            {
                xDC01DebugForm.WindowState = FormWindowState.Normal;
                xDC01DebugForm.TopMost = true;
            }
        }
        #endregion

        #region 测试记录
        private void OpenDataInfoForm()
        {
            data_InfoForm = new Data_information("");
            data_InfoForm.FormClosed += DataInfoForm_FormClosed;
            data_InfoForm.Show();
        }

        private void DataInfoForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            data_InfoForm = null;
        }

        /// <summary>
        /// 查询测试记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (data_InfoForm == null)
            {
                OpenDataInfoForm();
            }
            else
            {
                data_InfoForm.WindowState = FormWindowState.Normal;
                data_InfoForm.TopMost = true;
            }
        }
        #endregion

        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            logger = new Logger(ref richTextBoxRunLog, ref LogPath);
            InitTestParam();
            InitialTestSpec();
            serial = new XDC01Serial(serialPort1, testParam.serial_port, richTextBoxSerialLog);
            serial.OpenPort();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(serial != null)
            {
                serial.ClosePort();
            }
        }

        public void InitTestParam()
        {
            try
            {
                testParam = new TestParam
                {
                    db_ip = ConfigFile.IniReadValue("Server", "db_ip", Path_ini),
                    db_port = ConfigFile.IniReadValue("Server", "db_port", Path_ini),
                    spec_id = ConfigFile.IniReadValue("Server", "spec_id", Path_ini),
                    standard_id = ConfigFile.IniReadValue("Server", "standard_id", Path_ini),

                    serial_port = ConfigFile.IniReadValue("Run_Param", "serial_port", Path_ini),
                    cur_tagnumber = ConfigFile.IniReadValue("Run_Param", "cur_tagnumber", Path_ini),
                    test_mode = ConfigFile.IniReadValue("Run_Param", "test_mode", Path_ini),
                    stage_name = ConfigFile.IniReadValue("Run_Param", "stage_name", Path_ini),
                    ng_continue = ConfigFile.IniReadValue("Run_Param", "ng_continue", Path_ini),

                    ping_ip = ConfigFile.IniReadValue("wifi_param", "ping_ip", Path_ini),
                    ping_count = ConfigFile.IniReadValue("wifi_param", "ping_count", Path_ini),

                    led_interval = ConfigFile.IniReadValue("led_color", "interval", Path_ini),

                    mic_record_duration = ConfigFile.IniReadValue("mic_param", "record_duration", Path_ini),
                    mic_wave_file = ConfigFile.IniReadValue("mic_param", "wave_file", Path_ini),

                    cur_wifi_ssid = ConfigFile.IniReadValue("wifi_throughput", "cur_wifi_ssid", Path_ini),
                    cur_wifi_pwd = ConfigFile.IniReadValue("wifi_throughput", "cur_wifi_pwd", Path_ini),
                    up_rate_corrected = ConfigFile.IniReadValue("wifi_throughput", "up_rate_corrected", Path_ini),
                    down_rate_corrected = ConfigFile.IniReadValue("wifi_throughput", "down_rate_corrected", Path_ini),
                    server_ip = ConfigFile.IniReadValue("wifi_throughput", "server_ip", Path_ini),
                    duration = ConfigFile.IniReadValue("wifi_throughput", "duration", Path_ini),
                    bandwidth = ConfigFile.IniReadValue("wifi_throughput", "bandwidth", Path_ini),

                    rf_tx_delay = ConfigFile.IniReadValue("rf_tx", "rf_tx_delay", Path_ini),
                    rf_power_corrected = ConfigFile.IniReadValue("rf_tx", "rf_power_corrected", Path_ini),

                    rf_rx_timeout = ConfigFile.IniReadValue("rf_rx", "rf_rx_timeout", Path_ini),

                    next_tagnumber = ConfigFile.IniReadValue("next_station", "next_tagnumber", Path_ini),
                    next_wifi_ssid = ConfigFile.IniReadValue("next_station", "next_wifi_ssid", Path_ini),
                    next_wifi_pwd = ConfigFile.IniReadValue("next_station", "next_wifi_pwd", Path_ini),
                    write_next_wifi = ConfigFile.IniReadValue("next_station", "write_next_wifi", Path_ini),

                    cloud_username = ConfigFile.IniReadValue("cloud", "cloud_username", Path_ini),
                    cloud_password = ConfigFile.IniReadValue("cloud", "cloud_password", Path_ini),
                    factoryId = ConfigFile.IniReadValue("cloud", "factoryId", Path_ini),
                    colorId = ConfigFile.IniReadValue("cloud", "colorId", Path_ini),
                    productId = ConfigFile.IniReadValue("cloud", "productId", Path_ini),
                    modeId = ConfigFile.IniReadValue("cloud", "modeId", Path_ini),
                    productType = ConfigFile.IniReadValue("cloud", "productType", Path_ini),

                    local_server_name = ConfigFile.IniReadValue("rtos", "local_server_name", Path_ini),

                    charge_relay_index = ConfigFile.IniReadValue("voltage_current", "charge_relay_index", Path_ini),
                    voltage_tdm_port = ConfigFile.IniReadValue("voltage_current", "voltage_tdm_port", Path_ini),
                    current_tdm_port = ConfigFile.IniReadValue("voltage_current", "current_tdm_port", Path_ini),
                    current_fluke_port = ConfigFile.IniReadValue("voltage_current", "current_fluke_port", Path_ini),
                    relay_port = ConfigFile.IniReadValue("voltage_current", "relay_port", Path_ini),
                };
            }
            catch (Exception ee)
            {
                logger.ShowLog($"获取运行参数发生异常：[{ee.Message}]");
            }
        }

        /// <summary>
        /// 初始化测试规格
        /// </summary>
        public void InitialTestSpec()
        {
            
            testSpecification = new TestSpecification(testParam.db_ip, int.Parse(testParam.db_port));
            specMin = new TestSpecMin();
            specMax = new TestSpecMax();
            specStandard = new TestStandard();
            string str_error_log = "";
            if(testSpecification.GetTestSpec(int.Parse(testParam.spec_id), ref specMax, ref specMin, ref str_error_log) == false)
            {
                logger.ShowLog(str_error_log);
            }
            else
            {
                InitialDataGridViewTestSpec(specMax, specMin);
            }
            str_error_log = "";
            if (testSpecification.GetTestStandard(int.Parse(testParam.standard_id), ref specStandard, ref str_error_log) == false)
            {
                logger.ShowLog(str_error_log);
            }
            else
            {
                InitialDataGridViewTestStandard(specStandard);
            }
        }

        /// <summary>
        /// 初始化展示测试规格参数值
        /// </summary>
        /// <param name="testSpecMax"></param>
        /// <param name="testSpecMin"></param>
        public void InitialDataGridViewTestSpec(TestSpecMax testSpecMax, TestSpecMin testSpecMin)
        {
            dataGridViewTestSpec.Rows.Clear();
            int i = 1;
            dataGridViewTestSpec.Rows.Add(i++, "vol_wifi", testSpecMin.vol_wifi, testSpecMax.vol_wifi);
            dataGridViewTestSpec.Rows.Add(i++, "vol_vcc_core", testSpecMin.vol_vcc_core, testSpecMax.vol_vcc_core);
            dataGridViewTestSpec.Rows.Add(i++, "vol_sensor1", testSpecMin.vol_sensor1, testSpecMax.vol_sensor1);
            dataGridViewTestSpec.Rows.Add(i++, "vol_sensor2", testSpecMin.vol_sensor2, testSpecMax.vol_sensor2);
            dataGridViewTestSpec.Rows.Add(i++, "vol_rtc", testSpecMin.vol_rtc, testSpecMax.vol_rtc);
            dataGridViewTestSpec.Rows.Add(i++, "vol_vcc", testSpecMin.vol_vcc, testSpecMax.vol_vcc);
            dataGridViewTestSpec.Rows.Add(i++, "vol_mcu", testSpecMin.vol_mcu, testSpecMax.vol_mcu);
            dataGridViewTestSpec.Rows.Add(i++, "vol_ddr", testSpecMin.vol_ddr, testSpecMax.vol_ddr);
            dataGridViewTestSpec.Rows.Add(i++, "work_current", testSpecMin.work_current, testSpecMax.work_current);
            dataGridViewTestSpec.Rows.Add(i++, "charge_current", testSpecMin.charge_current, testSpecMax.charge_current);
            dataGridViewTestSpec.Rows.Add(i++, "standby_current", testSpecMin.standby_current, testSpecMax.standby_current);
            dataGridViewTestSpec.Rows.Add(i++, "ping_rtt", testSpecMin.ping_rtt, testSpecMax.ping_rtt);
            dataGridViewTestSpec.Rows.Add(i++, "light", testSpecMin.light_val, testSpecMax.light_val);
            dataGridViewTestSpec.Rows.Add(i++, "battery_voltage", testSpecMin.battery_voltage, testSpecMax.battery_voltage);
            dataGridViewTestSpec.Rows.Add(i++, "cpu_temperature", testSpecMin.cpu_temperature, testSpecMax.cpu_temperature);
            dataGridViewTestSpec.Rows.Add(i++, "wifi_up_rate", testSpecMin.wifi_up_rate, testSpecMax.wifi_up_rate);
            dataGridViewTestSpec.Rows.Add(i++, "wifi_up_loss", testSpecMin.wifi_up_loss, testSpecMax.wifi_up_loss);
            dataGridViewTestSpec.Rows.Add(i++, "wifi_down_rate", testSpecMin.wifi_down_rate, testSpecMax.wifi_down_rate);
            dataGridViewTestSpec.Rows.Add(i++, "wifi_down_loss", testSpecMin.wifi_down_loss, testSpecMax.wifi_down_loss);
            dataGridViewTestSpec.Rows.Add(i++, "rf_tx_frequency", testSpecMin.rf_frequency, testSpecMax.rf_frequency);
            dataGridViewTestSpec.Rows.Add(i++, "rf_tx_power", testSpecMin.rf_power, testSpecMax.rf_power);
        }

        public void InitialDataGridViewTestStandard(TestStandard testStandard)
        {
            dataGridViewTestStandard.Rows.Clear();
            int i = 1;
            dataGridViewTestStandard.Rows.Add(i++, "Firmware Version", testStandard.firmware);
            dataGridViewTestStandard.Rows.Add(i++, "Hardware Version", testStandard.hardware);
            dataGridViewTestStandard.Rows.Add(i++, "MCU Version", testStandard.mcu);
            dataGridViewTestStandard.Rows.Add(i++, "RN长度", testStandard.rn_length);
        }

        private void timerTestTime_Tick(object sender, EventArgs e)
        {
            textBoxTime.Text = (int.Parse(textBoxTime.Text) + 1).ToString();
        }

        public void RunTest()
        {
            // 先获取运行参数
            InitTestParam();
            // 初始化测试规格
            InitialTestSpec();
            pcCommand = new PCCommand(richTextBoxPCCMD);

            dataGridView1.Rows.Clear();
            richTextBoxRunLog.Clear();

            Model model = new Model
            {
                str_software_version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(),
                str_spec_id = testParam.spec_id,
                str_standard_id = testParam.standard_id,
                str_fixture_id = "fixture1",
                str_operator_id = "No.001",
                str_start_test_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                str_stage_name = testParam.stage_name,
                str_test_mode = testParam.test_mode,
            };

            TestAction testAction = new TestAction();
            #region 检查RN和工序号
            List<TestItem> RNandTagnumber = testAction.CheckRNandTagname(serial, dataGridView1, logger, testParam, specStandard);
            
            if (RNandTagnumber == null)
            {
                if(testParam.ng_continue == "false")
                {
                    logger.ShowLog($"RN号或工序号检查不通过，无法进行后续测试");
                    return;
                }
            }
            else
            {
                foreach (TestItem item in RNandTagnumber)
                {
                    if(item.NgItem == "rn")
                    {
                        model.str_rn = item.StrVal;
                    }
                    if(item.NgItem == "tagnumber")
                    {
                        model.str_read_tagnumber = item.StrVal;
                    }

                    if (item.Result == "FAIL")
                    {
                        logger.ShowLog($"{item.Name}检查不通过，无法进行后续测试");
                        return;
                    }
                }
            }
            #endregion

            #region 检查设备信息
            List<TestItem> DUTInfo = testAction.CheckDUTInfo(serial, dataGridView1, logger, specMax, specMin, specStandard);
            if(DUTInfo == null)
            {
                if (testParam.ng_continue == "false")
                {
                    logger.ShowLog($"设备信息检查不通过，无法进行后续测试");
                    return;
                }
            }
            else
            {
                foreach(TestItem item in DUTInfo)
                {
                    if(item.NgItem == "fw_version")
                    {
                        model.str_fw_version = item.StrVal;
                    }
                    if (item.NgItem == "hw_version")
                    {
                        model.str_hw_version = item.StrVal;
                    }
                    if (item.NgItem == "mcu_version")
                    {
                        model.str_mcu_version = item.StrVal;
                    }
                    if (item.NgItem == "battery_voltage")
                    {
                        model.str_battery_voltage = item.Value.ToString("F3");
                    }
                    if (item.NgItem == "cpu_temperature")
                    {
                        model.str_cpu_temperature = item.Value.ToString("F3");
                    }
                    if (item.Result == "FAIL")
                    {
                        model.str_ng_item += $"{item.NgItem},";
                    }
                }
            }
            #endregion

            #region ping网测试
            TestItem pingTest = testAction.CheckWiFi(serial, dataGridView1, logger, specMax, specMin, testParam);
            if(pingTest == null)
            {
                model.str_ping_rtt = "-1";
                if (testParam.ng_continue == "false")
                {
                    logger.ShowLog($"Ping测试异常，无法进行后续测试");
                    return;
                }
            }
            else
            {
                model.str_ping_rtt = pingTest.Value.ToString();
                if(pingTest.Result == "FAIL")
                {
                    model.str_ng_item += $"{pingTest.NgItem},";
                }
            }
            #endregion

            #region 麦克风测试
            axWindowsMediaPlayer1.settings.volume = 100;
            axWindowsMediaPlayer1.settings.playCount = 1;
            axWindowsMediaPlayer1.URL = $@"{wavePath}\{testParam.mic_wave_file}";
            TestItem mic_auto = testAction.CheckMicrophoneAuto(serial, dataGridView1, logger, pcCommand, axWindowsMediaPlayer1, testParam);
            if (mic_auto == null)
            {
                if (testParam.ng_continue == "false")
                {
                    logger.ShowLog($"麦克风测试(自动)异常，无法进行后续测试");
                    return;
                }
            }
            else
            {
                model.str_mic_data = mic_auto.StrVal;
                model.str_mic_auto = mic_auto.Result;
                if(mic_auto.Result == "FAIL")
                {
                    model.str_ng_item += $"{mic_auto.NgItem},";
                    TestItem mic_manual = testAction.CheckMicrophoneManual(serial, dataGridView1, logger, testParam);
                    if(mic_manual == null)
                    {
                        if (testParam.ng_continue == "false")
                        {
                            logger.ShowLog($"麦克风测试(人工)异常，无法进行后续测试");
                            return;
                        }
                    }
                    else
                    {
                        model.str_mic_manual = mic_manual.Result;
                        if(mic_manual.Result == "FAIL")
                        {
                            model.str_ng_item += $"{mic_manual.NgItem},";
                        }
                    }
                }
            }
            #endregion

            #region 按键和喇叭测试
            List<TestItem> btn_audio =  testAction.CheckButtonAndSpeaker(serial, dataGridView1, logger);
            if (btn_audio == null)
            {
                if (testParam.ng_continue == "false")
                {
                    logger.ShowLog($"按键和喇叭测试异常，无法进行后续测试");
                    return;
                }
            }
            else
            {
                foreach (TestItem testItem in btn_audio)
                {
                    if(testItem.NgItem == "button")
                    {
                        model.str_button = testItem.Result;
                    }
                    if(testItem.NgItem == "audio")
                    {
                        model.str_audio = testItem.Result;
                    }
                    if(testItem.Result == "FAIL")
                    {
                        model.str_ng_item += $"{testItem.NgItem},";
                    }
                }
            }
            #endregion

            #region 移动感应测试
            TestItem pirTest = testAction.CheckPIRMotion(serial, dataGridView1, logger);
            if (pirTest == null)
            {
                if (testParam.ng_continue == "false")
                {
                    logger.ShowLog($"移动感应测试异常，无法进行后续测试");
                    return;
                }
            }
            else
            {
                model.str_motion = pirTest.Result;
                if(pirTest.Result == "FAIL")
                {
                    model.str_ng_item += $"{pirTest.NgItem},";
                }
            }
            #endregion

            #region LED颜色测试
            TestItem led_color = testAction.CheckLEDColor(serial, dataGridView1, logger, testParam);
            if (led_color == null)
            {
                if (testParam.ng_continue == "false")
                {
                    logger.ShowLog($"LED颜色测试异常，无法进行后续测试");
                    return;
                }
            }
            else
            {
                model.str_led_color = led_color.Result;
                if(led_color.Result == "FAIL")
                {
                    model.str_ng_item += $"{led_color.NgItem},";
                }
            }
            #endregion

            #region VLC视频检查
            List<TestItem> rtsp = testAction.CheckRTSP(serial, dataGridView1, logger);
            if (rtsp == null)
            {
                if (testParam.ng_continue == "false")
                {
                    logger.ShowLog($"VLC视频检查异常，无法进行后续测试");
                    return;
                }
            }
            else
            {
                foreach (TestItem testItem in rtsp)
                {
                    if(testItem.NgItem == "vlc_rtsp")
                    {
                        model.str_vlc_rtsp = testItem.Result;
                    }
                    if (testItem.NgItem == "ir_led")
                    {
                        model.str_ir_led = testItem.Result;
                    }
                    if (testItem.NgItem == "ir_cut")
                    {
                        model.str_ir_cut = testItem.Result;
                    }
                    if(testItem.Result == "FAIL")
                    {
                        model.str_ng_item += $"{testItem.NgItem},";
                    }
                }
            }
            #endregion

            #region wifi吞吐量
            List<TestItem> wifiThroughput = testAction.CheckWiFiThroughput(serial, pcCommand, dataGridView1, logger, specMax, specMin, testParam);
            if (wifiThroughput == null)
            {
                if (testParam.ng_continue == "false")
                {
                    logger.ShowLog($"wifi吞吐量测试异常，无法进行后续测试");
                    return;
                }
            }
            else
            {
                foreach(TestItem testItem in wifiThroughput)
                {
                    if(testItem.NgItem == "wifi_up_rate")
                    {
                        model.str_wifi_up_rate = testItem.Value.ToString();
                        model.str_up_rate_revise = testParam.up_rate_corrected;
                    }
                    if (testItem.NgItem == "wifi_up_loss")
                    {
                        model.str_wifi_up_loss = testItem.Value.ToString();
                    }
                    if (testItem.NgItem == "wifi_down_rate")
                    {
                        model.str_wifi_down_rate = testItem.Value.ToString();
                        model.str_down_rate_revise = testParam.down_rate_corrected;
                    }
                    if (testItem.NgItem == "wifi_down_loss")
                    {
                        model.str_wifi_down_loss = testItem.Value.ToString();
                    }
                    if (testItem.Result == "FAIL")
                    {
                        model.str_ng_item += $"{testItem.NgItem},";
                    }
                }
            }
            #endregion

            #region 亮度值
            TestItem light = testAction.CheckLightSensor(serial, dataGridView1, logger, specMax, specMin);
            if (light == null)
            {
                if (testParam.ng_continue == "false")
                {
                    logger.ShowLog($"亮度值测试异常，无法进行后续测试");
                    return;
                }
            }
            else
            {
                model.str_light = light.Value.ToString();
                if(light.Result == "FAIL")
                {
                    model.str_ng_item += $"{light.NgItem},";
                }
            }
            #endregion

            #region RF TX
            DSA700Lib.CVisaOpt_control _DSA700 = new DSA700Lib.CVisaOpt_control();
            List<TestItem> rf_tx = testAction.CheckRFTxTest(serial, dataGridView1, logger, specMax, specMin, specStandard, _DSA700, testParam);
            if (rf_tx == null)
            {
                model.str_rf_tx_frenquency = "0";
                model.str_rf_tx_power = "0";
                model.str_rf_tx_power_revise = "0";
                if (testParam.ng_continue == "false")
                {
                    logger.ShowLog($"RF发送测试异常，无法进行后续测试");
                    return;
                }
            }
            else
            {
                foreach (TestItem testItem in rf_tx)
                {
                    if(testItem.NgItem == "rf_tx_frenquency")
                    {
                        model.str_rf_tx_frenquency = testItem.Value.ToString();
                    }
                    if (testItem.NgItem == "rf_tx_power")
                    {
                        model.str_rf_tx_power = testItem.Value.ToString();
                        model.str_rf_tx_power_revise = testParam.rf_power_corrected;
                    }
                    if (testItem.Result == "FAIL")
                    {
                        model.str_ng_item += $"{testItem.NgItem},";
                    }
                }
            }
            #endregion

            #region RF RX
            TestItem rf_rx = testAction.CheckRFRxTest(serial, dataGridView1, logger, testParam);
            if (rf_rx == null)
            {
                if (testParam.ng_continue == "false")
                {
                    logger.ShowLog($"RF接收测试异常，无法进行后续测试");
                    return;
                }
            }
            else
            {
                model.str_rf_rx = rf_rx.Result;
                if(rf_rx.Result == "FAIL")
                {
                    model.str_ng_item += $"{rf_rx.NgItem},";
                }
            }
            #endregion

            #region 申请SN/uid/mac
            string str_error_log = "";
            if(serial.GetSystemMac(ref model.str_mac, ref str_error_log))
            {
                CloudLoginForm cloudLoginForm = new CloudLoginForm();
                CloudModel cloudModel = new CloudModel
                {
                    str_username = testParam.cloud_username,
                    str_password = testParam.cloud_password,
                    str_factoryId = testParam.factoryId,
                    str_colorId = testParam.colorId,
                    str_productId = testParam.productId,
                    str_modeId = testParam.modeId,
                    str_productType = testParam.productType,
                    str_rn = model.str_rn,
                    str_mac = model.str_mac,
                };
                TestItem apply_cloud = testAction.ApplySNandUIDFromCloud(cloudLoginForm, dataGridView1, logger, cloudModel);
                if(apply_cloud == null)
                {
                    if (testParam.ng_continue == "false")
                    {
                        logger.ShowLog($"云端申请SN|UID|MAC异常，无法进行后续测试");
                        return;
                    }
                }
                else
                {
                    model.str_sn = cloudModel.str_sn;
                    model.str_uid = cloudModel.str_uid;
                    model.str_mac_cloud = cloudModel.str_mac_cloud;
                    if (apply_cloud.Result == "FAIL")
                    {
                        model.str_ng_item += $"{apply_cloud.NgItem},";
                    }
                }

                TestItem check_cloud = testAction.CheckSNandUIDFromCloud(cloudLoginForm, dataGridView1, logger, cloudModel);
                if(check_cloud == null)
                {
                    if (testParam.ng_continue == "false")
                    {
                        logger.ShowLog($"云端检查SN|UID|MAC异常，无法进行后续测试");
                        return;
                    }
                }
                else
                {
                    model.str_check_cloud = check_cloud.Result;
                    if(check_cloud.Result == "FAIL")
                    {
                        model.str_ng_item += $"{check_cloud.NgItem},";
                    }
                }
            }
            else
            {
                logger.ShowLog($"获取mac失败：[{str_error_log}], 无法向云端申请");
            }
            #endregion

            #region 复位键
            TestItem reset_btn = testAction.CheckResetButton(serial, dataGridView1, logger);
            if(reset_btn == null)
            {
                if (testParam.ng_continue == "false")
                {
                    logger.ShowLog($"复位键测试异常，无法进行后续测试");
                    return;
                }
            }
            else
            {
                model.str_reset_button = reset_btn.Result;
                if(reset_btn.Result == "FAIL")
                {
                    model.str_ng_item += $"{reset_btn.NgItem},";
                }
            }
            #endregion

            #region 恢复出厂设置
            TestItem factory_reset = testAction.SetFactoryReset(serial, dataGridView1, logger);
            if(factory_reset == null)
            {
                if (testParam.ng_continue == "false")
                {
                    logger.ShowLog($"恢复出厂设置异常，无法进行后续测试");
                    return;
                }
            }
            else
            {
                model.str_factory_reset = factory_reset.Result;
                if(factory_reset.Result == "FAIL")
                {
                    model.str_ng_item += $"{factory_reset.NgItem},";
                }
            }
            #endregion

            // 保存数据库
            model.str_end_test_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            if(model.str_ng_item.Length == 0)
            {
                bool isWriteWifi = true;
                #region 写入下一站wifi
                if (testParam.write_next_wifi == "True")
                {
                    TestItem writeWIFI = testAction.SetNextStationWiFi(serial, dataGridView1, logger, testParam);
                    if (writeWIFI == null)
                    {
                        if (testParam.ng_continue == "false")
                        {
                            logger.ShowLog($"写入下一站WIFI异常，无法进行后续测试");
                            return;
                        }
                    }
                    else
                    {
                        model.str_write_wifi = writeWIFI.Result;
                        if (writeWIFI.Result == "FAIL")
                        {
                            model.str_ng_item += $"{writeWIFI.NgItem},";
                            isWriteWifi = false;
                        }
                    }
                }
                #endregion

                bool isWriteTag = true;
                #region 写入下一站工序号
                TestItem writeTagnumber = testAction.WriteTagNumber(serial, dataGridView1, logger, testParam);
                if (writeTagnumber == null)
                {
                    if (testParam.ng_continue == "false")
                    {
                        logger.ShowLog($"写入下一站工序异常，无法进行后续测试");
                        return;
                    }
                }
                else
                {
                    model.str_write_tagnumber = testParam.next_tagnumber;
                    if (writeTagnumber.Result == "FAIL")
                    {
                        model.str_ng_item += $"{writeTagnumber.NgItem},";
                        isWriteTag = false;
                    }
                }
                #endregion

                if (isWriteWifi && isWriteTag)
                {
                    model.str_test_result = "PASS";

                    labelResult.Text = "PASS";
                    panelResult.BackColor = Color.Green;
                }
                else
                {
                    model.str_test_result = "FAIL";
                    labelResult.Text = "FAIL";
                    panelResult.BackColor = Color.Red;
                }
            }
            else
            {
                model.str_test_result = "FAIL";
                labelResult.Text = "FAIL";
                panelResult.BackColor = Color.Red;
            }
            string str_sql = $"INSERT INTO `t030_product_test_report` " +
                $"(`software_version`, `spec_id`, `standard_id`, `fixture_id`, `operator_id`, `rn`, " +
                $"`start_test_time`, `end_test_time`, `stage_name`, `test_mode`, `test_result`, " +
                $"`ng_items`, `read_tagnumber`, `write_tagnumber`, `ping_rtt`, " +
                $"`mic_auto`, `mic_data`, `mic_manual`, `led_color`, `motion`, `button`, `audio`, " +
                $"`fw_version`, `hw_version`, `mcu_version`, `battery_voltage`, `cpu_temperature`, " +
                $"`light`, `vlc_rtsp`, `ir_cut`, `ir_led`, `wifi_up_loss`, `wifi_up_rate`, `up_rate_revise`, " +
                $"`wifi_down_loss`, `wifi_down_rate`, `down_rate_revise`, `rf_rx`, `rf_tx_frenquency`, " +
                $"`rf_tx_power`, `rf_tx_power_revise`, `reset_button`, `factory_reset`, " +
                $"`mac`, `sn`, `uid`, `mac_cloud`, `check_sn_uid`)" +
                $" VALUES ('{model.str_software_version}', '{model.str_spec_id}', '{model.str_standard_id}', '{model.str_fixture_id}', '{model.str_operator_id}', '{model.str_rn}'," +
                $" '{model.str_start_test_time}', '{model.str_end_test_time}', '{model.str_stage_name}', '{model.str_test_mode}', '{model.str_test_result}'," +
                $" '{model.str_ng_item}', '{model.str_read_tagnumber}', '{model.str_write_tagnumber}', {model.str_ping_rtt}," +
                $" '{model.str_mic_auto}', '{model.str_mic_data}', '{model.str_mic_manual}', '{model.str_led_color}', '{model.str_motion}', '{model.str_button}', '{model.str_audio}'," +
                $" '{model.str_fw_version}', '{model.str_hw_version}', '{model.str_mcu_version}', {model.str_battery_voltage}, {model.str_cpu_temperature}," +
                $" '{model.str_light}', '{model.str_vlc_rtsp}', '{model.str_ir_cut}', '{model.str_ir_led}', '{model.str_wifi_up_loss}', '{model.str_wifi_up_rate}', '{model.str_up_rate_revise}'," +
                $" '{model.str_wifi_down_loss}', '{model.str_wifi_down_rate}', '{model.str_down_rate_revise}', '{model.str_rf_rx}', '{model.str_rf_tx_frenquency}'," +
                $" '{model.str_rf_tx_power}', '{model.str_rf_tx_power_revise}', '{model.str_reset_button}', '{model.str_factory_reset}'," +
                $" '{model.str_mac}', '{model.str_sn}', '{model.str_uid}', '{model.str_mac_cloud}', '{model.str_check_cloud}');";
            SaveLocalSqliteDB(str_sql);
            UploadToLocalServer(str_sql);
        }

        #region RTOS
        public void RunTestRTOS()
        {
            // 先获取运行参数
            InitTestParam();
            // 初始化测试规格
            InitialTestSpec();

            dataGridView1.Rows.Clear();
            richTextBoxRunLog.Clear();

            Model model = new Model
            {
                str_software_version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(),
                str_spec_id = testParam.spec_id,
                str_standard_id = testParam.standard_id,
                str_fixture_id = "fixture1",
                str_operator_id = "No.001",
                str_start_test_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                str_stage_name = testParam.stage_name,
                str_test_mode = testParam.test_mode,
            };
            string image_time = string.Format("{0:_HHmmss}", DateTime.Now);

            TestAction testAction = new TestAction();
            // 检查RN和工序号
            List<TestItem> RNandTagnumber = testAction.CheckRNandTagnameRTOS(serial, dataGridView1, logger, testParam, specStandard);

            if (RNandTagnumber == null)
            {
                logger.ShowLog($"RN号或工序号检查不通过，无法进行后续测试");
                if (testParam.ng_continue == "false")
                {
                    return;
                }
            }
            else
            {
                foreach (TestItem item in RNandTagnumber)
                {
                    if (item.NgItem == "rn")
                    {
                        model.str_rn = item.StrVal;
                    }
                    if (item.NgItem == "tagnumber")
                    {
                        model.str_read_tagnumber = item.StrVal;
                    }

                    if (item.Result == "FAIL")
                    {
                        logger.ShowLog($"{item.Name}检查不通过，无法进行后续测试");
                        return;
                    }
                }
            }

            // 打开摄像头
            string str_error_log = "";
            if (serial.OpenCameraRTOS(ref str_error_log))
            {
                ShowUSBCamera showUSBCamera = new ShowUSBCamera(0);
                showUSBCamera.Show();
                testAction.Delay(5000);

                // 镜头清晰度
                TestItem dayItem = testAction.CheckDayResolutionRTOS(serial, dataGridView1, logger);
                if (dayItem == null)
                {
                    logger.ShowLog($"镜头清晰度检查不通过，无法进行后续测试");
                    if (testParam.ng_continue == "false")
                    {
                        return;
                    }
                }
                else
                {
                    model.day_video_check = dayItem.Result;
                    if (dayItem.Result == "FAIL")
                    {
                        model.str_ng_item += $"{dayItem.NgItem},";
                    }
                }
                // 保存图片
                SaveDayPicture(showUSBCamera, model.str_rn, image_time);
                // 上传共享目录
                logger.ShowLog("开始上传正常模式截图到指定目录...");
                string fileOnServer = @"\\" + testParam.local_server_name + @"\xdc03_focus_test_img\" + model.str_rn + image_time + "_normal.png";
                string fileToCopy = Path_iamge_document + @"\" + model.str_rn + image_time + "_normal.png";
                model.day_video_photo = fileToCopy;
                Task<bool> UploadImageTask = new Task<bool>(() => UploadImage(fileOnServer, fileToCopy));
                UploadImageTask.Start();

                // 镜头暗角
                TestItem darkcornerItem = testAction.CheckDayDarkCornerRTOS(serial, dataGridView1, logger);
                if (darkcornerItem == null)
                {
                    logger.ShowLog($"镜头暗角检查不通过，无法进行后续测试");
                    if (testParam.ng_continue == "false")
                    {
                        return;
                    }
                }
                else
                {
                    model.dark_corner = darkcornerItem.Result;
                    if (darkcornerItem.Result == "FAIL")
                    {
                        model.str_ng_item += $"{darkcornerItem.NgItem},";
                    }
                }

                // 夜视切换
                List<TestItem> IRItems = testAction.CheckIR_CUT_LED_RTOS(serial, dataGridView1, logger);
                if(IRItems == null)
                {
                    logger.ShowLog($"夜视切换检查不通过，无法进行后续测试");
                    if (testParam.ng_continue == "false")
                    {
                        return;
                    }
                }
                else
                {
                    foreach(TestItem item in IRItems)
                    {
                        if(item.NgItem == "ir_led")
                        {
                            model.rtos_ir_led = item.Result;
                        }
                        if(item.NgItem == "ir_cut")
                        {
                            model.rtos_ir_cut = item.Result;
                        }
                        if(item.Result == "FAIL")
                        {
                            model.str_ng_item += $"{item.NgItem},";
                        }
                    }
                }

                // 夜视清晰度
                TestItem nightItem = testAction.CheckDayResolutionRTOS(serial, dataGridView1, logger);
                if (nightItem == null)
                {
                    logger.ShowLog($"夜视清晰度检查不通过，无法进行后续测试");
                    if (testParam.ng_continue == "false")
                    {
                        return;
                    }
                }
                else
                {
                    model.night_video_check = nightItem.Result;
                    if (nightItem.Result == "FAIL")
                    {
                        model.str_ng_item += $"{nightItem.NgItem},";
                    }
                }

                SaveNightPicture(showUSBCamera, model.str_rn, image_time);
                // 上传共享目录
                logger.ShowLog("开始上传夜视模式截图到指定目录...");
                string fileOnServer_night = @"\\" + testParam.local_server_name + @"\xdc03_focus_test_img\" + model.str_rn + image_time + "_night.png";
                string fileToCopy_night = Path_iamge_document + @"\" + model.str_rn + image_time + "_night.png";
                model.night_video_photo = fileToCopy_night;
                Task<bool> UploadNightImageTask = new Task<bool>(() => UploadNightImage(fileOnServer, fileToCopy));
                UploadNightImageTask.Start();

                // 等待上传白天照片任务结束
                int startVol = Environment.TickCount;
                while (!UploadImageTask.IsCompleted)
                {
                    testAction.Delay(1000);
                    logger.ShowLog("等待镜头清晰度照片上传任务结束...");
                }
                if (UploadImageTask.Result == false)
                {
                    model.str_ng_item += $"day_video_photo,";
                }

                // 等待上传黑夜照片任务结束
                int startCur = Environment.TickCount;
                while (!UploadNightImageTask.IsCompleted)
                {
                    testAction.Delay(1000);
                    logger.ShowLog("等待夜视清晰度照片上传任务结束...");
                }
                if (UploadNightImageTask.Result == false)
                {
                    model.str_ng_item += $"night_video_photo";
                }

            }
            else
            {
                logger.ShowLog($"打开摄像头失败：[{str_error_log}]");
            }

            // 保存数据库
            model.str_end_test_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            if (model.str_ng_item.Length == 0)
            {
                model.str_test_result = "PASS";
            }
            else
            {
                model.str_test_result = "FAIL";
            }
            string str_sql = $"INSERT INTO `t030_product_test_report` " +
                $"(`software_version`, `spec_id`, `standard_id`, `fixture_id`, `operator_id`, `rn`, " +
                $"`start_test_time`, `end_test_time`, `stage_name`, `test_mode`, `test_result`, " +
                $"`ng_items`, `read_tagnumber`, `write_tagnumber`, `ping_rtt`, " +
                $"`mic_auto`, `mic_data`, `mic_manual`, `led_color`, `motion`, `button`, `audio`, " +
                $"`fw_version`, `hw_version`, `mcu_version`, `battery_voltage`, `cpu_temperature`, " +
                $"`light`, `vlc_rtsp`, `ir_cut`, `ir_led`, `wifi_up_loss`, `wifi_up_rate`, `up_rate_revise`, " +
                $"`wifi_down_loss`, `wifi_down_rate`, `down_rate_revise`, `rf_rx`, `rf_tx_frenquency`, " +
                $"`rf_tx_power`, `rf_tx_power_revise`, `reset_button`, `factory_reset`, " +
                $"`mac`, `sn`, `uid`, `mac_cloud`, `check_sn_uid`)" +
                $" VALUES ('{model.str_software_version}', '{model.str_spec_id}', '{model.str_standard_id}', '{model.str_fixture_id}', '{model.str_operator_id}', '{model.str_rn}'," +
                $" '{model.str_start_test_time}', '{model.str_end_test_time}', '{model.str_stage_name}', '{model.str_test_mode}', '{model.str_test_result}'," +
                $" '{model.str_ng_item}', '{model.str_read_tagnumber}', '{model.str_write_tagnumber}', '{model.str_ping_rtt}'," +
                $" '{model.str_mic_auto}', '{model.str_mic_data}', '{model.str_mic_manual}', '{model.str_led_color}', '{model.str_motion}', '{model.str_button}', '{model.str_audio}'," +
                $" '{model.str_fw_version}', '{model.str_hw_version}', '{model.str_mcu_version}', '{model.str_battery_voltage}', '{model.str_cpu_temperature}'," +
                $" '{model.str_light}', '{model.str_vlc_rtsp}', '{model.str_ir_cut}', '{model.str_ir_led}', '{model.str_wifi_up_loss}', '{model.str_wifi_up_rate}', '{model.str_up_rate_revise}'," +
                $" '{model.str_wifi_down_loss}', '{model.str_wifi_down_rate}', '{model.str_down_rate_revise}', '{model.str_rf_rx}', '{model.str_rf_tx_frenquency}'," +
                $" '{model.str_rf_tx_power}', '{model.str_rf_tx_power_revise}', '{model.str_reset_button}', '{model.str_factory_reset}'," +
                $" '{model.str_mac}', '{model.str_sn}', '{model.str_uid}', '{model.str_mac_cloud}', '{model.str_check_cloud}');";
            SaveLocalSqliteDB(str_sql);
            UploadToLocalServer(str_sql);

        }

        private bool gen_CreateDirectory(string file_path_document)
        {
            try
            {
                //==========创建文件夹===========================
                if (System.IO.Directory.Exists(file_path_document) == false)
                {
                    System.IO.Directory.CreateDirectory(file_path_document);
                }

                return true;
            }
            catch (Exception ee)
            {
                return false;
            }
        }

        /// <summary>
        /// 保存正常模式图片
        /// </summary>
        /// <returns></returns>
        private bool SaveDayPicture(ShowUSBCamera showUSBCamera, string str_rn, string image_time)
        {
            try
            {
                gen_CreateDirectory(Path_iamge_document);
                Path_image_file = Path_iamge_document + @"\" + str_rn + image_time + "_normal.png";
                if (System.IO.File.Exists(Path_image_file))
                {
                    System.IO.File.Delete(Path_image_file);
                }
                if (showUSBCamera != null)
                {
                    Bitmap img = showUSBCamera.CapturePNG(Path_image_file);
                    if (img == null)
                    {
                        logger.ShowLog("截图失败");
                        return false;
                    }
                    img.Save(Path_image_file, System.Drawing.Imaging.ImageFormat.Png);
                    //rich_run_log("正在上传图片到指定目录...");
                    //trdUploadImage = new Thread(new ThreadStart(UploadImage));
                    //trdUploadImage.IsBackground = true;
                    //trdUploadImage.Start();
                    //trdUploadImage.Join();
                    return true;
                }
                else
                {
                    logger.ShowLog("截图失败");
                    return false;
                }
            }
            catch (Exception ee)
            {
                logger.ShowLog($"保存正常模式图片，发生异常：{ee.Message}");
                return false;
            }
        }

        /// <summary>
        /// 保存夜视模式图片
        /// </summary>
        /// <returns></returns>
        private bool SaveNightPicture(ShowUSBCamera showUSBCamera, string str_rn, string image_time)
        {
            try
            {
                gen_CreateDirectory(Path_iamge_document);
                Path_image_file = Path_iamge_document + @"\" + str_rn + image_time + "_night.png";
                if (System.IO.File.Exists(Path_image_file))
                {
                    System.IO.File.Delete(Path_image_file);
                }
                //Bitmap img = videoSourcePlayer1.GetCurrentVideoFrame();
                //img.Save(Path_image_file, System.Drawing.Imaging.ImageFormat.Png);
                if (showUSBCamera != null)
                {
                    Bitmap img = showUSBCamera.CapturePNG(Path_image_file);
                    if (img == null)
                    {
                        logger.ShowLog("截图失败");
                        return false;
                    }
                    img.Save(Path_image_file, System.Drawing.Imaging.ImageFormat.Png);
                    //rich_run_log("正在上传图片到指定目录...");
                    //trdUploadIRImage = new Thread(new ThreadStart(UploadIRImage));
                    //trdUploadIRImage.IsBackground = true;
                    //trdUploadIRImage.Start();
                    //trdUploadIRImage.Join();
                    return true;
                }
                else
                {
                    logger.ShowLog("截图失败");
                    return false;
                }
            }
            catch (Exception ee)
            {
                logger.ShowLog($"保存夜视模式图片，发生异常：{ee.Message}");
                return false;
            }
        }

        /// <summary>
        /// 夜视清晰度图片上传服务器
        /// </summary>
        private bool UploadNightImage(string fileOnServer, string fileToCopy)
        {
            
            if (!File.Exists(fileToCopy))
            {
                logger.ShowLog("--- 找不到夜视图片文件：" + fileToCopy);
                return false;
            }
            try
            {
                File.Copy(fileToCopy, fileOnServer, true);
                logger.ShowLog("--- 夜视清晰度图片上传本地服务器成功");
                return true;
            }
            catch (Exception e)
            {
                logger.ShowLog($"--- 夜视清晰度图片上传本地服务器({fileOnServer})失败：" + e.Message);
                return false;
            }
        }

        /// <summary>
        /// 镜头清晰度图片上传服务器
        /// </summary>
        private bool UploadImage(string fileOnServer, string fileToCopy)
        {
            if (!File.Exists(fileToCopy))
            {
                logger.ShowLog("--- 找不到图片文件：" + fileToCopy);
                return false;
            }
            try
            {
                File.Copy(fileToCopy, fileOnServer, true);
                logger.ShowLog("--- 清晰度图片上传本地服务器成功");
                return true;
            }
            catch (Exception e)
            {
                logger.ShowLog($"--- 清晰度图片上传本地服务器({fileOnServer})失败：" + e.Message);
                return false;
            }
        }
        #endregion

        public void RunTestVolCur()
        {
            // 先获取运行参数
            InitTestParam();
            // 初始化测试规格
            InitialTestSpec();

            dataGridView1.Rows.Clear();
            richTextBoxRunLog.Clear();

            Model model = new Model
            {
                str_software_version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(),
                str_spec_id = testParam.spec_id,
                str_standard_id = testParam.standard_id,
                str_fixture_id = "fixture1",
                str_operator_id = "No.001",
                str_start_test_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                str_stage_name = testParam.stage_name,
                str_test_mode = testParam.test_mode,
            };
            string image_time = string.Format("{0:_HHmmss}", DateTime.Now);

            TestAction testAction = new TestAction();

            TDMSerial volTDMSerial = new TDMSerial(serialPortVolTDM, testParam.voltage_tdm_port);
            TDMSerial curTDMSerial = new TDMSerial(serialPortCurTDM, testParam.current_tdm_port);
            FlukeSerial flukeSerial = new FlukeSerial(serialPortFluke, testParam.current_fluke_port, richTextBoxFluke);
            RelaySerial relaySerial = new RelaySerial(serialPortRelay, testParam.relay_port);
            TestItem standbyCurrent = testAction.PCBAStandbyCurrentTest(flukeSerial, dataGridView1, logger, specMax, specMin);
            if(standbyCurrent == null)
            {
                logger.ShowLog($"漏电流测试不通过，无法进行后续测试");
                if (testParam.ng_continue == "false")
                {
                    return;
                }
            }
            else
            {
                model.str_standby_current = standbyCurrent.Value.ToString();
                if(standbyCurrent.Result == "FAIL")
                {
                    model.str_ng_item += $"{standbyCurrent.NgItem},";
                }
            }

            TestItem chargeCurrent = testAction.PCBAChargeCurrentTest(flukeSerial, relaySerial, dataGridView1, logger, specMax, specMin, testParam);
            if (chargeCurrent == null)
            {
                logger.ShowLog($"充电电流测试不通过，无法进行后续测试");
                if (testParam.ng_continue == "false")
                {
                    return;
                }
            }
            else
            {
                model.str_standby_current = chargeCurrent.Value.ToString();
                if (chargeCurrent.Result == "FAIL")
                {
                    model.str_ng_item += $"{chargeCurrent.NgItem},";
                }
            }

            // 电压测试
            Task<List<TestItem>> voltageTask = new Task<List<TestItem>>(() => testAction.PCBAVoltageTest(relaySerial, volTDMSerial, dataGridView1, logger, specMax, specMin));
            voltageTask.Start();

            // 检查RN和工序号
            List<TestItem> RNandTagnumber = testAction.CheckRNandTagname(serial, dataGridView1, logger, testParam, specStandard);

            if (RNandTagnumber == null)
            {
                logger.ShowLog($"RN号或工序号检查不通过，无法进行后续测试");
                if (testParam.ng_continue == "false")
                {
                    return;
                }
            }
            else
            {
                foreach (TestItem item in RNandTagnumber)
                {
                    if (item.NgItem == "rn")
                    {
                        model.str_rn = item.StrVal;
                    }
                    if (item.NgItem == "tagnumber")
                    {
                        model.str_read_tagnumber = item.StrVal;
                    }

                    if (item.Result == "FAIL")
                    {
                        logger.ShowLog($"{item.Name}检查不通过，无法进行后续测试");
                        return;
                    }
                }
            }

            // 工作电流测试
            Task<TestItem> workCurrent = new Task<TestItem>(() => testAction.PCBAWorkCurrentTest(curTDMSerial, dataGridView1, logger, specMax, specMin));
            workCurrent.Start();

            // 等待电压测试任务结束
            int startVol = Environment.TickCount;
            while (!voltageTask.IsCompleted)
            {
                testAction.Delay(1000);
                logger.ShowLog("等待电压测试任务结束...");
            }
            List<TestItem> voltage_result = voltageTask.Result;
            if(voltage_result == null)
            {
                logger.ShowLog($"电压测试不通过，无法进行后续测试");
                if (testParam.ng_continue == "false")
                {
                    return;
                }
            }
            else
            {
                foreach (TestItem item in voltage_result)
                {
                    if (item.NgItem == "voltage_wifi")
                    {
                        model.str_voltage_wifi = item.Result;
                    }
                    if (item.NgItem == "voltage_vcc_core")
                    {
                        model.str_voltage_vcc_core = item.Result;
                    }
                    if (item.NgItem == "voltage_sensor1")
                    {
                        model.str_voltage_sensor1 = item.Result;
                    }
                    if (item.NgItem == "voltage_sensor2")
                    {
                        model.str_voltage_sensor2 = item.Result;
                    }
                    if (item.NgItem == "voltage_rtc")
                    {
                        model.str_voltage_rtc = item.Result;
                    }
                    if (item.NgItem == "voltage_vcc")
                    {
                        model.str_voltage_vcc = item.Result;
                    }
                    if (item.NgItem == "voltage_mcu")
                    {
                        model.str_voltage_mcu = item.Result;
                    }
                    if (item.NgItem == "voltage_ddr")
                    {
                        model.str_voltage_ddr = item.Result;
                    }
                    if (item.Result == "FAIL")
                    {
                        model.str_ng_item += $"{item.NgItem},";
                    }
                }
            }

            // 等待工作电流测试结束
            int startCur = Environment.TickCount;
            while (!workCurrent.IsCompleted)
            {
                testAction.Delay(1000);
                logger.ShowLog("等待工作电流测试任务结束...");
            }
            TestItem workCurrent_result = workCurrent.Result;
            if(workCurrent_result == null)
            {
                logger.ShowLog($"工作电流测试不通过，无法进行后续测试");
                if (testParam.ng_continue == "false")
                {
                    return;
                }
            }
            else
            {
                model.str_work_current = workCurrent_result.Value.ToString();
                if(workCurrent_result.Result == "FAIL")
                {
                    model.str_ng_item += $"{workCurrent_result.NgItem},";
                }
            }

        }

        /// <summary>
        /// 保存数据至本机Sqlite数据库
        /// </summary>
        /// <param name="str_error_log"></param>
        /// <returns></returns>
        private bool SaveLocalSqliteDB(string str_sql)
    {
        try
        {
            // 本机数据库sqlite
            LocalMachineDB localMachineDB = new LocalMachineDB();
            string db_file = Environment.CurrentDirectory + @"\Local.db";

            string str_error_log = "";
            if (localMachineDB.SaveDataToLocal(db_file, str_sql, ref str_error_log) == false)
            {
                logger.ShowLog("--- 数据保存本机Sqlite失败：" + str_error_log);
                return false;
            }
            else
            {
                logger.ShowLog("--- 数据保存本机Sqlite成功");
                return true;
            }
        }
        catch (Exception ee)
        {
            MessageBox.Show(ee.Message);
            return false;
        }
    }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str_sql"></param>
        /// <returns></returns>
        private bool UploadToLocalServer(string str_sql)
        {
            MySqlConnectionStringBuilder connectStr = new MySqlConnectionStringBuilder();
            connectStr.Server = ConfigFile.IniReadValue("Server", "db_ip", Path_ini);
            connectStr.Port = uint.Parse(ConfigFile.IniReadValue("Server", "db_port", Path_ini));
            connectStr.Database = "xdc01_management";
            connectStr.UserID = "rckxdc01";
            connectStr.Password = "2022xdc01";
            connectStr.SslMode = MySqlSslMode.None;
            connectStr.ConnectionTimeout = 10;
            /*connectStr.Pooling = true;
            connectStr.MinimumPoolSize = 3;
            connectStr.MaximumPoolSize = 10;*/
            MySqlConnection conn = new MySqlConnection(connectStr.ToString());
            try
            {
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = str_sql;

                if (cmd.ExecuteNonQuery() > 0)
                {
                    logger.ShowLog("--- 数据上传本地服务器成功");
                    return true;
                }
                else
                {
                    logger.ShowLog("--- 数据上传本地服务器失败");
                    return false;
                }
            }
            catch (Exception ee)
            {
                logger.ShowLog($"--- 数据上传本地服务器发生异常：{ee.Message}");
                return false;
            }
            finally
            {
                conn.Close();
            }
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            try
            {
                int start_time = Environment.TickCount;
                BtnStart.Enabled = false;
                textBoxTime.Text = "0";
                timerTestTime.Interval = 1000;
                timerTestTime.Start();
                labelResult.Text = string.Empty;
                panelResult.BackColor = Color.White;
                RunTest();
                //RunTestVolCur();
                float testTime = (Environment.TickCount - start_time) / 1000.00f;
                textBoxTime.Text = testTime.ToString("F2");
            }
            catch (Exception ee)
            {
                logger.ShowLog($"测试发生异常：[{ee.Message}]");
            }
            finally { 
                BtnStart.Enabled = true;
                timerTestTime.Stop();
            }
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            int yourColumnIndex = 6;
            DataGridView dgv = (DataGridView)sender;

            // 检查特定的列和行
            if (e.ColumnIndex == yourColumnIndex && e.RowIndex >= 0)
            {
                DataGridViewCell cell = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex];

                // 根据单元格的值设置颜色
                if (cell.Value != null && cell.Value.ToString() == "FAIL")
                {
                    cell.Style.BackColor = Color.Red;
                    cell.Style.ForeColor = Color.White;
                }
                else if (cell.Value != null && cell.Value.ToString() == "PASS")
                {
                    cell.Style.BackColor = Color.Green;
                    cell.Style.ForeColor = Color.Black;
                }
                else
                {
                    // 恢复默认颜色
                    //cell.Style.BackColor = dgv.DefaultCellStyle.BackColor;
                    //cell.Style.ForeColor = dgv.DefaultCellStyle.ForeColor;
                }
            }
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            //if (xDC01DebugForm != null)
            //{
            //    xDC01DebugForm.TopMost = false;
            //    this.TopMost = true;
            //}
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(tabControl1.SelectedTab.Name == "tabPageSetting") {
                InitTestParam();

                textBoxDbIp.Text = testParam.db_ip;
                textBoxDbPort.Text = testParam.db_port;
                numericUpDownSpecId.Value = int.Parse(testParam.spec_id);
                numericUpDownStandardId.Value = int.Parse(testParam.standard_id);

                comboBoxSerialPort.Items.Clear();
                foreach (string aa in System.IO.Ports.SerialPort.GetPortNames())
                {
                    comboBoxSerialPort.Items.Add(aa);
                    if (aa.Equals(testParam.serial_port))
                    {
                        comboBoxSerialPort.SelectedItem = testParam.serial_port;
                    }
                }
                comboBoxTestMode.SelectedItem = testParam.test_mode;
                comboBoxStage.SelectedItem = testParam.stage_name;
                comboBoxTagNumber.SelectedItem = testParam.cur_tagnumber;

                textBoxPingIP.Text = testParam.ping_ip;
                numericUpDownPingCount.Value = int.Parse(testParam.ping_count);

                numericUpDownLEDInterval.Value = int.Parse(testParam.led_interval);

                System.IO.DirectoryInfo _ini_file = new System.IO.DirectoryInfo(System.Windows.Forms.Application.StartupPath + @"\wav");
                foreach (System.IO.FileInfo file in _ini_file.GetFiles())
                {
                    if (System.IO.Path.GetFileName(file.Name).Contains(".wav"))
                    {
                        comboBoxWaveFile.Items.Add(file.Name.ToString());
                        if (file.Name.Equals(testParam.mic_wave_file))
                        {
                            comboBoxWaveFile.SelectedItem = testParam.mic_wave_file;
                        }
                    }
                }
                numericUpDownRecordDuration.Value = int.Parse(testParam.mic_record_duration);

                textBoxServerIP.Text = testParam.server_ip;
                numericUpDownBandWidth.Value = int.Parse(testParam.bandwidth.Replace("M", "").Replace("K", "").Replace("G", ""));
                comboBoxUnit.SelectedItem = testParam.bandwidth.Substring(testParam.bandwidth.Length -1, 1);
                numericUpDownDuration.Value = int.Parse(testParam.duration);
                textBoxWIFISSID.Text = testParam.cur_wifi_ssid;
                textBoxWIFIPWD.Text = testParam.cur_wifi_pwd;
                textBoxUpRateRevise.Text = testParam.up_rate_corrected;
                textBoxDownRateRevise.Text= testParam.down_rate_corrected;

                numericUpDownRFTXDelay.Value = int.Parse(testParam.rf_tx_delay);
                textBoxRFPowerRevise.Text = testParam.rf_power_corrected;

                numericUpDownRFRXTimeout.Value = int.Parse(testParam.rf_rx_timeout);

                comboBoxNextTagNumber.SelectedItem = testParam.next_tagnumber;
                textBoxNextWIFISSID.Text = testParam.next_wifi_ssid;
                textBoxNextWIFIPWD.Text = testParam.next_wifi_pwd;
                checkBoxWriteNextWIFI.Checked = testParam.write_next_wifi == "True";
                checkBoxNGContinue.Checked = testParam.ng_continue == "True";

                comboBoxFactoryid.SelectedItem = testParam.factoryId;
                comboBoxColorid.SelectedItem = testParam.colorId;
                comboBoxModeid.SelectedItem = testParam.modeId;
                comboBoxProductid.SelectedItem = testParam.productId;
                comboBoxProducttype.SelectedItem = testParam.productType;
                textBoxCloudUsername.Text = testParam.cloud_username;
                textBoxCloudPassword.Text = testParam.cloud_password;

                textBoxImageServer.Text = testParam.local_server_name;

                comboBoxChargeRelayIndex.SelectedItem = testParam.charge_relay_index;

                comboBoxVolTDMSerial.Items.Clear();
                foreach (string aa in System.IO.Ports.SerialPort.GetPortNames())
                {
                    comboBoxVolTDMSerial.Items.Add(aa);
                    if (aa.Equals(testParam.voltage_tdm_port))
                    {
                        comboBoxVolTDMSerial.SelectedItem = testParam.voltage_tdm_port;
                    }
                }

                comboBoxCurTDMSerial.Items.Clear();
                foreach (string aa in System.IO.Ports.SerialPort.GetPortNames())
                {
                    comboBoxCurTDMSerial.Items.Add(aa);
                    if (aa.Equals(testParam.current_tdm_port))
                    {
                        comboBoxCurTDMSerial.SelectedItem = testParam.current_tdm_port;
                    }
                }

                comboBoxCurFluke.Items.Clear();
                foreach (string aa in System.IO.Ports.SerialPort.GetPortNames())
                {
                    comboBoxCurFluke.Items.Add(aa);
                    if (aa.Equals(testParam.current_fluke_port))
                    {
                        comboBoxCurFluke.SelectedItem = testParam.current_fluke_port;
                    }
                }

                comboBoxRelay.Items.Clear();
                foreach (string aa in System.IO.Ports.SerialPort.GetPortNames())
                {
                    comboBoxRelay.Items.Add(aa);
                    if (aa.Equals(testParam.relay_port))
                    {
                        comboBoxRelay.SelectedItem = testParam.relay_port;
                    }
                }
            }
        }

        #region 设置界面
        private void BtnServer_Click(object sender, EventArgs e)
        {
            ConfigFile.IniWriteValue("Server", "db_ip", textBoxDbIp.Text, Path_ini);
            ConfigFile.IniWriteValue("Server", "db_port", textBoxDbPort.Text, Path_ini);
            ConfigFile.IniWriteValue("Server", "spec_id", numericUpDownSpecId.Value.ToString(), Path_ini);
            ConfigFile.IniWriteValue("Server", "standard_id", numericUpDownStandardId.Value.ToString(), Path_ini);
        }

        private void BtnRunParam_Click(object sender, EventArgs e)
        {
            ConfigFile.IniWriteValue("Run_Param", "serial_port", comboBoxSerialPort.SelectedItem.ToString(), Path_ini);
            ConfigFile.IniWriteValue("Run_Param", "test_mode", comboBoxTestMode.SelectedItem.ToString(), Path_ini);
            ConfigFile.IniWriteValue("Run_Param", "stage_name", comboBoxStage.SelectedItem.ToString(), Path_ini);
        }

        private void BtnPing_Click(object sender, EventArgs e)
        {
            ConfigFile.IniWriteValue("wifi_param", "ping_ip", textBoxPingIP.Text, Path_ini);
            ConfigFile.IniWriteValue("wifi_param", "ping_count", numericUpDownPingCount.Value.ToString(), Path_ini);
        }

        private void BtnMic_Click(object sender, EventArgs e)
        {
            ConfigFile.IniWriteValue("mic_param", "record_duration", numericUpDownRecordDuration.Text, Path_ini);
            ConfigFile.IniWriteValue("mic_param", "wave_file", comboBoxWaveFile.SelectedItem.ToString(), Path_ini);
        }

        private void BtnLedColor_Click(object sender, EventArgs e)
        {
            ConfigFile.IniWriteValue("led_color", "interval", numericUpDownLEDInterval.Value.ToString(), Path_ini);
        }

        private void BtnWiFIthroughput_Click(object sender, EventArgs e)
        {
            ConfigFile.IniWriteValue("wifi_throughput", "cur_wifi_ssid", textBoxWIFISSID.Text, Path_ini);
            ConfigFile.IniWriteValue("wifi_throughput", "cur_wifi_pwd", textBoxWIFIPWD.Text, Path_ini);
            ConfigFile.IniWriteValue("wifi_throughput", "server_ip", textBoxServerIP.Text, Path_ini);
            ConfigFile.IniWriteValue("wifi_throughput", "duration", numericUpDownDuration.Value.ToString(), Path_ini);
            ConfigFile.IniWriteValue("wifi_throughput", "bandwidth", numericUpDownBandWidth.Value.ToString() + comboBoxUnit.SelectedItem.ToString(), Path_ini);
            ConfigFile.IniWriteValue("wifi_throughput", "up_rate_corrected", textBoxUpRateRevise.Text, Path_ini);
            ConfigFile.IniWriteValue("wifi_throughput", "down_rate_corrected", textBoxDownRateRevise.Text, Path_ini);
        }

        private void BtnRFTX_Click(object sender, EventArgs e)
        {
            ConfigFile.IniWriteValue("rf_tx", "rf_tx_delay", numericUpDownRFTXDelay.Value.ToString(), Path_ini);
            ConfigFile.IniWriteValue("rf_tx", "rf_power_corrected", textBoxRFPowerRevise.Text, Path_ini);
        }

        private void BtnRFRX_Click(object sender, EventArgs e)
        {
            ConfigFile.IniWriteValue("rf_rx", "rf_rx_timeout", numericUpDownRFRXTimeout.Value.ToString(), Path_ini);
        }

        private void BtnNextStation_Click(object sender, EventArgs e)
        {
            ConfigFile.IniWriteValue("Run_Param", "cur_tagnumber", comboBoxTagNumber.SelectedItem.ToString(), Path_ini);
            ConfigFile.IniWriteValue("next_station", "next_tagnumber", comboBoxNextTagNumber.SelectedItem.ToString(), Path_ini);
            ConfigFile.IniWriteValue("next_station", "next_wifi_ssid", textBoxNextWIFISSID.Text, Path_ini);
            ConfigFile.IniWriteValue("next_station", "next_wifi_pwd", textBoxNextWIFIPWD.Text, Path_ini);
            ConfigFile.IniWriteValue("next_station", "write_next_wifi", checkBoxWriteNextWIFI.Checked.ToString(), Path_ini);
        }
        
        private void BtnPanelInfo_Click(object sender, EventArgs e)
        {
            ConfigFile.IniWriteValue("cloud", "cloud_username", textBoxCloudUsername.Text, Path_ini);
            ConfigFile.IniWriteValue("cloud", "cloud_password", textBoxCloudPassword.Text, Path_ini);
            ConfigFile.IniWriteValue("cloud", "factoryId", comboBoxFactoryid.SelectedItem.ToString(), Path_ini);
            ConfigFile.IniWriteValue("cloud", "colorId", comboBoxColorid.SelectedItem.ToString(), Path_ini);
            ConfigFile.IniWriteValue("cloud", "productId", comboBoxProductid.SelectedItem.ToString(), Path_ini);
            ConfigFile.IniWriteValue("cloud", "modeId", comboBoxModeid.SelectedItem.ToString(), Path_ini);
            ConfigFile.IniWriteValue("cloud", "productType", comboBoxProducttype.SelectedItem.ToString(), Path_ini);
        }

        private void BtnNGcontinue_Click(object sender, EventArgs e)
        {
            ConfigFile.IniWriteValue("Run_Param", "ng_continue", checkBoxNGContinue.Checked.ToString(), Path_ini);
        }

        private void BtnImageServer_Click(object sender, EventArgs e)
        {
            ConfigFile.IniWriteValue("rtos", "local_server_name", textBoxImageServer.Text, Path_ini);
        }

        private void BtnChargeRelay_Click(object sender, EventArgs e)
        {
            ConfigFile.IniWriteValue("voltage_current", "charge_relay_index", comboBoxChargeRelayIndex.SelectedItem.ToString(), Path_ini);
            ConfigFile.IniWriteValue("voltage_current", "voltage_tdm_port", comboBoxVolTDMSerial.SelectedItem.ToString(), Path_ini);
            ConfigFile.IniWriteValue("voltage_current", "current_tdm_port", comboBoxCurTDMSerial.SelectedItem.ToString(), Path_ini);
            ConfigFile.IniWriteValue("voltage_current", "current_fluke_port", comboBoxCurFluke.SelectedItem.ToString(), Path_ini);
            ConfigFile.IniWriteValue("voltage_current", "relay_port", comboBoxRelay.SelectedItem.ToString(), Path_ini);
        }
        #endregion

        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            // 滚动到最后一行
            dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.Rows.Count - 1;

            // 确保最后一行完全可见
            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Selected = true;
                dataGridView1.CurrentCell = dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[0];
            }
        }

    }

}
