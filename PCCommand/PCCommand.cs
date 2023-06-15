using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PCCommandLib
{
    public class PCCommand
    {
        private Process curProcess = new Process();
        string str_Rec_cmd = null;
        System.Windows.Forms.RichTextBox _richTextBox;

        /// <summary>
        /// 非阻塞式等待
        /// </summary>
        /// <param name="t"></param>
        public void Delay(int t)
        {
            int numa = Environment.TickCount;
            while (true)
            {
                Application.DoEvents();
                if (Environment.TickCount - numa > t)
                {
                    break;
                }
            }
        }

        public PCCommand(System.Windows.Forms.RichTextBox richTextBox)
        {
            _richTextBox = richTextBox;
            InitInfo();
        }

        private void InitInfo()
        {
            //curProcess.OutputDataReceived -= new DataReceivedEventHandler(ProcessOutDataReceived);
            ProcessStartInfo p = new ProcessStartInfo();
            p.FileName = "cmd.exe";
            //p.Arguments = " -t 192.168.1.103";
            p.UseShellExecute = false;
            p.WindowStyle = ProcessWindowStyle.Hidden;
            p.CreateNoWindow = true;
            p.RedirectStandardError = true;
            p.RedirectStandardInput = true;
            p.RedirectStandardOutput = true;
            curProcess.StartInfo = p;
            curProcess.Start();

            curProcess.BeginOutputReadLine();
            curProcess.OutputDataReceived += new DataReceivedEventHandler(ProcessOutDataReceived);
        }

        public bool SendCMDToPCCMD(string str_send, int t, bool bool_response, string str_end, ref string str_ret_value)
        {
            try
            {
                str_Rec_cmd = "";
                curProcess.StandardInput.WriteLine(str_send);
                //curProcess.StandardInput.WriteLine("\r");
                if (bool_response == false)
                {
                    return true;
                }
                int numa = Environment.TickCount;
                while (true)
                {
                    Application.DoEvents();
                    if (Environment.TickCount - numa > t)
                    {
                        break;
                    }
                    if (str_Rec_cmd.Contains(str_end))
                    {
                        break;
                    }
                }
                str_ret_value = str_Rec_cmd;
                return true;
            }
            catch (Exception ee)
            {
                return false;
            }
        }

        public void ProcessOutDataReceived(object sender, DataReceivedEventArgs e)
        {

            string indata = e.Data + "\r\n";
            str_Rec_cmd += indata;
            _richTextBox.Invoke((Action)(() =>
            {
                _richTextBox.Text += indata;
                _richTextBox.SelectionStart = _richTextBox.TextLength;
                _richTextBox.ScrollToCaret();
            }));

        }

        public bool OpenIperf3(ref string str_error_log)
        {
            try
            {
                CheckIperf3(true);
                string CMD_OPEN_IPERF3 = "iperf3 -s -i 1";
                string str_ret_value = null;
                if (SendCMDToPCCMD(CMD_OPEN_IPERF3, 10000, false, " ", ref str_ret_value) == false)
                {
                    str_error_log = $"发送指令[{CMD_OPEN_IPERF3}]到PC cmd失败";
                    return false;
                }
                return true;
            }
            catch (Exception ee)
            {
                str_error_log = $"OpenIperf3发生异常：[{ee.Message}]";
                return false;
            }
        }

        public bool CheckIperf3(bool bool_close_iperf3)
        {
            try
            {
                System.Diagnostics.Process[] process = System.Diagnostics.Process.GetProcesses();
                foreach (System.Diagnostics.Process p in process)
                {
                    if (p.ProcessName.ToLower() == "iperf3")
                    {
                        if (bool_close_iperf3)
                        {
                            p.Kill();
                        }
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ee)
            {
                return false;
            }

        }

        public List<string> GetLocalIPAddress()
        {
            string ipAddress = "";
            List<string> all = new List<string>();

            // 获取所有网络接口
            NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

            foreach (NetworkInterface networkInterface in networkInterfaces)
            {
                // 过滤回环接口和非活动接口
                if (networkInterface.NetworkInterfaceType == NetworkInterfaceType.Loopback ||
                    networkInterface.OperationalStatus != OperationalStatus.Up)
                {
                    continue;
                }

                // 获取网络接口的 IP 地址集合
                IPInterfaceProperties ipProperties = networkInterface.GetIPProperties();
                UnicastIPAddressInformationCollection ipAddresses = ipProperties.UnicastAddresses;

                foreach (UnicastIPAddressInformation ipAddressInfo in ipAddresses)
                {
                    // 过滤 IPv6 地址和非本地链路地址
                    if (ipAddressInfo.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork &&
                        !IPAddress.IsLoopback(ipAddressInfo.Address))
                    {
                        ipAddress = ipAddressInfo.Address.ToString();
                        //break;
                        if (!string.IsNullOrEmpty(ipAddress))
                        {
                            all.Add(ipAddress);
                        }
                    }
                }

            }

            return all;
        }

        /// <summary>
        /// PC 播放/停止播放 音频文件
        /// </summary>
        /// <param name="play_stop"></param>
        /// <returns></returns>
        public bool PlayWavFile(AxWMPLib.AxWindowsMediaPlayer axWindowsMediaPlayer1, string wav_file, bool play_stop)
        {
            try
            {
                if (play_stop)
                {
                    //axWindowsMediaPlayer1.settings.volume = 100;//int.Parse(_set_param.str_wav_volume);
                    //axWindowsMediaPlayer1.settings.playCount = 1;// int.Parse(_set_param.str_wav_paly_count);
                    //axWindowsMediaPlayer1.URL = wav_file;
                    axWindowsMediaPlayer1.Ctlcontrols.play();
                }
                if (play_stop == false)
                {
                    axWindowsMediaPlayer1.Ctlcontrols.stop();
                }
                return true;
            }
            catch (Exception ee)
            {
                return false;
            }
        }

    }
}
