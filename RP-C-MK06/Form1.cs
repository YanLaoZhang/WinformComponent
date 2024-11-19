using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RP_C_MK06
{
    public partial class Form1 : Form
    {
        private PressureSensor _adcParser;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void comboBoxSerialPort_DropDown(object sender, EventArgs e)
        {
            comboBoxSerialPort.Items.Clear();
            foreach(string  serialPort in System.IO.Ports.SerialPort.GetPortNames())
            {
                comboBoxSerialPort.Items.Add(serialPort);
            }
        }

        private void BtnOpenPort_Click(object sender, EventArgs e)
        {
            try
            {
                BtnOpenPort.Enabled = false;
                if (comboBoxSerialPort.Text.Length == 0) {
                    MessageBox.Show($"请先选择串口");
                    comboBoxSerialPort.Focus();
                    return;
                }

                // 创建 AdcParser 实例并传入 RichTextBox 控件
                if (_adcParser == null)
                {
                    _adcParser = new PressureSensor(comboBoxSerialPort.Text, richTextBoxLog);
                }
                _adcParser.Open();
            }
            finally
            {
                BtnOpenPort.Enabled = true;
            }
        }

        private async void BtnReadADC_Click(object sender, EventArgs e)
        {
            try
            {
                BtnReadADC.Enabled = false;
                textBoxADC1Max.Text = string.Empty;
                textBoxADC1Min.Text = string.Empty;
                textBoxADC1HFV.Text = string.Empty;
                textBoxADC2Max.Text = string.Empty;
                textBoxADC2Min.Text = string.Empty;

                if (_adcParser == null)
                {
                    MessageBox.Show($"请先打开串口");
                    BtnOpenPort.Focus();
                    return;
                }

                double duration = (double)numericUpDownDuration.Value;

                PressureResult pressureResult = await _adcParser.GetResult(duration);

                if (pressureResult == null)
                {
                    Console.WriteLine("Pressure Data is null.");
                    return;
                }
                textBoxADC1Min.Text = pressureResult.Adc1Min.ToString();
                textBoxADC1Max.Text = pressureResult.Adc1Max.ToString();
                textBoxADC1HFV.Text = pressureResult.Adc1Hfv.ToString();

                textBoxADC2Min.Text = pressureResult.Adc2Min.ToString();
                textBoxADC2Max.Text = pressureResult.Adc2Max.ToString();
                textBoxADC2HFV.Text = pressureResult.Adc2Hfv.ToString();
            }
            finally
            {
                BtnReadADC.Enabled = true;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _adcParser?.Dispose();
        }

        private void BtnClosePort_Click(object sender, EventArgs e)
        {
            _adcParser?.Close();
        }
    }
}
