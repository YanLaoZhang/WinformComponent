using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RelaySerialLib
{
    public partial class RelayForm : Form
    {
        private RelaySerial relaySerial;
        public RelayForm()
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
                string port = comboBoxCurPort.SelectedItem.ToString();
                if (port != "" && relaySerial == null)
                {
                    relaySerial = new RelaySerial(serialPortRelay, port);
                    relaySerial.OpenRelayPort();
                }
            }
        }

        private void btn_relay_all_off_Click(object sender, EventArgs e)
        {
            if (relaySerial == null)
            {
                MessageBox.Show("请先打开串口");
                comboBoxCurPort.Focus();
                return;
            }
            if (relaySerial.CloseAllRelay() == false)
            {

            }
        }

        private void btn_read_state_Click(object sender, EventArgs e)
        {
            try
            {
                if (relaySerial == null)
                {
                    MessageBox.Show("请先打开串口");
                    comboBoxCurPort.Focus();
                    return;
                }
                btn_read_state.Enabled = false;
                string str_error_log = "";
                int[] int_relay_status = new int[16];
                if (relaySerial.GetRelayStatus(ref str_error_log, ref int_relay_status) == false)
                {
                    MessageBox.Show(str_error_log, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                rTBRelayStatus.Text += DateTime.Now.ToString() + ": \r\n";
                for (int i = 0; i < 16; i++)
                {
                    if (int_relay_status[i] == 1)
                    {
                        rTBRelayStatus.Text += "channel:" + (i + 1).ToString().PadRight(3) + " ON" + "\r\n";
                    }
                    else
                    {
                        rTBRelayStatus.Text += "channel:" + (i + 1).ToString().PadRight(3) + " OFF" + "\r\n";
                    }
                }
                rTBRelayStatus.Text += "\r\n";
                rTBRelayStatus.SelectionStart = rTBRelayStatus.TextLength;
                rTBRelayStatus.ScrollToCaret();
                SetColorInRichTextBox(rTBRelayStatus, "ON", Color.Green);
                SetColorInRichTextBox(rTBRelayStatus, "OFF", Color.Red);
            }
            catch (Exception ee)
            {

            }
            finally
            {
                btn_read_state.Enabled = true;
            }
        }

        void btn_on_click(System.Object sender, System.EventArgs e)
        {
            try
            {
                if (relaySerial == null)
                {
                    MessageBox.Show("请先打开串口");
                    comboBoxCurPort.Focus();
                    return;
                }
                string _str = sender.ToString();  //System.Windows.Forms.Button, Text: 1_吸合
                _str = _str.Replace("System.Windows.Forms.Button, Text: ", "").Replace("_吸合", "").Trim();
                if (relaySerial.TriggerRelaySingle(ushort.Parse(_str), true) == false)
                {

                }
            }
            catch
            {

            }
        }

        void btn_off_click(System.Object sender, System.EventArgs e)
        {
            try
            {
                if (relaySerial == null)
                {
                    MessageBox.Show("请先打开串口");
                    comboBoxCurPort.Focus();
                    return;
                }
                string _str = sender.ToString();  //System.Windows.Forms.Button, Text: 1_断开
                _str = _str.Replace("System.Windows.Forms.Button, Text: ", "").Replace("_断开", "").Trim();
                if (relaySerial.TriggerRelaySingle(ushort.Parse(_str), false) == false)
                {

                }
            }
            catch
            {

            }
        }

    }
}
