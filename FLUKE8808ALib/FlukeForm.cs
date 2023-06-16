using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FLUKE8808ALib
{
    public partial class FlukeForm : Form
    {
        private FlukeSerial serial;

        private void InitDebug()
        {
            richTextBoxMessage.Text = "";
            // 测试量程
            string error_log = "";
            string currentRange = "";
            serial.GetRange(ref currentRange, ref error_log);
            richTextBoxMessage.Text = richTextBoxMessage.Text + error_log + "；";
            //comboBoxRange.Text = currentRange;
            textBoxRange.Text = currentRange;

            // 测试速度
            string currentRate = "";
            serial.GetRate(ref currentRate, ref error_log);
            richTextBoxMessage.Text = richTextBoxMessage.Text + error_log + "；";
            //comboBoxRate.Text = currentRate;
            textBoxRate.Text = currentRate;

            // 输出格式
            string currentFormat = "";
            serial.GetFormat(ref currentFormat, ref error_log);
            richTextBoxMessage.Text = richTextBoxMessage.Text + error_log + "；";
            //comboBoxFormat.Text = currentFormat;
            textBoxFormat.Text = currentFormat;
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

        private void FlukeForm_Load(object sender, EventArgs e)
        {
            RefreshPort();
        }

        public FlukeForm()
        {
            InitializeComponent();
        }

        private void buttonLock_Click(object sender, EventArgs e)
        {
            if (serial == null)
            {
                MessageBox.Show("请先打开串口");
                comboBoxCurPort.Focus();
                return;
            }
            string error_log = "";
            if (serial.Lock(ref error_log))
            {
                richTextBoxMessage.Text = $"锁定万用表面板成功";
            }
            else
            {
                richTextBoxMessage.Text = error_log;
                MessageBox.Show(error_log);
            }
        }

        private void buttonSetModel_Click(object sender, EventArgs e)
        {
            if (serial == null)
            {
                MessageBox.Show("请先打开串口");
                comboBoxCurPort.Focus();
                return;
            }
            string error_log = "";
            if (serial.SetFunction(ref error_log))
            {
                richTextBoxMessage.Text = $"设置万用表直流电流测试成功";
            }
            else
            {
                richTextBoxMessage.Text = error_log;
                MessageBox.Show(error_log);
            }
        }

        private void buttonSetTrigger_Click(object sender, EventArgs e)
        {
            if (serial == null)
            {
                MessageBox.Show("请先打开串口");
                comboBoxCurPort.Focus();
                return;
            }
            string error_log = "";
            if (serial.SetTriggerModel("1", ref error_log))
            {
                richTextBoxMessage.Text = $"设置万用表内部触发成功";
            }
            else
            {
                richTextBoxMessage.Text = error_log;
                MessageBox.Show(error_log);
            }
        }

        private void buttonRange_Click(object sender, EventArgs e)
        {
            if (serial == null)
            {
                MessageBox.Show("请先打开串口");
                comboBoxCurPort.Focus();
                return;
            }
            if (comboBoxRange.Text.Length == 0)
            {
                MessageBox.Show("请选择要设置的量程");
                return;
            }
            string rangeVal = comboBoxRange.Text.Substring(0, 1);
            string error_log = "";
            if (serial.SetRange(rangeVal, ref error_log))
            {
                richTextBoxMessage.Text = $"设置万用表量程成功";
            }
            else
            {
                richTextBoxMessage.Text = error_log;
                MessageBox.Show(error_log);
            }
        }

        private void buttonRate_Click(object sender, EventArgs e)
        {
            if (serial == null)
            {
                MessageBox.Show("请先打开串口");
                comboBoxCurPort.Focus();
                return;
            }
            if (comboBoxRate.Text.Length == 0)
            {
                MessageBox.Show("请选择要设置的测量速度");
                return;
            }
            string rateVal = comboBoxRate.Text.Substring(0, 1);
            string error_log = "";
            if (serial.SetRate(rateVal, ref error_log))
            {
                richTextBoxMessage.Text = $"设置万用表测量速度成功";
            }
            else
            {
                richTextBoxMessage.Text = error_log;
                MessageBox.Show(error_log);
            }
        }

        private void buttonFormat_Click(object sender, EventArgs e)
        {
            if (serial == null)
            {
                MessageBox.Show("请先打开串口");
                comboBoxCurPort.Focus();
                return;
            }
            if (comboBoxFormat.Text.Length == 0)
            {
                MessageBox.Show("请选择要设置的输出格式");
                return;
            }
            string formatVal = comboBoxFormat.Text.Substring(0, 1);
            string error_log = "";
            if (serial.SetFormat(formatVal, ref error_log))
            {
                richTextBoxMessage.Text = $"设置万用表输出格式成功";
            }
            else
            {
                richTextBoxMessage.Text = error_log;
                MessageBox.Show(error_log);
            }
        }

        private void buttonRead_Click(object sender, EventArgs e)
        {
            if (serial == null)
            {
                MessageBox.Show("请先打开串口");
                comboBoxCurPort.Focus();
                return;
            }
            string result = "";
            string unit = "";
            string error_log = "";
            bool ret = serial.GetCurrent(ref result, ref unit, ref error_log);
            if (ret)
            {
                textBoxRead.Text = result;
                labelUnit.Text = unit;
                richTextBoxMessage.Text = $"读取成功";
            }
            else
            {
                textBoxRead.Text = "error";
                richTextBoxMessage.Text = error_log;
            }
        }

        private void buttonReadRange_Click(object sender, EventArgs e)
        {
            if (serial == null)
            {
                MessageBox.Show("请先打开串口");
                comboBoxCurPort.Focus();
                return;
            }
            string result = "";
            string error_log = "";
            bool ret = serial.GetRange(ref result, ref error_log);
            if (ret)
            {
                textBoxRange.Text = result;
                richTextBoxMessage.Text = $"读取成功";
            }
            else
            {
                textBoxRange.Text = "error";
                richTextBoxMessage.Text = error_log;
            }
        }

        private void buttonReadFormat_Click(object sender, EventArgs e)
        {
            if (serial == null)
            {
                MessageBox.Show("请先打开串口");
                comboBoxCurPort.Focus();
                return;
            }
            string result = "";
            string error_log = "";
            bool ret = serial.GetFormat(ref result, ref error_log);
            if (ret)
            {
                textBoxFormat.Text = result;
                richTextBoxMessage.Text = $"读取成功";
            }
            else
            {
                textBoxFormat.Text = "error";
                richTextBoxMessage.Text = error_log;
            }
        }

        private void buttonReadRate_Click(object sender, EventArgs e)
        {
            if (serial == null)
            {
                MessageBox.Show("请先打开串口");
                comboBoxCurPort.Focus();
                return;
            }
            string result = "";
            string error_log = "";
            bool ret = serial.GetRate(ref result, ref error_log);
            if (ret)
            {
                textBoxRate.Text = result;
                richTextBoxMessage.Text = $"读取成功";
            }
            else
            {
                textBoxRate.Text = "error";
                richTextBoxMessage.Text = error_log;
            }
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            comboBoxRate.Text = "";
            comboBoxRange.Text = "";
            comboBoxFormat.Text = "";
            textBoxRate.Text = "";
            textBoxRange.Text = "";
            textBoxFormat.Text = "";
            textBoxRead.Text = "";
            richTextBoxMessage.Text = "";
            richTextBoxCurrentReceive.Text = "";
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
                if (port != "" && serial == null)
                {
                    serial = new FlukeSerial(serialPortFluke, port, richTextBoxCurrentReceive);
                    serial.OpenPort();
                    InitDebug();
                }
            }
        }
    }
}
