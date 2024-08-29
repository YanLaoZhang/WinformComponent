using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MS8040Lib
{
    public partial class MS8040Form : Form
    {
        private MS8040Serial mS8040;
        public MS8040Form()
        {
            InitializeComponent();
        }

        private void MS8040Form_Load(object sender, EventArgs e)
        {
            this.Text += $"_V{System.Windows.Forms.Application.ProductVersion}";
        }

        private void comboBoxCurPort_DropDown(object sender, EventArgs e)
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
                if (port != "" && mS8040 == null)
                {
                    mS8040 = new MS8040Serial(serialPortMS8040, port);
                    mS8040.OpenSerialPort();
                }
            }
        }

        private void buttonRead_Click(object sender, EventArgs e)
        {
            try
            {
                buttonRead.Enabled = false;
                if (mS8040 == null)
                {
                    MessageBox.Show("请先打开串口");
                    comboBoxCurPort.Focus();
                    return;
                }
                string str_error_log = "";
                float current = 0.0f;
                bool ret = mS8040.GetCurrentData(ref current, ref str_error_log);
                if (ret)
                {
                    textBoxRead.Text = current.ToString();
                    richTextBoxMessage.Text = $"读取成功";
                }
                else
                {
                    textBoxRead.Text = "error";
                    richTextBoxMessage.Text = str_error_log;
                }
            }
            finally
            {
                buttonRead.Enabled = true;
            }

        }
    }
}
