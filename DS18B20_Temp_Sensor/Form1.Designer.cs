namespace DS18B20_Temp_Sensor
{
    partial class Form1
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
            this.BtnClosePort = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.richTextBoxLog = new System.Windows.Forms.RichTextBox();
            this.BtnOpenPort = new System.Windows.Forms.Button();
            this.comboBoxSerialPort = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.richTextBoxTips = new System.Windows.Forms.RichTextBox();
            this.BtnTriggerQandAMode = new System.Windows.Forms.Button();
            this.BtnTriggerActiveMode = new System.Windows.Forms.Button();
            this.BtnActiveRead = new System.Windows.Forms.Button();
            this.BtnQandARead = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // BtnClosePort
            // 
            this.BtnClosePort.Location = new System.Drawing.Point(308, 17);
            this.BtnClosePort.Name = "BtnClosePort";
            this.BtnClosePort.Size = new System.Drawing.Size(75, 23);
            this.BtnClosePort.TabIndex = 12;
            this.BtnClosePort.Text = "关闭串口";
            this.BtnClosePort.UseVisualStyleBackColor = true;
            this.BtnClosePort.Click += new System.EventHandler(this.BtnClosePort_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.richTextBoxLog);
            this.groupBox1.Location = new System.Drawing.Point(13, 53);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(420, 187);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "串口内容：";
            // 
            // richTextBoxLog
            // 
            this.richTextBoxLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.richTextBoxLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxLog.Location = new System.Drawing.Point(3, 17);
            this.richTextBoxLog.Name = "richTextBoxLog";
            this.richTextBoxLog.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.richTextBoxLog.Size = new System.Drawing.Size(414, 167);
            this.richTextBoxLog.TabIndex = 0;
            this.richTextBoxLog.Text = "";
            // 
            // BtnOpenPort
            // 
            this.BtnOpenPort.Location = new System.Drawing.Point(227, 17);
            this.BtnOpenPort.Name = "BtnOpenPort";
            this.BtnOpenPort.Size = new System.Drawing.Size(75, 23);
            this.BtnOpenPort.TabIndex = 10;
            this.BtnOpenPort.Text = "打开串口";
            this.BtnOpenPort.UseVisualStyleBackColor = true;
            this.BtnOpenPort.Click += new System.EventHandler(this.BtnOpenPort_Click);
            // 
            // comboBoxSerialPort
            // 
            this.comboBoxSerialPort.FormattingEnabled = true;
            this.comboBoxSerialPort.Location = new System.Drawing.Point(91, 19);
            this.comboBoxSerialPort.Name = "comboBoxSerialPort";
            this.comboBoxSerialPort.Size = new System.Drawing.Size(121, 20);
            this.comboBoxSerialPort.TabIndex = 9;
            this.comboBoxSerialPort.DropDown += new System.EventHandler(this.comboBoxSerialPort_DropDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(38, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 8;
            this.label1.Text = "串口号:";
            // 
            // richTextBoxTips
            // 
            this.richTextBoxTips.Location = new System.Drawing.Point(13, 262);
            this.richTextBoxTips.Name = "richTextBoxTips";
            this.richTextBoxTips.Size = new System.Drawing.Size(775, 176);
            this.richTextBoxTips.TabIndex = 13;
            this.richTextBoxTips.Text = "";
            // 
            // BtnTriggerQandAMode
            // 
            this.BtnTriggerQandAMode.Location = new System.Drawing.Point(452, 67);
            this.BtnTriggerQandAMode.Name = "BtnTriggerQandAMode";
            this.BtnTriggerQandAMode.Size = new System.Drawing.Size(120, 23);
            this.BtnTriggerQandAMode.TabIndex = 14;
            this.BtnTriggerQandAMode.Text = "切换问答模式";
            this.BtnTriggerQandAMode.UseVisualStyleBackColor = true;
            this.BtnTriggerQandAMode.Click += new System.EventHandler(this.BtnTriggerQandAMode_Click);
            // 
            // BtnTriggerActiveMode
            // 
            this.BtnTriggerActiveMode.Location = new System.Drawing.Point(452, 96);
            this.BtnTriggerActiveMode.Name = "BtnTriggerActiveMode";
            this.BtnTriggerActiveMode.Size = new System.Drawing.Size(120, 23);
            this.BtnTriggerActiveMode.TabIndex = 15;
            this.BtnTriggerActiveMode.Text = "切换主动模式";
            this.BtnTriggerActiveMode.UseVisualStyleBackColor = true;
            this.BtnTriggerActiveMode.Click += new System.EventHandler(this.BtnTriggerActiveMode_Click);
            // 
            // BtnActiveRead
            // 
            this.BtnActiveRead.Location = new System.Drawing.Point(589, 96);
            this.BtnActiveRead.Name = "BtnActiveRead";
            this.BtnActiveRead.Size = new System.Drawing.Size(120, 23);
            this.BtnActiveRead.TabIndex = 16;
            this.BtnActiveRead.Text = "主动模式读取数据";
            this.BtnActiveRead.UseVisualStyleBackColor = true;
            this.BtnActiveRead.Click += new System.EventHandler(this.BtnActiveRead_Click);
            // 
            // BtnQandARead
            // 
            this.BtnQandARead.Location = new System.Drawing.Point(589, 67);
            this.BtnQandARead.Name = "BtnQandARead";
            this.BtnQandARead.Size = new System.Drawing.Size(120, 23);
            this.BtnQandARead.TabIndex = 17;
            this.BtnQandARead.Text = "问答模式读取数据";
            this.BtnQandARead.UseVisualStyleBackColor = true;
            this.BtnQandARead.Click += new System.EventHandler(this.BtnQandARead_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.BtnQandARead);
            this.Controls.Add(this.BtnActiveRead);
            this.Controls.Add(this.BtnTriggerActiveMode);
            this.Controls.Add(this.BtnTriggerQandAMode);
            this.Controls.Add(this.richTextBoxTips);
            this.Controls.Add(this.BtnClosePort);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.BtnOpenPort);
            this.Controls.Add(this.comboBoxSerialPort);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "DS18B20温度传感器";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnClosePort;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RichTextBox richTextBoxLog;
        private System.Windows.Forms.Button BtnOpenPort;
        private System.Windows.Forms.ComboBox comboBoxSerialPort;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox richTextBoxTips;
        private System.Windows.Forms.Button BtnTriggerQandAMode;
        private System.Windows.Forms.Button BtnTriggerActiveMode;
        private System.Windows.Forms.Button BtnActiveRead;
        private System.Windows.Forms.Button BtnQandARead;
    }
}

