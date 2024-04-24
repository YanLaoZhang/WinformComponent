using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TDMSerialLib
{
    public partial class TDMForm : Form
    {
        private TDMSerial tdmSerial;
        public TDMForm()
        {
            InitializeComponent();
        }

        private void RelayForm_Load(object sender, EventArgs e)
        {
            RefreshPort();
        }

        private void labelRefreshPort_Click(object sender, EventArgs e)
        {
            RefreshPort();
        }

        private void RefreshPort()
        {
            comboBoxCurPort.Items.Clear();
            foreach (string aa in System.IO.Ports.SerialPort.GetPortNames())
            {
                comboBoxCurPort.Items.Add(aa);
            }
        }

        /// <summary>
        /// 设置richTextBox的特定内容的颜色
        /// </summary>
        /// <param name="richTextBox">控件</param>
        /// <param name="keyword">内容</param>
        /// <param name="color">颜色</param>
        private void SetColorInRichTextBox(System.Windows.Forms.RichTextBox richTextBox, string keyword, Color color)
        {
            int startIndex = 0;
            while (startIndex < richTextBox.TextLength)
            {
                int index = richTextBox.Find(keyword, startIndex, RichTextBoxFinds.None);
                if (index == -1)
                    break;

                richTextBox.SelectionStart = index;
                richTextBox.SelectionLength = keyword.Length;
                richTextBox.SelectionColor = color;

                startIndex = index + keyword.Length;
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
                int N = (int)numericUpDownN.Value;
                string port = comboBoxCurPort.SelectedItem.ToString();
                if (port != "" && tdmSerial == null)
                {
                    tdmSerial = new TDMSerial(serialPortRelay, port, N);
                    tdmSerial.OpenSerialPort();
                }
            }
        }

        private void BtnInit_Click(object sender, EventArgs e)
        {
            try
            {
                BtnInit.Enabled = false;
                if (tdmSerial == null)
                {
                    MessageBox.Show("请先打开串口");
                    comboBoxCurPort.Focus();
                    return;
                }
                float val = 0.0f;
                // 停止连续数据
                if (tdmSerial._vol_stop_continuous())
                {
                    MessageBox.Show($"停止连续数据");
                }
            }
            finally
            {
                BtnInit.Enabled = true;
            }
        }

        private void BtnRead_Click(object sender, EventArgs e)
        {
            try
            {
                BtnRead.Enabled = false;
                textBoxValue.Text = string.Empty;
                if (tdmSerial == null)
                {
                    MessageBox.Show("请先打开串口");
                    comboBoxCurPort.Focus();
                    return;
                }
                float val = 0.0f;
                if (tdmSerial._voltage_value(ref val))
                {
                    textBoxValue.Text = val.ToString();
                }
            }
            finally
            {
                BtnRead.Enabled = true;
            }
        }

        private void comboBoxCurPort_DropDown(object sender, EventArgs e)
        {
            RefreshPort();
        }
    }
}
