using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CMW500Lib
{
    public partial class FormCMW500 : Form
    {
        CMW500Controler cMW500Controler;

        public FormCMW500()
        {
            InitializeComponent();
        }

        private void FormCMW500_Load(object sender, EventArgs e)
        {
            richTextBoxTips.Text = $"步骤一：将PC与综测仪使用网线进行连接；\r\n" +
                $"步骤二：配置综测仪的IP地址，如10.10.10.xx\r\n" +
                $"步骤三：配置PC的网卡IP地址，如10.10.10.xx,子网掩码与综测仪网络配置一致即可\r\n" +
                $"注意：PC可能需要装NI_VISA的驱动";
        }

        private void FormCMW500_Shown(object sender, EventArgs e)
        {

        }

        private void FormCMW500_FormClosing(object sender, FormClosingEventArgs e)
        {
            cMW500Controler?.Close();
        }

        private void richTextBoxRun_TextChanged(object sender, EventArgs e)
        {
            richTextBoxRun.SelectionStart = richTextBoxRun.Text.Length;
            richTextBoxRun.ScrollToCaret();
        }

        private void BtnOpen_Click(object sender, EventArgs e)
        {
            try
            {
                BtnOpen.Enabled = false;
                string ip = textBoxCMW500IP.Text;
                Regex regex = new Regex(@"^((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)(\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)){3})$");
                if (!regex.IsMatch(ip))
                {
                    MessageBox.Show($"请输入正确的IP");
                    return;
                }
                string resourceName = $"TCPIP0::{ip}::inst0::INSTR";
                cMW500Controler = new CMW500Controler(resourceName);
                cMW500Controler.Open();

                // 发送SCPI命令以获取设备标识
                cMW500Controler.SendCommandWithResponse("*IDN?", out SCPIRunDetail sCPIRunDetail);
                richTextBoxRun.Text += sCPIRunDetail.ToString();
                MessageBox.Show($"连接的设备：{sCPIRunDetail.Response}");
            }
            finally
            {
                BtnOpen.Enabled = true;
            }
        }

        private void BtnSend_Click(object sender, EventArgs e)
        {
            try
            {
                BtnSend.Enabled = false;
                if (cMW500Controler == null)
                {
                    MessageBox.Show($"请先连接设备...");
                    BtnOpen.Focus();
                    return;
                }
                string cmd = textBoxSCPICMD.Text.Trim();
                if (cmd.Length == 0)
                {
                    MessageBox.Show($"请先输入指令");
                    textBoxSCPICMD.SelectAll();
                    return;
                }
                cMW500Controler.SendCommandWithResponse(cmd, out SCPIRunDetail sCPIRunDetail);

                richTextBoxRun.Text += sCPIRunDetail.ToString();
            }
            finally
            {
                textBoxSCPICMD.SelectAll();
                textBoxSCPICMD.Focus();
                BtnSend.Enabled = true;
            }
        }

        private void BtnSend1_Click(object sender, EventArgs e)
        {
            try
            {
                BtnSend1.Enabled = false;
                if (cMW500Controler == null)
                {
                    MessageBox.Show($"请先连接设备...");
                    BtnOpen.Focus();
                    return;
                }
                string cmd = comboBoxSCPICMD.Text.Trim();
                if(cmd.Length == 0)
                {
                    MessageBox.Show($"请先输入指令");
                    return;
                }
                cMW500Controler.SendCommandWithResponse(cmd, out SCPIRunDetail sCPIRunDetail);
                richTextBoxRun.Text += sCPIRunDetail.ToString();
            }
            finally
            {
                BtnSend1.Enabled = true;
            }
        }

        private void BtnLTESignal_Click(object sender, EventArgs e)
        {
            try
            {
                if (cMW500Controler == null)
                {
                    MessageBox.Show($"请先连接设备...");
                    BtnOpen.Focus();
                    return;
                }
                BtnLTESignal.Enabled = false;
                if (cMW500Controler == null)
                {
                    MessageBox.Show($"请先打开连接");
                    return;
                }

                // 启动LTE Signal
                cMW500Controler.SendCommandWithResponse(CMW500Controler.CMD_LTE_Signal_ON, out SCPIRunDetail sCPIRunDetail);
                richTextBoxRun.Text += sCPIRunDetail.ToString();
                Thread.Sleep(1000);
                cMW500Controler.SendCommandWithResponse(CMW500Controler.CMD_SET_BAND_3, out SCPIRunDetail sCPIRunDetail1);
                richTextBoxRun.Text += sCPIRunDetail1.ToString();
                Thread.Sleep(1000);
                cMW500Controler.SendCommandWithResponse(CMW500Controler.CMD_LTE_MEAS_INIT, out SCPIRunDetail sCPIRunDetail2);
                richTextBoxRun.Text += sCPIRunDetail2.ToString();
            }
            finally
            {
                BtnLTESignal.Enabled = true;
            }
        }

        private void BtnLTEMultiEvaluation_Click(object sender, EventArgs e)
        {
            try
            {
                if (cMW500Controler == null)
                {
                    MessageBox.Show($"请先连接设备...");
                    BtnOpen.Focus();
                    return;
                }
                BtnLTEMultiEvaluation.Enabled = false;
                string CMD_INIT_LTE_MEvaluation = "INIT:LTE:MEAS:MEValuation";
                string CMD_SET_LTE_MEvaluation_SCENario = "ROUTe:LTE:MEAS:SCENario:CSPath";
                cMW500Controler.SendCommandWithResponse(CMD_SET_LTE_MEvaluation_SCENario, out SCPIRunDetail sCPIRunDetail);
                richTextBoxRun.Text += sCPIRunDetail.ToString();
                Thread.Sleep(1000);
                cMW500Controler.SendCommandWithResponse(CMD_INIT_LTE_MEvaluation, out SCPIRunDetail sCPIRunDetail1);
                richTextBoxRun.Text += sCPIRunDetail1.ToString();
                Thread.Sleep(1000);
            }
            finally
            {
                BtnLTEMultiEvaluation.Enabled = true;
            }
        }

        private void BtnIDN_Click(object sender, EventArgs e)
        {
            try
            {
                if (cMW500Controler == null)
                {
                    MessageBox.Show($"请先连接设备...");
                    BtnOpen.Focus();
                    return;
                }
                BtnIDN.Enabled = false;
                string CMD_IDN = "*IDN?";
                cMW500Controler.SendCommandWithResponse(CMD_IDN, out SCPIRunDetail sCPIRunDetail);
                richTextBoxRun.Text += sCPIRunDetail.ToString();
            }
            catch (Exception)
            {

                throw;
            }
            finally 
            {
                BtnIDN.Enabled = true;
            }
        }

        private void BtnHWOP_Click(object sender, EventArgs e)
        {
            //SYSTem:BASE:OPTion:LIST? HWOP,ALL
            try
            {
                if(cMW500Controler == null)
                {
                    MessageBox.Show($"请先连接设备...");
                    BtnOpen.Focus();
                    return;
                }
                BtnHWOP.Enabled = false;
                string CMD = "SYSTem:BASE:OPTion:LIST? HWOP,ALL";
                cMW500Controler.SendCommandWithResponse(CMD, out SCPIRunDetail sCPIRunDetail);
                richTextBoxRun.Text += sCPIRunDetail.ToString();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                BtnHWOP.Enabled = true;
            }
        }

        private void BtnRTS_Click(object sender, EventArgs e)
        {
            //*RST;*CLS;*OPC?
            try
            {
                if (cMW500Controler == null)
                {
                    MessageBox.Show($"请先连接设备...");
                    BtnOpen.Focus();
                    return;
                }
                BtnRTS.Enabled = false;
                string CMD = "*RST;*CLS;*OPC?";
                cMW500Controler.SendCommandWithResponse(CMD, out SCPIRunDetail sCPIRunDetail);
                richTextBoxRun.Text += sCPIRunDetail.ToString();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                BtnRTS.Enabled = true;
            }
        }

        private void BtnCheckError_Click(object sender, EventArgs e)
        {
            // SYSTem:ERRor:ALL?
            try
            {
                if (cMW500Controler == null)
                {
                    MessageBox.Show($"请先连接设备...");
                    BtnOpen.Focus();
                    return;
                }
                BtnRTS.Enabled = false;
                string CMD = "SYSTem:ERRor:ALL?";
                cMW500Controler.SendCommandWithResponse(CMD, out SCPIRunDetail sCPIRunDetail);
                richTextBoxRun.Text += sCPIRunDetail.ToString();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                BtnRTS.Enabled = true;
            }
        }

        private void BtnSetEATT_Click(object sender, EventArgs e)
        {
            //CONFigure:WLAN:MEAS:RFSettings:EATTenuation 7.96
            try
            {
                if (cMW500Controler == null)
                {
                    MessageBox.Show($"请先连接设备...");
                    BtnOpen.Focus();
                    return;
                }
                double eatt = (double)NudEATT.Value;
                BtnSetEATT.Enabled = false;
                string CMD = $"CONFigure:WLAN:MEAS:RFSettings:EATTenuation {eatt}";
                cMW500Controler.SendCommandWithResponse(CMD, out SCPIRunDetail sCPIRunDetail);
                richTextBoxRun.Text += sCPIRunDetail.ToString();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                BtnSetEATT.Enabled = true;
            }
        }

        private void BtnSetScenario_Click(object sender, EventArgs e)
        {
            //ROUTe:WLAN:MEAS:SCENario:SALone RF1C, RX1
            try
            {
                if (cMW500Controler == null)
                {
                    MessageBox.Show($"请先连接设备...");
                    BtnOpen.Focus();
                    return;
                }
                BtnSetScenario.Enabled = false;
                string CMD = $"ROUTe:WLAN:MEAS:SCENario:SALone RF1C, RX1";
                cMW500Controler.SendCommandWithResponse(CMD, out SCPIRunDetail sCPIRunDetail);
                richTextBoxRun.Text += sCPIRunDetail.ToString();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                BtnSetScenario.Enabled = true;
            }
        }

        private void BtnSetSignal_Click(object sender, EventArgs e)
        {
            //CONFigure:WLAN:MEAS:ISIGnal:STANdard LOFDm;BWIDth BW20mhz
            try
            {
                if (cMW500Controler == null)
                {
                    MessageBox.Show($"请先连接设备...");
                    BtnOpen.Focus();
                    return;
                }
                if(comboBoxStandard.Text.Length == 0)
                {
                    MessageBox.Show($"请先选择Standard");
                    comboBoxStandard.Focus();
                    return;
                }
                BtnSetSignal.Enabled = false;
                string standard = comboBoxStandard.Text.Split('-')[0];
                string CMD = $"CONFigure:WLAN:MEAS:ISIGnal:STANdard {standard};BWIDth BW20mhz";
                cMW500Controler.SendCommandWithResponse(CMD, out SCPIRunDetail sCPIRunDetail);
                richTextBoxRun.Text += sCPIRunDetail.ToString();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                BtnSetSignal.Enabled = true;
            }
        }

        private void BtnSetFreq_Click(object sender, EventArgs e)
        {
            try
            {
                if (cMW500Controler == null)
                {
                    MessageBox.Show($"请先连接设备...");
                    BtnOpen.Focus();
                    return;
                }
                double freq = (double)NudFrequency.Value;
                double enPower = (double)NudEnPower.Value;
                BtnSetFreq.Enabled = false;
                string CMD = $"CONFigure:WLAN:MEAS:RFSettings:FREQuency {freq}MHz;UMAR 0;ENPower {enPower}";
                cMW500Controler.SendCommandWithResponse(CMD, out SCPIRunDetail sCPIRunDetail);
                richTextBoxRun.Text += sCPIRunDetail.ToString();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                BtnSetFreq.Enabled = true;
            }
        }

        private void BtnSetTrigger_Click(object sender, EventArgs e)
        {
            try
            {
                if (cMW500Controler == null)
                {
                    MessageBox.Show($"请先连接设备...");
                    BtnOpen.Focus();
                    return;
                }
                if (comboBoxTriggerSrc.Text.Length == 0) 
                {
                    MessageBox.Show($"请先选择TriggerSRC");
                    comboBoxTriggerSrc.Focus();
                    return;
                }
                string src = comboBoxTriggerSrc.Text;
                double thresold = (double)NudTriggerThreshold.Value;
                BtnSetTrigger.Enabled = false;
                string CMD = $"TRIGger:WLAN:MEAS:MEValuation:SOURce '{src}';THReshold {thresold};";
                cMW500Controler.SendCommandWithResponse(CMD, out SCPIRunDetail sCPIRunDetail);
                richTextBoxRun.Text += sCPIRunDetail.ToString();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                BtnSetTrigger.Enabled = true;
            }
        }

        private void BtnWlanInit_Click(object sender, EventArgs e)
        {
            try
            {
                if (cMW500Controler == null)
                {
                    MessageBox.Show($"请先连接设备...");
                    BtnOpen.Focus();
                    return;
                }
                BtnWlanInit.Enabled = false;
                string CMD = $"INIT:WLAN:MEAS:MEValuation";
                cMW500Controler.SendCommandWithResponse(CMD, out SCPIRunDetail sCPIRunDetail);
                richTextBoxRun.Text += sCPIRunDetail.ToString();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                BtnWlanInit.Enabled = true;
            }
        }

        private void BtnWlanState_Click(object sender, EventArgs e)
        {
            try
            {
                if (cMW500Controler == null)
                {
                    MessageBox.Show($"请先连接设备...");
                    BtnOpen.Focus();
                    return;
                }
                BtnWlanState.Enabled = false;
                string CMD = $"FETCh:WLAN:MEAS:MEValuation:STATe?";
                cMW500Controler.SendCommandWithResponse(CMD, out SCPIRunDetail sCPIRunDetail);
                richTextBoxRun.Text += sCPIRunDetail.ToString();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                BtnWlanState.Enabled = true;
            }
        }

        private void BtnWlanCur_Click(object sender, EventArgs e)
        {
            /*
            CMD:
            -> FETCh:WLAN:MEAS:MEValuation:MODulation:CURR?;
            Response:
            <- 
            0,0.000000E+000,BPSK12,NCAP,336,336,NCAP,NCAP,1,1,NCAP,NCAP,1.405426E+001,NCAP,NCAP,-1.449240E+001,-1.447574E+001,-1.469754E+001,-3.607944E+001,-2.552697E-001,-4.547980E+001,NCAP,7.169678E-002,2.087402E-001
             */
            try
            {
                if (cMW500Controler == null)
                {
                    MessageBox.Show($"请先连接设备...");
                    BtnOpen.Focus();
                    return;
                }
                BtnWlanCur.Enabled = false;
                string CMD = $"FETCh:WLAN:MEAS:MEValuation:MODulation:CURR?";
                cMW500Controler.SendCommandWithResponse(CMD, out SCPIRunDetail sCPIRunDetail);
                richTextBoxRun.Text += sCPIRunDetail.ToString();
                string[] result = sCPIRunDetail.Response.Split(',');
                if(result.Length != 24)
                {
                    MessageBox.Show($"返回结果无法解析");
                    return;
                }
                MeasResult measResult = new MeasResult()
                {
                    ModulationType = result[2],
                    PayloadLength = Convert.ToInt32(result[4]),
                    BrustPower = Math.Round(Convert.ToDouble(result[12]), 2),
                    EVMAllCarriers = Math.Round(Convert.ToDouble(result[15]), 2),
                    EVMDataCarriers = Math.Round(Convert.ToDouble(result[16]), 2),
                    EVMPilotCarriers = Math.Round(Convert.ToDouble(result[17]), 2),
                    CenterFrequencyError = Math.Round(Convert.ToDouble(result[18]), 2),
                    SymbolClockError = Math.Round(Convert.ToDouble(result[19]), 3),
                    IQOffset = Math.Round(Convert.ToDouble(result[20]), 2),
                    GainImbalance = Math.Round(Convert.ToDouble(result[22]), 2),
                    QuadratureError = Math.Round(Convert.ToDouble(result[23]), 2)
                };
                richTextBoxRun.Text += measResult.ToString();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                BtnWlanCur.Enabled = true;
            }
        }

    }
}
