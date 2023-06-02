using System;
using System.Drawing;
using System.Windows.Forms;

namespace XDC01_Test_Tool
{
    public partial class Check_Status : Form
    {
        string _content = "";
        int _x = 0;
        int _y = 0;
        public Check_Status(string str_content, int x, int y)
        {
            _content = str_content;
            _x = x;
            _y = y;
            InitializeComponent();
        }

        private void Check_Status_Load(object sender, EventArgs e)
        {
            this.Location = new Point(_x - (int)this.Width / 2, _y - (int)this.Height / 2);
            labelContent.Text = _content;
            labelTime.Text = "0s";
            timer1.Interval = 1000;
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            labelTime.Text = (int.Parse(labelTime.Text.TrimEnd('s')) + 1).ToString() + "s";
        }

        private void Check_Status_FormClosing(object sender, FormClosingEventArgs e)
        {
            //timer1.Stop();
        }
    }
}
