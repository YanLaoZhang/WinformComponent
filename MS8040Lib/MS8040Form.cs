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


        private void BtnRead_Click(object sender, EventArgs e)
        {
            try
            {
                BtnRead.Enabled = false;
                if (comboBoxCurPort.SelectedItem == null)
                {
                    MessageBox.Show("请先选择端口");
                    return;
                }
                string port = comboBoxCurPort.SelectedItem.ToString();
                double duration = (double)numericUpDownDuration.Value;
                // 1. 实例化串口数据处理类
                SerialDataProcessor processor = new SerialDataProcessor(port);

                // 2. 采集数据，采集时间为 10 秒
                string str_error_log = "";
                DataResult dataResult = processor.GetCurrentData(duration, ref str_error_log);
                if (dataResult != null)
                {
                    textBoxRead.Text = dataResult.AverageExcludingMinMax.ToString();
                    labelUnit.Text = dataResult.Unit;
                    richTextBoxMessage.Text = $"读取成功,\r\n{dataResult.ToString()}";
                }
                else
                {
                    textBoxRead.Text = "error";
                    labelUnit.Text = "";
                    richTextBoxMessage.Text = str_error_log;
                }
            }
            finally
            {
                BtnRead.Enabled = true;
            }

        }
    }
}
