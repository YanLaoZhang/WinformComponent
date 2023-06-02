using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vlc.DotNet.Forms;

namespace XDC01Debug
{
    public partial class FormVLC : Form
    {
        string uri = "";
        public FormVLC(string uri)
        {
            InitializeComponent();
            this.uri = uri;
            this.labelRTSPURI.Text = uri;
        }

        private void FormVLC_Shown(object sender, EventArgs e)
        {
            //string sout = ":rtsp-tcp";
            vlcControl1.Play(new Uri(this.uri));
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            //Form1.For_control_modify.key_press_replace("F2");
            vlcControl1.Stop();
            this.Close();
        }

        private void buttonNG_Click(object sender, EventArgs e)
        {
            //Form1.For_control_modify.key_press_replace("F12");
            vlcControl1.Stop();
            this.Close();
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
            vlcControl1.Stop();
        }

        private void FormVLC_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.F2)
                {
                    //Form1.For_control_modify.key_press_replace("F2");
                    this.Close();
                }
                if (e.KeyCode == Keys.F12)
                {
                    //Form1.For_control_modify.key_press_replace("F12");
                    this.Close();
                }

            }
            catch (Exception ee)
            {

            }
        }
    }
}
