using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
    }
}
