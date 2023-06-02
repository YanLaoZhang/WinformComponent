using ConfigFileLib;
using LogLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XDC01Debug;
using XDC01SerialLib;

namespace XDC01_Test_Tool
{
    public partial class Form1 : Form
    {
        private string Path_ini = System.Windows.Forms.Application.StartupPath + @"\MyConfig.config";

        private XDC01Serial serial;

        Logger logger;
        public string LogPath = System.Windows.Forms.Application.StartupPath + @"\Log_file";

        XDC01DebugForm XDC01DebugForm;

        Check_Status TipsForm;

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

        public Form1()
        {
            InitializeComponent();
        }

        private void OpenXDC03DebugForm()
        {
            XDC01DebugForm = new XDC01DebugForm();
            XDC01DebugForm.FormClosed += XDC03DebugForm_FormClosed;  // 订阅XDC03DebugForm的关闭事件
            XDC01DebugForm.Show();
        }

        private void XDC03DebugForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            XDC01DebugForm = null;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if(XDC01DebugForm == null)
            {
                OpenXDC03DebugForm();
            }
            else {
                XDC01DebugForm.WindowState = FormWindowState.Normal;
                XDC01DebugForm.TopMost = true; 
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(XDC01DebugForm != null)
            {
                XDC01DebugForm.Close();
            }
            if(serial != null)
            {
                serial.ClosePort();
                serial = null;
            }
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            XDC01DebugForm.TopMost = false;
            this.TopMost = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitDataGridView();
            RefreshPort();
            logger = new Logger(ref RichTBRunningLog, ref LogPath);
            comboBoxCurPort.SelectedItem = ConfigFile.IniReadValue("Run_Param", "port", Path_ini);
            serial = new XDC01Serial(serialPort1, comboBoxCurPort.SelectedItem.ToString(), RichTBSerial);
        }

        private void RefreshPort()
        {
            comboBoxCurPort.Items.Clear();
            foreach (string aa in System.IO.Ports.SerialPort.GetPortNames())
            {
                comboBoxCurPort.Items.Add(aa);
            }
        }

        private void labelRefreshPort_Click(object sender, EventArgs e)
        {
            RefreshPort();
        }

        private void InitDataGridView()
        {
            int i = 1;
            dataGridView1.Rows.Add(i++, "RN", "", "");
            dataGridView1.Rows.Add(i++, "固件版本", "", "");
            dataGridView1.Rows.Add(i++, "恢复产测模式", "", "");
            //dataGridView1.Rows.Add(i++, "按RESET键重启", "", "");
            dataGridView1.Rows.Add(i++, "检查产测文件", "", "");
            dataGridView1.Rows.Add(i++, "检查WiFi模式", "", "");
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            RunTest();
        }

        private void RunTest()
        {
            try
            {
                if (comboBoxCurPort.SelectedItem == null)
                {
                    logger.ShowLog("请先选择串口号");
                    MessageBox.Show("请先选择串口号");
                    return;
                }
                logger.ShowLog("开始");
                serial.OpenPort();
                int poweron_wait = 10;
                try
                {
                    poweron_wait = int.Parse(ConfigFile.IniReadValue("Run_Param", "powerOn_wait", Path_ini));
                }
                catch (Exception ee)
                {

                }
                logger.ShowLog($"串口已连接,等待{poweron_wait}s");

                Delay(poweron_wait * 1000);

                int i = 0;
                ReadRN(i++);

                CheckVersion(i++);

                ChangeToFactoryTestMode(i++);

                ShowPopupAndWait();

                bool check1 = CheckFactoryTestFile(i++);

                bool check2 = CheckWiFiFactoryMode(i++);

                if (check1 && check2)
                {
                    labelResult.Text = "PASS";
                    panelResult.BackColor = Color.Green;
                }
                else
                {
                    labelResult.Text = "FAIL";
                    panelResult.BackColor = Color.Red;
                }
            }
            catch (Exception ee)
            {
                logger.ShowLog($"发生异常：[{ee.Message}]");
            }
        }

        private void ReadRN(int dgvRowIndex)
        {
            if (serial == null)
            {
                MessageBox.Show("请先打开串口");
                comboBoxCurPort.Focus();
                return;
            }
            string str_error_log = "";
            string RN = "";
            if (serial.GetRN(ref RN, ref str_error_log) == false)
            {
                logger.ShowLog($"异常：[{str_error_log}]");
                dataGridView1.Rows[dgvRowIndex].Cells[3].Value = "FAIL";
                dataGridView1.Rows[dgvRowIndex].Cells[3].Style.BackColor = Color.Red;
            }
            else
            {
                dataGridView1.Rows[dgvRowIndex].Cells[3].Value = "PASS";
                dataGridView1.Rows[dgvRowIndex].Cells[3].Style.BackColor = Color.Green;
            }
            dataGridView1.Rows[0].Cells[2].Value = RN;
        }

