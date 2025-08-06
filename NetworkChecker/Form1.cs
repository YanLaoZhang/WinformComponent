using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetworkChecker
{
    public partial class Form1: Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void buttonCheckIP_Click(object sender, EventArgs e)
        {
            try
            {
                buttonCheckIP.Enabled = true;

                string ip = textBoxIP.Text;

                var result = await NetworkChecker.PingHostAsync(ip, 2000);

                labelIPStatus.Text = result ? "可ping通" : "不可ping通";
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                buttonCheckIP.Enabled = true;
            }
        }

        private async void buttonCheckIPPort_Click(object sender, EventArgs e)
        {
            try
            {
                buttonCheckIP.Enabled = false;
                string ip = textBoxIP.Text;
                int port = (int)numericUpDownPort.Value;

                var result = await NetworkChecker.CheckIpAndPortAsync(ip, port);
                switch (result)
                {
                    case NetworkChecker.ConnectionStatus.Success:
                        Console.WriteLine("IP和端口均可访问");
                        labelPortStatus.Text = "Success";
                        break;
                    case NetworkChecker.ConnectionStatus.PortClosed:
                        Console.WriteLine("主机在线，但端口关闭");
                        labelPortStatus.Text = "PortClosed";
                        break;
                    case NetworkChecker.ConnectionStatus.HostUnreachable:
                        Console.WriteLine("主机不可达");
                        labelPortStatus.Text = "HostUnreachable";
                        break;
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show($"error:[{ee.Message}]");
            }
            finally
            {
                buttonCheckIP.Enabled = true;
            }
        }
    }
}
