namespace XDC03Debug
{
    partial class XDC03DebugForm
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
            this.serialPortXdc03 = new System.IO.Ports.SerialPort(this.components);
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.BtnReadSysInfo = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBoxCpuTemp = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBoxBatteryPer = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxBatteryVol = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxWiFiVer = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxMcuVer = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxHwVer = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxFwVer = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBoxWriteRN = new System.Windows.Forms.TextBox();
            this.BtnWriteRN = new System.Windows.Forms.Button();
            this.BtnReadRN = new System.Windows.Forms.Button();
            this.textBoxReadRN = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.comboBoxWriteTagNum = new System.Windows.Forms.ComboBox();
            this.BtnWriteTagNum = new System.Windows.Forms.Button();
            this.BtnReadTagNum = new System.Windows.Forms.Button();
            this.textBoxReadTagNum = new System.Windows.Forms.TextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.textBoxWriteWiFiPWD = new System.Windows.Forms.TextBox();
            this.BtnWriteWiFi = new System.Windows.Forms.Button();
            this.textBoxWriteWiFiSSID = new System.Windows.Forms.TextBox();
            this.textBoxReadWiFiPWD = new System.Windows.Forms.TextBox();
            this.textBoxReadWiFiSSID = new System.Windows.Forms.TextBox();
            this.BtnReadWiFi = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.textBoxRTSPUrl = new System.Windows.Forms.TextBox();
            this.BtnOpenRTSP = new System.Windows.Forms.Button();
            this.textBoxReadIP = new System.Windows.Forms.TextBox();
            this.BtnReadIP = new System.Windows.Forms.Button();
            this.BtnReadMAC = new System.Windows.Forms.Button();
            this.textBoxReadMAC = new System.Windows.Forms.TextBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.BtnReadLight = new System.Windows.Forms.Button();
            this.textBoxReadLight = new System.Windows.Forms.TextBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.BtnFactoryReset = new System.Windows.Forms.Button();
            this.BtnFactoryMode = new System.Windows.Forms.Button();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.CbxLedColor = new System.Windows.Forms.ComboBox();
            this.BtnSetLEDColor = new System.Windows.Forms.Button();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.BtnClosePIR = new System.Windows.Forms.Button();
            this.BtnOpenPIR = new System.Windows.Forms.Button();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.CbxWavIndex = new System.Windows.Forms.ComboBox();
            this.BtnPlayWav = new System.Windows.Forms.Button();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.textBoxMicResult = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.textBoxMicDelta = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.textBoxMicMaxAbs = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.BtnAutoTestMic = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.textBoxRecordDuration = new System.Windows.Forms.TextBox();
            this.BtnPlayRecord = new System.Windows.Forms.Button();
            this.BtnRecordMic = new System.Windows.Forms.Button();
            this.BtnCloseMic = new System.Windows.Forms.Button();
            this.BtnOpenMic = new System.Windows.Forms.Button();
            this.groupBox12 = new System.Windows.Forms.GroupBox();
            this.comboBoxUnit = new System.Windows.Forms.ComboBox();
            this.label21 = new System.Windows.Forms.Label();
            this.numericUpDownBandWidth = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownDuration = new System.Windows.Forms.NumericUpDown();
            this.comboBoxServerIp = new System.Windows.Forms.ComboBox();
            this.label20 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.textBoxDownLoss = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.textBoxDownRate = new System.Windows.Forms.TextBox();
            this.BtnWifiDownT = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.textBoxUpLoss = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.textBoxUpRate = new System.Windows.Forms.TextBox();
            this.BtnWiFiUpT = new System.Windows.Forms.Button();
            this.BtnCloseIperf3 = new System.Windows.Forms.Button();
            this.BtnOpenIperf3 = new System.Windows.Forms.Button();
            this.groupBox13 = new System.Windows.Forms.GroupBox();
            this.BtnCloseIRLed = new System.Windows.Forms.Button();
            this.BtnOpenIRLed = new System.Windows.Forms.Button();
            this.BtnQuitBW = new System.Windows.Forms.Button();
            this.BtnEnterBW = new System.Windows.Forms.Button();
            this.groupBox14 = new System.Windows.Forms.GroupBox();
            this.CbxPCBARfIndex = new System.Windows.Forms.ComboBox();
            this.BtnRfRx = new System.Windows.Forms.Button();
            this.BtnRfTx = new System.Windows.Forms.Button();
            this.BtnPCBARfTx = new System.Windows.Forms.Button();
            this.groupBox15 = new System.Windows.Forms.GroupBox();
            this.BtnWriteSNUID = new System.Windows.Forms.Button();
            this.textBoxUID = new System.Windows.Forms.TextBox();
            this.BtnReadUID = new System.Windows.Forms.Button();
            this.textBoxSN = new System.Windows.Forms.TextBox();
            this.BtnReadSN = new System.Windows.Forms.Button();
            this.groupBox16 = new System.Windows.Forms.GroupBox();
            this.label22 = new System.Windows.Forms.Label();
            this.BtnStopCharge = new System.Windows.Forms.Button();
            this.BtnStartCharge = new System.Windows.Forms.Button();
            this.BtnSetChargeMode = new System.Windows.Forms.Button();
            this.textBoxChargeLevel = new System.Windows.Forms.TextBox();
            this.groupBox17 = new System.Windows.Forms.GroupBox();
            this.BtnReadRNRTOS = new System.Windows.Forms.Button();
            this.tBReadRNRTOS = new System.Windows.Forms.TextBox();
            this.CbxWriteTagNumRTOS = new System.Windows.Forms.ComboBox();
            this.BtnWriteTagNumRTOS = new System.Windows.Forms.Button();
            this.BtnReadTagNumRTOS = new System.Windows.Forms.Button();
            this.tBReadTagNumRTOS = new System.Windows.Forms.TextBox();
            this.BtnQuitBWRTOS = new System.Windows.Forms.Button();
            this.BtnEnterBWRTOS = new System.Windows.Forms.Button();
            this.BtnOpenCamera = new System.Windows.Forms.Button();
            this.comboBoxCurPort = new System.Windows.Forms.ComboBox();
            this.labelRefreshPort = new System.Windows.Forms.Label();
            this.BtnOpenPort = new System.Windows.Forms.Button();
            this.richTextBoxPCCmd = new System.Windows.Forms.RichTextBox();
            this.textBoxSendCmd = new System.Windows.Forms.TextBox();
            this.BtnSendCMD = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox18 = new System.Windows.Forms.GroupBox();
            this.groupBox19 = new System.Windows.Forms.GroupBox();
            this.groupBox20 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.groupBox10.SuspendLayout();
            this.groupBox11.SuspendLayout();
            this.groupBox12.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBandWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDuration)).BeginInit();
            this.groupBox13.SuspendLayout();
            this.groupBox14.SuspendLayout();
            this.groupBox15.SuspendLayout();
            this.groupBox16.SuspendLayout();
            this.groupBox17.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox18.SuspendLayout();
            this.groupBox19.SuspendLayout();
            this.groupBox20.SuspendLayout();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox1.BackColor = System.Drawing.SystemColors.InfoText;
            this.richTextBox1.Font = new System.Drawing.Font("宋体", 9F);
            this.richTextBox1.ForeColor = System.Drawing.SystemColors.Info;
            this.richTextBox1.Location = new System.Drawing.Point(13, 475);
            this.richTextBox1.Margin = new System.Windows.Forms.Padding(4);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.richTextBox1.Size = new System.Drawing.Size(1238, 193);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            this.richTextBox1.WordWrap = false;
            // 
            // BtnReadSysInfo
            // 
            this.BtnReadSysInfo.Location = new System.Drawing.Point(116, 15);
            this.BtnReadSysInfo.Name = "BtnReadSysInfo";
            this.BtnReadSysInfo.Size = new System.Drawing.Size(144, 34);
            this.BtnReadSysInfo.TabIndex = 5;
            this.BtnReadSysInfo.Text = "读取系统信息";
            this.BtnReadSysInfo.UseVisualStyleBackColor = true;
            this.BtnReadSysInfo.Click += new System.EventHandler(this.BtnReadSysInfo_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBoxCpuTemp);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.textBoxBatteryPer);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.textBoxBatteryVol);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.textBoxWiFiVer);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.textBoxMcuVer);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.textBoxHwVer);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.textBoxFwVer);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.BtnReadSysInfo);
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(268, 313);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "系统信息";
            // 
            // textBoxCpuTemp
            // 
            this.textBoxCpuTemp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxCpuTemp.Location = new System.Drawing.Point(116, 265);
            this.textBoxCpuTemp.Name = "textBoxCpuTemp";
            this.textBoxCpuTemp.Size = new System.Drawing.Size(146, 29);
            this.textBoxCpuTemp.TabIndex = 19;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(14, 268);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(96, 19);
            this.label8.TabIndex = 18;
            this.label8.Text = "CPU温度：";
            // 
            // textBoxBatteryPer
            // 
            this.textBoxBatteryPer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxBatteryPer.Location = new System.Drawing.Point(116, 230);
            this.textBoxBatteryPer.Name = "textBoxBatteryPer";
            this.textBoxBatteryPer.Size = new System.Drawing.Size(146, 29);
            this.textBoxBatteryPer.TabIndex = 17;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 233);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(104, 19);
            this.label7.TabIndex = 16;
            this.label7.Text = "电池电量：";
            // 
            // textBoxBatteryVol
            // 
            this.textBoxBatteryVol.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxBatteryVol.Location = new System.Drawing.Point(116, 195);
            this.textBoxBatteryVol.Name = "textBoxBatteryVol";
            this.textBoxBatteryVol.Size = new System.Drawing.Size(146, 29);
            this.textBoxBatteryVol.TabIndex = 15;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 198);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(104, 19);
            this.label6.TabIndex = 14;
            this.label6.Text = "电池电压：";
            // 
            // textBoxWiFiVer
            // 
            this.textBoxWiFiVer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxWiFiVer.Location = new System.Drawing.Point(116, 160);
            this.textBoxWiFiVer.Name = "textBoxWiFiVer";
            this.textBoxWiFiVer.Size = new System.Drawing.Size(146, 29);
            this.textBoxWiFiVer.TabIndex = 13;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 166);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(106, 19);
            this.label5.TabIndex = 12;
            this.label5.Text = "WiFi版本：";
            // 
            // textBoxMcuVer
            // 
            this.textBoxMcuVer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxMcuVer.Location = new System.Drawing.Point(116, 125);
            this.textBoxMcuVer.Name = "textBoxMcuVer";
            this.textBoxMcuVer.Size = new System.Drawing.Size(146, 29);
            this.textBoxMcuVer.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 129);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 19);
            this.label4.TabIndex = 10;
            this.label4.Text = "MCU版本：";
            // 
            // textBoxHwVer
            // 
            this.textBoxHwVer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxHwVer.Location = new System.Drawing.Point(116, 90);
            this.textBoxHwVer.Name = "textBoxHwVer";
            this.textBoxHwVer.Size = new System.Drawing.Size(146, 29);
            this.textBoxHwVer.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 93);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 19);
            this.label3.TabIndex = 8;
            this.label3.Text = "硬件版本：";
            // 
            // textBoxFwVer
            // 
            this.textBoxFwVer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFwVer.Location = new System.Drawing.Point(116, 55);
            this.textBoxFwVer.Name = "textBoxFwVer";
            this.textBoxFwVer.Size = new System.Drawing.Size(146, 29);
            this.textBoxFwVer.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 19);
            this.label2.TabIndex = 6;
            this.label2.Text = "固件版本：";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBoxWriteRN);
            this.groupBox2.Controls.Add(this.BtnWriteRN);
            this.groupBox2.Controls.Add(this.BtnReadRN);
            this.groupBox2.Controls.Add(this.textBoxReadRN);
            this.groupBox2.Location = new System.Drawing.Point(568, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(259, 112);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "RN";
            // 
            // textBoxWriteRN
            // 
            this.textBoxWriteRN.Location = new System.Drawing.Point(87, 67);
            this.textBoxWriteRN.Name = "textBoxWriteRN";
            this.textBoxWriteRN.Size = new System.Drawing.Size(161, 29);
            this.textBoxWriteRN.TabIndex = 4;
            // 
            // BtnWriteRN
            // 
            this.BtnWriteRN.Location = new System.Drawing.Point(6, 66);
            this.BtnWriteRN.Name = "BtnWriteRN";
            this.BtnWriteRN.Size = new System.Drawing.Size(75, 26);
            this.BtnWriteRN.TabIndex = 3;
            this.BtnWriteRN.Text = "写入RN";
            this.BtnWriteRN.UseVisualStyleBackColor = true;
            this.BtnWriteRN.Click += new System.EventHandler(this.BtnWriteRN_Click);
            // 
            // BtnReadRN
            // 
            this.BtnReadRN.Location = new System.Drawing.Point(6, 25);
            this.BtnReadRN.Name = "BtnReadRN";
            this.BtnReadRN.Size = new System.Drawing.Size(75, 26);
            this.BtnReadRN.TabIndex = 2;
            this.BtnReadRN.Text = "读取RN";
            this.BtnReadRN.UseVisualStyleBackColor = true;
            this.BtnReadRN.Click += new System.EventHandler(this.BtnReadRN_Click);
            // 
            // textBoxReadRN
            // 
            this.textBoxReadRN.Location = new System.Drawing.Point(87, 24);
            this.textBoxReadRN.Name = "textBoxReadRN";
            this.textBoxReadRN.Size = new System.Drawing.Size(161, 29);
            this.textBoxReadRN.TabIndex = 1;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.comboBoxWriteTagNum);
            this.groupBox3.Controls.Add(this.BtnWriteTagNum);
            this.groupBox3.Controls.Add(this.BtnReadTagNum);
            this.groupBox3.Controls.Add(this.textBoxReadTagNum);
            this.groupBox3.Location = new System.Drawing.Point(280, 6);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(282, 112);
            this.groupBox3.TabIndex = 8;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "工序号";
            // 
            // comboBoxWriteTagNum
            // 
            this.comboBoxWriteTagNum.FormattingEnabled = true;
            this.comboBoxWriteTagNum.Items.AddRange(new object[] {
            "T010",
            "T020",
            "T030",
            "T040",
            "T050",
            "T060",
            "T070",
            "T080",
            "T100",
            "T110"});
            this.comboBoxWriteTagNum.Location = new System.Drawing.Point(136, 67);
            this.comboBoxWriteTagNum.Name = "comboBoxWriteTagNum";
            this.comboBoxWriteTagNum.Size = new System.Drawing.Size(130, 27);
            this.comboBoxWriteTagNum.TabIndex = 4;
            // 
            // BtnWriteTagNum
            // 
            this.BtnWriteTagNum.Location = new System.Drawing.Point(6, 66);
            this.BtnWriteTagNum.Name = "BtnWriteTagNum";
            this.BtnWriteTagNum.Size = new System.Drawing.Size(113, 26);
            this.BtnWriteTagNum.TabIndex = 3;
            this.BtnWriteTagNum.Text = "写入工序号";
            this.BtnWriteTagNum.UseVisualStyleBackColor = true;
            this.BtnWriteTagNum.Click += new System.EventHandler(this.BtnWriteTagNum_Click);
            // 
            // BtnReadTagNum
            // 
            this.BtnReadTagNum.Location = new System.Drawing.Point(6, 25);
            this.BtnReadTagNum.Name = "BtnReadTagNum";
            this.BtnReadTagNum.Size = new System.Drawing.Size(113, 26);
            this.BtnReadTagNum.TabIndex = 2;
            this.BtnReadTagNum.Text = "读取工序号";
            this.BtnReadTagNum.UseVisualStyleBackColor = true;
            this.BtnReadTagNum.Click += new System.EventHandler(this.BtnReadTagNum_Click);
            // 
            // textBoxReadTagNum
            // 
            this.textBoxReadTagNum.Location = new System.Drawing.Point(136, 24);
            this.textBoxReadTagNum.Name = "textBoxReadTagNum";
            this.textBoxReadTagNum.Size = new System.Drawing.Size(131, 29);
            this.textBoxReadTagNum.TabIndex = 1;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.textBoxWriteWiFiPWD);
            this.groupBox4.Controls.Add(this.BtnWriteWiFi);
            this.groupBox4.Controls.Add(this.textBoxWriteWiFiSSID);
            this.groupBox4.Controls.Add(this.textBoxReadWiFiPWD);
            this.groupBox4.Controls.Add(this.textBoxReadWiFiSSID);
            this.groupBox4.Controls.Add(this.BtnReadWiFi);
            this.groupBox4.Location = new System.Drawing.Point(6, 6);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(438, 180);
            this.groupBox4.TabIndex = 8;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "WiFi";
            // 
            // textBoxWriteWiFiPWD
            // 
            this.textBoxWriteWiFiPWD.Location = new System.Drawing.Point(139, 127);
            this.textBoxWriteWiFiPWD.Name = "textBoxWriteWiFiPWD";
            this.textBoxWriteWiFiPWD.Size = new System.Drawing.Size(192, 29);
            this.textBoxWriteWiFiPWD.TabIndex = 12;
            // 
            // BtnWriteWiFi
            // 
            this.BtnWriteWiFi.Location = new System.Drawing.Point(8, 107);
            this.BtnWriteWiFi.Name = "BtnWriteWiFi";
            this.BtnWriteWiFi.Size = new System.Drawing.Size(125, 38);
            this.BtnWriteWiFi.TabIndex = 11;
            this.BtnWriteWiFi.Text = "设置WiFi";
            this.BtnWriteWiFi.UseVisualStyleBackColor = true;
            this.BtnWriteWiFi.Click += new System.EventHandler(this.BtnWriteWiFi_Click);
            // 
            // textBoxWriteWiFiSSID
            // 
            this.textBoxWriteWiFiSSID.Location = new System.Drawing.Point(139, 92);
            this.textBoxWriteWiFiSSID.Name = "textBoxWriteWiFiSSID";
            this.textBoxWriteWiFiSSID.Size = new System.Drawing.Size(192, 29);
            this.textBoxWriteWiFiSSID.TabIndex = 10;
            // 
            // textBoxReadWiFiPWD
            // 
            this.textBoxReadWiFiPWD.Location = new System.Drawing.Point(139, 57);
            this.textBoxReadWiFiPWD.Name = "textBoxReadWiFiPWD";
            this.textBoxReadWiFiPWD.Size = new System.Drawing.Size(192, 29);
            this.textBoxReadWiFiPWD.TabIndex = 8;
            // 
            // textBoxReadWiFiSSID
            // 
            this.textBoxReadWiFiSSID.Location = new System.Drawing.Point(139, 22);
            this.textBoxReadWiFiSSID.Name = "textBoxReadWiFiSSID";
            this.textBoxReadWiFiSSID.Size = new System.Drawing.Size(192, 29);
            this.textBoxReadWiFiSSID.TabIndex = 6;
            // 
            // BtnReadWiFi
            // 
            this.BtnReadWiFi.Location = new System.Drawing.Point(8, 35);
            this.BtnReadWiFi.Name = "BtnReadWiFi";
            this.BtnReadWiFi.Size = new System.Drawing.Size(125, 37);
            this.BtnReadWiFi.TabIndex = 5;
            this.BtnReadWiFi.Text = "读取WiFi";
            this.BtnReadWiFi.UseVisualStyleBackColor = true;
            this.BtnReadWiFi.Click += new System.EventHandler(this.BtnReadWiFi_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.textBoxRTSPUrl);
            this.groupBox5.Controls.Add(this.BtnOpenRTSP);
            this.groupBox5.Controls.Add(this.textBoxReadIP);
            this.groupBox5.Controls.Add(this.BtnReadIP);
            this.groupBox5.Controls.Add(this.BtnReadMAC);
            this.groupBox5.Controls.Add(this.textBoxReadMAC);
            this.groupBox5.Location = new System.Drawing.Point(6, 192);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(438, 155);
            this.groupBox5.TabIndex = 9;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "MAC / IP";
            // 
            // textBoxRTSPUrl
            // 
            this.textBoxRTSPUrl.Location = new System.Drawing.Point(117, 120);
            this.textBoxRTSPUrl.Name = "textBoxRTSPUrl";
            this.textBoxRTSPUrl.ReadOnly = true;
            this.textBoxRTSPUrl.Size = new System.Drawing.Size(298, 29);
            this.textBoxRTSPUrl.TabIndex = 10;
            // 
            // BtnOpenRTSP
            // 
            this.BtnOpenRTSP.Location = new System.Drawing.Point(7, 119);
            this.BtnOpenRTSP.Name = "BtnOpenRTSP";
            this.BtnOpenRTSP.Size = new System.Drawing.Size(103, 28);
            this.BtnOpenRTSP.TabIndex = 9;
            this.BtnOpenRTSP.Text = "打开RTSP流";
            this.BtnOpenRTSP.UseVisualStyleBackColor = true;
            this.BtnOpenRTSP.Click += new System.EventHandler(this.BtnOpenRTSP_Click);
            // 
            // textBoxReadIP
            // 
            this.textBoxReadIP.Location = new System.Drawing.Point(117, 71);
            this.textBoxReadIP.Name = "textBoxReadIP";
            this.textBoxReadIP.Size = new System.Drawing.Size(161, 29);
            this.textBoxReadIP.TabIndex = 8;
            // 
            // BtnReadIP
            // 
            this.BtnReadIP.Location = new System.Drawing.Point(8, 71);
            this.BtnReadIP.Name = "BtnReadIP";
            this.BtnReadIP.Size = new System.Drawing.Size(102, 34);
            this.BtnReadIP.TabIndex = 7;
            this.BtnReadIP.Text = "读取IP";
            this.BtnReadIP.UseVisualStyleBackColor = true;
            this.BtnReadIP.Click += new System.EventHandler(this.BtnReadIP_Click);
            // 
            // BtnReadMAC
            // 
            this.BtnReadMAC.Location = new System.Drawing.Point(8, 25);
            this.BtnReadMAC.Name = "BtnReadMAC";
            this.BtnReadMAC.Size = new System.Drawing.Size(102, 38);
            this.BtnReadMAC.TabIndex = 6;
            this.BtnReadMAC.Text = "读取MAC";
            this.BtnReadMAC.UseVisualStyleBackColor = true;
            this.BtnReadMAC.Click += new System.EventHandler(this.BtnReadMAC_Click);
            // 
            // textBoxReadMAC
            // 
            this.textBoxReadMAC.Location = new System.Drawing.Point(117, 25);
            this.textBoxReadMAC.Name = "textBoxReadMAC";
            this.textBoxReadMAC.Size = new System.Drawing.Size(161, 29);
            this.textBoxReadMAC.TabIndex = 5;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.BtnReadLight);
            this.groupBox6.Controls.Add(this.textBoxReadLight);
            this.groupBox6.Location = new System.Drawing.Point(371, 82);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(311, 67);
            this.groupBox6.TabIndex = 10;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "亮度值";
            // 
            // BtnReadLight
            // 
            this.BtnReadLight.Location = new System.Drawing.Point(6, 25);
            this.BtnReadLight.Name = "BtnReadLight";
            this.BtnReadLight.Size = new System.Drawing.Size(101, 29);
            this.BtnReadLight.TabIndex = 8;
            this.BtnReadLight.Text = "读取亮度值";
            this.BtnReadLight.UseVisualStyleBackColor = true;
            this.BtnReadLight.Click += new System.EventHandler(this.BtnReadLight_Click);
            // 
            // textBoxReadLight
            // 
            this.textBoxReadLight.Location = new System.Drawing.Point(113, 25);
            this.textBoxReadLight.Name = "textBoxReadLight";
            this.textBoxReadLight.Size = new System.Drawing.Size(161, 29);
            this.textBoxReadLight.TabIndex = 7;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.BtnFactoryReset);
            this.groupBox7.Controls.Add(this.BtnFactoryMode);
            this.groupBox7.Location = new System.Drawing.Point(833, 6);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(393, 112);
            this.groupBox7.TabIndex = 11;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "产测模式 / 恢复出厂模式";
            // 
            // BtnFactoryReset
            // 
            this.BtnFactoryReset.Location = new System.Drawing.Point(207, 28);
            this.BtnFactoryReset.Name = "BtnFactoryReset";
            this.BtnFactoryReset.Size = new System.Drawing.Size(136, 34);
            this.BtnFactoryReset.TabIndex = 1;
            this.BtnFactoryReset.Text = "恢复出厂模式";
            this.BtnFactoryReset.UseVisualStyleBackColor = true;
            this.BtnFactoryReset.Click += new System.EventHandler(this.BtnFactoryReset_Click);
            // 
            // BtnFactoryMode
            // 
            this.BtnFactoryMode.Location = new System.Drawing.Point(6, 28);
            this.BtnFactoryMode.Name = "BtnFactoryMode";
            this.BtnFactoryMode.Size = new System.Drawing.Size(147, 34);
            this.BtnFactoryMode.TabIndex = 0;
            this.BtnFactoryMode.Text = "进入产测模式";
            this.BtnFactoryMode.UseVisualStyleBackColor = true;
            this.BtnFactoryMode.Click += new System.EventHandler(this.BtnFactoryMode_Click);
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.CbxLedColor);
            this.groupBox8.Controls.Add(this.BtnSetLEDColor);
            this.groupBox8.Location = new System.Drawing.Point(688, 82);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(339, 67);
            this.groupBox8.TabIndex = 12;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "LED颜色";
            // 
            // CbxLedColor
            // 
            this.CbxLedColor.FormattingEnabled = true;
            this.CbxLedColor.Items.AddRange(new object[] {
            "red",
            "green",
            "blue",
            "white",
            "off"});
            this.CbxLedColor.Location = new System.Drawing.Point(143, 25);
            this.CbxLedColor.Name = "CbxLedColor";
            this.CbxLedColor.Size = new System.Drawing.Size(130, 27);
            this.CbxLedColor.TabIndex = 6;
            // 
            // BtnSetLEDColor
            // 
            this.BtnSetLEDColor.Location = new System.Drawing.Point(6, 25);
            this.BtnSetLEDColor.Name = "BtnSetLEDColor";
            this.BtnSetLEDColor.Size = new System.Drawing.Size(117, 29);
            this.BtnSetLEDColor.TabIndex = 5;
            this.BtnSetLEDColor.Text = "切换灯颜色";
            this.BtnSetLEDColor.UseVisualStyleBackColor = true;
            this.BtnSetLEDColor.Click += new System.EventHandler(this.BtnSetLEDColor_Click);
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.BtnClosePIR);
            this.groupBox9.Controls.Add(this.BtnOpenPIR);
            this.groupBox9.Location = new System.Drawing.Point(688, 155);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(339, 175);
            this.groupBox9.TabIndex = 13;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "PIR切换";
            // 
            // BtnClosePIR
            // 
            this.BtnClosePIR.Location = new System.Drawing.Point(18, 94);
            this.BtnClosePIR.Name = "BtnClosePIR";
            this.BtnClosePIR.Size = new System.Drawing.Size(147, 31);
            this.BtnClosePIR.TabIndex = 1;
            this.BtnClosePIR.Text = "关闭PIR功能";
            this.BtnClosePIR.UseVisualStyleBackColor = true;
            this.BtnClosePIR.Click += new System.EventHandler(this.BtnClosePIR_Click);
            // 
            // BtnOpenPIR
            // 
            this.BtnOpenPIR.Location = new System.Drawing.Point(18, 41);
            this.BtnOpenPIR.Name = "BtnOpenPIR";
            this.BtnOpenPIR.Size = new System.Drawing.Size(147, 31);
            this.BtnOpenPIR.TabIndex = 0;
            this.BtnOpenPIR.Text = "打开PIR功能";
            this.BtnOpenPIR.UseVisualStyleBackColor = true;
            this.BtnOpenPIR.Click += new System.EventHandler(this.BtnOpenPIR_Click);
            // 
            // groupBox10
            // 
            this.groupBox10.Controls.Add(this.label1);
            this.groupBox10.Controls.Add(this.CbxWavIndex);
            this.groupBox10.Controls.Add(this.BtnPlayWav);
            this.groupBox10.Location = new System.Drawing.Point(6, 6);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(359, 70);
            this.groupBox10.TabIndex = 13;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "喇叭播放音频";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(207, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 19);
            this.label1.TabIndex = 7;
            this.label1.Text = ".wav";
            // 
            // CbxWavIndex
            // 
            this.CbxWavIndex.FormattingEnabled = true;
            this.CbxWavIndex.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "8",
            "9"});
            this.CbxWavIndex.Location = new System.Drawing.Point(117, 30);
            this.CbxWavIndex.Name = "CbxWavIndex";
            this.CbxWavIndex.Size = new System.Drawing.Size(83, 27);
            this.CbxWavIndex.TabIndex = 6;
            // 
            // BtnPlayWav
            // 
            this.BtnPlayWav.Location = new System.Drawing.Point(6, 30);
            this.BtnPlayWav.Name = "BtnPlayWav";
            this.BtnPlayWav.Size = new System.Drawing.Size(105, 26);
            this.BtnPlayWav.TabIndex = 5;
            this.BtnPlayWav.Text = "播放音频";
            this.BtnPlayWav.UseVisualStyleBackColor = true;
            this.BtnPlayWav.Click += new System.EventHandler(this.BtnPlayWav_Click);
            // 
            // groupBox11
            // 
            this.groupBox11.Controls.Add(this.textBoxMicResult);
            this.groupBox11.Controls.Add(this.label12);
            this.groupBox11.Controls.Add(this.textBoxMicDelta);
            this.groupBox11.Controls.Add(this.label11);
            this.groupBox11.Controls.Add(this.textBoxMicMaxAbs);
            this.groupBox11.Controls.Add(this.label10);
            this.groupBox11.Controls.Add(this.BtnAutoTestMic);
            this.groupBox11.Controls.Add(this.label9);
            this.groupBox11.Controls.Add(this.textBoxRecordDuration);
            this.groupBox11.Controls.Add(this.BtnPlayRecord);
            this.groupBox11.Controls.Add(this.BtnRecordMic);
            this.groupBox11.Controls.Add(this.BtnCloseMic);
            this.groupBox11.Controls.Add(this.BtnOpenMic);
            this.groupBox11.Location = new System.Drawing.Point(6, 82);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.Size = new System.Drawing.Size(359, 246);
            this.groupBox11.TabIndex = 14;
            this.groupBox11.TabStop = false;
            this.groupBox11.Text = "麦克风测试";
            // 
            // textBoxMicResult
            // 
            this.textBoxMicResult.Location = new System.Drawing.Point(272, 204);
            this.textBoxMicResult.Name = "textBoxMicResult";
            this.textBoxMicResult.Size = new System.Drawing.Size(80, 29);
            this.textBoxMicResult.TabIndex = 15;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(197, 209);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(69, 19);
            this.label12.TabIndex = 14;
            this.label12.Text = "result";
            // 
            // textBoxMicDelta
            // 
            this.textBoxMicDelta.Location = new System.Drawing.Point(272, 169);
            this.textBoxMicDelta.Name = "textBoxMicDelta";
            this.textBoxMicDelta.Size = new System.Drawing.Size(80, 29);
            this.textBoxMicDelta.TabIndex = 13;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(207, 172);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(59, 19);
            this.label11.TabIndex = 12;
            this.label11.Text = "delta";
            // 
            // textBoxMicMaxAbs
            // 
            this.textBoxMicMaxAbs.Location = new System.Drawing.Point(272, 134);
            this.textBoxMicMaxAbs.Name = "textBoxMicMaxAbs";
            this.textBoxMicMaxAbs.Size = new System.Drawing.Size(81, 29);
            this.textBoxMicMaxAbs.TabIndex = 11;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(187, 137);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(79, 19);
            this.label10.TabIndex = 10;
            this.label10.Text = "max_abs";
            // 
            // BtnAutoTestMic
            // 
            this.BtnAutoTestMic.Location = new System.Drawing.Point(9, 172);
            this.BtnAutoTestMic.Name = "BtnAutoTestMic";
            this.BtnAutoTestMic.Size = new System.Drawing.Size(132, 31);
            this.BtnAutoTestMic.TabIndex = 9;
            this.BtnAutoTestMic.Text = "自动分析录音";
            this.BtnAutoTestMic.UseVisualStyleBackColor = true;
            this.BtnAutoTestMic.Click += new System.EventHandler(this.BtnAutoTestMic_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(206, 83);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(28, 19);
            this.label9.TabIndex = 8;
            this.label9.Text = "秒";
            // 
            // textBoxRecordDuration
            // 
            this.textBoxRecordDuration.Location = new System.Drawing.Point(140, 78);
            this.textBoxRecordDuration.Name = "textBoxRecordDuration";
            this.textBoxRecordDuration.Size = new System.Drawing.Size(60, 29);
            this.textBoxRecordDuration.TabIndex = 4;
            // 
            // BtnPlayRecord
            // 
            this.BtnPlayRecord.Location = new System.Drawing.Point(6, 114);
            this.BtnPlayRecord.Name = "BtnPlayRecord";
            this.BtnPlayRecord.Size = new System.Drawing.Size(127, 31);
            this.BtnPlayRecord.TabIndex = 3;
            this.BtnPlayRecord.Text = "播放录音";
            this.BtnPlayRecord.UseVisualStyleBackColor = true;
            this.BtnPlayRecord.Click += new System.EventHandler(this.BtnPlayRecord_Click);
            // 
            // BtnRecordMic
            // 
            this.BtnRecordMic.Location = new System.Drawing.Point(6, 77);
            this.BtnRecordMic.Name = "BtnRecordMic";
            this.BtnRecordMic.Size = new System.Drawing.Size(127, 31);
            this.BtnRecordMic.TabIndex = 2;
            this.BtnRecordMic.Text = "开始录音";
            this.BtnRecordMic.UseVisualStyleBackColor = true;
            this.BtnRecordMic.Click += new System.EventHandler(this.BtnRecordMic_Click);
            // 
            // BtnCloseMic
            // 
            this.BtnCloseMic.Location = new System.Drawing.Point(139, 28);
            this.BtnCloseMic.Name = "BtnCloseMic";
            this.BtnCloseMic.Size = new System.Drawing.Size(117, 31);
            this.BtnCloseMic.TabIndex = 1;
            this.BtnCloseMic.Text = "关闭麦克风";
            this.BtnCloseMic.UseVisualStyleBackColor = true;
            this.BtnCloseMic.Click += new System.EventHandler(this.BtnCloseMic_Click);
            // 
            // BtnOpenMic
            // 
            this.BtnOpenMic.Location = new System.Drawing.Point(6, 28);
            this.BtnOpenMic.Name = "BtnOpenMic";
            this.BtnOpenMic.Size = new System.Drawing.Size(127, 31);
            this.BtnOpenMic.TabIndex = 0;
            this.BtnOpenMic.Text = "打开麦克风";
            this.BtnOpenMic.UseVisualStyleBackColor = true;
            this.BtnOpenMic.Click += new System.EventHandler(this.BtnOpenMic_Click);
            // 
            // groupBox12
            // 
            this.groupBox12.Controls.Add(this.comboBoxUnit);
            this.groupBox12.Controls.Add(this.label21);
            this.groupBox12.Controls.Add(this.numericUpDownBandWidth);
            this.groupBox12.Controls.Add(this.richTextBoxPCCmd);
            this.groupBox12.Controls.Add(this.numericUpDownDuration);
            this.groupBox12.Controls.Add(this.comboBoxServerIp);
            this.groupBox12.Controls.Add(this.label20);
            this.groupBox12.Controls.Add(this.label19);
            this.groupBox12.Controls.Add(this.label18);
            this.groupBox12.Controls.Add(this.label17);
            this.groupBox12.Controls.Add(this.label15);
            this.groupBox12.Controls.Add(this.textBoxDownLoss);
            this.groupBox12.Controls.Add(this.label16);
            this.groupBox12.Controls.Add(this.textBoxDownRate);
            this.groupBox12.Controls.Add(this.BtnWifiDownT);
            this.groupBox12.Controls.Add(this.label14);
            this.groupBox12.Controls.Add(this.textBoxUpLoss);
            this.groupBox12.Controls.Add(this.label13);
            this.groupBox12.Controls.Add(this.textBoxUpRate);
            this.groupBox12.Controls.Add(this.BtnWiFiUpT);
            this.groupBox12.Controls.Add(this.BtnCloseIperf3);
            this.groupBox12.Controls.Add(this.BtnOpenIperf3);
            this.groupBox12.Location = new System.Drawing.Point(450, 6);
            this.groupBox12.Name = "groupBox12";
            this.groupBox12.Size = new System.Drawing.Size(776, 341);
            this.groupBox12.TabIndex = 15;
            this.groupBox12.TabStop = false;
            this.groupBox12.Text = "WiFi吞吐量";
            // 
            // comboBoxUnit
            // 
            this.comboBoxUnit.FormattingEnabled = true;
            this.comboBoxUnit.Items.AddRange(new object[] {
            "K",
            "M",
            "G"});
            this.comboBoxUnit.Location = new System.Drawing.Point(235, 232);
            this.comboBoxUnit.Name = "comboBoxUnit";
            this.comboBoxUnit.Size = new System.Drawing.Size(36, 27);
            this.comboBoxUnit.TabIndex = 24;
            this.comboBoxUnit.Text = "M";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("宋体", 10F);
            this.label21.Location = new System.Drawing.Point(277, 240);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(28, 14);
            this.label21.TabIndex = 23;
            this.label21.Text = "bps";
            // 
            // numericUpDownBandWidth
            // 
            this.numericUpDownBandWidth.Location = new System.Drawing.Point(154, 230);
            this.numericUpDownBandWidth.Name = "numericUpDownBandWidth";
            this.numericUpDownBandWidth.Size = new System.Drawing.Size(67, 29);
            this.numericUpDownBandWidth.TabIndex = 22;
            this.numericUpDownBandWidth.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // numericUpDownDuration
            // 
            this.numericUpDownDuration.Location = new System.Drawing.Point(154, 270);
            this.numericUpDownDuration.Name = "numericUpDownDuration";
            this.numericUpDownDuration.Size = new System.Drawing.Size(67, 29);
            this.numericUpDownDuration.TabIndex = 21;
            this.numericUpDownDuration.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // comboBoxServerIp
            // 
            this.comboBoxServerIp.FormattingEnabled = true;
            this.comboBoxServerIp.Location = new System.Drawing.Point(597, 22);
            this.comboBoxServerIp.Name = "comboBoxServerIp";
            this.comboBoxServerIp.Size = new System.Drawing.Size(173, 27);
            this.comboBoxServerIp.TabIndex = 19;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(6, 232);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(104, 19);
            this.label20.TabIndex = 16;
            this.label20.Text = "指定带宽：";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(227, 274);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(28, 19);
            this.label19.TabIndex = 8;
            this.label19.Text = "秒";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(6, 274);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(142, 19);
            this.label18.TabIndex = 15;
            this.label18.Text = "测试持续时间：";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(486, 26);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(105, 19);
            this.label17.TabIndex = 13;
            this.label17.Text = "服务端IP：";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(540, 310);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(123, 19);
            this.label15.TabIndex = 11;
            this.label15.Text = "下行丢包率：";
            // 
            // textBoxDownLoss
            // 
            this.textBoxDownLoss.Location = new System.Drawing.Point(669, 304);
            this.textBoxDownLoss.Name = "textBoxDownLoss";
            this.textBoxDownLoss.Size = new System.Drawing.Size(100, 29);
            this.textBoxDownLoss.TabIndex = 10;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(559, 272);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(104, 19);
            this.label16.TabIndex = 9;
            this.label16.Text = "下行速率：";
            // 
            // textBoxDownRate
            // 
            this.textBoxDownRate.Location = new System.Drawing.Point(669, 269);
            this.textBoxDownRate.Name = "textBoxDownRate";
            this.textBoxDownRate.Size = new System.Drawing.Size(100, 29);
            this.textBoxDownRate.TabIndex = 8;
            // 
            // BtnWifiDownT
            // 
            this.BtnWifiDownT.Location = new System.Drawing.Point(669, 223);
            this.BtnWifiDownT.Name = "BtnWifiDownT";
            this.BtnWifiDownT.Size = new System.Drawing.Size(100, 36);
            this.BtnWifiDownT.TabIndex = 7;
            this.BtnWifiDownT.Text = "WiFi下行";
            this.BtnWifiDownT.UseVisualStyleBackColor = true;
            this.BtnWifiDownT.Click += new System.EventHandler(this.BtnWifiDownT_Click);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(305, 310);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(123, 19);
            this.label14.TabIndex = 6;
            this.label14.Text = "上行丢包率：";
            // 
            // textBoxUpLoss
            // 
            this.textBoxUpLoss.Location = new System.Drawing.Point(434, 306);
            this.textBoxUpLoss.Name = "textBoxUpLoss";
            this.textBoxUpLoss.Size = new System.Drawing.Size(100, 29);
            this.textBoxUpLoss.TabIndex = 5;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(324, 274);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(104, 19);
            this.label13.TabIndex = 4;
            this.label13.Text = "上行速率：";
            // 
            // textBoxUpRate
            // 
            this.textBoxUpRate.Location = new System.Drawing.Point(434, 271);
            this.textBoxUpRate.Name = "textBoxUpRate";
            this.textBoxUpRate.Size = new System.Drawing.Size(100, 29);
            this.textBoxUpRate.TabIndex = 3;
            // 
            // BtnWiFiUpT
            // 
            this.BtnWiFiUpT.Location = new System.Drawing.Point(434, 223);
            this.BtnWiFiUpT.Name = "BtnWiFiUpT";
            this.BtnWiFiUpT.Size = new System.Drawing.Size(100, 36);
            this.BtnWiFiUpT.TabIndex = 2;
            this.BtnWiFiUpT.Text = "WiFi上行";
            this.BtnWiFiUpT.UseVisualStyleBackColor = true;
            this.BtnWiFiUpT.Click += new System.EventHandler(this.BtnWiFiUpT_Click);
            // 
            // BtnCloseIperf3
            // 
            this.BtnCloseIperf3.Location = new System.Drawing.Point(170, 28);
            this.BtnCloseIperf3.Name = "BtnCloseIperf3";
            this.BtnCloseIperf3.Size = new System.Drawing.Size(139, 26);
            this.BtnCloseIperf3.TabIndex = 1;
            this.BtnCloseIperf3.Text = "关闭iPerf3";
            this.BtnCloseIperf3.UseVisualStyleBackColor = true;
            this.BtnCloseIperf3.Click += new System.EventHandler(this.BtnCloseIperf3_Click);
            // 
            // BtnOpenIperf3
            // 
            this.BtnOpenIperf3.Location = new System.Drawing.Point(10, 28);
            this.BtnOpenIperf3.Name = "BtnOpenIperf3";
            this.BtnOpenIperf3.Size = new System.Drawing.Size(138, 26);
            this.BtnOpenIperf3.TabIndex = 0;
            this.BtnOpenIperf3.Text = "打开iPerf3";
            this.BtnOpenIperf3.UseVisualStyleBackColor = true;
            this.BtnOpenIperf3.Click += new System.EventHandler(this.BtnOpenIperf3_Click);
            // 
            // groupBox13
            // 
            this.groupBox13.Controls.Add(this.BtnCloseIRLed);
            this.groupBox13.Controls.Add(this.BtnOpenIRLed);
            this.groupBox13.Controls.Add(this.BtnQuitBW);
            this.groupBox13.Controls.Add(this.BtnEnterBW);
            this.groupBox13.Location = new System.Drawing.Point(371, 6);
            this.groupBox13.Name = "groupBox13";
            this.groupBox13.Size = new System.Drawing.Size(656, 70);
            this.groupBox13.TabIndex = 16;
            this.groupBox13.TabStop = false;
            this.groupBox13.Text = "IR_CUT / IR_LED";
            // 
            // BtnCloseIRLed
            // 
            this.BtnCloseIRLed.Font = new System.Drawing.Font("宋体", 10F);
            this.BtnCloseIRLed.Location = new System.Drawing.Point(460, 27);
            this.BtnCloseIRLed.Name = "BtnCloseIRLed";
            this.BtnCloseIRLed.Size = new System.Drawing.Size(145, 35);
            this.BtnCloseIRLed.TabIndex = 3;
            this.BtnCloseIRLed.Text = "关闭IR_LED灯";
            this.BtnCloseIRLed.UseVisualStyleBackColor = true;
            this.BtnCloseIRLed.Click += new System.EventHandler(this.BtnCloseIRLed_Click);
            // 
            // BtnOpenIRLed
            // 
            this.BtnOpenIRLed.Font = new System.Drawing.Font("宋体", 10F);
            this.BtnOpenIRLed.Location = new System.Drawing.Point(309, 27);
            this.BtnOpenIRLed.Name = "BtnOpenIRLed";
            this.BtnOpenIRLed.Size = new System.Drawing.Size(145, 35);
            this.BtnOpenIRLed.TabIndex = 2;
            this.BtnOpenIRLed.Text = "打开IR_LED灯";
            this.BtnOpenIRLed.UseVisualStyleBackColor = true;
            this.BtnOpenIRLed.Click += new System.EventHandler(this.BtnOpenIRLed_Click);
            // 
            // BtnQuitBW
            // 
            this.BtnQuitBW.Font = new System.Drawing.Font("宋体", 10F);
            this.BtnQuitBW.Location = new System.Drawing.Point(158, 27);
            this.BtnQuitBW.Name = "BtnQuitBW";
            this.BtnQuitBW.Size = new System.Drawing.Size(145, 35);
            this.BtnQuitBW.TabIndex = 1;
            this.BtnQuitBW.Text = "IR_CUT退出黑白模式";
            this.BtnQuitBW.UseVisualStyleBackColor = true;
            this.BtnQuitBW.Click += new System.EventHandler(this.BtnQuitBW_Click);
            // 
            // BtnEnterBW
            // 
            this.BtnEnterBW.Font = new System.Drawing.Font("宋体", 10F);
            this.BtnEnterBW.Location = new System.Drawing.Point(7, 27);
            this.BtnEnterBW.Name = "BtnEnterBW";
            this.BtnEnterBW.Size = new System.Drawing.Size(145, 35);
            this.BtnEnterBW.TabIndex = 0;
            this.BtnEnterBW.Text = "IR_CUT进入黑白模式";
            this.BtnEnterBW.UseVisualStyleBackColor = true;
            this.BtnEnterBW.Click += new System.EventHandler(this.BtnEnterBW_Click);
            // 
            // groupBox14
            // 
            this.groupBox14.Controls.Add(this.CbxPCBARfIndex);
            this.groupBox14.Controls.Add(this.BtnRfRx);
            this.groupBox14.Controls.Add(this.BtnRfTx);
            this.groupBox14.Controls.Add(this.BtnPCBARfTx);
            this.groupBox14.Location = new System.Drawing.Point(371, 155);
            this.groupBox14.Name = "groupBox14";
            this.groupBox14.Size = new System.Drawing.Size(311, 173);
            this.groupBox14.TabIndex = 17;
            this.groupBox14.TabStop = false;
            this.groupBox14.Text = "RF Mode";
            // 
            // CbxPCBARfIndex
            // 
            this.CbxPCBARfIndex.FormattingEnabled = true;
            this.CbxPCBARfIndex.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4"});
            this.CbxPCBARfIndex.Location = new System.Drawing.Point(7, 31);
            this.CbxPCBARfIndex.Name = "CbxPCBARfIndex";
            this.CbxPCBARfIndex.Size = new System.Drawing.Size(120, 27);
            this.CbxPCBARfIndex.TabIndex = 3;
            // 
            // BtnRfRx
            // 
            this.BtnRfRx.Location = new System.Drawing.Point(144, 94);
            this.BtnRfRx.Name = "BtnRfRx";
            this.BtnRfRx.Size = new System.Drawing.Size(130, 30);
            this.BtnRfRx.TabIndex = 2;
            this.BtnRfRx.Text = "RF接收模式";
            this.BtnRfRx.UseVisualStyleBackColor = true;
            this.BtnRfRx.Click += new System.EventHandler(this.BtnRfRx_Click);
            // 
            // BtnRfTx
            // 
            this.BtnRfTx.Location = new System.Drawing.Point(7, 94);
            this.BtnRfTx.Name = "BtnRfTx";
            this.BtnRfTx.Size = new System.Drawing.Size(120, 30);
            this.BtnRfTx.TabIndex = 1;
            this.BtnRfTx.Text = "RF发送模式";
            this.BtnRfTx.UseVisualStyleBackColor = true;
            this.BtnRfTx.Click += new System.EventHandler(this.BtnRfTx_Click);
            // 
            // BtnPCBARfTx
            // 
            this.BtnPCBARfTx.Location = new System.Drawing.Point(144, 28);
            this.BtnPCBARfTx.Name = "BtnPCBARfTx";
            this.BtnPCBARfTx.Size = new System.Drawing.Size(150, 30);
            this.BtnPCBARfTx.TabIndex = 0;
            this.BtnPCBARfTx.Text = "PCBA RF TX";
            this.BtnPCBARfTx.UseVisualStyleBackColor = true;
            this.BtnPCBARfTx.Click += new System.EventHandler(this.BtnPCBARfTx_Click);
            // 
            // groupBox15
            // 
            this.groupBox15.Controls.Add(this.BtnWriteSNUID);
            this.groupBox15.Controls.Add(this.textBoxUID);
            this.groupBox15.Controls.Add(this.BtnReadUID);
            this.groupBox15.Controls.Add(this.textBoxSN);
            this.groupBox15.Controls.Add(this.BtnReadSN);
            this.groupBox15.Location = new System.Drawing.Point(280, 124);
            this.groupBox15.Name = "groupBox15";
            this.groupBox15.Size = new System.Drawing.Size(547, 195);
            this.groupBox15.TabIndex = 18;
            this.groupBox15.TabStop = false;
            this.groupBox15.Text = "SN / UID";
            // 
            // BtnWriteSNUID
            // 
            this.BtnWriteSNUID.Location = new System.Drawing.Point(202, 124);
            this.BtnWriteSNUID.Name = "BtnWriteSNUID";
            this.BtnWriteSNUID.Size = new System.Drawing.Size(125, 33);
            this.BtnWriteSNUID.TabIndex = 12;
            this.BtnWriteSNUID.Text = "设置UID和SN";
            this.BtnWriteSNUID.UseVisualStyleBackColor = true;
            this.BtnWriteSNUID.Click += new System.EventHandler(this.BtnWriteSNUID_Click);
            // 
            // textBoxUID
            // 
            this.textBoxUID.Location = new System.Drawing.Point(136, 70);
            this.textBoxUID.Name = "textBoxUID";
            this.textBoxUID.Size = new System.Drawing.Size(191, 29);
            this.textBoxUID.TabIndex = 9;
            // 
            // BtnReadUID
            // 
            this.BtnReadUID.Location = new System.Drawing.Point(6, 71);
            this.BtnReadUID.Name = "BtnReadUID";
            this.BtnReadUID.Size = new System.Drawing.Size(113, 28);
            this.BtnReadUID.TabIndex = 8;
            this.BtnReadUID.Text = "读取UID";
            this.BtnReadUID.UseVisualStyleBackColor = true;
            this.BtnReadUID.Click += new System.EventHandler(this.BtnReadUID_Click);
            // 
            // textBoxSN
            // 
            this.textBoxSN.Location = new System.Drawing.Point(136, 28);
            this.textBoxSN.Name = "textBoxSN";
            this.textBoxSN.Size = new System.Drawing.Size(191, 29);
            this.textBoxSN.TabIndex = 7;
            // 
            // BtnReadSN
            // 
            this.BtnReadSN.Location = new System.Drawing.Point(6, 29);
            this.BtnReadSN.Name = "BtnReadSN";
            this.BtnReadSN.Size = new System.Drawing.Size(113, 28);
            this.BtnReadSN.TabIndex = 0;
            this.BtnReadSN.Text = "读取SN";
            this.BtnReadSN.UseVisualStyleBackColor = true;
            this.BtnReadSN.Click += new System.EventHandler(this.BtnReadSN_Click);
            // 
            // groupBox16
            // 
            this.groupBox16.Controls.Add(this.label22);
            this.groupBox16.Controls.Add(this.BtnStopCharge);
            this.groupBox16.Controls.Add(this.BtnStartCharge);
            this.groupBox16.Controls.Add(this.BtnSetChargeMode);
            this.groupBox16.Controls.Add(this.textBoxChargeLevel);
            this.groupBox16.Location = new System.Drawing.Point(6, 6);
            this.groupBox16.Name = "groupBox16";
            this.groupBox16.Size = new System.Drawing.Size(335, 170);
            this.groupBox16.TabIndex = 19;
            this.groupBox16.TabStop = false;
            this.groupBox16.Text = "充电控制";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(82, 37);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(19, 19);
            this.label22.TabIndex = 4;
            this.label22.Text = "%";
            // 
            // BtnStopCharge
            // 
            this.BtnStopCharge.Location = new System.Drawing.Point(128, 98);
            this.BtnStopCharge.Name = "BtnStopCharge";
            this.BtnStopCharge.Size = new System.Drawing.Size(97, 37);
            this.BtnStopCharge.TabIndex = 3;
            this.BtnStopCharge.Text = "停止充电";
            this.BtnStopCharge.UseVisualStyleBackColor = true;
            this.BtnStopCharge.Click += new System.EventHandler(this.BtnStopCharge_Click);
            // 
            // BtnStartCharge
            // 
            this.BtnStartCharge.Location = new System.Drawing.Point(6, 98);
            this.BtnStartCharge.Name = "BtnStartCharge";
            this.BtnStartCharge.Size = new System.Drawing.Size(95, 37);
            this.BtnStartCharge.TabIndex = 2;
            this.BtnStartCharge.Text = "开始充电";
            this.BtnStartCharge.UseVisualStyleBackColor = true;
            this.BtnStartCharge.Click += new System.EventHandler(this.BtnStartCharge_Click);
            // 
            // BtnSetChargeMode
            // 
            this.BtnSetChargeMode.Location = new System.Drawing.Point(128, 28);
            this.BtnSetChargeMode.Name = "BtnSetChargeMode";
            this.BtnSetChargeMode.Size = new System.Drawing.Size(132, 36);
            this.BtnSetChargeMode.TabIndex = 1;
            this.BtnSetChargeMode.Text = "设置充电模式";
            this.BtnSetChargeMode.UseVisualStyleBackColor = true;
            this.BtnSetChargeMode.Click += new System.EventHandler(this.BtnSetChargeMode_Click);
            // 
            // textBoxChargeLevel
            // 
            this.textBoxChargeLevel.Location = new System.Drawing.Point(6, 34);
            this.textBoxChargeLevel.Name = "textBoxChargeLevel";
            this.textBoxChargeLevel.Size = new System.Drawing.Size(69, 29);
            this.textBoxChargeLevel.TabIndex = 0;
            // 
            // groupBox17
            // 
            this.groupBox17.Controls.Add(this.BtnQuitBWRTOS);
            this.groupBox17.Controls.Add(this.BtnEnterBWRTOS);
            this.groupBox17.Location = new System.Drawing.Point(6, 245);
            this.groupBox17.Name = "groupBox17";
            this.groupBox17.Size = new System.Drawing.Size(633, 102);
            this.groupBox17.TabIndex = 20;
            this.groupBox17.TabStop = false;
            this.groupBox17.Text = "夜视模式切换";
            // 
            // BtnReadRNRTOS
            // 
            this.BtnReadRNRTOS.Location = new System.Drawing.Point(6, 42);
            this.BtnReadRNRTOS.Name = "BtnReadRNRTOS";
            this.BtnReadRNRTOS.Size = new System.Drawing.Size(103, 35);
            this.BtnReadRNRTOS.TabIndex = 10;
            this.BtnReadRNRTOS.Text = "读取RN";
            this.BtnReadRNRTOS.UseVisualStyleBackColor = true;
            this.BtnReadRNRTOS.Click += new System.EventHandler(this.BtnReadRNRTOS_Click);
            // 
            // tBReadRNRTOS
            // 
            this.tBReadRNRTOS.Location = new System.Drawing.Point(115, 47);
            this.tBReadRNRTOS.Name = "tBReadRNRTOS";
            this.tBReadRNRTOS.Size = new System.Drawing.Size(186, 29);
            this.tBReadRNRTOS.TabIndex = 9;
            // 
            // CbxWriteTagNumRTOS
            // 
            this.CbxWriteTagNumRTOS.FormattingEnabled = true;
            this.CbxWriteTagNumRTOS.Items.AddRange(new object[] {
            "T010",
            "T020",
            "T030",
            "T040",
            "T050",
            "T060",
            "T070",
            "T080",
            "T100",
            "T110"});
            this.CbxWriteTagNumRTOS.Location = new System.Drawing.Point(127, 92);
            this.CbxWriteTagNumRTOS.Name = "CbxWriteTagNumRTOS";
            this.CbxWriteTagNumRTOS.Size = new System.Drawing.Size(99, 27);
            this.CbxWriteTagNumRTOS.TabIndex = 8;
            // 
            // BtnWriteTagNumRTOS
            // 
            this.BtnWriteTagNumRTOS.Location = new System.Drawing.Point(6, 92);
            this.BtnWriteTagNumRTOS.Name = "BtnWriteTagNumRTOS";
            this.BtnWriteTagNumRTOS.Size = new System.Drawing.Size(115, 31);
            this.BtnWriteTagNumRTOS.TabIndex = 7;
            this.BtnWriteTagNumRTOS.Text = "写入工序号";
            this.BtnWriteTagNumRTOS.UseVisualStyleBackColor = true;
            this.BtnWriteTagNumRTOS.Click += new System.EventHandler(this.BtnWriteTagNumRTOS_Click);
            // 
            // BtnReadTagNumRTOS
            // 
            this.BtnReadTagNumRTOS.Location = new System.Drawing.Point(6, 42);
            this.BtnReadTagNumRTOS.Name = "BtnReadTagNumRTOS";
            this.BtnReadTagNumRTOS.Size = new System.Drawing.Size(115, 35);
            this.BtnReadTagNumRTOS.TabIndex = 6;
            this.BtnReadTagNumRTOS.Text = "读取工序号";
            this.BtnReadTagNumRTOS.UseVisualStyleBackColor = true;
            this.BtnReadTagNumRTOS.Click += new System.EventHandler(this.BtnReadTagNumRTOS_Click);
            // 
            // tBReadTagNumRTOS
            // 
            this.tBReadTagNumRTOS.Location = new System.Drawing.Point(127, 47);
            this.tBReadTagNumRTOS.Name = "tBReadTagNumRTOS";
            this.tBReadTagNumRTOS.Size = new System.Drawing.Size(99, 29);
            this.tBReadTagNumRTOS.TabIndex = 5;
            // 
            // BtnQuitBWRTOS
            // 
            this.BtnQuitBWRTOS.Location = new System.Drawing.Point(139, 38);
            this.BtnQuitBWRTOS.Name = "BtnQuitBWRTOS";
            this.BtnQuitBWRTOS.Size = new System.Drawing.Size(107, 38);
            this.BtnQuitBWRTOS.TabIndex = 2;
            this.BtnQuitBWRTOS.Text = "退出夜视";
            this.BtnQuitBWRTOS.UseVisualStyleBackColor = true;
            this.BtnQuitBWRTOS.Click += new System.EventHandler(this.BtnQuitBWRTOS_Click);
            // 
            // BtnEnterBWRTOS
            // 
            this.BtnEnterBWRTOS.Location = new System.Drawing.Point(6, 38);
            this.BtnEnterBWRTOS.Name = "BtnEnterBWRTOS";
            this.BtnEnterBWRTOS.Size = new System.Drawing.Size(114, 38);
            this.BtnEnterBWRTOS.TabIndex = 1;
            this.BtnEnterBWRTOS.Text = "进入夜视";
            this.BtnEnterBWRTOS.UseVisualStyleBackColor = true;
            this.BtnEnterBWRTOS.Click += new System.EventHandler(this.BtnEnterBWRTOS_Click);
            // 
            // BtnOpenCamera
            // 
            this.BtnOpenCamera.Location = new System.Drawing.Point(6, 33);
            this.BtnOpenCamera.Name = "BtnOpenCamera";
            this.BtnOpenCamera.Size = new System.Drawing.Size(114, 36);
            this.BtnOpenCamera.TabIndex = 0;
            this.BtnOpenCamera.Text = "打开摄像头";
            this.BtnOpenCamera.UseVisualStyleBackColor = true;
            this.BtnOpenCamera.Click += new System.EventHandler(this.BtnOpenCamera_Click);
            // 
            // comboBoxCurPort
            // 
            this.comboBoxCurPort.FormattingEnabled = true;
            this.comboBoxCurPort.Location = new System.Drawing.Point(148, 21);
            this.comboBoxCurPort.Name = "comboBoxCurPort";
            this.comboBoxCurPort.Size = new System.Drawing.Size(121, 27);
            this.comboBoxCurPort.TabIndex = 21;
            this.comboBoxCurPort.SelectedIndexChanged += new System.EventHandler(this.comboBoxCurPort_SelectedIndexChanged);
            // 
            // labelRefreshPort
            // 
            this.labelRefreshPort.AutoSize = true;
            this.labelRefreshPort.Location = new System.Drawing.Point(95, 23);
            this.labelRefreshPort.Name = "labelRefreshPort";
            this.labelRefreshPort.Size = new System.Drawing.Size(47, 19);
            this.labelRefreshPort.TabIndex = 22;
            this.labelRefreshPort.Text = "串口";
            this.labelRefreshPort.Click += new System.EventHandler(this.labelRefreshPort_Click);
            // 
            // BtnOpenPort
            // 
            this.BtnOpenPort.Location = new System.Drawing.Point(275, 20);
            this.BtnOpenPort.Name = "BtnOpenPort";
            this.BtnOpenPort.Size = new System.Drawing.Size(101, 30);
            this.BtnOpenPort.TabIndex = 23;
            this.BtnOpenPort.Text = "打开串口";
            this.BtnOpenPort.UseVisualStyleBackColor = true;
            this.BtnOpenPort.Click += new System.EventHandler(this.BtnOpenPort_Click);
            // 
            // richTextBoxPCCmd
            // 
            this.richTextBoxPCCmd.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBoxPCCmd.BackColor = System.Drawing.SystemColors.InfoText;
            this.richTextBoxPCCmd.Font = new System.Drawing.Font("宋体", 9F);
            this.richTextBoxPCCmd.ForeColor = System.Drawing.SystemColors.Info;
            this.richTextBoxPCCmd.Location = new System.Drawing.Point(7, 56);
            this.richTextBoxPCCmd.Margin = new System.Windows.Forms.Padding(4);
            this.richTextBoxPCCmd.Name = "richTextBoxPCCmd";
            this.richTextBoxPCCmd.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.richTextBoxPCCmd.Size = new System.Drawing.Size(762, 160);
            this.richTextBoxPCCmd.TabIndex = 24;
            this.richTextBoxPCCmd.Text = "";
            this.richTextBoxPCCmd.WordWrap = false;
            // 
            // textBoxSendCmd
            // 
            this.textBoxSendCmd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxSendCmd.Location = new System.Drawing.Point(385, 21);
            this.textBoxSendCmd.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxSendCmd.Name = "textBoxSendCmd";
            this.textBoxSendCmd.Size = new System.Drawing.Size(480, 29);
            this.textBoxSendCmd.TabIndex = 29;
            this.textBoxSendCmd.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxSendCmd_KeyDown);
            // 
            // BtnSendCMD
            // 
            this.BtnSendCMD.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BtnSendCMD.Location = new System.Drawing.Point(873, 20);
            this.BtnSendCMD.Margin = new System.Windows.Forms.Padding(4);
            this.BtnSendCMD.Name = "BtnSendCMD";
            this.BtnSendCMD.Size = new System.Drawing.Size(100, 31);
            this.BtnSendCMD.TabIndex = 30;
            this.BtnSendCMD.Text = "发送";
            this.BtnSendCMD.UseVisualStyleBackColor = true;
            this.BtnSendCMD.Click += new System.EventHandler(this.BtnSendCMD_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Font = new System.Drawing.Font("宋体", 14F);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1240, 386);
            this.tabControl1.TabIndex = 31;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.groupBox15);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Controls.Add(this.groupBox7);
            this.tabPage1.Location = new System.Drawing.Point(4, 29);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1232, 353);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "信息读写";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox4);
            this.tabPage2.Controls.Add(this.groupBox5);
            this.tabPage2.Controls.Add(this.groupBox12);
            this.tabPage2.Location = new System.Drawing.Point(4, 29);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1232, 353);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "WiFi";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.groupBox8);
            this.tabPage3.Controls.Add(this.groupBox10);
            this.tabPage3.Controls.Add(this.groupBox11);
            this.tabPage3.Controls.Add(this.groupBox9);
            this.tabPage3.Controls.Add(this.groupBox14);
            this.tabPage3.Controls.Add(this.groupBox13);
            this.tabPage3.Controls.Add(this.groupBox6);
            this.tabPage3.Location = new System.Drawing.Point(4, 29);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(1232, 353);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "功能测试";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.groupBox16);
            this.tabPage4.Location = new System.Drawing.Point(4, 29);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(1232, 353);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "充电";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.groupBox20);
            this.tabPage5.Controls.Add(this.groupBox19);
            this.tabPage5.Controls.Add(this.groupBox18);
            this.tabPage5.Controls.Add(this.groupBox17);
            this.tabPage5.Location = new System.Drawing.Point(4, 29);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(1232, 353);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "RTOS指令";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.labelRefreshPort);
            this.panel1.Controls.Add(this.comboBoxCurPort);
            this.panel1.Controls.Add(this.BtnSendCMD);
            this.panel1.Controls.Add(this.BtnOpenPort);
            this.panel1.Controls.Add(this.textBoxSendCmd);
            this.panel1.Font = new System.Drawing.Font("宋体", 14F);
            this.panel1.Location = new System.Drawing.Point(13, 405);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1239, 63);
            this.panel1.TabIndex = 32;
            // 
            // groupBox18
            // 
            this.groupBox18.Controls.Add(this.tBReadRNRTOS);
            this.groupBox18.Controls.Add(this.BtnReadRNRTOS);
            this.groupBox18.Location = new System.Drawing.Point(6, 6);
            this.groupBox18.Name = "groupBox18";
            this.groupBox18.Size = new System.Drawing.Size(328, 140);
            this.groupBox18.TabIndex = 21;
            this.groupBox18.TabStop = false;
            this.groupBox18.Text = "RN";
            // 
            // groupBox19
            // 
            this.groupBox19.Controls.Add(this.CbxWriteTagNumRTOS);
            this.groupBox19.Controls.Add(this.BtnReadTagNumRTOS);
            this.groupBox19.Controls.Add(this.BtnWriteTagNumRTOS);
            this.groupBox19.Controls.Add(this.tBReadTagNumRTOS);
            this.groupBox19.Location = new System.Drawing.Point(340, 6);
            this.groupBox19.Name = "groupBox19";
            this.groupBox19.Size = new System.Drawing.Size(299, 140);
            this.groupBox19.TabIndex = 22;
            this.groupBox19.TabStop = false;
            this.groupBox19.Text = "工序号";
            // 
            // groupBox20
            // 
            this.groupBox20.Controls.Add(this.BtnOpenCamera);
            this.groupBox20.Location = new System.Drawing.Point(6, 152);
            this.groupBox20.Name = "groupBox20";
            this.groupBox20.Size = new System.Drawing.Size(634, 87);
            this.groupBox20.TabIndex = 23;
            this.groupBox20.TabStop = false;
            this.groupBox20.Text = "摄像头";
            // 
            // XDC03DebugForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.richTextBox1);
            this.Font = new System.Drawing.Font("宋体", 12F);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "XDC03DebugForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox8.ResumeLayout(false);
            this.groupBox9.ResumeLayout(false);
            this.groupBox10.ResumeLayout(false);
            this.groupBox10.PerformLayout();
            this.groupBox11.ResumeLayout(false);
            this.groupBox11.PerformLayout();
            this.groupBox12.ResumeLayout(false);
            this.groupBox12.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBandWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDuration)).EndInit();
            this.groupBox13.ResumeLayout(false);
            this.groupBox14.ResumeLayout(false);
            this.groupBox15.ResumeLayout(false);
            this.groupBox15.PerformLayout();
            this.groupBox16.ResumeLayout(false);
            this.groupBox16.PerformLayout();
            this.groupBox17.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox18.ResumeLayout(false);
            this.groupBox18.PerformLayout();
            this.groupBox19.ResumeLayout(false);
            this.groupBox19.PerformLayout();
            this.groupBox20.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.IO.Ports.SerialPort serialPortXdc03;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button BtnReadSysInfo;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxFwVer;
        private System.Windows.Forms.TextBox textBoxHwVer;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxMcuVer;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxBatteryVol;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxWiFiVer;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxCpuTemp;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBoxBatteryPer;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button BtnReadRN;
        private System.Windows.Forms.TextBox textBoxReadRN;
        private System.Windows.Forms.Button BtnWriteRN;
        private System.Windows.Forms.TextBox textBoxWriteRN;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button BtnWriteTagNum;
        private System.Windows.Forms.Button BtnReadTagNum;
        private System.Windows.Forms.TextBox textBoxReadTagNum;
        private System.Windows.Forms.ComboBox comboBoxWriteTagNum;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox textBoxReadWiFiPWD;
        private System.Windows.Forms.TextBox textBoxReadWiFiSSID;
        private System.Windows.Forms.Button BtnReadWiFi;
        private System.Windows.Forms.TextBox textBoxWriteWiFiPWD;
        private System.Windows.Forms.Button BtnWriteWiFi;
        private System.Windows.Forms.TextBox textBoxWriteWiFiSSID;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TextBox textBoxReadIP;
        private System.Windows.Forms.Button BtnReadIP;
        private System.Windows.Forms.Button BtnReadMAC;
        private System.Windows.Forms.TextBox textBoxReadMAC;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button BtnReadLight;
        private System.Windows.Forms.TextBox textBoxReadLight;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Button BtnFactoryMode;
        private System.Windows.Forms.Button BtnFactoryReset;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.ComboBox CbxLedColor;
        private System.Windows.Forms.Button BtnSetLEDColor;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.Button BtnOpenPIR;
        private System.Windows.Forms.Button BtnClosePIR;
        private System.Windows.Forms.GroupBox groupBox10;
        private System.Windows.Forms.ComboBox CbxWavIndex;
        private System.Windows.Forms.Button BtnPlayWav;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox11;
        private System.Windows.Forms.Button BtnOpenMic;
        private System.Windows.Forms.Button BtnCloseMic;
        private System.Windows.Forms.Button BtnRecordMic;
        private System.Windows.Forms.Button BtnPlayRecord;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBoxRecordDuration;
        private System.Windows.Forms.Button BtnAutoTestMic;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textBoxMicResult;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox textBoxMicDelta;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBoxMicMaxAbs;
        private System.Windows.Forms.GroupBox groupBox12;
        private System.Windows.Forms.Button BtnOpenIperf3;
        private System.Windows.Forms.Button BtnCloseIperf3;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox textBoxUpRate;
        private System.Windows.Forms.Button BtnWiFiUpT;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox textBoxUpLoss;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox textBoxDownLoss;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox textBoxDownRate;
        private System.Windows.Forms.Button BtnWifiDownT;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Button BtnOpenRTSP;
        private System.Windows.Forms.TextBox textBoxRTSPUrl;
        private System.Windows.Forms.GroupBox groupBox13;
        private System.Windows.Forms.Button BtnEnterBW;
        private System.Windows.Forms.Button BtnQuitBW;
        private System.Windows.Forms.Button BtnOpenIRLed;
        private System.Windows.Forms.Button BtnCloseIRLed;
        private System.Windows.Forms.GroupBox groupBox14;
        private System.Windows.Forms.Button BtnPCBARfTx;
        private System.Windows.Forms.Button BtnRfTx;
        private System.Windows.Forms.Button BtnRfRx;
        private System.Windows.Forms.ComboBox CbxPCBARfIndex;
        private System.Windows.Forms.GroupBox groupBox15;
        private System.Windows.Forms.Button BtnReadSN;
        private System.Windows.Forms.TextBox textBoxSN;
        private System.Windows.Forms.TextBox textBoxUID;
        private System.Windows.Forms.Button BtnReadUID;
        private System.Windows.Forms.Button BtnWriteSNUID;
        private System.Windows.Forms.GroupBox groupBox16;
        private System.Windows.Forms.Button BtnSetChargeMode;
        private System.Windows.Forms.TextBox textBoxChargeLevel;
        private System.Windows.Forms.Button BtnStartCharge;
        private System.Windows.Forms.Button BtnStopCharge;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.GroupBox groupBox17;
        private System.Windows.Forms.Button BtnOpenCamera;
        private System.Windows.Forms.Button BtnQuitBWRTOS;
        private System.Windows.Forms.Button BtnEnterBWRTOS;
        private System.Windows.Forms.ComboBox CbxWriteTagNumRTOS;
        private System.Windows.Forms.Button BtnWriteTagNumRTOS;
        private System.Windows.Forms.Button BtnReadTagNumRTOS;
        private System.Windows.Forms.TextBox tBReadTagNumRTOS;
        private System.Windows.Forms.Button BtnReadRNRTOS;
        private System.Windows.Forms.TextBox tBReadRNRTOS;
        private System.Windows.Forms.ComboBox comboBoxCurPort;
        private System.Windows.Forms.Label labelRefreshPort;
        private System.Windows.Forms.Button BtnOpenPort;
        private System.Windows.Forms.RichTextBox richTextBoxPCCmd;
        private System.Windows.Forms.ComboBox comboBoxServerIp;
        private System.Windows.Forms.TextBox textBoxSendCmd;
        private System.Windows.Forms.Button BtnSendCMD;
        private System.Windows.Forms.NumericUpDown numericUpDownDuration;
        private System.Windows.Forms.NumericUpDown numericUpDownBandWidth;
        private System.Windows.Forms.ComboBox comboBoxUnit;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox18;
        private System.Windows.Forms.GroupBox groupBox19;
        private System.Windows.Forms.GroupBox groupBox20;
    }
}