        private bool CheckVersion(int dgvRowIndex)
        {
            try
            {
                logger.ShowLog("读取固件版本信息");
                string str_fw = "";
                string str_error_log = "";
                if(serial.GetFirmwareVersion(ref str_fw, ref str_error_log) == false)
                {
                    logger.ShowLog($"读取Firmware信息异常：[{str_error_log}]");
                    return false;
                }
                else
                {
                    logger.ShowLog($"已读取Firmware版本信息[{str_fw}]");
                }
                dataGridView1.Rows[dgvRowIndex].Cells[2].Value = str_fw;
                string standard_fw = ConfigFile.IniReadValue("Standard", "firmware", Path_ini);
                string result = "";
                bool ret = false;
                if (standard_fw == str_fw)
                {
                    result = "PASS";
                    ret = true;
                    dataGridView1.Rows[dgvRowIndex].Cells[3].Style.BackColor = Color.Green;
                }
                else
                {
                    result = "FAIL";
                    logger.ShowLog($"读取到的Firmware版本信息不匹配。标准版本[{standard_fw}]");
                    ret = false;
                    dataGridView1.Rows[dgvRowIndex].Cells[3].Style.BackColor = Color.Red;
                }
                dataGridView1.Rows[dgvRowIndex].Cells[3].Value = result;
                return ret;
            }
            catch (Exception)
            {

                return false;
            }
        }

        private bool ChangeToFactoryTestMode(int dgvRowIndex)
        {
            try
            {
                logger.ShowLog("恢复产测模式");
                dataGridView1.Rows[dgvRowIndex].Cells[2].Value = "-";
                string str_error_log = "";
                serial.ChangeFactoryMode(ref str_error_log);
                dataGridView1.Rows[dgvRowIndex].Cells[3].Value = "PASS";
                dataGridView1.Rows[dgvRowIndex].Cells[3].Style.BackColor = Color.Green;
                return true;
                //if(serial.ChangeFactoryMode(ref str_error_log))
                //{
                //    dataGridView1.Rows[dgvRowIndex].Cells[3].Value = "PASS";
                //    dataGridView1.Rows[dgvRowIndex].Cells[3].Style.BackColor = Color.Green;
                //    logger.ShowLog("发送恢复产测模式指令成功");
                //    return true;
                //}
                //else
                //{
                //    dataGridView1.Rows[dgvRowIndex].Cells[3].Value = "FAIL";
                //    dataGridView1.Rows[dgvRowIndex].Cells[3].Style.BackColor = Color.Red;
                //    logger.ShowLog($"发送恢复产测模式指令异常：[{str_error_log}]");
                //    return false;
                //}
            }
            catch (Exception ee)
            {
                logger.ShowLog($"发生异常：[{ee.Message}]");
                dataGridView1.Rows[dgvRowIndex].Cells[3].Value = "FAIL";
                dataGridView1.Rows[dgvRowIndex].Cells[3].Style.BackColor = Color.Red;
                return false;
            }
        }

        private void ShowPopupAndWait()
        {
            logger.ShowLog("请按RESET键重启机器");
            int x = this.Location.X + (int)this.Width / 2;
            int y = this.Location.Y + (int)this.Height / 2;
            TipsForm = new Check_Status("请按RESET键，并等待机器重启", x, y);
            TipsForm.Show();
            int reset_wait = 10;
            try
            {
                reset_wait = int.Parse(ConfigFile.IniReadValue("Run_Param", "reset_wait", Path_ini));
            }
            catch (Exception ee)
            {

            }
            logger.ShowLog($"等待{reset_wait}s");
            Delay(reset_wait * 1000);
            TipsForm.Close();
        }

        private bool CheckFactoryTestFile(int dgvRowIndex)
        {
            try
            {
                string str_error_log = "";
                if (serial.CheckFactoryTestFile(ref str_error_log))
                {
                    logger.ShowLog("当前为产测模式");
                    dataGridView1.Rows[dgvRowIndex].Cells[2].Value = "产测";
                    dataGridView1.Rows[dgvRowIndex].Cells[3].Value = "PASS";
                    dataGridView1.Rows[dgvRowIndex].Cells[3].Style.BackColor = Color.Green;
                    return true;
                }
                else
                {
                    logger.ShowLog($"当前为非产测模式：[{str_error_log}]");
                    dataGridView1.Rows[dgvRowIndex].Cells[2].Value = "非产测";
                    dataGridView1.Rows[dgvRowIndex].Cells[3].Value = "FAIL";
                    dataGridView1.Rows[dgvRowIndex].Cells[3].Style.BackColor = Color.Red;
                    return false;
                }
            }
            catch (Exception ee)
            {
                logger.ShowLog($"发生异常：[{ee.Message}]");
                return false;
            }
        }

        private bool CheckWiFiFactoryMode(int dgvRowIndex)
        {
            try
            {
                string str_error_log = "";
                if (serial.CheckWiFiFactoryMode(ref str_error_log))
                {
                    logger.ShowLog("WIFI为产测模式");
                    dataGridView1.Rows[dgvRowIndex].Cells[2].Value = "产测";
                    dataGridView1.Rows[dgvRowIndex].Cells[3].Value = "PASS";
                    dataGridView1.Rows[dgvRowIndex].Cells[3].Style.BackColor = Color.Green;
                    return true;
                }
                else
                {
                    logger.ShowLog($"WiFI为非产测模式：[{str_error_log}]");
                    dataGridView1.Rows[dgvRowIndex].Cells[2].Value = "非产测";
                    dataGridView1.Rows[dgvRowIndex].Cells[3].Value = "FAIL";
                    dataGridView1.Rows[dgvRowIndex].Cells[3].Style.BackColor = Color.Red;
                    return false;
                }
            }
            catch (Exception ee)
            {
                logger.ShowLog($"发生异常：[{ee.Message}]");
                return false;
            }
        }

        private void RichTBScanRN_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
