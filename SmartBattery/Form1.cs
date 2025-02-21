using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmartBattery
{
    public partial class Form1 : Form
    {
        SmartToolControl control;
        string defaultexePath = @"D:\05Magnifier\Eone\PCTool\eone-mfg-tool\Eone_MFG2.0_Release\SmartBattery\SH366002_V6297_Customer_2.82_20201217.exe";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            control = new SmartToolControl();
            control.KillAllSmartTool(defaultexePath);
        }

        private void BtnAFIFlash_Click(object sender, EventArgs e)
        {
            try
            {
                BtnAFIFlash.Enabled = false;

                string exePath = textBoxExePath.Text;
                if (exePath == "")
                {
                    MessageBox.Show($"请先选择烧录工具路径");
                    return;
                }

                string AFIPath = textBoxAFIPath.Text;
                if(AFIPath == "")
                {
                    MessageBox.Show($"请先选择AFI文件");
                    return;
                }

                string str_error_log = "";
                control.KillAllSmartTool(exePath);
                control.StartUp(exePath, ref str_error_log);
                control.SetAFIFile(AFIPath, out bool result, out string error_log);
                this.Activate();
                if (result)
                {
                    MessageBox.Show("Success");
                }
                else
                {
                    MessageBox.Show($"Error：[{error_log}]");
                }
            }
            finally
            {
                BtnAFIFlash.Enabled = true;
            }
        }

        private void BtnAFIFlashVD12D_Click(object sender, EventArgs e)
        {
            try
            {
                BtnAFIFlashVD12D.Enabled = false;

                string exePath = textBoxExePath.Text;
                if (exePath == "")
                {
                    MessageBox.Show($"请先选择烧录工具路径");
                    return;
                }

                string AFIPath = textBoxAFIPath.Text;
                if (AFIPath == "")
                {
                    MessageBox.Show($"请先选择AFI文件");
                    return;
                }

                string str_error_log = "";
                //control.KillAllSmartTool(exePath);
                control.StartUp(exePath, ref str_error_log);
                control.SetAFIFileVD12D(AFIPath, out bool result, out string error_log);
                this.Activate();
                if (result)
                {
                    MessageBox.Show("Success");
                }
                else
                {
                    MessageBox.Show($"Error：[{error_log}]");
                }
            }
            finally
            {
                BtnAFIFlashVD12D.Enabled = true;
            }
        }

        private void BtnScanAll_Click(object sender, EventArgs e)
        {
            try
            {
                BtnScanAll.Enabled = false;

                string exePath = textBoxExePath.Text;
                if (exePath == "")
                {
                    MessageBox.Show($"请先选择烧录工具路径");
                    return;
                }

                string str_error_log = "";
                Console.WriteLine(DateTime.Now + "--- 1---");
                control.StartUp(exePath, ref str_error_log);
                Console.WriteLine(DateTime.Now + "--- 2---");
                control.ScanAll(out bool result, out string error_log);
                Console.WriteLine(DateTime.Now + "--- 3---");
                this.Activate();
                if (result)
                {
                    MessageBox.Show("Success");
                }
                else
                {
                    MessageBox.Show($"Error：[{error_log}]");
                }
            }
            finally
            {
                BtnScanAll.Enabled = true;
            }
        }

        private void BtnActive_Click(object sender, EventArgs e)
        {
            try
            {
                BtnActive.Enabled = false;

                string exePath = textBoxExePath.Text;
                if (exePath == "")
                {
                    MessageBox.Show($"请先选择烧录工具路径");
                    return;
                }

                string str_error_log = "";
                Console.WriteLine(DateTime.Now + "--- 1---");
                control.StartUp(exePath, ref str_error_log);
                Console.WriteLine(DateTime.Now + "--- 2---");
                control.ActivateProcess();
                MessageBox.Show("Success");
            }
            finally
            {
                BtnActive.Enabled = true;
            }
        }

        private void BtnOffsetCalibrate_Click(object sender, EventArgs e)
        {
            try
            {
                BtnOffsetCalibrate.Enabled = false;

                string exePath = textBoxExePath.Text;
                if (exePath == "")
                {
                    MessageBox.Show($"请先选择烧录工具路径");
                    return;
                }

                string str_error_log = "";
                control.StartUp(exePath, ref str_error_log);
                control.OffsetCalibrate(out bool result, out string error_log);
                this.Activate();
                if(result)
                {
                    MessageBox.Show("Success");
                }
                else
                {
                    MessageBox.Show($"Error：[{error_log}]");
                }
            }
            finally
            {
                BtnOffsetCalibrate.Enabled = true;
            }
        }

        private void BtnVoltageCalibrate_Click(object sender, EventArgs e)
        {
            try
            {
                BtnVoltageCalibrate.Enabled = false;
                string exePath = textBoxExePath.Text;
                if (exePath == "")
                {
                    MessageBox.Show($"请先选择烧录工具路径");
                    return;
                }
                string str_error_log = "";
                string act_vol = "3290";
                control._diff_allow = (double)NUDDiffer.Value;
                control.StartUp(exePath, ref str_error_log);
                control.VoltageCalibrate(act_vol, out bool result, out string mes_vol, out string error_log);
                this.Activate();
                if (result)
                {
                    MessageBox.Show("Success");
                }
                else
                {
                    MessageBox.Show($"Error：[{error_log}]");
                }
            }
            finally
            {
                BtnVoltageCalibrate.Enabled = true;
            }
        }

        private void BtnVoltageCalibrateVD12D_Click(object sender, EventArgs e)
        {
            try
            {
                BtnVoltageCalibrateVD12D.Enabled = false;
                string exePath = textBoxExePath.Text;
                if (exePath == "")
                {
                    MessageBox.Show($"请先选择烧录工具路径");
                    return;
                }
                string str_error_log = "";
                string act_vol_cell = "3577";
                string act_vol_bat = "7244";
                string act_vol_pack = "7255";
                control._diff_allow = (double)NUDDiffer.Value;
                control.StartUp(exePath, ref str_error_log);
                control.VoltageCalibrateVD12D(act_vol_cell, act_vol_bat, act_vol_pack, out bool result, out string mes_vol_cell, out string mes_vol_bat, out string mes_vol_pack, out string error_log);
                this.Activate();
                if (result)
                {
                    MessageBox.Show("Success");
                }
                else
                {
                    MessageBox.Show($"Error：[{error_log}]");
                }
            }
            finally
            {
                BtnVoltageCalibrateVD12D.Enabled = true;
            }
        }

        private void BtnCurrentCalibrate_Click(object sender, EventArgs e)
        {
            try
            {
                BtnCurrentCalibrate.Enabled = false;
                string exePath = textBoxExePath.Text;
                if (exePath == "")
                {
                    MessageBox.Show($"请先选择烧录工具路径");
                    return;
                }
                string str_error_log = "";
                string act_cur = "3290";
                control._diff_allow = (double)NUDDiffer.Value;
                control.StartUp(exePath, ref str_error_log);
                control.CurrentCalibrate(act_cur, out bool result, out string mes_cur, out string error_log);
                this.Activate();
                if (result)
                {
                    MessageBox.Show("Success");
                }
                else
                {
                    MessageBox.Show($"Error：[{error_log}]");
                }
            }
            finally
            {
                BtnCurrentCalibrate.Enabled = true;
            }
        }

        private void BtnCMDPanel_Click(object sender, EventArgs e)
        {
            try
            {
                BtnCMDPanel.Enabled = false;
                string exePath = textBoxExePath.Text;
                if (exePath == "")
                {
                    MessageBox.Show($"请先选择烧录工具路径");
                    return;
                }
                string str_error_log = "";
                string item = "0x0020(Seal Device)";
                control.StartUp(exePath, ref str_error_log);
                control.CMDPanelHandle(item, out bool result, out string error_log);
                this.Activate();
                if (result)
                {
                    MessageBox.Show("Success");
                }
                else
                {
                    MessageBox.Show($"Error：[{error_log}]");
                }
            }
            finally
            {
                BtnCMDPanel.Enabled = true;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            control.KillAllSmartTool(defaultexePath);
        }

        private void BtnSelectExePath_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                
            };
            if(ofd.ShowDialog() == DialogResult.OK)
            {
                textBoxExePath.Text = ofd.FileName;
            }
        }

        private void BtnSelectAFIPath_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {

            };
            if(openFileDialog.ShowDialog() == DialogResult.OK)
            {
                textBoxAFIPath.Text = openFileDialog.FileName;
            }
        }

        private void BtnCheckUI_Click(object sender, EventArgs e)
        {
            try
            {
                BtnCheckUI.Enabled = false;
                string exePath = textBoxExePath.Text;
                if (exePath == "")
                {
                    MessageBox.Show($"请先选择烧录工具路径");
                    return;
                }
                string str_error_log = "";
                control.StartUp(exePath, ref str_error_log);
                control.CheckUI(out bool result, out string error_log);
                this.Activate();
                if (result)
                {
                    MessageBox.Show("Success");
                }
                else
                {
                    MessageBox.Show($"Error：[{error_log}]");
                }
            }
            finally
            {
                BtnCheckUI.Enabled = true;
            }
        }

        private void BtnCMDPanelVD12D_Click(object sender, EventArgs e)
        {
            try
            {
                BtnCMDPanelVD12D.Enabled = false;
                string exePath = textBoxExePath.Text;
                if (exePath == "")
                {
                    MessageBox.Show($"请先选择烧录工具路径");
                    return;
                }
                string str_error_log = "";
                string item = "0x0020(DSGFET_Toggle)";
                control.StartUp(exePath, ref str_error_log);
                control.CMDPanelHandle(item, out bool result, out string error_log);
                this.Activate();
                if (result)
                {
                    MessageBox.Show("Success");
                }
                else
                {
                    MessageBox.Show($"Error：[{error_log}]");
                }
            }
            finally
            {
                BtnCMDPanelVD12D.Enabled = true;
            }
        }

        private void BtnAFIFlashFlaUI_Click(object sender, EventArgs e)
        {
            try
            {
                BtnAFIFlashFlaUI.Enabled = false;
                string exePath = textBoxExePath.Text;
                if (exePath == "")
                {
                    MessageBox.Show($"请先选择烧录工具路径");
                    return;
                }
                string AFIPath = textBoxAFIPath.Text;
                if (AFIPath == "")
                {
                    MessageBox.Show($"请先选择AFI文件");
                    return;
                }

                string str_error_log = "";
                SmartToolControlFlaUI smartToolControlFlatUI = new SmartToolControlFlaUI();
                smartToolControlFlatUI.StartUp(exePath, ref str_error_log);
                smartToolControlFlatUI.SetAFIFile(AFIPath, out bool result, out string error_log);
                this.Activate();
                if (result)
                {
                    MessageBox.Show("Success");
                }
                else
                {
                    MessageBox.Show($"Error：[{error_log}]");
                }
                this.Activate();
            }
            finally
            {
                BtnAFIFlashFlaUI.Enabled = true;
            }
        }

        private void BtnScanAllFlaUI_Click(object sender, EventArgs e)
        {
            try
            {
                BtnScanAllFlaUI.Enabled = false;

                string exePath = textBoxExePath.Text;
                if (exePath == "")
                {
                    MessageBox.Show($"请先选择烧录工具路径");
                    return;
                }

                string str_error_log = "";
                SmartToolControlFlaUI smartToolControlFlatUI = new SmartToolControlFlaUI();
                smartToolControlFlatUI.StartUp(exePath, ref str_error_log);
                smartToolControlFlatUI.ScanAll(out bool result, out string error_log);
                this.Activate();
                if (result)
                {
                    MessageBox.Show("Success");
                }
                else
                {
                    MessageBox.Show($"Error：[{error_log}]");
                }
            }
            finally
            {
                BtnScanAllFlaUI.Enabled = true;
            }
        }

        private void BtnOffsetCalibrateFlaUI_Click(object sender, EventArgs e)
        {
            try
            {
                BtnOffsetCalibrateFlaUI.Enabled = false;

                string exePath = textBoxExePath.Text;
                if (exePath == "")
                {
                    MessageBox.Show($"请先选择烧录工具路径");
                    return;
                }

                string str_error_log = ""; 
                SmartToolControlFlaUI smartToolControlFlatUI = new SmartToolControlFlaUI();
                smartToolControlFlatUI.StartUp(exePath, ref str_error_log);
                smartToolControlFlatUI.OffsetCalibrate(out bool result, out string error_log);
                this.Activate();
                if (result)
                {
                    MessageBox.Show("Success");
                }
                else
                {
                    MessageBox.Show($"Error：[{error_log}]");
                }
            }
            finally
            {
                BtnOffsetCalibrateFlaUI.Enabled = true;
            }
        }

        private void BtnVoltageCalibrateFlaUI_Click(object sender, EventArgs e)
        {
            try
            {
                BtnVoltageCalibrateFlaUI.Enabled = false;
                string exePath = textBoxExePath.Text;
                if (exePath == "")
                {
                    MessageBox.Show($"请先选择烧录工具路径");
                    return;
                }
                string str_error_log = "";
                string act_vol = "3290";
                double _diff_allow = (double)NUDDiffer.Value;
                SmartToolControlFlaUI smartToolControlFlatUI = new SmartToolControlFlaUI();
                smartToolControlFlatUI.StartUp(exePath, ref str_error_log);
                smartToolControlFlatUI.VoltageCalibrateEone(act_vol, out bool result, out string mes_vol, out string error_log, _diff_allow);
                this.Activate();
                if (result)
                {
                    MessageBox.Show("Success");
                }
                else
                {
                    MessageBox.Show($"Error：[{error_log}]");
                }
            }
            finally
            {
                BtnVoltageCalibrateFlaUI.Enabled = true;
            }
        }

        private void BtnVoltageCalibrateVD12DFlaUI_Click(object sender, EventArgs e)
        {
            try
            {
                BtnVoltageCalibrateVD12DFlaUI.Enabled = false;
                string exePath = textBoxExePath.Text;
                if (exePath == "")
                {
                    MessageBox.Show($"请先选择烧录工具路径");
                    return;
                }
                string str_error_log = "";
                string act_vol_cell = "3577";
                string act_vol_bat = "7244";
                string act_vol_pack = "7255";
                double _diff_allow = (double)NUDDiffer.Value;
                SmartToolControlFlaUI smartToolControlFlatUI = new SmartToolControlFlaUI();
                smartToolControlFlatUI.StartUp(exePath, ref str_error_log);
                smartToolControlFlatUI.VoltageCalibrateVD12D(act_vol_cell, act_vol_bat, act_vol_pack, out bool result, out string mes_vol_cell, out string mes_vol_bat, out string mes_vol_pack, out string error_log, _diff_allow);
                this.Activate();
                if (result)
                {
                    MessageBox.Show("Success");
                }
                else
                {
                    MessageBox.Show($"Error：[{error_log}]");
                }
            }
            finally
            {
                BtnVoltageCalibrateVD12DFlaUI.Enabled = true;
            }
        }

        private void BtnCurrentCalibrateFlaUI_Click(object sender, EventArgs e)
        {
            try
            {
                BtnCurrentCalibrateFlaUI.Enabled = false;
                string exePath = textBoxExePath.Text;
                if (exePath == "")
                {
                    MessageBox.Show($"请先选择烧录工具路径");
                    return;
                }
                string str_error_log = "";
                string act_cur = "3290";
                double _diff_allow = (double)NUDDiffer.Value;
                SmartToolControlFlaUI smartToolControlFlatUI = new SmartToolControlFlaUI();
                smartToolControlFlatUI.StartUp(exePath, ref str_error_log);
                smartToolControlFlatUI.CurrentCalibrate(act_cur, out bool result, out string mes_cur, out string error_log, _diff_allow);
                this.Activate();
                if (result)
                {
                    MessageBox.Show("Success");
                }
                else
                {
                    MessageBox.Show($"Error：[{error_log}]");
                }
            }
            finally
            {
                BtnCurrentCalibrateFlaUI.Enabled = true;
            }
        }

        private void BtnCMDPanelFlaUI_Click(object sender, EventArgs e)
        {
            try
            {
                BtnCMDPanelFlaUI.Enabled = false;
                string exePath = textBoxExePath.Text;
                if (exePath == "")
                {
                    MessageBox.Show($"请先选择烧录工具路径");
                    return;
                }
                string str_error_log = "";
                string item = "0x0020(Seal Device)";
                SmartToolControlFlaUI smartToolControlFlatUI = new SmartToolControlFlaUI();
                smartToolControlFlatUI.StartUp(exePath, ref str_error_log);
                smartToolControlFlatUI.CMDPanelHandle(item, out bool result, out string error_log);
                this.Activate();
                if (result)
                {
                    MessageBox.Show("Success");
                }
                else
                {
                    MessageBox.Show($"Error：[{error_log}]");
                }
            }
            finally
            {
                BtnCMDPanelFlaUI.Enabled = true;
            }
        }

        private void BtnCMDPanelVD12DFlaUI_Click(object sender, EventArgs e)
        {
            try
            {
                BtnCMDPanelVD12DFlaUI.Enabled = false;
                string exePath = textBoxExePath.Text;
                if (exePath == "")
                {
                    MessageBox.Show($"请先选择烧录工具路径");
                    return;
                }
                string str_error_log = "";
                string item = "0x0020(DSGFET_Toggle)";
                SmartToolControlFlaUI smartToolControlFlatUI = new SmartToolControlFlaUI();
                smartToolControlFlatUI.StartUp(exePath, ref str_error_log);
                smartToolControlFlatUI.CMDPanelHandle(item, out bool result, out string error_log);
                this.Activate();
                if (result)
                {
                    MessageBox.Show("Success");
                }
                else
                {
                    MessageBox.Show($"Error：[{error_log}]");
                }
            }
            finally
            {
                BtnCMDPanelVD12DFlaUI.Enabled = true;
            }
        }
    }
}
