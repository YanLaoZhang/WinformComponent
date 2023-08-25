namespace MyCustomDialog
{
    partial class CustomDialog
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.RichTextBoxContent = new System.Windows.Forms.RichTextBox();
            this.BtnPass = new System.Windows.Forms.Button();
            this.BtnFail = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // RichTextBoxContent
            // 
            this.RichTextBoxContent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RichTextBoxContent.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.RichTextBoxContent.Enabled = false;
            this.RichTextBoxContent.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.RichTextBoxContent.Location = new System.Drawing.Point(12, 12);
            this.RichTextBoxContent.Name = "RichTextBoxContent";
            this.RichTextBoxContent.ReadOnly = true;
            this.RichTextBoxContent.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.RichTextBoxContent.Size = new System.Drawing.Size(486, 91);
            this.RichTextBoxContent.TabIndex = 1;
            this.RichTextBoxContent.Text = "麦克风录音是否正常？麦克风录音是否正常？麦克风录音是否正常？麦克风录音是否正常？";
            // 
            // BtnPass
            // 
            this.BtnPass.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.BtnPass.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.BtnPass.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.BtnPass.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.BtnPass.Font = new System.Drawing.Font("宋体", 20F);
            this.BtnPass.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.BtnPass.Location = new System.Drawing.Point(112, 109);
            this.BtnPass.Name = "BtnPass";
            this.BtnPass.Size = new System.Drawing.Size(98, 35);
            this.BtnPass.TabIndex = 2;
            this.BtnPass.Text = "PASS";
            this.BtnPass.UseVisualStyleBackColor = false;
            this.BtnPass.Click += new System.EventHandler(this.BtnPass_Click);
            // 
            // BtnFail
            // 
            this.BtnFail.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.BtnFail.BackColor = System.Drawing.Color.Tomato;
            this.BtnFail.DialogResult = System.Windows.Forms.DialogResult.No;
            this.BtnFail.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.BtnFail.Font = new System.Drawing.Font("宋体", 20F);
            this.BtnFail.ForeColor = System.Drawing.Color.Black;
            this.BtnFail.Location = new System.Drawing.Point(273, 109);
            this.BtnFail.Name = "BtnFail";
            this.BtnFail.Size = new System.Drawing.Size(98, 35);
            this.BtnFail.TabIndex = 3;
            this.BtnFail.Text = "FAIL";
            this.BtnFail.UseVisualStyleBackColor = false;
            this.BtnFail.Click += new System.EventHandler(this.BtnFail_Click);
            // 
            // CustomDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.BtnFail;
            this.ClientSize = new System.Drawing.Size(510, 156);
            this.ControlBox = false;
            this.Controls.Add(this.BtnFail);
            this.Controls.Add(this.BtnPass);
            this.Controls.Add(this.RichTextBoxContent);
            this.Font = new System.Drawing.Font("宋体", 16F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CustomDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.CustomDialog_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.RichTextBox RichTextBoxContent;
        private System.Windows.Forms.Button BtnPass;
        private System.Windows.Forms.Button BtnFail;
    }
}

