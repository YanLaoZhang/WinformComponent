namespace XDC01Debug
{
    partial class XDC01DebugForm
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
            this.components = new System.ComponentModel.Container();
            this.BtnOpenPort = new System.Windows.Forms.Button();
            this.labelRefreshPort = new System.Windows.Forms.Label();
            this.comboBoxCurPort = new System.Windows.Forms.ComboBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.serialPortXdc01 = new System.IO.Ports.SerialPort(this.components);
            this.textBoxSendCmd = new System.Windows.Forms.TextBox();
            this.BtnSendCMD = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // BtnOpenPort
            // 
            this.BtnOpenPort.Location = new System.Drawing.Point(232, 4);
            this.BtnOpenPort.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.BtnOpenPort.Name = "BtnOpenPort";
            this.BtnOpenPort.Size = new System.Drawing.Size(135, 32);
            this.BtnOpenPort.TabIndex = 27;
            this.BtnOpenPort.Text = "打开串口";
            this.BtnOpenPort.UseVisualStyleBackColor = true;
            this.BtnOpenPort.Click += new System.EventHandler(this.BtnOpenPort_Click);
            // 
            // labelRefreshPort
            // 
            this.labelRefreshPort.AutoSize = true;
            this.labelRefreshPort.Location = new System.Drawing.Point(16, 12);
            this.labelRefreshPort.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelRefreshPort.Name = "labelRefreshPort";
            this.labelRefreshPort.Size = new System.Drawing.Size(39, 16);
            this.labelRefreshPort.TabIndex = 26;
            this.labelRefreshPort.Text = "串口";
            this.labelRefreshPort.Click += new System.EventHandler(this.labelRefreshPort_Click);
            // 
            // comboBoxCurPort
            // 
            this.comboBoxCurPort.FormattingEnabled = true;
            this.comboBoxCurPort.Location = new System.Drawing.Point(63, 8);
            this.comboBoxCurPort.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.comboBoxCurPort.Name = "comboBoxCurPort";
            this.comboBoxCurPort.Size = new System.Drawing.Size(160, 24);
            this.comboBoxCurPort.TabIndex = 25;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.richTextBox1.BackColor = System.Drawing.SystemColors.InfoText;
            this.richTextBox1.Font = new System.Drawing.Font("宋体", 9F);
            this.richTextBox1.ForeColor = System.Drawing.SystemColors.Info;
            this.richTextBox1.Location = new System.Drawing.Point(17, 116);
            this.richTextBox1.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(805, 465);
            this.richTextBox1.TabIndex = 24;
            this.richTextBox1.Text = "";
            // 
            // textBoxSendCmd
            // 
            this.textBoxSendCmd.Location = new System.Drawing.Point(17, 55);
            this.textBoxSendCmd.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxSendCmd.Name = "textBoxSendCmd";
            this.textBoxSendCmd.Size = new System.Drawing.Size(685, 26);
            this.textBoxSendCmd.TabIndex = 28;
            // 
            // BtnSendCMD
            // 
            this.BtnSendCMD.Location = new System.Drawing.Point(713, 51);
            this.BtnSendCMD.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.BtnSendCMD.Name = "BtnSendCMD";
            this.BtnSendCMD.Size = new System.Drawing.Size(100, 31);
            this.BtnSendCMD.TabIndex = 29;
            this.BtnSendCMD.Text = "发送";
            this.BtnSendCMD.UseVisualStyleBackColor = true;
            this.BtnSendCMD.Click += new System.EventHandler(this.BtnSendCMD_Click);
            // 
            // XDC01DebugForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 600);
            this.Controls.Add(this.BtnSendCMD);
            this.Controls.Add(this.textBoxSendCmd);
            this.Controls.Add(this.BtnOpenPort);
            this.Controls.Add(this.labelRefreshPort);
            this.Controls.Add(this.comboBoxCurPort);
            this.Controls.Add(this.richTextBox1);
            this.Font = new System.Drawing.Font("宋体", 12F);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "XDC01DebugForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "XDC01调试";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnOpenPort;
        private System.Windows.Forms.Label labelRefreshPort;
        private System.Windows.Forms.ComboBox comboBoxCurPort;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.IO.Ports.SerialPort serialPortXdc01;
        private System.Windows.Forms.TextBox textBoxSendCmd;
        private System.Windows.Forms.Button BtnSendCMD;
    }
}

