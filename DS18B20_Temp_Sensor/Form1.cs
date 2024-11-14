using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DS18B20_Temp_Sensor
{
    public partial class Form1 : Form
    {
        TempSensorSerial tempSensorSerial;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void BtnOpenPort_Click(object sender, EventArgs e)
        {
            try
            {
                BtnOpenPort.Enabled = false;
                if (comboBoxSerialPort.Text.Length == 0)
                {
                    MessageBox.Show($"请先选择串口");
                    comboBoxSerialPort.Focus();
                    return;
                }

                // 创建 AdcParser 实例并传入 RichTextBox 控件
                if (tempSensorSerial == null)
                {
                    tempSensorSerial = new TempSensorSerial(comboBoxSerialPort.Text, richTextBoxLog);
                }
                tempSensorSerial.OpenRelayPort();
            }
            finally
            {
                BtnOpenPort.Enabled = true;
            }
        }

        private void BtnClosePort_Click(object sender, EventArgs e)
        {
            tempSensorSerial?.ClosePort();
        }

        private void comboBoxSerialPort_DropDown(object sender, EventArgs e)
        {
            comboBoxSerialPort.Items.Clear();
            foreach (string serialPort in System.IO.Ports.SerialPort.GetPortNames())
            {
                comboBoxSerialPort.Items.Add(serialPort);
            }
        }

        private void BtnTriggerQandAMode_Click(object sender, EventArgs e)
        {
            try
            {
                BtnTriggerQandAMode.Enabled = true;
                if (tempSensorSerial == null)
                {
                    MessageBox.Show($"请先选择串口");
                    comboBoxSerialPort.Focus();
                    return;
                }
                tempSensorSerial.TriggerQandAMode();
            }
            catch (Exception)
            {

            }
            finally 
            { 
                BtnTriggerQandAMode.Enabled = true;
            }
        }

        private void BtnTriggerActiveMode_Click(object sender, EventArgs e)
        {
            try
            {
                BtnTriggerActiveMode.Enabled = true;
                if (tempSensorSerial == null)
                {
                    MessageBox.Show($"请先选择串口");
                    comboBoxSerialPort.Focus();
                    return;
                }
                tempSensorSerial.TriggerActiveMode();
            }
            catch (Exception)
            {

            }
            finally
            {
                BtnTriggerActiveMode.Enabled = true;
            }
        }

        private void BtnActiveRead_Click(object sender, EventArgs e)
        {
            try
            {
                BtnActiveRead.Enabled = true;
                if (tempSensorSerial == null)
                {
                    MessageBox.Show($"请先选择串口");
                    comboBoxSerialPort.Focus();
                    return;
                }
                string str_error_log = "";
                byte[] data = null;
                tempSensorSerial.GetActiveData(ref str_error_log, ref data, out double temp);
                MessageBox.Show($"raw data:[{BitConverter.ToString(data).Replace("-", " ")}], Parse Temp:[{temp}]");
            }
            catch (Exception)
            {

            }
            finally
            {
                BtnActiveRead.Enabled = true;
            }
        }

        private void BtnQandARead_Click(object sender, EventArgs e)
        {
            try
            {
                BtnQandARead.Enabled = true;
                if (tempSensorSerial == null)
                {
                    MessageBox.Show($"请先选择串口");
                    comboBoxSerialPort.Focus();
                    return;
                }
                string str_error_log = "";
                byte[] data = null;
                tempSensorSerial.GetQandAData(ref str_error_log, ref data, out double temp);
                MessageBox.Show($"raw data:[{BitConverter.ToString(data).Replace("-", " ")}], Parse Temp:[{temp}]");
            }
            catch (Exception)
            {

            }
            finally
            {
                BtnQandARead.Enabled = true;
            }
        }
    }
}
