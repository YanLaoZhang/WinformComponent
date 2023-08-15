using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
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
            vlcControl1.Play(new Uri(this.uri));
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

    }
}
