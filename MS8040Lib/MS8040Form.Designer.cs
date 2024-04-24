namespace MS8040Lib
{
    partial class MS8040Form
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
            this.serialPortMS8040 = new System.IO.Ports.SerialPort(this.components);
            this.labelUnit = new System.Windows.Forms.Label();
            this.textBoxRead = new System.Windows.Forms.TextBox();
            this.buttonRead = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.richTextBoxMessage = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // BtnOpenPort
            // 
            this.BtnOpenPort.Location = new System.Drawing.Point(258, 22);
            this.BtnOpenPort.Margin = new System.Windows.Forms.Padding(4);
            this.BtnOpenPort.Name = "BtnOpenPort";
            this.BtnOpenPort.Size = new System.Drawing.Size(135, 32);
            this.BtnOpenPort.TabIndex = 38;
            this.BtnOpenPort.Text = "打开串口";
            this.BtnOpenPort.UseVisualStyleBackColor = true;
            this.BtnOpenPort.Click += new System.EventHandler(this.BtnOpenPort_Click);
            // 
            // labelRefreshPort
            // 
            this.labelRefreshPort.AutoSize = true;
            this.labelRefreshPort.Location = new System.Drawing.Point(47, 32);
            this.labelRefreshPort.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelRefreshPort.Name = "labelRefreshPort";
            this.labelRefreshPort.Size = new System.Drawing.Size(29, 12);
            this.labelRefreshPort.TabIndex = 37;
            this.labelRefreshPort.Text = "串口";
            // 
            // comboBoxCurPort
            // 
            this.comboBoxCurPort.Font = new System.Drawing.Font("宋体", 14F);
            this.comboBoxCurPort.FormattingEnabled = true;
            this.comboBoxCurPort.Location = new System.Drawing.Point(84, 23);
            this.comboBoxCurPort.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxCurPort.Name = "comboBoxCurPort";
            this.comboBoxCurPort.Size = new System.Drawing.Size(160, 27);
            this.comboBoxCurPort.TabIndex = 36;
            this.comboBoxCurPort.DropDown += new System.EventHandler(this.comboBoxCurPort_DropDown);
            // 
            // labelUnit
            // 
            this.labelUnit.AutoSize = true;
            this.labelUnit.Location = new System.Drawing.Point(376, 90);
            this.labelUnit.Name = "labelUnit";
            this.labelUnit.Size = new System.Drawing.Size(17, 12);
            this.labelUnit.TabIndex = 41;
            this.labelUnit.Text = "mA";
            // 
            // textBoxRead
            // 
            this.textBoxRead.Font = new System.Drawing.Font("宋体", 14F);
            this.textBoxRead.Location = new System.Drawing.Point(235, 78);
            this.textBoxRead.Name = "textBoxRead";
            this.textBoxRead.Size = new System.Drawing.Size(135, 29);
            this.textBoxRead.TabIndex = 40;
            // 
            // buttonRead
            // 
            this.buttonRead.Font = new System.Drawing.Font("宋体", 10F);
            this.buttonRead.Location = new System.Drawing.Point(84, 75);
            this.buttonRead.Name = "buttonRead";
            this.buttonRead.Size = new System.Drawing.Size(145, 35);
            this.buttonRead.TabIndex = 39;
            this.buttonRead.Text = "读取电流值";
            this.buttonRead.UseVisualStyleBackColor = true;
            this.buttonRead.Click += new System.EventHandler(this.buttonRead_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("宋体", 12F);
            this.label12.Location = new System.Drawing.Point(81, 146);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(87, 16);
            this.label12.TabIndex = 43;
            this.label12.Text = "执行情况：";
            // 
            // richTextBoxMessage
            // 
            this.richTextBoxMessage.BackColor = System.Drawing.Color.Black;
            this.richTextBoxMessage.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBoxMessage.ForeColor = System.Drawing.SystemColors.Window;
            this.richTextBoxMessage.Location = new System.Drawing.Point(174, 143);
            this.richTextBoxMessage.Name = "richTextBoxMessage";
            this.richTextBoxMessage.ReadOnly = true;
            this.richTextBoxMessage.Size = new System.Drawing.Size(403, 35);
            this.richTextBoxMessage.TabIndex = 42;
            this.richTextBoxMessage.Text = "";
            // 
            // MS8040Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 214);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.richTextBoxMessage);
            this.Controls.Add(this.labelUnit);
            this.Controls.Add(this.textBoxRead);
            this.Controls.Add(this.buttonRead);
            this.Controls.Add(this.BtnOpenPort);
            this.Controls.Add(this.labelRefreshPort);
            this.Controls.Add(this.comboBoxCurPort);
            this.Name = "MS8040Form";
            this.Text = "MS8040电流表";
            this.Load += new System.EventHandler(this.MS8040Form_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnOpenPort;
        private System.Windows.Forms.Label labelRefreshPort;
        private System.Windows.Forms.ComboBox comboBoxCurPort;
        private System.IO.Ports.SerialPort serialPortMS8040;
        private System.Windows.Forms.Label labelUnit;
        private System.Windows.Forms.TextBox textBoxRead;
        private System.Windows.Forms.Button buttonRead;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.RichTextBox richTextBoxMessage;
    }
}

