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
        string exePath = @"D:\05Magnifier\Eone\PCTool\eone-mfg-tool\Eone_MFG2.0_Release\SmartBattery\SH366002_V6297_Customer_2.82_20201217.exe";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            control = new SmartToolControl();
            control.KillAllSmartTool(exePath);
        }

        private void BtnOffsetCalibrate_Click(object sender, EventArgs e)
        {
            try
            {
                BtnOffsetCalibrate.Enabled = false;
                string str_error_log = "";
                control
                    .StartUp(exePath, ref str_error_log)
                    .OffsetCalibrate();
                this.Activate();
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
                string str_error_log = "";
                string act_vol = "3290";
                control
                    .StartUp(exePath, ref str_error_log)
                    .VoltageCalibrate(act_vol);
                this.Activate();
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
                string str_error_log = "";
                string act_cur = "3290";
                control
                    .StartUp(exePath, ref str_error_log)
                    .CurrentCalibrate(act_cur);
                this.Activate();
            }
            finally
            {
                BtnCurrentCalibrate.Enabled = true;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            control.KillAllSmartTool(exePath);
        }

        private void BtnCMDPanel_Click(object sender, EventArgs e)
        {
            try
            {
                BtnCMDPanel.Enabled = false;
                string str_error_log = "";
                string item = "0x0020(Seal Device)";
                control
                    .StartUp(exePath, ref str_error_log)
                    .CMDPanelHandle(item);
                this.Activate();
            }
            finally
            {
                BtnCMDPanel.Enabled = true;
            }
        }
    }
}
