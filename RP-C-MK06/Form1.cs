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
        private AdcParser _adcParser;
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
                    _adcParser = new AdcParser(comboBoxSerialPort.Text, 9600, richTextBoxLog);
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

                TimeSpan duration = TimeSpan.FromSeconds((double)numericUpDownDuration.Value);

                List<AdcData> dataList = await _adcParser.ReadForDuration(duration);

                /*Console.WriteLine($"Parsed {dataList.Count} data points:");
                foreach (var data in dataList)
                {
                    Console.WriteLine($"ADC1 Value: {data.Adc1Value}, Pressed: {data.Adc1Pressed}");
                    Console.WriteLine($"ADC2 Value: {data.Adc2Value}, Pressed: {data.Adc2Pressed}");
                }*/
                if (dataList == null || dataList.Count == 0)
                {
                    Console.WriteLine("ADC 数据列表为空。");
                    return;
                }
                textBoxADC1Min.Text = dataList.Min(data=>data.Adc1Value).ToString();
                textBoxADC1Max.Text = dataList.Max(data=>data.Adc1Value).ToString();

                // 获取出现次数最多的 Adc1Value
                var mostFrequentAdc1Value = dataList
                    .GroupBy(data => data.Adc1Value) // 按 Adc1Value 分组
                    .OrderByDescending(group => group.Count()) // 按出现次数降序排列
                    .Select(group => new
                    {
                        Value = group.Key,
                        Count = group.Count()
                    })
                    .FirstOrDefault(); // 获取出现次数最多的值

                if (mostFrequentAdc1Value != null)
                {
                    Console.WriteLine($"出现次数最多的 Adc1Value: {mostFrequentAdc1Value.Value}, 次数: {mostFrequentAdc1Value.Count}");
                    textBoxADC1HFV.Text = mostFrequentAdc1Value.Value.ToString();
                }
                else
                {
                    Console.WriteLine("列表为空，没有值。");
                }

                textBoxADC2Min.Text = dataList.Min(data=>data.Adc2Value).ToString();
                textBoxADC2Max.Text = dataList.Max(data=>data.Adc2Value).ToString();
                // 获取出现次数最多的 Adc1Value
                var mostFrequentAdc2Value = dataList
                    .GroupBy(data => data.Adc2Value) // 按 Adc1Value 分组
                    .OrderByDescending(group => group.Count()) // 按出现次数降序排列
                    .Select(group => new
                    {
                        Value = group.Key,
                        Count = group.Count()
                    })
                    .FirstOrDefault(); // 获取出现次数最多的值

                if (mostFrequentAdc1Value != null)
                {
                    Console.WriteLine($"出现次数最多的 Adc2Value: {mostFrequentAdc2Value.Value}, 次数: {mostFrequentAdc2Value.Count}");
                    textBoxADC2HFV.Text = mostFrequentAdc2Value.Value.ToString();
                }
                else
                {
                    Console.WriteLine("列表为空，没有值。");
                }
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
