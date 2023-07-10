using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace MyCustomDialog
{
    public partial class CustomDialog : Form
    {
        public DialogResult Result { get; private set; }

        public CustomDialog(string str_title, string str_content, bool isBottom=false, bool isButton=true)
        {
            InitializeComponent();
            this.Text = str_title;
            RichTextBoxContent.Text = str_content;

            int lineCount = RichTextBoxContent.GetLineFromCharIndex(RichTextBoxContent.TextLength) + 1;
            int lineHeight = RichTextBoxContent.Font.Height;
            int padding = 10; // 为了给文本框内容添加一些额外的间距

            int newHeight = lineCount * lineHeight + padding;
            RichTextBoxContent.Height = newHeight;

            this.Height = RichTextBoxContent.Height + BtnPass.Height + 80; 
            if(isBottom)
            {
                int screenHight = Screen.PrimaryScreen.WorkingArea.Height;
                int screenWidth = Screen.PrimaryScreen.WorkingArea.Width;
                StartPosition = FormStartPosition.Manual;
                Location = new Point((screenWidth-Width)/2, screenHight - Height);
            }
            if(isButton == false)
            {
                BtnPass.Visible = false;
                BtnFail.Visible = false;
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
