using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DSA700Lib
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void labelDevices_Click(object sender, EventArgs e)
        {
            try
            {
                // 获取所有可用设备
                //var availableDevices = DSA700Controller.GetAvailableDevices();
                var availableDevices = await DSA700Controller.FindAvailableDevicesAsync();

                if (availableDevices.Length == 0)
                {
                    Console.WriteLine("No devices found.");
                    return;
                }

                // 打印所有可用设备
                Console.WriteLine("Available devices:");
                comboBoxDevices.Items.Clear();
                foreach (var device in availableDevices)
                {
                    Console.WriteLine(device);
                    comboBoxDevices.Items.Add(device);
                }
                /*
                // 选择第一个可用设备进行连接
                string resourceName = availableDevices[0];

                using (var dsa700 = new DSA700Controller(resourceName))
                {
                    // 设置中心频率为1 GHz
                    dsa700.SendCommand(":FREQ:CENT 1E9");
                    Console.WriteLine("Center Frequency set to 1 GHz.");

                    // 查询并显示当前中心频率
                    string frequency = dsa700.Query(":FREQ:CENT?");
                    Console.WriteLine($"Current Center Frequency: {frequency} Hz");
                }*/
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnSendCMD_Click(object sender, EventArgs e)
        {
            try
            {
                BtnSendCMD.Enabled = false;
                string device = comboBoxDevices.Text;
                if (device.Length == 0) 
                {
                    MessageBox.Show($"请先选择设备");
                    comboBoxDevices.Focus();
                    return;
                }
                string cmd = comboBoxCMD.Text;
                if (cmd.Length == 0) 
                {
                    MessageBox.Show($"请先选择指令");
                    comboBoxCMD.Focus();
                    return;
                }
                richTextBox1.Clear();
                using (var dsa700 = new DSA700Controller(device))
                {
                    Console.WriteLine($"CMD Send: {cmd}");
                    richTextBox1.Text += $"CMD Send: {cmd}";
                    if (cmd.Contains("?"))
                    {
                        // 查询指令
                        string result = dsa700.Query(cmd);
                        Console.WriteLine($"CMD Result: {result}");
                        richTextBox1.Text += $"CMD Result: {result}";
                    }
                    else
                    {
                        // 设置指令
                        dsa700.SendCommand(cmd);
                        richTextBox1.Text += $"CMD Send OK";
                    }
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
            finally
            {
                BtnSendCMD.Enabled = true;
            }
        }

        public void SendDSA(string device, string cmd, out string result)
        {
            using (var dsa700 = new DSA700Controller(device))
            {
                richTextBox1.Text += $"CMD Send: {cmd}";
                if (cmd.Contains("?"))
                {
                    // 查询指令
                    result = dsa700.Query(cmd);
                    Console.WriteLine($"CMD Result: {result}");
                    richTextBox1.Text += $"\r\nCMD Result: {result}";
                }
                else
                {
                    // 设置指令
                    result = "";
                    dsa700.SendCommand(cmd);
                    richTextBox1.Text += $"\r\nCMD Send OK";
                }
            }
        }

        private void BtnSetFreq_Click(object sender, EventArgs e)
        {
            try
            {
                BtnSetFreq.Enabled = false;
                string device = comboBoxDevices.Text;
                if (device.Length == 0)
                {
                    MessageBox.Show($"请先选择设备");
                    comboBoxDevices.Focus();
                    return;
                }
                richTextBox1.Clear();
                string cmd = DSA700Controller.DSA_SET_FREQ;
                SendDSA(device, cmd, out string result);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
            finally 
            {
                BtnSetFreq.Enabled = true;
            }
        }

        private void BtnTrackingOFF_Click(object sender, EventArgs e)
        {
            try
            {
                BtnTrackingOFF.Enabled = false;
                string device = comboBoxDevices.Text;
                if (device.Length == 0)
                {
                    MessageBox.Show($"请先选择设备");
                    comboBoxDevices.Focus();
                    return;
                }
                richTextBox1.Clear();
                string cmd = DSA700Controller.DSA_CLOSE_TRACKING;
                SendDSA(device, cmd, out string result);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
            finally
            {
                BtnTrackingOFF.Enabled = true;
            }
        }

        private void BtnMaxHold_Click(object sender, EventArgs e)
        {
            try
            {
                BtnMaxHold.Enabled = false;
                string device = comboBoxDevices.Text;
                if (device.Length == 0)
                {
                    MessageBox.Show($"请先选择设备");
                    comboBoxDevices.Focus();
                    return;
                }
                richTextBox1.Clear();
                string cmd = DSA700Controller.DSA_TRACe1_MODE_MAX_HOLD;
                SendDSA(device, cmd, out string result);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
            finally
            {
                BtnMaxHold.Enabled = true;
            }
        }

        private void BtnWRTIE_Click(object sender, EventArgs e)
        {
            try
            {
                BtnWRTIE.Enabled = false;
                string device = comboBoxDevices.Text;
                if (device.Length == 0)
                {
                    MessageBox.Show($"请先选择设备");
                    comboBoxDevices.Focus();
                    return;
                }
                richTextBox1.Clear();
                string cmd = DSA700Controller.DSA_TRACe1_MODE_WRITe;
                SendDSA(device, cmd, out string result);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
            finally
            {
                BtnWRTIE.Enabled = true;
            }
        }

        private void BtnMaxPeakSearch_Click(object sender, EventArgs e)
        {
            try
            {
                BtnMaxPeakSearch.Enabled = false;
                string device = comboBoxDevices.Text;
                if (device.Length == 0)
                {
                    MessageBox.Show($"请先选择设备");
                    comboBoxDevices.Focus();
                    return;
                }
                richTextBox1.Clear();
                string cmd = DSA700Controller.DSA_SET_MARKER1_PEAK_SERACH_MODE;
                SendDSA(device, cmd, out string result);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
            finally
            {
                BtnMaxPeakSearch.Enabled = true;
            }
        }

        private void BtnPeakSearch_Click(object sender, EventArgs e)
        {
            try
            {
                BtnPeakSearch.Enabled = false;
                string device = comboBoxDevices.Text;
                if (device.Length == 0)
                {
                    MessageBox.Show($"请先选择设备");
                    comboBoxDevices.Focus();
                    return;
                }
                richTextBox1.Clear();
                string cmd = DSA700Controller.DSA_SET_MARKER1_MAX;
                SendDSA(device, cmd, out string result);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
            finally
            {
                BtnPeakSearch.Enabled = true;
            }
        }

        private void BtnReadPower_Click(object sender, EventArgs e)
        {
            try
            {
                BtnReadPower.Enabled = false;
                string device = comboBoxDevices.Text;
                if (device.Length == 0)
                {
                    MessageBox.Show($"请先选择设备");
                    comboBoxDevices.Focus();
                    return;
                }
                richTextBox1.Clear();
                string cmd = DSA700Controller.DSA_QUERY_MARKE_RANGE;
                SendDSA(device, cmd, out string result);
                double power = Convert.ToDouble(result);
                double final_power = Math.Round(power, 2);
                richTextBox1.Text += final_power;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
            finally
            {
                BtnReadPower.Enabled = true;
            }
        }

        private void BtnReadFreq_Click(object sender, EventArgs e)
        {
            try
            {
                BtnReadFreq.Enabled = false;
                string device = comboBoxDevices.Text;
                if (device.Length == 0)
                {
                    MessageBox.Show($"请先选择设备");
                    comboBoxDevices.Focus();
                    return;
                }
                richTextBox1.Clear();
                string cmd = DSA700Controller.DSA_QUERY_MARKE_FREQ;
                SendDSA(device, cmd, out string result);
                float freq = Convert.ToSingle(result)/1000000.0f;
                richTextBox1.Text += freq;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
            finally
            {
                BtnReadFreq.Enabled = true;
            }
        }

        private async void Form1_Shown(object sender, EventArgs e)
        {
            var availableDevices = await DSA700Controller.FindAvailableDevicesAsync();

            if (availableDevices.Length == 0)
            {
                Console.WriteLine("No devices found.");
                return;
            }

            // 打印所有可用设备
            Console.WriteLine("Available devices:");
            comboBoxDevices.Items.Clear();
            foreach (var device in availableDevices)
            {
                Console.WriteLine(device);
                comboBoxDevices.Items.Add(device);
                if (device.Contains("DSA"))
                {
                    comboBoxDevices.Text = device;
                }
            }
            
        }
    }
}
