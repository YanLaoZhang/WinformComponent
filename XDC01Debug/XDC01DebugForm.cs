using System;
using XDC01SerialLib;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XDC01Debug
{
    public partial class XDC01DebugForm : Form
    {
        private XDC01Serial serial;
        public XDC01DebugForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
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

        private void BtnOpenPort_Click(object sender, EventArgs e)
        {
            string port = comboBoxCurPort.SelectedItem.ToString();
            serial = new XDC01Serial(serialPortXdc01, port, richTextBox1);
            serial.OpenPort();
        }

        private void BtnSendCMD_Click(object sender, EventArgs e)
        {
            string str_cmd = textBoxSendCmd.Text.Trim();
            string str_ret_value = "";
            if (serial.SendCMDToXDC03(str_cmd, 2000, true, ref str_ret_value, "#") == false)
            {
                MessageBox.Show($"发送获取系统信息指令[{str_cmd}]失败");
            }
        }
    }
}
