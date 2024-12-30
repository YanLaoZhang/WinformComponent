using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KP184Lib
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            comboBoxSerialPort.Items.Clear();
            foreach (string port in System.IO.Ports.SerialPort.GetPortNames())
            {
                comboBoxSerialPort.Items.Add(port);
            }
        }

        private void BtnLoadON_Click(object sender, EventArgs e)
        {
            try
            {
                BtnLoadON.Enabled = false;
                string portName = comboBoxSerialPort.Text;
                if (portName.Length == 0) {
                    MessageBox.Show("请先选择串口", "错误");
                    comboBoxSerialPort.Focus();
                    return;
                }
                int.TryParse(comboBoxBaudRate.Text, out int baudRate);
                byte.TryParse(numericUpDownADD.Value.ToString(), out byte deviceaAddress);
                using (KP184 kP184 = new KP184(portName, baudRate, deviceaAddress))
                {
                    kP184.SetLoadSwitch(true);
                    MessageBox.Show($"打开操作已完成");
                }
            }
            finally
            {
                BtnLoadON.Enabled = true;

            }

        }

        private void BtnLoadOFF_Click(object sender, EventArgs e)
        {
            try
            {
                BtnLoadOFF.Enabled = false;
                string portName = comboBoxSerialPort.Text;
                if (portName.Length == 0)
                {
                    MessageBox.Show("请先选择串口", "错误");
                    comboBoxSerialPort.Focus();
                    return;
                }
                int.TryParse(comboBoxBaudRate.Text, out int baudRate);
                byte.TryParse(numericUpDownADD.Value.ToString(), out byte deviceaAddress);
                using (KP184 kP184 = new KP184(portName, baudRate, deviceaAddress))
                {
                    kP184.SetLoadSwitch(false);
                    MessageBox.Show($"关闭操作已完成");
                }
            }
            finally
            {
                BtnLoadOFF.Enabled = true;

            }
        }

        private void BtnLoadMode_Click(object sender, EventArgs e)
        {
            try
            {
                BtnLoadMode.Enabled = false;
                string portName = comboBoxSerialPort.Text;
                if (portName.Length == 0)
                {
                    MessageBox.Show("请先选择串口", "错误");
                    comboBoxSerialPort.Focus();
                    return;
                }
                int.TryParse(comboBoxBaudRate.Text, out int baudRate);
                byte.TryParse(numericUpDownADD.Value.ToString(), out byte deviceaAddress);
                int.TryParse(comboBoxMode.Text.Split('-')[0], out int mode);
                using (KP184 kP184 = new KP184(portName, baudRate, deviceaAddress))
                {
                    kP184.SetLoadMode(mode);
                    MessageBox.Show($"设置模式[{comboBoxMode.Text}]已完成");
                }
            }
            finally
            {
                BtnLoadMode.Enabled = true;

            }
        }

        private void BtnCVSetting_Click(object sender, EventArgs e)
        {
            try
            {
                BtnCVSetting.Enabled = false;
                string portName = comboBoxSerialPort.Text;
                if (portName.Length == 0)
                {
                    MessageBox.Show("请先选择串口", "错误");
                    comboBoxSerialPort.Focus();
                    return;
                }
                int.TryParse(comboBoxBaudRate.Text, out int baudRate);
                byte.TryParse(numericUpDownADD.Value.ToString(), out byte deviceaAddress);
                int.TryParse(numericUpDownVol.Value.ToString(), out int voltage);
                using (KP184 kP184 = new KP184(portName, baudRate, deviceaAddress))
                {
                    kP184.SetVoltageLoad(voltage);
                    MessageBox.Show($"设置CV的电压[{voltage}]已完成");
                }
            }
            finally
            {
                BtnCVSetting.Enabled = true;

            }
        }

        private void BtnCCSetting_Click(object sender, EventArgs e)
        {
            try
            {
                BtnCCSetting.Enabled = false;
                string portName = comboBoxSerialPort.Text;
                if (portName.Length == 0)
                {
                    MessageBox.Show("请先选择串口", "错误");
                    comboBoxSerialPort.Focus();
                    return;
                }
                int.TryParse(comboBoxBaudRate.Text, out int baudRate);
                byte.TryParse(numericUpDownADD.Value.ToString(), out byte deviceaAddress);
                int.TryParse(numericUpDownCur.Value.ToString(), out int current);
                using (KP184 kP184 = new KP184(portName, baudRate, deviceaAddress))
                {
                    kP184.SetCurrentLoad(current);
                    MessageBox.Show($"设置CC的电流[{current}]已完成");
                }
            }
            finally
            {
                BtnCCSetting.Enabled = true;

            }
        }

        private void BtnCRSetting_Click(object sender, EventArgs e)
        {
            try
            {
                BtnCRSetting.Enabled = false;
                string portName = comboBoxSerialPort.Text;
                if (portName.Length == 0)
                {
                    MessageBox.Show("请先选择串口", "错误");
                    comboBoxSerialPort.Focus();
                    return;
                }
                int.TryParse(comboBoxBaudRate.Text, out int baudRate);
                byte.TryParse(numericUpDownADD.Value.ToString(), out byte deviceaAddress);
                int.TryParse(numericUpDownResis.Value.ToString(), out int current);
                using (KP184 kP184 = new KP184(portName, baudRate, deviceaAddress))
                {
                    kP184.SetResistanceLoad(current);
                    MessageBox.Show($"设置CR的电阻[{current}]已完成");
                }
            }
            finally
            {
                BtnCRSetting.Enabled = true;

            }
        }

        private void BtnCWSetting_Click(object sender, EventArgs e)
        {
            try
            {
                BtnCWSetting.Enabled = false;
                string portName = comboBoxSerialPort.Text;
                if (portName.Length == 0)
                {
                    MessageBox.Show("请先选择串口", "错误");
                    comboBoxSerialPort.Focus();
                    return;
                }
                int.TryParse(comboBoxBaudRate.Text, out int baudRate);
                byte.TryParse(numericUpDownADD.Value.ToString(), out byte deviceaAddress);
                int.TryParse(numericUpDownPower.Value.ToString(), out int power);
                using (KP184 kP184 = new KP184(portName, baudRate, deviceaAddress))
                {
                    kP184.SetPowerLoad(power);
                    MessageBox.Show($"设置CW的功率[{power}]已完成");
                }
            }
            finally
            {
                BtnCWSetting.Enabled = true;

            }
        }

        private void BtnReadVolAndCur_Click(object sender, EventArgs e)
        {
            try
            {
                BtnReadVolAndCur.Enabled = false;
                string portName = comboBoxSerialPort.Text;
                if (portName.Length == 0)
                {
                    MessageBox.Show("请先选择串口", "错误");
                    comboBoxSerialPort.Focus();
                    return;
                }
                int.TryParse(comboBoxBaudRate.Text, out int baudRate);
                byte.TryParse(numericUpDownADD.Value.ToString(), out byte deviceaAddress);

                textBoxCur.Clear();
                textBoxVol.Clear();
                using (KP184 kP184 = new KP184(portName, baudRate, deviceaAddress))
                {
                    kP184.ReadVoltageAndCurrent(out int voltage, out int current);
                    textBoxVol.Text = voltage.ToString();
                    textBoxCur.Text = current.ToString();
                }
            }
            finally
            {
                BtnReadVolAndCur.Enabled = true;

            }
        }

        private void BtnReadVol_Click(object sender, EventArgs e)
        {
            try
            {
                BtnReadVol.Enabled = false;
                string portName = comboBoxSerialPort.Text;
                if (portName.Length == 0)
                {
                    MessageBox.Show("请先选择串口", "错误");
                    comboBoxSerialPort.Focus();
                    return;
                }
                int.TryParse(comboBoxBaudRate.Text, out int baudRate);
                byte.TryParse(numericUpDownADD.Value.ToString(), out byte deviceaAddress);

                //textBoxCur.Clear();
                textBoxVol.Clear();
                using (KP184 kP184 = new KP184(portName, baudRate, deviceaAddress))
                {
                    kP184.ReadVoltageMeasure();
                    //textBoxVol.Text = voltage.ToString();
                    //textBoxCur.Text = current.ToString();
                }
            }
            finally
            {
                BtnReadVol.Enabled = true;

            }
        }
    }
}
