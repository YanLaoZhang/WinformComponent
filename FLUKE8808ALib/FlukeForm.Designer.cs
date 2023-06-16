namespace FLUKE8808ALib
{
    partial class FlukeForm
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
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.buttonClear = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.richTextBoxMessage = new System.Windows.Forms.RichTextBox();
            this.textBoxFormat = new System.Windows.Forms.TextBox();
            this.buttonReadFormat = new System.Windows.Forms.Button();
            this.textBoxRate = new System.Windows.Forms.TextBox();
            this.buttonReadRate = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.labelUnit = new System.Windows.Forms.Label();
            this.textBoxRange = new System.Windows.Forms.TextBox();
            this.buttonReadRange = new System.Windows.Forms.Button();
            this.textBoxRead = new System.Windows.Forms.TextBox();
            this.buttonRead = new System.Windows.Forms.Button();
            this.comboBoxFormat = new System.Windows.Forms.ComboBox();
            this.comboBoxRate = new System.Windows.Forms.ComboBox();
            this.comboBoxRange = new System.Windows.Forms.ComboBox();
            this.buttonFormat = new System.Windows.Forms.Button();
            this.buttonRate = new System.Windows.Forms.Button();
            this.buttonSetTrigger = new System.Windows.Forms.Button();
            this.buttonRange = new System.Windows.Forms.Button();
            this.buttonSetModel = new System.Windows.Forms.Button();
            this.buttonLock = new System.Windows.Forms.Button();
            this.BtnOpenPort = new System.Windows.Forms.Button();
            this.labelRefreshPort = new System.Windows.Forms.Label();
            this.comboBoxCurPort = new System.Windows.Forms.ComboBox();
            this.serialPortFluke = new System.IO.Ports.SerialPort(this.components);
            this.richTextBoxCurrentReceive = new System.Windows.Forms.RichTextBox();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.buttonClear);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.richTextBoxMessage);
            this.groupBox3.Controls.Add(this.textBoxFormat);
            this.groupBox3.Controls.Add(this.buttonReadFormat);
            this.groupBox3.Controls.Add(this.textBoxRate);
            this.groupBox3.Controls.Add(this.buttonReadRate);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.labelUnit);
            this.groupBox3.Controls.Add(this.textBoxRange);
            this.groupBox3.Controls.Add(this.buttonReadRange);
            this.groupBox3.Controls.Add(this.textBoxRead);
            this.groupBox3.Controls.Add(this.buttonRead);
            this.groupBox3.Controls.Add(this.comboBoxFormat);
            this.groupBox3.Controls.Add(this.comboBoxRate);
            this.groupBox3.Controls.Add(this.comboBoxRange);
            this.groupBox3.Controls.Add(this.buttonFormat);
            this.groupBox3.Controls.Add(this.buttonRate);
            this.groupBox3.Controls.Add(this.buttonSetTrigger);
            this.groupBox3.Controls.Add(this.buttonRange);
            this.groupBox3.Controls.Add(this.buttonSetModel);
            this.groupBox3.Controls.Add(this.buttonLock);
            this.groupBox3.Location = new System.Drawing.Point(12, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(1100, 264);
            this.groupBox3.TabIndex = 30;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "FLUKE 8808A调试";
            // 
            // buttonClear
            // 
            this.buttonClear.Location = new System.Drawing.Point(892, 21);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(75, 23);
            this.buttonClear.TabIndex = 23;
            this.buttonClear.Text = "清空";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("宋体", 12F);
            this.label12.Location = new System.Drawing.Point(490, 226);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(87, 16);
            this.label12.TabIndex = 22;
            this.label12.Text = "执行情况：";
            // 
            // richTextBoxMessage
            // 
            this.richTextBoxMessage.BackColor = System.Drawing.Color.Black;
            this.richTextBoxMessage.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBoxMessage.ForeColor = System.Drawing.SystemColors.Window;
            this.richTextBoxMessage.Location = new System.Drawing.Point(583, 223);
            this.richTextBoxMessage.Name = "richTextBoxMessage";
            this.richTextBoxMessage.ReadOnly = true;
            this.richTextBoxMessage.Size = new System.Drawing.Size(403, 35);
            this.richTextBoxMessage.TabIndex = 21;
            this.richTextBoxMessage.Text = "";
            // 
            // textBoxFormat
            // 
            this.textBoxFormat.Font = new System.Drawing.Font("宋体", 14F);
            this.textBoxFormat.Location = new System.Drawing.Point(583, 157);
            this.textBoxFormat.Name = "textBoxFormat";
            this.textBoxFormat.Size = new System.Drawing.Size(65, 29);
            this.textBoxFormat.TabIndex = 20;
            // 
            // buttonReadFormat
            // 
            this.buttonReadFormat.Font = new System.Drawing.Font("宋体", 10F);
            this.buttonReadFormat.Location = new System.Drawing.Point(446, 155);
            this.buttonReadFormat.Name = "buttonReadFormat";
            this.buttonReadFormat.Size = new System.Drawing.Size(131, 35);
            this.buttonReadFormat.TabIndex = 19;
            this.buttonReadFormat.Text = "读取输出格式";
            this.buttonReadFormat.UseVisualStyleBackColor = true;
            this.buttonReadFormat.Click += new System.EventHandler(this.buttonReadFormat_Click);
            // 
            // textBoxRate
            // 
            this.textBoxRate.Font = new System.Drawing.Font("宋体", 14F);
            this.textBoxRate.Location = new System.Drawing.Point(701, 76);
            this.textBoxRate.Name = "textBoxRate";
            this.textBoxRate.Size = new System.Drawing.Size(69, 29);
            this.textBoxRate.TabIndex = 18;
            // 
            // buttonReadRate
            // 
            this.buttonReadRate.Font = new System.Drawing.Font("宋体", 10F);
            this.buttonReadRate.Location = new System.Drawing.Point(583, 73);
            this.buttonReadRate.Name = "buttonReadRate";
            this.buttonReadRate.Size = new System.Drawing.Size(112, 35);
            this.buttonReadRate.TabIndex = 17;
            this.buttonReadRate.Text = "读取当前测速";
            this.buttonReadRate.UseVisualStyleBackColor = true;
            this.buttonReadRate.Click += new System.EventHandler(this.buttonReadRate_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(176, 85);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(401, 12);
            this.label10.TabIndex = 16;
            this.label10.Text = "S-慢速（2.5 读数/秒）；M-中速（20 读数/秒）；F-快速（100 读数/秒）";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(174, 167);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(257, 12);
            this.label9.TabIndex = 15;
            this.label9.Text = "1-输出值没有测量单位；2-可输出包括测量单位";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(174, 126);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(647, 12);
            this.label5.TabIndex = 14;
            this.label5.Text = "1：200uA；2：2000uA；3：20mA；4：200mA；5：2A；6：10A(编号1-4：测试线需连mA端子 编号5-6：测试线需连10A端子)";
            // 
            // labelUnit
            // 
            this.labelUnit.AutoSize = true;
            this.labelUnit.Location = new System.Drawing.Point(298, 230);
            this.labelUnit.Name = "labelUnit";
            this.labelUnit.Size = new System.Drawing.Size(29, 12);
            this.labelUnit.TabIndex = 13;
            this.labelUnit.Text = "单位";
            // 
            // textBoxRange
            // 
            this.textBoxRange.Font = new System.Drawing.Font("宋体", 14F);
            this.textBoxRange.Location = new System.Drawing.Point(934, 119);
            this.textBoxRange.Name = "textBoxRange";
            this.textBoxRange.Size = new System.Drawing.Size(62, 29);
            this.textBoxRange.TabIndex = 12;
            // 
            // buttonReadRange
            // 
            this.buttonReadRange.Font = new System.Drawing.Font("宋体", 10F);
            this.buttonReadRange.Location = new System.Drawing.Point(827, 117);
            this.buttonReadRange.Name = "buttonReadRange";
            this.buttonReadRange.Size = new System.Drawing.Size(101, 35);
            this.buttonReadRange.TabIndex = 11;
            this.buttonReadRange.Text = "读取当前量程";
            this.buttonReadRange.UseVisualStyleBackColor = true;
            this.buttonReadRange.Click += new System.EventHandler(this.buttonReadRange_Click);
            // 
            // textBoxRead
            // 
            this.textBoxRead.Font = new System.Drawing.Font("宋体", 14F);
            this.textBoxRead.Location = new System.Drawing.Point(157, 218);
            this.textBoxRead.Name = "textBoxRead";
            this.textBoxRead.Size = new System.Drawing.Size(135, 29);
            this.textBoxRead.TabIndex = 10;
            // 
            // buttonRead
            // 
            this.buttonRead.Font = new System.Drawing.Font("宋体", 10F);
            this.buttonRead.Location = new System.Drawing.Point(6, 215);
            this.buttonRead.Name = "buttonRead";
            this.buttonRead.Size = new System.Drawing.Size(145, 35);
            this.buttonRead.TabIndex = 9;
            this.buttonRead.Text = "读取主屏电流值";
            this.buttonRead.UseVisualStyleBackColor = true;
            this.buttonRead.Click += new System.EventHandler(this.buttonRead_Click);
            // 
            // comboBoxFormat
            // 
            this.comboBoxFormat.Font = new System.Drawing.Font("宋体", 14F);
            this.comboBoxFormat.FormattingEnabled = true;
            this.comboBoxFormat.Items.AddRange(new object[] {
            "1",
            "2"});
            this.comboBoxFormat.Location = new System.Drawing.Point(115, 158);
            this.comboBoxFormat.Name = "comboBoxFormat";
            this.comboBoxFormat.Size = new System.Drawing.Size(53, 27);
            this.comboBoxFormat.TabIndex = 8;
            // 
            // comboBoxRate
            // 
            this.comboBoxRate.Font = new System.Drawing.Font("宋体", 14F);
            this.comboBoxRate.FormattingEnabled = true;
            this.comboBoxRate.Items.AddRange(new object[] {
            "S",
            "M",
            "F"});
            this.comboBoxRate.Location = new System.Drawing.Point(115, 76);
            this.comboBoxRate.Name = "comboBoxRate";
            this.comboBoxRate.Size = new System.Drawing.Size(53, 27);
            this.comboBoxRate.TabIndex = 7;
            // 
            // comboBoxRange
            // 
            this.comboBoxRange.Font = new System.Drawing.Font("宋体", 14F);
            this.comboBoxRange.FormattingEnabled = true;
            this.comboBoxRange.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6"});
            this.comboBoxRange.Location = new System.Drawing.Point(115, 117);
            this.comboBoxRange.Name = "comboBoxRange";
            this.comboBoxRange.Size = new System.Drawing.Size(53, 27);
            this.comboBoxRange.TabIndex = 6;
            // 
            // buttonFormat
            // 
            this.buttonFormat.Font = new System.Drawing.Font("宋体", 10F);
            this.buttonFormat.Location = new System.Drawing.Point(6, 155);
            this.buttonFormat.Name = "buttonFormat";
            this.buttonFormat.Size = new System.Drawing.Size(103, 35);
            this.buttonFormat.TabIndex = 5;
            this.buttonFormat.Text = "设置输出格式";
            this.buttonFormat.UseVisualStyleBackColor = true;
            this.buttonFormat.Click += new System.EventHandler(this.buttonFormat_Click);
            // 
            // buttonRate
            // 
            this.buttonRate.Font = new System.Drawing.Font("宋体", 10F);
            this.buttonRate.Location = new System.Drawing.Point(6, 73);
            this.buttonRate.Name = "buttonRate";
            this.buttonRate.Size = new System.Drawing.Size(103, 35);
            this.buttonRate.TabIndex = 4;
            this.buttonRate.Text = "设置测量速度";
            this.buttonRate.UseVisualStyleBackColor = true;
            this.buttonRate.Click += new System.EventHandler(this.buttonRate_Click);
            // 
            // buttonSetTrigger
            // 
            this.buttonSetTrigger.Font = new System.Drawing.Font("宋体", 10F);
            this.buttonSetTrigger.Location = new System.Drawing.Point(361, 32);
            this.buttonSetTrigger.Name = "buttonSetTrigger";
            this.buttonSetTrigger.Size = new System.Drawing.Size(145, 35);
            this.buttonSetTrigger.TabIndex = 3;
            this.buttonSetTrigger.Text = "设置万用表内部触发";
            this.buttonSetTrigger.UseVisualStyleBackColor = true;
            this.buttonSetTrigger.Click += new System.EventHandler(this.buttonSetTrigger_Click);
            // 
            // buttonRange
            // 
            this.buttonRange.Font = new System.Drawing.Font("宋体", 10F);
            this.buttonRange.Location = new System.Drawing.Point(6, 114);
            this.buttonRange.Name = "buttonRange";
            this.buttonRange.Size = new System.Drawing.Size(103, 35);
            this.buttonRange.TabIndex = 2;
            this.buttonRange.Text = "设置测试量程";
            this.buttonRange.UseVisualStyleBackColor = true;
            this.buttonRange.Click += new System.EventHandler(this.buttonRange_Click);
            // 
            // buttonSetModel
            // 
            this.buttonSetModel.Font = new System.Drawing.Font("宋体", 10F);
            this.buttonSetModel.Location = new System.Drawing.Point(158, 32);
            this.buttonSetModel.Name = "buttonSetModel";
            this.buttonSetModel.Size = new System.Drawing.Size(174, 35);
            this.buttonSetModel.TabIndex = 1;
            this.buttonSetModel.Text = "设置万用表直流电流测试";
            this.buttonSetModel.UseVisualStyleBackColor = true;
            this.buttonSetModel.Click += new System.EventHandler(this.buttonSetModel_Click);
            // 
            // buttonLock
            // 
            this.buttonLock.Font = new System.Drawing.Font("宋体", 10F);
            this.buttonLock.Location = new System.Drawing.Point(6, 32);
            this.buttonLock.Name = "buttonLock";
            this.buttonLock.Size = new System.Drawing.Size(131, 35);
            this.buttonLock.TabIndex = 0;
            this.buttonLock.Text = "锁定万用表面板";
            this.buttonLock.UseVisualStyleBackColor = true;
            this.buttonLock.Click += new System.EventHandler(this.buttonLock_Click);
            // 
            // BtnOpenPort
            // 
            this.BtnOpenPort.Location = new System.Drawing.Point(244, 280);
            this.BtnOpenPort.Margin = new System.Windows.Forms.Padding(4);
            this.BtnOpenPort.Name = "BtnOpenPort";
            this.BtnOpenPort.Size = new System.Drawing.Size(135, 32);
            this.BtnOpenPort.TabIndex = 35;
            this.BtnOpenPort.Text = "打开串口";
            this.BtnOpenPort.UseVisualStyleBackColor = true;
            this.BtnOpenPort.Click += new System.EventHandler(this.BtnOpenPort_Click);
            // 
            // labelRefreshPort
            // 
            this.labelRefreshPort.AutoSize = true;
            this.labelRefreshPort.Location = new System.Drawing.Point(39, 290);
            this.labelRefreshPort.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelRefreshPort.Name = "labelRefreshPort";
            this.labelRefreshPort.Size = new System.Drawing.Size(29, 12);
            this.labelRefreshPort.TabIndex = 34;
            this.labelRefreshPort.Text = "串口";
            this.labelRefreshPort.Click += new System.EventHandler(this.labelRefreshPort_Click);
            // 
            // comboBoxCurPort
            // 
            this.comboBoxCurPort.FormattingEnabled = true;
            this.comboBoxCurPort.Location = new System.Drawing.Point(76, 285);
            this.comboBoxCurPort.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxCurPort.Name = "comboBoxCurPort";
            this.comboBoxCurPort.Size = new System.Drawing.Size(160, 20);
            this.comboBoxCurPort.TabIndex = 33;
            // 
            // richTextBoxCurrentReceive
            // 
            this.richTextBoxCurrentReceive.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBoxCurrentReceive.BackColor = System.Drawing.SystemColors.InfoText;
            this.richTextBoxCurrentReceive.ForeColor = System.Drawing.SystemColors.Info;
            this.richTextBoxCurrentReceive.Location = new System.Drawing.Point(13, 321);
            this.richTextBoxCurrentReceive.Name = "richTextBoxCurrentReceive";
            this.richTextBoxCurrentReceive.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.richTextBoxCurrentReceive.Size = new System.Drawing.Size(1099, 362);
            this.richTextBoxCurrentReceive.TabIndex = 36;
            this.richTextBoxCurrentReceive.Text = "";
            // 
            // FlukeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1124, 695);
            this.Controls.Add(this.richTextBoxCurrentReceive);
            this.Controls.Add(this.BtnOpenPort);
            this.Controls.Add(this.labelRefreshPort);
            this.Controls.Add(this.comboBoxCurPort);
            this.Controls.Add(this.groupBox3);
            this.Name = "FlukeForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.FlukeForm_Load);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button buttonClear;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.RichTextBox richTextBoxMessage;
        private System.Windows.Forms.TextBox textBoxFormat;
        private System.Windows.Forms.Button buttonReadFormat;
        private System.Windows.Forms.TextBox textBoxRate;
        private System.Windows.Forms.Button buttonReadRate;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label labelUnit;
        private System.Windows.Forms.TextBox textBoxRange;
        private System.Windows.Forms.Button buttonReadRange;
        private System.Windows.Forms.TextBox textBoxRead;
        private System.Windows.Forms.Button buttonRead;
        private System.Windows.Forms.ComboBox comboBoxFormat;
        private System.Windows.Forms.ComboBox comboBoxRate;
        private System.Windows.Forms.ComboBox comboBoxRange;
        private System.Windows.Forms.Button buttonFormat;
        private System.Windows.Forms.Button buttonRate;
        private System.Windows.Forms.Button buttonSetTrigger;
        private System.Windows.Forms.Button buttonRange;
        private System.Windows.Forms.Button buttonSetModel;
        private System.Windows.Forms.Button buttonLock;
        private System.Windows.Forms.Button BtnOpenPort;
        private System.Windows.Forms.Label labelRefreshPort;
        private System.Windows.Forms.ComboBox comboBoxCurPort;
        private System.IO.Ports.SerialPort serialPortFluke;
        private System.Windows.Forms.RichTextBox richTextBoxCurrentReceive;
    }
}

