using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XDC03Debug;

namespace XDC03_Test_Tool
{
    public partial class Form1 : Form
    {
        XDC03DebugForm XDC03DebugForm;
        public Form1()
        {
            InitializeComponent();
        }

        private void OpenXDC03DebugForm()
        {
            XDC03DebugForm = new XDC03DebugForm();
            XDC03DebugForm.FormClosed += XDC03DebugForm_FormClosed;  // 订阅XDC03DebugForm的关闭事件
            XDC03DebugForm.Show();
        }

        private void XDC03DebugForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            XDC03DebugForm = null;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if(XDC03DebugForm == null)
            {
                OpenXDC03DebugForm();
            }
            else {
                XDC03DebugForm.WindowState = FormWindowState.Normal;
                XDC03DebugForm.TopMost = true; 
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(XDC03DebugForm != null)
            {
                XDC03DebugForm.Close();
            }
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            XDC03DebugForm.TopMost = false;
            this.TopMost = true;
        }
    }
}
