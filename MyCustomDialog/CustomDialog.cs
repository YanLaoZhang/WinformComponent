using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyCustomDialog
{
    public partial class CustomDialog : Form
    {
        public DialogResult Result { get; private set; }

        public CustomDialog(string str_title, string str_content, bool isBottom=false)
        {
            InitializeComponent();
            this.Text = str_title;
            RichTextBoxContent.Text = str_content;
            if(isBottom)
            {
                int screenHight = Screen.PrimaryScreen.WorkingArea.Height;
                int screenWidth = Screen.PrimaryScreen.WorkingArea.Width;
                StartPosition = FormStartPosition.Manual;
                Location = new Point((screenWidth-Width)/2, screenHight - Height);
            }
        }

        private void BtnPass_Click(object sender, EventArgs e)
        {
            this.Result = DialogResult.Yes;
            Close();
        }

        private void BtnFail_Click(object sender, EventArgs e)
        {
            this.Result = DialogResult.No;
            Close();
        }

    }
}
