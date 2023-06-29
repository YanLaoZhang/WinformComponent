using ConfigFileLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XDC01Action
{
    public partial class TestParamForm : Form
    {
        // 配置文件路径
        private static string Path_ini = System.Windows.Forms.Application.StartupPath + @"\MyConfig.config";
        public TestParamForm()
        {
            InitializeComponent();
        }

        public static TestParam InitTestParam()
        {
            try
            {
                TestParam testParam = new TestParam
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
                    poweron_delay = ConfigFile.IniReadValue("Run_Param", "poweron_delay", Path_ini),

                    ping_ip = ConfigFile.IniReadValue("wifi_param", "ping_ip", Path_ini),
                    ping_count = ConfigFile.IniReadValue("wifi_param", "ping_count", Path_ini),

                    led_interval = ConfigFile.IniReadValue("led_color", "interval", Path_ini),
                    btn_timeout = ConfigFile.IniReadValue("button", "button_test_timeout", Path_ini),
                    pir_timeout = ConfigFile.IniReadValue("motion", "motion_test_timeout", Path_ini),

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
                    rtos_serial_port = ConfigFile.IniReadValue("rtos", "rtos_serial_port", Path_ini),

                    charge_relay_index = ConfigFile.IniReadValue("voltage_current", "charge_relay_index", Path_ini),
                    voltage_tdm_port = ConfigFile.IniReadValue("voltage_current", "voltage_tdm_port", Path_ini),
                    current_tdm_port = ConfigFile.IniReadValue("voltage_current", "current_tdm_port", Path_ini),
                    current_fluke_port = ConfigFile.IniReadValue("voltage_current", "current_fluke_port", Path_ini),
                    relay_port = ConfigFile.IniReadValue("voltage_current", "relay_port", Path_ini),
                    trigger_relay_interval = ConfigFile.IniReadValue("voltage_current", "trigger_relay_interval", Path_ini),
                    serial_relay_index = ConfigFile.IniReadValue("voltage_current", "serial_relay_index", Path_ini),

                    printer_name = ConfigFile.IniReadValue("printer", "printer_name", Path_ini),
                    sn_count = ConfigFile.IniReadValue("printer", "sn_count", Path_ini),
                    mac_count = ConfigFile.IniReadValue("printer", "mac_count", Path_ini),

                    wav_1 = ConfigFile.IniReadValue("audio", "wav_1", Path_ini),
                    wav_2 = ConfigFile.IniReadValue("audio", "wav_2", Path_ini),
                    wav_3 = ConfigFile.IniReadValue("audio", "wav_3", Path_ini),
                    wav_4 = ConfigFile.IniReadValue("audio", "wav_4", Path_ini),
                    wav_5 = ConfigFile.IniReadValue("audio", "wav_5", Path_ini),
                    wav_8 = ConfigFile.IniReadValue("audio", "wav_8", Path_ini),
                    wav_9 = ConfigFile.IniReadValue("audio", "wav_9", Path_ini),
                };
                return testParam;
            }
            catch (Exception ee)
            {
                MessageBox.Show($"获取运行参数发生异常：[{ee.Message}]");
                return null;
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            TestParam testParam = InitTestParam();
            if (tabControl1.SelectedTab == tabPageCommon)
            {
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
                checkBoxNGContinue.Checked = testParam.ng_continue == "True";
                numericUpDownPoweronDelay.Value = decimal.Parse(testParam.poweron_delay);

                comboBoxNextTagNumber.SelectedItem = testParam.next_tagnumber;
                textBoxNextWIFISSID.Text = testParam.next_wifi_ssid;
                textBoxNextWIFIPWD.Text = testParam.next_wifi_pwd;
                checkBoxWriteNextWIFI.Checked = testParam.write_next_wifi == "True";

                // 云端服务器
                comboBoxFactoryid.SelectedItem = testParam.factoryId;
                comboBoxColorid.SelectedItem = testParam.colorId;
                comboBoxModeid.SelectedItem = testParam.modeId;
                comboBoxProductid.SelectedItem = testParam.productId;
                comboBoxProducttype.SelectedItem = testParam.productType;
                textBoxCloudUsername.Text = testParam.cloud_username;
                textBoxCloudPassword.Text = testParam.cloud_password;

            }
            if(tabControl1.SelectedTab == tabPageVoltage)
            {
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

                numericUpDownTriggerRelay.Value = decimal.Parse(testParam.trigger_relay_interval);

                comboBoxSerialRelayIndex.SelectedItem = testParam.serial_relay_index;
            }
            if(tabControl1.SelectedTab == tabPageFunTest)
            {
                // ping测试
                textBoxPingIP.Text = testParam.ping_ip;
                numericUpDownPingCount.Value = decimal.Parse(testParam.ping_count);
                // led灯测试
                numericUpDownLEDInterval.Value = decimal.Parse(testParam.led_interval);
                // button测试
                numericUpDownBtnTest.Value = decimal.Parse(testParam.btn_timeout);
                // 移动感应测试
                numericUpDownMotion.Value = decimal.Parse(testParam.pir_timeout);

                // 麦克风测试
                System.IO.DirectoryInfo _ini_file = new System.IO.DirectoryInfo(System.Windows.Forms.Application.StartupPath + @"\wav");
                comboBoxWaveFile.Items.Clear();
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

                // wifi吞吐量测试
                textBoxServerIP.Text = testParam.server_ip;
                numericUpDownBandWidth.Value = int.Parse(testParam.bandwidth.Replace("M", "").Replace("K", "").Replace("G", ""));
                comboBoxUnit.SelectedItem = testParam.bandwidth.Substring(testParam.bandwidth.Length - 1, 1);
                numericUpDownDuration.Value = int.Parse(testParam.duration);
                textBoxWIFISSID.Text = testParam.cur_wifi_ssid;
                textBoxWIFIPWD.Text = testParam.cur_wifi_pwd;
                textBoxUpRateRevise.Text = testParam.up_rate_corrected;
                textBoxDownRateRevise.Text = testParam.down_rate_corrected;

                // RF tx
                numericUpDownRFTXDelay.Value = int.Parse(testParam.rf_tx_delay);
                textBoxRFPowerRevise.Text = testParam.rf_power_corrected;

                // RF rx
                numericUpDownRFRXTimeout.Value = int.Parse(testParam.rf_rx_timeout);

                // 喇叭播放
                checkBoxWav_1.Checked = testParam.wav_1 == "True";
                checkBoxWav_2.Checked = testParam.wav_2 == "True";
                checkBoxWav_3.Checked = testParam.wav_3 == "True";
                checkBoxWav_4.Checked = testParam.wav_4 == "True";
                checkBoxWav_5.Checked = testParam.wav_5 == "True";
                checkBoxWav_8.Checked = testParam.wav_8 == "True";
                checkBoxWav_9.Checked = testParam.wav_9 == "True";
            }
            if(tabControl1.SelectedTab == tabPageRTOS)
            {
                textBoxImageServer.Text = testParam.local_server_name;
                comboBoxRTOSPort.Items.Clear();
                foreach (string aa in System.IO.Ports.SerialPort.GetPortNames())
                {
                    comboBoxRTOSPort.Items.Add(aa);
                    if (aa.Equals(testParam.rtos_serial_port))
                    {
                        comboBoxRTOSPort.SelectedItem = testParam.rtos_serial_port;
                    }
                }
            }
            if(tabControl1.SelectedTab == tabPagePrinter)
            {
                comboBoxPrinter.Items.Clear();
                foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
                {
                    comboBoxPrinter.Items.Add(printer);
                    if (printer.Equals(testParam.printer_name))
                    {
                        comboBoxPrinter.SelectedItem = testParam.printer_name;
                    }
                }
                numericUpDownSNCount.Value = decimal.Parse(testParam.sn_count);
                numericUpDownMacCount.Value = decimal.Parse(testParam.mac_count);
            }
        }

        #region 设置界面
        private void BtnServer_Click(object sender, EventArgs e)
        {
            try
            {
                ConfigFile.IniWriteValue("Server", "db_ip", textBoxDbIp.Text, Path_ini);
                ConfigFile.IniWriteValue("Server", "db_port", textBoxDbPort.Text, Path_ini);
                ConfigFile.IniWriteValue("Server", "spec_id", numericUpDownSpecId.Value.ToString(), Path_ini);
                ConfigFile.IniWriteValue("Server", "standard_id", numericUpDownStandardId.Value.ToString(), Path_ini);
                MessageBox.Show("服务器设置保存成功");
            }
            catch (Exception ee)
            {
                MessageBox.Show("保存异常：" + ee.Message);
            }
        }

        private void BtnRunParam_Click(object sender, EventArgs e)
        {
            try
            {
                ConfigFile.IniWriteValue("Run_Param", "serial_port", comboBoxSerialPort.SelectedItem.ToString(), Path_ini);
                ConfigFile.IniWriteValue("Run_Param", "test_mode", comboBoxTestMode.SelectedItem.ToString(), Path_ini);
                ConfigFile.IniWriteValue("Run_Param", "stage_name", comboBoxStage.SelectedItem.ToString(), Path_ini);
                ConfigFile.IniWriteValue("Run_Param", "ng_continue", checkBoxNGContinue.Checked.ToString(), Path_ini);
                ConfigFile.IniWriteValue("Run_Param", "poweron_delay", numericUpDownPoweronDelay.Value.ToString(), Path_ini);
                MessageBox.Show("运行参数保存成功");
            }
            catch (Exception ee)
            {
                MessageBox.Show("保存异常：" + ee.Message);
            }
        }

        private void BtnPing_Click(object sender, EventArgs e)
        {
            try
            {
                ConfigFile.IniWriteValue("wifi_param", "ping_ip", textBoxPingIP.Text, Path_ini);
                ConfigFile.IniWriteValue("wifi_param", "ping_count", numericUpDownPingCount.Value.ToString(), Path_ini);
                MessageBox.Show("Ping测试设置保存成功");
            }
            catch (Exception ee)
            {
                MessageBox.Show("保存异常：" + ee.Message);
            }
        }

        private void BtnMic_Click(object sender, EventArgs e)
        {
            try
            {
                ConfigFile.IniWriteValue("mic_param", "record_duration", numericUpDownRecordDuration.Text, Path_ini);
                ConfigFile.IniWriteValue("mic_param", "wave_file", comboBoxWaveFile.SelectedItem.ToString(), Path_ini);
                MessageBox.Show("麦克风测试保存成功");
            }
            catch (Exception ee)
            {
                MessageBox.Show("保存异常：" + ee.Message);
            }
        }

        private void BtnLedColor_Click(object sender, EventArgs e)
        {
            try
            {
                ConfigFile.IniWriteValue("led_color", "interval", numericUpDownLEDInterval.Value.ToString(), Path_ini);
                MessageBox.Show("LED灯颜色检查保存成功");
            }
            catch (Exception ee)
            {
                MessageBox.Show("保存异常：" + ee.Message);
            }
        }

        private void BtnWiFIthroughput_Click(object sender, EventArgs e)
        {
            try
            {
                ConfigFile.IniWriteValue("wifi_throughput", "cur_wifi_ssid", textBoxWIFISSID.Text, Path_ini);
                ConfigFile.IniWriteValue("wifi_throughput", "cur_wifi_pwd", textBoxWIFIPWD.Text, Path_ini);
                ConfigFile.IniWriteValue("wifi_throughput", "server_ip", textBoxServerIP.Text, Path_ini);
                ConfigFile.IniWriteValue("wifi_throughput", "duration", numericUpDownDuration.Value.ToString(), Path_ini);
                ConfigFile.IniWriteValue("wifi_throughput", "bandwidth", numericUpDownBandWidth.Value.ToString() + comboBoxUnit.SelectedItem.ToString(), Path_ini);
                ConfigFile.IniWriteValue("wifi_throughput", "up_rate_corrected", textBoxUpRateRevise.Text, Path_ini);
                ConfigFile.IniWriteValue("wifi_throughput", "down_rate_corrected", textBoxDownRateRevise.Text, Path_ini);
                MessageBox.Show("WiFi吞吐量测试保存成功");
            }
            catch (Exception ee)
            {
                MessageBox.Show("保存异常：" + ee.Message);
            }
        }

        private void BtnRFTX_Click(object sender, EventArgs e)
        {
            try
            {
                ConfigFile.IniWriteValue("rf_tx", "rf_tx_delay", numericUpDownRFTXDelay.Value.ToString(), Path_ini);
                ConfigFile.IniWriteValue("rf_tx", "rf_power_corrected", textBoxRFPowerRevise.Text, Path_ini);
                MessageBox.Show("RF TX测试保存成功");
            }
            catch (Exception ee)
            {
                MessageBox.Show("保存异常：" + ee.Message);
            }
        }

        private void BtnRFRX_Click(object sender, EventArgs e)
        {
            try
            {

                ConfigFile.IniWriteValue("rf_rx", "rf_rx_timeout", numericUpDownRFRXTimeout.Value.ToString(), Path_ini);
                MessageBox.Show("RF RX测试保存成功");
            }
            catch (Exception ee)
            {
                MessageBox.Show("保存异常：" + ee.Message);
            }
        }

        private void BtnbtnTest_Click(object sender, EventArgs e)
        {
            try
            {

                ConfigFile.IniWriteValue("button", "button_test_timeout", numericUpDownBtnTest.Value.ToString(), Path_ini);
                MessageBox.Show("按键测试保存成功");
            }
            catch (Exception ee)
            {
                MessageBox.Show("保存异常：" + ee.Message);
            }
        }

        private void BtnMotion_Click(object sender, EventArgs e)
        {
            try
            {

                ConfigFile.IniWriteValue("motion", "motion_test_timeout", numericUpDownMotion.Value.ToString(), Path_ini);
                MessageBox.Show("移动感应测试保存成功");
            }
            catch (Exception ee)
            {
                MessageBox.Show("保存异常：" + ee.Message);
            }
        }

        private void BtnAudioTest_Click(object sender, EventArgs e)
        {
            try
            {
                ConfigFile.IniWriteValue("audio", "wav_1", checkBoxWav_1.Checked.ToString(), Path_ini);
                ConfigFile.IniWriteValue("audio", "wav_2", checkBoxWav_2.Checked.ToString(), Path_ini);
                ConfigFile.IniWriteValue("audio", "wav_3", checkBoxWav_3.Checked.ToString(), Path_ini);
                ConfigFile.IniWriteValue("audio", "wav_4", checkBoxWav_4.Checked.ToString(), Path_ini);
                ConfigFile.IniWriteValue("audio", "wav_5", checkBoxWav_5.Checked.ToString(), Path_ini);
                ConfigFile.IniWriteValue("audio", "wav_8", checkBoxWav_8.Checked.ToString(), Path_ini);
                ConfigFile.IniWriteValue("audio", "wav_9", checkBoxWav_9.Checked.ToString(), Path_ini);
                MessageBox.Show("喇叭测试保存成功");
            }
            catch (Exception ee)
            {
                MessageBox.Show("保存异常：" + ee.Message);
            }
        }

        private void BtnNextStation_Click(object sender, EventArgs e)
        {
            try
            {
                ConfigFile.IniWriteValue("Run_Param", "cur_tagnumber", comboBoxTagNumber.SelectedItem.ToString(), Path_ini);
                ConfigFile.IniWriteValue("next_station", "next_tagnumber", comboBoxNextTagNumber.SelectedItem.ToString(), Path_ini);
                ConfigFile.IniWriteValue("next_station", "next_wifi_ssid", textBoxNextWIFISSID.Text, Path_ini);
                ConfigFile.IniWriteValue("next_station", "next_wifi_pwd", textBoxNextWIFIPWD.Text, Path_ini);
                ConfigFile.IniWriteValue("next_station", "write_next_wifi", checkBoxWriteNextWIFI.Checked.ToString(), Path_ini);
                MessageBox.Show("下一站信息设置保存成功");
            }
            catch (Exception ee)
            {
                MessageBox.Show("保存异常：" + ee.Message);
            }
        }

        private void BtnPanelInfo_Click(object sender, EventArgs e)
        {
            try
            {
                ConfigFile.IniWriteValue("cloud", "cloud_username", textBoxCloudUsername.Text, Path_ini);
                ConfigFile.IniWriteValue("cloud", "cloud_password", textBoxCloudPassword.Text, Path_ini);
                ConfigFile.IniWriteValue("cloud", "factoryId", comboBoxFactoryid.SelectedItem.ToString(), Path_ini);
                ConfigFile.IniWriteValue("cloud", "colorId", comboBoxColorid.SelectedItem.ToString(), Path_ini);
                ConfigFile.IniWriteValue("cloud", "productId", comboBoxProductid.SelectedItem.ToString(), Path_ini);
                ConfigFile.IniWriteValue("cloud", "modeId", comboBoxModeid.SelectedItem.ToString(), Path_ini);
                ConfigFile.IniWriteValue("cloud", "productType", comboBoxProducttype.SelectedItem.ToString(), Path_ini);
                MessageBox.Show("云服务器设置保存成功");
            }
            catch (Exception ee)
            {
                MessageBox.Show("保存异常：" + ee.Message);
            }
        }

        private void BtnImageServer_Click(object sender, EventArgs e)
        {
            try
            {
                ConfigFile.IniWriteValue("rtos", "local_server_name", textBoxImageServer.Text, Path_ini);
                ConfigFile.IniWriteValue("rtos", "rtos_serial_port", comboBoxRTOSPort.SelectedItem.ToString(), Path_ini);
                MessageBox.Show("RTOS镜头测试设置保存成功");
            }
            catch (Exception ee)
            {
                MessageBox.Show("保存异常：" + ee.Message);
            }
        }

        private void BtnChargeRelay_Click(object sender, EventArgs e)
        {
            try
            {
                ConfigFile.IniWriteValue("voltage_current", "charge_relay_index", comboBoxChargeRelayIndex.SelectedItem.ToString(), Path_ini);
                ConfigFile.IniWriteValue("voltage_current", "voltage_tdm_port", comboBoxVolTDMSerial.SelectedItem.ToString(), Path_ini);
                ConfigFile.IniWriteValue("voltage_current", "current_tdm_port", comboBoxCurTDMSerial.SelectedItem.ToString(), Path_ini);
                ConfigFile.IniWriteValue("voltage_current", "current_fluke_port", comboBoxCurFluke.SelectedItem.ToString(), Path_ini);
                ConfigFile.IniWriteValue("voltage_current", "relay_port", comboBoxRelay.SelectedItem.ToString(), Path_ini);
                ConfigFile.IniWriteValue("voltage_current", "trigger_relay_interval", numericUpDownTriggerRelay.Value.ToString(), Path_ini);
                ConfigFile.IniWriteValue("voltage_current", "serial_relay_index", comboBoxSerialRelayIndex.SelectedItem.ToString(), Path_ini);
                MessageBox.Show("继电器设置保存成功");
            }
            catch (Exception ee)
            {
                MessageBox.Show("保存异常：" + ee.Message);
            }
        }
        
        private void BtnPrinter_Click(object sender, EventArgs e)
        {
            try
            {
                ConfigFile.IniWriteValue("printer", "printer_name", comboBoxPrinter.SelectedItem.ToString(), Path_ini);
                ConfigFile.IniWriteValue("printer", "sn_count", numericUpDownSNCount.Value.ToString(), Path_ini);
                ConfigFile.IniWriteValue("printer", "mac_count", numericUpDownMacCount.Value.ToString(), Path_ini);
                MessageBox.Show("打印设置保存成功");
            }
            catch (Exception ee)
            {
                MessageBox.Show("保存异常：" + ee.Message);
            }
        }
        #endregion

        private void TestParamForm_Load(object sender, EventArgs e)
        {
            tabControl1_SelectedIndexChanged(tabControl1, EventArgs.Empty);
        }

        private void labelSerialPort_Click(object sender, EventArgs e)
        {
            comboBoxSerialPort.Items.Clear();
            foreach(string a in System.IO.Ports.SerialPort.GetPortNames())
            {
                comboBoxSerialPort.Items.Add(a);
            }
        }

        private void labelVolPort_Click(object sender, EventArgs e)
        {
            comboBoxVolTDMSerial.Items.Clear();
            foreach(string a in System.IO.Ports.SerialPort.GetPortNames())
            {
                comboBoxVolTDMSerial.Items.Add(a);
            }
        }

        private void labelCurPort_Click(object sender, EventArgs e)
        {
            comboBoxCurTDMSerial.Items.Clear();
            foreach(string a in System.IO.Ports.SerialPort.GetPortNames())
            {
                comboBoxCurTDMSerial.Items.Add(a);
            }
        }

        private void labelFlukePort_Click(object sender, EventArgs e)
        {
            comboBoxCurFluke.Items.Clear();
            foreach(string a in System.IO.Ports.SerialPort.GetPortNames())
            {
                comboBoxCurFluke.Items.Add(a);
            }
        }

        private void labelRelayPort_Click(object sender, EventArgs e)
        {
            comboBoxRelay.Items.Clear();
            foreach(string a in System.IO.Ports.SerialPort.GetPortNames())
            {
                comboBoxRelay.Items.Add(a);
            }
        }

        private void labelRTOSPort_Click(object sender, EventArgs e)
        {
            comboBoxRTOSPort.Items.Clear();
            foreach (string aa in System.IO.Ports.SerialPort.GetPortNames())
            {
                comboBoxRTOSPort.Items.Add(aa);
            }
        }

        private void labelPrinter_Click(object sender, EventArgs e)
        {
            comboBoxPrinter.Items.Clear();
            foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                comboBoxPrinter.Items.Add(printer);
            }
        }

        
    }
}
