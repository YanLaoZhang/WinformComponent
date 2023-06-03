using XDC03SerialLib;
using PCCommandLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace XDC03Debug
{
    public partial class XDC03DebugForm : Form
    {
        private XDC03Serial serial;

        private PCCommand pcCommand;

        public XDC03DebugForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            RefreshPort();
        }

        private void labelRefreshPort_Click(object sender, EventArgs e)
        {
            RefreshPort();
        }

        private void RefreshPort()
        {
            comboBoxCurPort.Items.Clear();
            foreach (string aa in System.IO.Ports.SerialPort.GetPortNames())
            {
                comboBoxCurPort.Items.Add(aa);
            }
        }

        private void BtnOpenPort_Click(object sender, EventArgs e)
        {
            if (comboBoxCurPort.SelectedItem == null)
            {
                MessageBox.Show("请先选择端口");
                return;
            }
            else
            {
                string port = comboBoxCurPort.SelectedItem.ToString();
                if (port != "")
                {
                    serial = new XDC03Serial(serialPortXdc03, port, richTextBox1);
                    serial.OpenPort();
                }
            }
        }

        private void BtnSendCMD_Click(object sender, EventArgs e)
        {
            if (serial == null)
            {
                MessageBox.Show("请先打开串口");
                comboBoxCurPort.Focus();
                return;
            }
            string str_cmd = textBoxSendCmd.Text.Trim();
            string str_ret_value = "";
            if (serial.SendCMDToXDC03(str_cmd, 2000, true, ref str_ret_value, "#") == false)
            {
                MessageBox.Show($"发送获取系统信息指令[{str_cmd}]失败");
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {

        }

        private void BtnReadSysInfo_Click(object sender, EventArgs e)
        {
            if (serial == null)
            {
                MessageBox.Show("请先打开串口");
                comboBoxCurPort.Focus();
                return;
            }
            string str_error_log = "";
            CameraInfo cameraInfo = new CameraInfo();
            serial.GetSystemParam(ref cameraInfo, ref str_error_log);
            textBoxFwVer.Text = cameraInfo.FwVersion;
            textBoxHwVer.Text = cameraInfo.HwVersion;
            textBoxMcuVer.Text = cameraInfo.McuAppV;
            textBoxWiFiVer.Text = cameraInfo.WifiVer;
            textBoxBatteryVol.Text = cameraInfo.Battery;
            textBoxBatteryPer.Text = cameraInfo.BatteryPerCent;
            textBoxCpuTemp.Text = cameraInfo.Temp;
        }

        private void BtnReadRN_Click(object sender, EventArgs e)
        {
            if (serial == null)
            {
                MessageBox.Show("请先打开串口");
                comboBoxCurPort.Focus();
                return;
            }
            textBoxReadRN.Text = string.Empty;
            string str_error_log = "";
            string RN = "";
            serial.GetRN(ref RN, ref str_error_log);
            textBoxReadRN.Text = RN;
        }

        private void BtnWriteRN_Click(object sender, EventArgs e)
        {
            if (serial == null)
            {
                MessageBox.Show("请先打开串口");
                comboBoxCurPort.Focus();
                return;
            }
            string write_rn = textBoxWriteRN.Text;
            if (write_rn.Length != 15)
            {
                MessageBox.Show("RN必须为15位");
                return;
            }
            string str_error_log = "";
            serial.SetRN(write_rn, ref str_error_log);
        }

        private void BtnReadTagNum_Click(object sender, EventArgs e)
        {
            if (serial == null)
            {
                MessageBox.Show("请先打开串口");
                comboBoxCurPort.Focus();
                return;
            }
            textBoxReadTagNum.Text = string.Empty;
            string str_error_log = "";
            string str_tagnum = "";
            serial.GetTagNumber(ref str_tagnum, ref str_error_log);
            textBoxReadTagNum.Text = str_tagnum;
        }

        private void BtnWriteTagNum_Click(object sender, EventArgs e)
        {
            if (serial == null)
            {
                MessageBox.Show("请先打开串口");
                comboBoxCurPort.Focus();
                return;
            }
            if (comboBoxWriteTagNum.SelectedItem == null)
            {
                MessageBox.Show("请先选择要写入的工序号");
                comboBoxWriteTagNum.Focus();
                return;
            }
            string str_tagNum = comboBoxWriteTagNum.SelectedItem.ToString();
            string str_error_log = "";
            serial.SetTagNumber(str_tagNum, ref str_error_log);
        }

        private void BtnReadMAC_Click(object sender, EventArgs e)
        {
            if (serial == null)
            {
                MessageBox.Show("请先打开串口");
                comboBoxCurPort.Focus();
                return;
            }
            textBoxReadMAC.Text = string.Empty;
            string str_mac = "";
            string str_error_log = "";
            serial.GetSystemMac(ref str_mac, ref str_error_log);
            textBoxReadMAC.Text = str_mac;
        }

        private void BtnReadIP_Click(object sender, EventArgs e)
        {
            if (serial == null)
            {
                MessageBox.Show("请先打开串口");
                comboBoxCurPort.Focus();
                return;
            }
            textBoxReadIP.Text = string.Empty;
            string str_ip = "";
            string str_error_log = "";
            serial.GetSystemIP(ref str_ip, ref str_error_log);
            textBoxReadIP.Text = str_ip;
        }

        private void BtnReadWiFi_Click(object sender, EventArgs e)
        {
            if (serial == null)
            {
                MessageBox.Show("请先打开串口");
                comboBoxCurPort.Focus();
                return;
            }
            textBoxReadWiFiSSID.Text = string.Empty;
            textBoxReadWiFiPWD.Text = string.Empty;
            string str_wifi_ssid = "";
            string str_error_log = "";
            serial.GetWiFiSSID(ref str_wifi_ssid, ref str_error_log);
            textBoxReadWiFiSSID.Text = str_wifi_ssid;
            string str_wifi_pwd = "";
            str_error_log = "";
            serial.GetWiFiPassword(ref str_wifi_pwd, ref str_error_log);
            textBoxReadWiFiPWD.Text = str_wifi_pwd;
        }

        private void BtnWriteWiFi_Click(object sender, EventArgs e)
        {
            if (serial == null)
            {
                MessageBox.Show("请先打开串口");
                comboBoxCurPort.Focus();
                return;
            }
            string str_wifi_ssid = textBoxWriteWiFiSSID.Text;
            string str_wifi_pwd = textBoxWriteWiFiPWD.Text;
            string str_error_log = "";
            serial.SetWIFIConfig(str_wifi_ssid, str_wifi_pwd, ref str_error_log);
        }

        private void BtnReadLight_Click(object sender, EventArgs e)
        {
            if (serial == null)
            {
                MessageBox.Show("请先打开串口");
                comboBoxCurPort.Focus();
                return;
            }
            textBoxReadLight.Text = string.Empty;
            string str_light = "";
            string str_error_log = "";
            serial.GetLightSensor(ref str_light, ref str_error_log);
            textBoxReadLight.Text = str_light;
        }

        private void BtnFactoryMode_Click(object sender, EventArgs e)
        {
            if (serial == null)
            {
                MessageBox.Show("请先打开串口");
                comboBoxCurPort.Focus();
                return;
            }
            BtnFactoryMode.Enabled = false;
            string str_error_log = "";
            serial.ChangeFactoryMode(ref str_error_log);
            BtnFactoryMode.Enabled = true;
        }

        private void BtnFactoryReset_Click(object sender, EventArgs e)
        {
            if (serial == null)
            {
                MessageBox.Show("请先打开串口");
                comboBoxCurPort.Focus();
                return;
            }
            BtnFactoryReset.Enabled = false;
            string str_error_log = "";
            serial.ResetFactory(ref str_error_log);
            BtnFactoryReset.Enabled = true;
        }

        private void BtnSetLEDColor_Click(object sender, EventArgs e)
        {
            if (serial == null)
            {
                MessageBox.Show("请先打开串口");
                comboBoxCurPort.Focus();
                return;
            }
            if (CbxLedColor.SelectedItem == null)
            {
                MessageBox.Show("请先选择要设置的颜色");
                CbxLedColor.Focus();
                return;
            }
            string color = CbxLedColor.SelectedItem.ToString();
            string str_error_log = "";
            serial.SetBtnLEDColor(color, ref str_error_log);
        }

        private void BtnOpenPIR_Click(object sender, EventArgs e)
        {
            if (serial == null)
            {
                MessageBox.Show("请先打开串口");
                comboBoxCurPort.Focus();
                return;
            }
            string str_error_log = "";
            serial.SetPIR("on", ref str_error_log);
        }

        private void BtnClosePIR_Click(object sender, EventArgs e)
        {
            if (serial == null)
            {
                MessageBox.Show("请先打开串口");
                comboBoxCurPort.Focus();
                return;
            }
            string str_error_log = "";
            serial.SetPIR("off", ref str_error_log);
        }

        private void BtnPlayWav_Click(object sender, EventArgs e)
        {
            if (serial == null)
            {
                MessageBox.Show("请先打开串口");
                comboBoxCurPort.Focus();
                return;
            }
            BtnPlayWav.Enabled = false;
            if (CbxWavIndex.SelectedItem == null)
            {
                MessageBox.Show("请先选择要播放的音频文件");
                CbxWavIndex.Focus();
                return;
            }
            string wavIndex = CbxWavIndex.SelectedItem.ToString();
            string str_error_log = "";
            serial.PlayWav(wavIndex, ref str_error_log);
            //serial.Delay(10000);
            BtnPlayWav.Enabled = true;
        }

        private void BtnOpenMic_Click(object sender, EventArgs e)
        {
            if (serial == null)
            {
                MessageBox.Show("请先打开串口");
                comboBoxCurPort.Focus();
                return;
            }
            string str_error_log = "";
            serial.OpenMic(ref str_error_log);
        }

        private void BtnCloseMic_Click(object sender, EventArgs e)
        {
            if (serial == null)
            {
                MessageBox.Show("请先打开串口");
                comboBoxCurPort.Focus();
                return;
            }
            string str_error_log = "";
            serial.CloseMic(ref str_error_log);
        }

        private void BtnRecordMic_Click(object sender, EventArgs e)
        {
            if (serial == null)
            {
                MessageBox.Show("请先打开串口");
                comboBoxCurPort.Focus();
                return;
            }
            BtnRecordMic.Enabled = false;
            string str_error_log = "";
            string str_duration = textBoxRecordDuration.Text;
            try
            {
                int t = int.Parse(str_duration);
                serial.RecordMic(str_duration, ref str_error_log);
                serial.Delay(t*1000);
            }
            catch (FormatException ee)
            {
                MessageBox.Show($"录音时间格式错误，必须为数字");
            }
            finally {
                BtnRecordMic.Enabled = true;
            }
        }

        private void BtnPlayRecord_Click(object sender, EventArgs e)
        {
            if (serial == null)
            {
                MessageBox.Show("请先打开串口");
                comboBoxCurPort.Focus();
                return;
            }
            BtnPlayRecord.Enabled = false;
            string str_error_log = "";
            serial.PlayRecordWav(ref str_error_log);
            BtnPlayRecord.Enabled = true;
        }

        private void BtnAutoTestMic_Click(object sender, EventArgs e)
        {
            if (serial == null)
            {
                MessageBox.Show("请先打开串口");
                comboBoxCurPort.Focus();
                return;
            }
            textBoxMicMaxAbs.Text = string.Empty;
            textBoxMicDelta.Text = string.Empty;
            textBoxMicResult.Text = string.Empty;
            BtnAutoTestMic.Enabled = false;
            string str_max_abs = "";
            string str_delta = "";
            string str_result = "";
            string str_error_log = "";
            serial.TestRecordWav(ref str_max_abs, ref str_delta, ref str_result, ref str_error_log);
            textBoxMicMaxAbs.Text = str_max_abs;
            textBoxMicDelta.Text = str_delta;
            textBoxMicResult.Text = str_result;
            BtnAutoTestMic.Enabled = true;
        }

        private void BtnOpenIperf3_Click(object sender, EventArgs e)
        {
            pcCommand = new PCCommand(richTextBoxPCCmd);
            string str_error_log = "";
            if (pcCommand.OpenIperf3(ref str_error_log) == false)
            {
                MessageBox.Show($"打开iPerf3失败：[{str_error_log}]");
            }
            comboBoxServerIp.Items.Clear();
            foreach (string ip in pcCommand.GetLocalIPAddress())
            {
                comboBoxServerIp.Items.Add(ip);
            }
        }

        private void BtnWiFiUpT_Click(object sender, EventArgs e)
        {
            if (serial == null)
            {
                MessageBox.Show("请先打开串口");
                comboBoxCurPort.Focus();
                return;
            }
            textBoxUpRate.Text = string.Empty;
            textBoxUpLoss.Text = string.Empty;
            string str_rate = "";
            string str_loss = "";
            string str_error_log = "";
            string str_ip = "";
            if (comboBoxServerIp.SelectedItem != null)
            {
                str_ip = comboBoxServerIp.SelectedItem.ToString();
            }
            else
            {
                MessageBox.Show("请先选择服务器IP");
            }
            string str_duration = numericUpDownDuration.Text.Trim();
            string str_bandwidth = numericUpDownBandWidth.Text.Trim() + comboBoxUnit.SelectedItem.ToString();
            serial.TestWifiUpThroughput(ref str_rate, ref str_loss, ref str_error_log, str_ip, str_duration, str_bandwidth);
            textBoxUpRate.Text = str_rate;
            textBoxUpLoss.Text = str_loss;
        }

        private void BtnWifiDownT_Click(object sender, EventArgs e)
        {
            if (serial == null)
            {
                MessageBox.Show("请先打开串口");
                comboBoxCurPort.Focus();
                return;
            }
            textBoxDownRate.Text = string.Empty;
            textBoxDownLoss.Text = string.Empty;
            string str_rate = "";
            string str_loss = "";
            string str_error_log = "";
            string str_ip = "";
            if (comboBoxServerIp.SelectedItem != null)
            {
                str_ip = comboBoxServerIp.SelectedItem.ToString();
            }
            else
            {
                MessageBox.Show("请先选择服务器IP");
            }
            string str_duration = numericUpDownDuration.Text.Trim();
            string str_bandwidth = numericUpDownBandWidth.Text.Trim() + comboBoxUnit.SelectedItem.ToString();
            serial.TestWifiDownThroughput(ref str_rate, ref str_loss, ref str_error_log, str_ip, str_duration, str_bandwidth);
            textBoxDownRate.Text = str_rate;
            textBoxDownLoss.Text = str_loss;
        }

        private void BtnCloseIperf3_Click(object sender, EventArgs e)
        {
            if (pcCommand != null)
            {
                pcCommand.CheckIperf3(true);
            }
        }

        private void BtnOpenRTSP_Click(object sender, EventArgs e)
        {
            if (serial == null)
            {
                MessageBox.Show("请先打开串口");
                comboBoxCurPort.Focus();
                return;
            }
            string str_ip = "";
            string str_error_log = "";
            serial.GetSystemIP(ref str_ip, ref str_error_log);
            serial.OpenRTSP(ref str_error_log);
            string rtsp_ip = $"rtsp://{str_ip}/1536x1536";
            textBoxRTSPUrl.Text = rtsp_ip;
            FormVLC vLC = new FormVLC(rtsp_ip);
            vLC.Show();
        }

        private void BtnEnterBW_Click(object sender, EventArgs e)
        {
            if (serial == null)
            {
                MessageBox.Show("请先打开串口");
                comboBoxCurPort.Focus();
                return;
            }
            string str_error_log = "";
            string str_state = "on";
            serial.SwitchIR_CUT(str_state, ref str_error_log);
        }

        private void BtnQuitBW_Click(object sender, EventArgs e)
        {
            if (serial == null)
            {
                MessageBox.Show("请先打开串口");
                comboBoxCurPort.Focus();
                return;
            }
            string str_error_log = "";
            string str_state = "off";
            serial.SwitchIR_CUT(str_state, ref str_error_log);
        }

        private void BtnOpenIRLed_Click(object sender, EventArgs e)
        {
            if (serial == null)
            {
                MessageBox.Show("请先打开串口");
                comboBoxCurPort.Focus();
                return;
            }
            string str_error_log = "";
            string str_state = "on";
            serial.SwitchIR_LED(str_state, ref str_error_log);
        }

        private void BtnCloseIRLed_Click(object sender, EventArgs e)
        {
            if (serial == null)
            {
                MessageBox.Show("请先打开串口");
                comboBoxCurPort.Focus();
                return;
            }
            string str_error_log = "";
            string str_state = "off";
            serial.SwitchIR_LED(str_state, ref str_error_log);
        }

        private void BtnPCBARfTx_Click(object sender, EventArgs e)
        {
            if (serial == null)
            {
                MessageBox.Show("请先打开串口");
                comboBoxCurPort.Focus();
                return;
            }
            string str_error_log = "";
            string str_index = CbxPCBARfIndex.SelectedIndex.ToString();
            serial.PCBA_RFTX(str_index, ref str_error_log);
        }

        private void BtnRfTx_Click(object sender, EventArgs e)
        {
            if (serial == null)
            {
                MessageBox.Show("请先打开串口");
                comboBoxCurPort.Focus();
                return;
            }
            string str_error_log = "";
            string str_state = "send";
            serial.SetRFMode(str_state, ref str_error_log);
        }

        private void BtnRfRx_Click(object sender, EventArgs e)
        {
            if (serial == null)
            {
                MessageBox.Show("请先打开串口");
                comboBoxCurPort.Focus();
                return;
            }
            string str_error_log = "";
            string str_state = "rcv";
            serial.SetRFMode(str_state, ref str_error_log);
        }

        private void BtnReadSN_Click(object sender, EventArgs e)
        {
            if (serial == null)
            {
                MessageBox.Show("请先打开串口");
                comboBoxCurPort.Focus();
                return;
            }
            textBoxSN.Text = string.Empty;
            string str_error_log = "";
            string str_sn = "";
            serial.GetSN(ref str_sn, ref str_error_log);
            textBoxSN.Text = str_sn;
        }

        private void BtnReadUID_Click(object sender, EventArgs e)
        {
            if (serial == null)
            {
                MessageBox.Show("请先打开串口");
                comboBoxCurPort.Focus();
                return;
            }
            textBoxUID.Text = string.Empty;
            string str_error_log = "";
            string str_uid = "";
            serial.GetUID(ref str_uid, ref str_error_log);
            textBoxUID.Text = str_uid;
        }

        private void BtnWriteSNUID_Click(object sender, EventArgs e)
        {
            if (serial == null)
            {
                MessageBox.Show("请先打开串口");
                comboBoxCurPort.Focus();
                return;
            }
            string str_uid = textBoxUID.Text.Trim();
            string str_sn = textBoxSN.Text.Trim();
            if (str_uid.Length != 20)
            {
                MessageBox.Show("UID必须为20位");
                textBoxUID.Focus();
                return;
            }
            if (str_sn.Length == 0)
            {
                MessageBox.Show("SN必须为16位");
                textBoxSN.Focus();
                return;
            }
            string str_error_log = "";
            serial.SetUIDandSN(str_uid, str_sn, ref str_error_log);
        }

        private void BtnSetChargeMode_Click(object sender, EventArgs e)
        {
            if (serial == null)
            {
                MessageBox.Show("请先打开串口");
                comboBoxCurPort.Focus();
                return;
            }
            string str_level = textBoxChargeLevel.Text.Trim();
            string str_error_log = "";
            serial.SetChargeMode(str_level, ref str_error_log);
        }

        private void BtnStartCharge_Click(object sender, EventArgs e)
        {
            if (serial == null)
            {
                MessageBox.Show("请先打开串口");
                comboBoxCurPort.Focus();
                return;
            }
            string str_error_log = "";
            string str_state = "enable";
            serial.SetChargeIC(str_state, ref str_error_log);
        }

        private void BtnStopCharge_Click(object sender, EventArgs e)
        {
            if (serial == null)
            {
                MessageBox.Show("请先打开串口");
                comboBoxCurPort.Focus();
                return;
            }
            string str_error_log = "";
            string str_state = "disable";
            serial.SetChargeIC(str_state, ref str_error_log);
        }

        private void BtnOpenCamera_Click(object sender, EventArgs e)
        {
            if (serial == null)
            {
                MessageBox.Show("请先打开串口");
                comboBoxCurPort.Focus();
                return;
            }
            string str_error_log = "";
            serial.OpenCameraRTOS(ref str_error_log);
        }

        private void BtnEnterBWRTOS_Click(object sender, EventArgs e)
        {
            if (serial == null)
            {
                MessageBox.Show("请先打开串口");
                comboBoxCurPort.Focus();
                return;
            }
            string str_error_log = "";
            string str_state = "on";
            serial.SwitchIR_CUT_RTOS(str_state, ref str_error_log);
        }

        private void BtnQuitBWRTOS_Click(object sender, EventArgs e)
        {
            if (serial == null)
            {
                MessageBox.Show("请先打开串口");
                comboBoxCurPort.Focus();
                return;
            }
            string str_error_log = "";
            string str_state = "off";
            serial.SwitchIR_CUT_RTOS(str_state, ref str_error_log);
        }

        private void BtnReadTagNumRTOS_Click(object sender, EventArgs e)
        {
            if (serial == null)
            {
                MessageBox.Show("请先打开串口");
                comboBoxCurPort.Focus();
                return;
            }
            tBReadTagNumRTOS.Text = string.Empty;
            string str_error_log = "";
            string str_tagnum = "";
            serial.GetTagNumberRTOS(ref str_tagnum, ref str_error_log);
            tBReadTagNumRTOS.Text = str_tagnum;
        }

        private void BtnWriteTagNumRTOS_Click(object sender, EventArgs e)
        {
            if (serial == null)
            {
                MessageBox.Show("请先打开串口");
                comboBoxCurPort.Focus();
                return;
            }
            string str_tagNum = CbxWriteTagNumRTOS.SelectedItem.ToString();
            string str_error_log = "";
            serial.SetTagNumberRTOS(str_tagNum, ref str_error_log);
        }

        private void BtnReadRNRTOS_Click(object sender, EventArgs e)
        {
            if (serial == null)
            {
                MessageBox.Show("请先打开串口");
                comboBoxCurPort.Focus();
                return;
            }
            tBReadRNRTOS.Text = string.Empty;
            string str_error_log = "";
            string RN = "";
            serial.GetRnRTOS(ref RN, ref str_error_log);
            tBReadRNRTOS.Text = RN;
        }

        private void comboBoxCurPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (serial != null)
            {
                string newPort = "";
                if (comboBoxCurPort.SelectedItem != null)
                {
                    newPort = comboBoxCurPort.SelectedItem.ToString();
                    serial.ChangePort(newPort);
                }
            }
        }
    }
}
