using ConfigFileLib;
using LogLib;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestDataLib;
using XDC01Debug;
using XDC01SerialLib;

namespace XDC01_Test_Tool
{
    public partial class Form1 : Form
    {
        private string Path_ini = System.Windows.Forms.Application.StartupPath + @"\MyConfig.config";

        private XDC01Serial serial;

        MySqlConnection conn;

        Logger logger;
        public string LogPath = System.Windows.Forms.Application.StartupPath + @"\Log_file";

        XDC01DebugForm XDC01DebugForm;
        Data_information data_InfoForm;

        Check_Status TipsForm;

        private string start_test_time = "";

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

        /// <summary>
        /// 打开调试界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if(serial != null)
            {
                serial.ClosePort();
            }
            if(XDC01DebugForm == null)
            {
                OpenXDC03DebugForm();
            }
            else {
                XDC01DebugForm.WindowState = FormWindowState.Normal;
                XDC01DebugForm.TopMost = true; 
            }
        }

        private void OpenDataInfoForm()
        {
            data_InfoForm = new Data_information();
            data_InfoForm.FormClosed += DataInfoForm_FormClosed;  // 订阅XDC03DebugForm的关闭事件
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
            if(data_InfoForm == null)
            {
                OpenDataInfoForm();
            }
            else
            {
                data_InfoForm.WindowState = FormWindowState.Normal;
                data_InfoForm.TopMost = true;
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
            if(XDC01DebugForm != null)
            {
                XDC01DebugForm.TopMost = false;
                this.TopMost = true;
            }
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
            comboBoxCurPort.SelectedIndex = 0;
        }

        private void labelRefreshPort_Click(object sender, EventArgs e)
        {
            RefreshPort();
        }

        private void InitDataGridView()
        {
            int i = 1;
            dataGridView1.Rows.Clear();
            dataGridView1.Rows.Add(i++, "SN", "", "");
            dataGridView1.Rows.Add(i++, "固件版本", "", "");
            dataGridView1.Rows.Add(i++, "恢复产测模式", "", "");
            //dataGridView1.Rows.Add(i++, "按RESET键重启", "", "");
            dataGridView1.Rows.Add(i++, "检查产测文件", "", "");
            dataGridView1.Rows.Add(i++, "检查WiFi模式", "", "");
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            try
            {
                BtnStart.Enabled = false;
                toolStripButton1.Enabled = false;
                RunTest();
            }
            catch (Exception)
            {
            }
            finally
            {
                BtnStart.Enabled = true;
                toolStripButton1.Enabled = true;
            }
        }

        private void RunTest()
        {
            try
            {
                InitDataGridView();
                if (comboBoxCurPort.SelectedItem == null)
                {
                    logger.ShowLog("请先选择串口号");
                    MessageBox.Show("请先选择串口号");
                    return;
                }
                logger.ShowLog("开始切换");
                labelResult.Text = string.Empty;
                panelResult.BackColor = Color.White;
                start_test_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                labelTestTime.Text = "0s";
                timerTest.Interval = 1000;
                timerTest.Start();

                serial.OpenPort();
                logger.ShowLog($"串口已连接");

                WaitPowerON();

                int i = 0;
                ReadSN(i++);

                CheckVersion(i++);
                ChangeToFactoryTestMode(i++);
                Delay(3000);
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

                string end_test_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string str_sn = dataGridView1.Rows[0].Cells[2].Value.ToString();
                string str_fw = dataGridView1.Rows[1].Cells[2].Value.ToString();
                string str_wifi_mode = dataGridView1.Rows[3].Cells[3].Value.ToString();
                string str_test_file = dataGridView1.Rows[4].Cells[3].Value.ToString();
                string str_test_result = labelResult.Text.ToString();
                string str_sql = $"INSERT INTO change_mode (sn, firmware, start_test_time, end_test_time, test_result, wifi_mode, test_file)" +
                    $"VALUES ('{str_sn}', '{str_fw}', '{start_test_time}', '{end_test_time}', '{str_test_result}', '{str_wifi_mode}', '{str_test_file}');";

                SaveLocalSqliteDB(str_sql);

                UploadToLocalServer(str_sql);

                timerTest.Stop();
                logger.ShowLog($"本轮切换已完成\r\n");
            }
            catch (Exception ee)
            {
                logger.ShowLog($"发生异常：[{ee.Message}]");
            }
        }

        private void WaitPowerON()
        {
            logger.ShowLog("等待机器完全开机");
            int x = this.Location.X + (int)this.Width / 2;
            int y = this.Location.Y + (int)this.Height / 2;
            int poweron_wait = 10;
            try
            {
                poweron_wait = int.Parse(ConfigFile.IniReadValue("Run_Param", "powerOn_wait", Path_ini));
            }
            catch (Exception ee)
            {

            }
            TipsForm = new Check_Status($"等待{poweron_wait}秒，待机器完全开机", x, y);
            TipsForm.Show();
            logger.ShowLog($"等待{poweron_wait}s");
            Delay(poweron_wait * 1000);
            TipsForm.Close();
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

        private void ReadSN(int dgvRowIndex)
        {
            if (serial == null)
            {
                MessageBox.Show("请先打开串口");
                comboBoxCurPort.Focus();
                return;
            }
            string str_error_log = "";
            string SN = "";
            if (serial.GetSN(ref SN, ref str_error_log) == false)
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
            dataGridView1.Rows[0].Cells[2].Value = SN;
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
            conn = new MySqlConnection(connectStr.ToString());
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

        private void timerTest_Tick(object sender, EventArgs e)
        {
            labelTestTime.Text = (int.Parse(labelTestTime.Text.TrimEnd('s')) + 1).ToString() + "s";
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab.Name == "tabPageSwitch")
            {
                comboBoxCurPort.SelectedItem = ConfigFile.IniReadValue("Run_Param", "port", Path_ini);
                //serial = new XDC01Serial(serialPort1, comboBoxCurPort.SelectedItem.ToString(), RichTBSerial);
                dataGridView1.Rows[1].Cells[1].Value = $"固件版本({ConfigFile.IniReadValue("Standard", "firmware", Path_ini)})";
                CheckConnect();
            }
            if (tabControl1.SelectedTab.Name == "tabPageSetting") {
                string standardFw = ConfigFile.IniReadValue("Standard", "firmware", Path_ini);
                textBoxFirmware.Text = standardFw;
                string poweronWait = ConfigFile.IniReadValue("Run_Param", "powerOn_wait", Path_ini);
                string resetWait = ConfigFile.IniReadValue("Run_Param", "reset_wait", Path_ini);
                numericUpDownPowerON.Value = Convert.ToInt32(poweronWait);
                numericUpDownReset.Value = Convert.ToInt32(resetWait);
                textBoxServerIP.Text = ConfigFile.IniReadValue("Server", "db_ip", Path_ini);
                textBoxServerPort.Text = ConfigFile.IniReadValue("Server", "db_port", Path_ini);
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            ConfigFile.IniWriteValue("Standard", "firmware", textBoxFirmware.Text, Path_ini);
            ConfigFile.IniWriteValue("Run_Param", "powerOn_wait", numericUpDownPowerON.Value.ToString(), Path_ini);
            ConfigFile.IniWriteValue("Run_Param", "reset_wait", numericUpDownReset.Value.ToString(), Path_ini);
            ConfigFile.IniWriteValue("Server", "db_ip", textBoxServerIP.Text.ToString(), Path_ini);
            ConfigFile.IniWriteValue("Server", "db_port", textBoxServerPort.Text.ToString(), Path_ini);
            tabControl1.SelectedTab = tabPageSwitch;
        }

        private void CheckConnect()
        {
            MySqlConnectionStringBuilder connectStr = new MySqlConnectionStringBuilder();
            connectStr.Server = ConfigFile.IniReadValue("Server", "db_ip", Path_ini);
            connectStr.Port = uint.Parse(ConfigFile.IniReadValue("Server", "db_port", Path_ini));
            connectStr.Database = "xdc01_management";
            connectStr.UserID = "rckxdc01";
            connectStr.Password = "2022xdc01";
            connectStr.SslMode = MySqlSslMode.None;
            connectStr.ConnectionTimeout = 10;
            conn = new MySqlConnection(connectStr.ToString());
            try
            {
                conn.Open();
                logger.ShowLog("--- 打开本地数据库成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"本地数据库{connectStr.Server}:{connectStr.Port}打开失败，继续操作将无法保存数据", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logger.ShowLog($"--- 打开本地数据库{connectStr.Server}:{connectStr.Port}失败：{ex.Message}");
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            CheckConnect();

            BtnStart.Focus();
        }

        private void comboBoxCurPort_SelectedValueChanged(object sender, EventArgs e)
        {
            if (serial != null)
            {
                logger.ShowLog("切换串口");
                string newPort = "";
                if (comboBoxCurPort.SelectedItem != null)
                {
                    newPort = comboBoxCurPort.SelectedItem.ToString();
                    ConfigFile.IniWriteValue("Run_Param", "port", newPort, Path_ini);
                    serial.ChangePort(newPort);
                }
            }
        }
    }
}
