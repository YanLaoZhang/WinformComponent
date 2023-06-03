namespace XDC01_Test_Tool
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageSwitch = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.RichTBRunningLog = new System.Windows.Forms.RichTextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelTestTime = new System.Windows.Forms.Label();
            this.RichTBScanRN = new System.Windows.Forms.RichTextBox();
            this.comboBoxCurPort = new System.Windows.Forms.ComboBox();
            this.labelRefreshPort = new System.Windows.Forms.Label();
            this.panelResult = new System.Windows.Forms.Panel();
            this.labelResult = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.BtnStart = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.RichTBSerial = new System.Windows.Forms.RichTextBox();
            this.tabPageSetting = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.textBoxFirmware = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.BtnSave = new System.Windows.Forms.Button();
            this.numericUpDownReset = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.numericUpDownPowerON = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.timerTest = new System.Windows.Forms.Timer(this.components);
            this.tabControl1.SuspendLayout();
            this.tabPageSwitch.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel1.SuspendLayout();
            this.panelResult.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabPageSetting.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownReset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPowerON)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPageSwitch);
            this.tabControl1.Controls.Add(this.tabPageSetting);
            this.tabControl1.Font = new System.Drawing.Font("宋体", 14F);
            this.tabControl1.Location = new System.Drawing.Point(9, 9);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1246, 647);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPageSwitch
            // 
            this.tabPageSwitch.Controls.Add(this.groupBox1);
            this.tabPageSwitch.Location = new System.Drawing.Point(4, 29);
            this.tabPageSwitch.Margin = new System.Windows.Forms.Padding(5);
            this.tabPageSwitch.Name = "tabPageSwitch";
            this.tabPageSwitch.Padding = new System.Windows.Forms.Padding(5);
            this.tabPageSwitch.Size = new System.Drawing.Size(1238, 614);
            this.tabPageSwitch.TabIndex = 0;
            this.tabPageSwitch.Text = "出货模式转产测模式";
            this.tabPageSwitch.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.RichTBRunningLog);
            this.groupBox1.Controls.Add(this.dataGridView1);
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(5, 5);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(5);
            this.groupBox1.Size = new System.Drawing.Size(1228, 604);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // RichTBRunningLog
            // 
            this.RichTBRunningLog.Location = new System.Drawing.Point(565, 99);
            this.RichTBRunningLog.Name = "RichTBRunningLog";
            this.RichTBRunningLog.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.RichTBRunningLog.Size = new System.Drawing.Size(660, 200);
            this.RichTBRunningLog.TabIndex = 4;
            this.RichTBRunningLog.Text = "";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4});
            this.dataGridView1.Location = new System.Drawing.Point(10, 99);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 33;
            this.dataGridView1.Size = new System.Drawing.Size(549, 200);
            this.dataGridView1.TabIndex = 3;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "序号";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 70;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "测项";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 200;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "内容";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Width = 200;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "状态";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.Width = 70;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.labelTestTime);
            this.panel1.Controls.Add(this.RichTBScanRN);
            this.panel1.Controls.Add(this.comboBoxCurPort);
            this.panel1.Controls.Add(this.labelRefreshPort);
            this.panel1.Controls.Add(this.panelResult);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.BtnStart);
            this.panel1.Location = new System.Drawing.Point(8, 30);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1212, 62);
            this.panel1.TabIndex = 2;
            // 
            // labelTestTime
            // 
            this.labelTestTime.AutoSize = true;
            this.labelTestTime.Location = new System.Drawing.Point(923, 23);
            this.labelTestTime.Name = "labelTestTime";
            this.labelTestTime.Size = new System.Drawing.Size(29, 19);
            this.labelTestTime.TabIndex = 8;
            this.labelTestTime.Text = "0s";
            // 
            // RichTBScanRN
            // 
            this.RichTBScanRN.Location = new System.Drawing.Point(290, 8);
            this.RichTBScanRN.Name = "RichTBScanRN";
            this.RichTBScanRN.Size = new System.Drawing.Size(493, 44);
            this.RichTBScanRN.TabIndex = 7;
            this.RichTBScanRN.Text = "";
            this.RichTBScanRN.Visible = false;
            this.RichTBScanRN.TextChanged += new System.EventHandler(this.RichTBScanRN_TextChanged);
            // 
            // comboBoxCurPort
            // 
            this.comboBoxCurPort.FormattingEnabled = true;
            this.comboBoxCurPort.Location = new System.Drawing.Point(94, 19);
            this.comboBoxCurPort.Name = "comboBoxCurPort";
            this.comboBoxCurPort.Size = new System.Drawing.Size(96, 27);
            this.comboBoxCurPort.TabIndex = 6;
            this.comboBoxCurPort.SelectedValueChanged += new System.EventHandler(this.comboBoxCurPort_SelectedValueChanged);
            // 
            // labelRefreshPort
            // 
            this.labelRefreshPort.AutoSize = true;
            this.labelRefreshPort.Location = new System.Drawing.Point(3, 23);
            this.labelRefreshPort.Name = "labelRefreshPort";
            this.labelRefreshPort.Size = new System.Drawing.Size(85, 19);
            this.labelRefreshPort.TabIndex = 5;
            this.labelRefreshPort.Text = "串口号：";
            this.labelRefreshPort.Click += new System.EventHandler(this.labelRefreshPort_Click);
            // 
            // panelResult
            // 
            this.panelResult.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.panelResult.Controls.Add(this.labelResult);
            this.panelResult.Location = new System.Drawing.Point(1074, 4);
            this.panelResult.Name = "panelResult";
            this.panelResult.Size = new System.Drawing.Size(135, 55);
            this.panelResult.TabIndex = 4;
            // 
            // labelResult
            // 
            this.labelResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelResult.AutoSize = true;
            this.labelResult.Font = new System.Drawing.Font("宋体", 24.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelResult.Location = new System.Drawing.Point(30, 11);
            this.labelResult.Name = "labelResult";
            this.labelResult.Size = new System.Drawing.Size(0, 33);
            this.labelResult.TabIndex = 0;
            this.labelResult.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1011, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 19);
            this.label2.TabIndex = 3;
            this.label2.Text = "结果:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(216, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 19);
            this.label1.TabIndex = 2;
            this.label1.Text = "扫描RN";
            this.label1.Visible = false;
            // 
            // BtnStart
            // 
            this.BtnStart.BackColor = System.Drawing.Color.SeaGreen;
            this.BtnStart.Font = new System.Drawing.Font("宋体", 16F);
            this.BtnStart.Location = new System.Drawing.Point(789, 4);
            this.BtnStart.Name = "BtnStart";
            this.BtnStart.Size = new System.Drawing.Size(127, 55);
            this.BtnStart.TabIndex = 0;
            this.BtnStart.Text = "开始";
            this.BtnStart.UseVisualStyleBackColor = false;
            this.BtnStart.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.RichTBSerial);
            this.groupBox2.Font = new System.Drawing.Font("宋体", 14F);
            this.groupBox2.Location = new System.Drawing.Point(10, 307);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(5);
            this.groupBox2.Size = new System.Drawing.Size(1208, 287);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "XDC01串口日志";
            // 
            // RichTBSerial
            // 
            this.RichTBSerial.BackColor = System.Drawing.SystemColors.InfoText;
            this.RichTBSerial.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RichTBSerial.Font = new System.Drawing.Font("宋体", 12F);
            this.RichTBSerial.ForeColor = System.Drawing.SystemColors.Info;
            this.RichTBSerial.Location = new System.Drawing.Point(5, 27);
            this.RichTBSerial.Name = "RichTBSerial";
            this.RichTBSerial.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.RichTBSerial.Size = new System.Drawing.Size(1198, 255);
            this.RichTBSerial.TabIndex = 0;
            this.RichTBSerial.Text = "";
            // 
            // tabPageSetting
            // 
            this.tabPageSetting.Controls.Add(this.groupBox3);
            this.tabPageSetting.Location = new System.Drawing.Point(4, 29);
            this.tabPageSetting.Name = "tabPageSetting";
            this.tabPageSetting.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSetting.Size = new System.Drawing.Size(1238, 614);
            this.tabPageSetting.TabIndex = 1;
            this.tabPageSetting.Text = "设置";
            this.tabPageSetting.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.textBoxFirmware);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.BtnSave);
            this.groupBox3.Controls.Add(this.numericUpDownReset);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.numericUpDownPowerON);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Location = new System.Drawing.Point(27, 23);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(550, 274);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            // 
            // textBoxFirmware
            // 
            this.textBoxFirmware.Location = new System.Drawing.Point(159, 43);
            this.textBoxFirmware.Name = "textBoxFirmware";
            this.textBoxFirmware.Size = new System.Drawing.Size(121, 29);
            this.textBoxFirmware.TabIndex = 8;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(11, 46);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(104, 19);
            this.label6.TabIndex = 7;
            this.label6.Text = "固件版本：";
            // 
            // BtnSave
            // 
            this.BtnSave.Location = new System.Drawing.Point(15, 206);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(120, 43);
            this.BtnSave.TabIndex = 6;
            this.BtnSave.Text = "保存";
            this.BtnSave.UseVisualStyleBackColor = true;
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // numericUpDownReset
            // 
            this.numericUpDownReset.Location = new System.Drawing.Point(209, 154);
            this.numericUpDownReset.Name = "numericUpDownReset";
            this.numericUpDownReset.Size = new System.Drawing.Size(120, 29);
            this.numericUpDownReset.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(11, 157);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(192, 19);
            this.label5.TabIndex = 4;
            this.label5.Text = "等待RESET重启时间：";
            // 
            // numericUpDownPowerON
            // 
            this.numericUpDownPowerON.Location = new System.Drawing.Point(159, 97);
            this.numericUpDownPowerON.Name = "numericUpDownPowerON";
            this.numericUpDownPowerON.Size = new System.Drawing.Size(120, 29);
            this.numericUpDownPowerON.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 102);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(142, 19);
            this.label4.TabIndex = 2;
            this.label4.Text = "等待开机时间：";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2});
            this.toolStrip1.Location = new System.Drawing.Point(0, 656);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.toolStrip1.Size = new System.Drawing.Size(1264, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(84, 22);
            this.toolStripButton1.Text = "打开调试界面";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(84, 22);
            this.toolStripButton2.Text = "查询测试记录";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // timerTest
            // 
            this.timerTest.Tick += new System.EventHandler(this.timerTest_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("宋体", 14F);
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.Click += new System.EventHandler(this.Form1_Click);
            this.tabControl1.ResumeLayout(false);
            this.tabPageSwitch.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panelResult.ResumeLayout(false);
            this.panelResult.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.tabPageSetting.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownReset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPowerON)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageSwitch;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RichTextBox RichTBSerial;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button BtnStart;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panelResult;
        private System.Windows.Forms.Label labelResult;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.RichTextBox RichTBRunningLog;
        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.Label labelRefreshPort;
        private System.Windows.Forms.ComboBox comboBoxCurPort;
        private System.Windows.Forms.RichTextBox RichTBScanRN;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.Label labelTestTime;
        private System.Windows.Forms.Timer timerTest;
        private System.Windows.Forms.TabPage tabPageSetting;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numericUpDownPowerON;
        private System.Windows.Forms.NumericUpDown numericUpDownReset;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button BtnSave;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxFirmware;
    }
}

