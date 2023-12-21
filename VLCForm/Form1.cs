using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vlc.DotNet.Core.Interops.Signatures;
using Vlc.DotNet.Forms;

namespace VLCForm
{
    public partial class FormVLC : Form
    {
        string uri = "";

        public static FormVLC For_control_modify = null;

        public int int_formVLC_x;
        public int int_formVLC_y;
        public int int_formVLC_width;
        public int int_formVLC_height;
        public FormVLC(string uri)
        {
            For_control_modify = this;
            InitializeComponent();
            this.uri = uri;
            this.labelRTSPURI.Text = uri;
        }

        private void FormVLC_Shown(object sender, EventArgs e)
        {
            try
            {
                labelError.ForeColor = Color.Black;
                // 初始化 Timer
                updateTimer.Interval = 1000; // 1秒更新一次
                updateTimer.Start();

                //// 初始化网络状态
                //UpdateNetworkStatus();

                Console.WriteLine($"Before State: [{vlcControl1.State}]");
                vlcControl1.Play(new Uri(this.uri));
                Console.WriteLine($"After State: [{vlcControl1.State}]");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public bool FormVLC_position_get()
        {
            try
            {
                int_formVLC_x = this.Location.X;
                int_formVLC_y = this.Location.Y;
                int_formVLC_width = this.Width;
                int_formVLC_height = this.Height;
                return true;
            }
            catch (Exception ee)
            {
                return false;
            }
        }

        private void vlcControl1_VlcLibDirectoryNeeded(object sender, VlcLibDirectoryNeededEventArgs e)
        {
            try
            {
                string vlc_default_path = System.Windows.Forms.Application.StartupPath;
                e.VlcLibDirectory = new System.IO.DirectoryInfo(vlc_default_path);
            }
            catch (Exception ee)
            {

            }
        }

        private void FormVLC_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (updateTimer.Enabled)
            {
                updateTimer.Stop();
            }
            //try
            //{
            //    // 在后台线程中执行停止播放操作
            //    Thread stopThread = new Thread(() =>
            //    {
            //        if (vlcControl1 != null)
            //        {
            //            System.Diagnostics.Trace.WriteLine("开始停止播放");
            //            vlcControl1.Stop();
            //            System.Diagnostics.Trace.WriteLine("完成停止播放");
            //            vlcControl1.Dispose();
            //            vlcControl1 = null;
            //            System.Diagnostics.Trace.WriteLine("释放LibVLC实例");
            //        }
            //    });
            //    stopThread.Start();
            //}
            //catch (Exception ee)
            //{
            //    System.Diagnostics.Trace.WriteLine($"发生异常[{ee.Message}]");
            //}
        }

        private void vlcControl1_EncounteredError(object sender, Vlc.DotNet.Core.VlcMediaPlayerEncounteredErrorEventArgs e)
        {
            Console.WriteLine($"Error:{e}");
            if (InvokeRequired)
            {
                // 如果在不同的线程上调用，使用Invoke将操作委托到UI线程
                Invoke(new Action(() => {
                    labelError.Text = $"RTSP拉流Error,可能存在网络卡顿";
                    labelError.ForeColor = Color.Red;
                }));
            }
            else
            {
                // 如果在同一线程上调用，直接处理错误
                labelError.Text = $"RTSP拉流Error,可能存在网络卡顿";
                labelError.ForeColor = Color.Red;
            }
            
        }

        private void UpdateNetworkStatus()
        {
            // 获取所有网络接口
            NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

            // 清空先前的信息
            //listBoxNetworkStatus.Items.Clear();

            foreach (NetworkInterface networkInterface in networkInterfaces)
            {
                // 过滤回环接口和非活动接口
                if (networkInterface.NetworkInterfaceType == NetworkInterfaceType.Loopback ||
                    networkInterface.OperationalStatus != OperationalStatus.Up)
                {
                    continue;
                }

                if(networkInterface.Name != "WLAN")
                {
                    continue;
                }
                // 获取网络接口的状态和属性
                string interfaceInfo = $"{networkInterface.Name} - {networkInterface.Description} - {networkInterface.OperationalStatus}";


                // 获取更详细的信息，如 IP 地址、速度等
                IPInterfaceProperties properties = networkInterface.GetIPProperties();
                foreach (UnicastIPAddressInformation address in properties.UnicastAddresses)
                {
                    interfaceInfo += $"\n  IP Address: {address.Address}";

                    string ip = address.Address.ToString();
                    //if (ip == "")
                    //{
                    //    groupBox1.Text = $"实时RTSP - 客户端IP：{ip}, 当前网速: {networkInterface.Speed / 1000000} Mbps，当前播放状态：{vlcControl1.State}";
                    //}

                }
            }
        }

        private void updateTimer_Tick(object sender, EventArgs e)
        {
            // 定期更新网络状态
            //UpdateNetworkStatus();
            groupBox1.Text = $"实时RTSP - 当前播放状态：{vlcControl1.State}";
        }
    }
}
