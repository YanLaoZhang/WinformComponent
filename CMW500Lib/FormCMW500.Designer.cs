namespace CMW500Lib
{
    partial class FormCMW500
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.richTextBoxTips = new System.Windows.Forms.RichTextBox();
            this.textBoxCMW500IP = new System.Windows.Forms.TextBox();
            this.BtnOpen = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.BtnLTEMultiEvaluation = new System.Windows.Forms.Button();
            this.BtnLTESignal = new System.Windows.Forms.Button();
            this.BtnSend1 = new System.Windows.Forms.Button();
            this.comboBoxSCPICMD = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.BtnSend = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxSCPICMD = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.BtnIDN = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.richTextBoxRun = new System.Windows.Forms.RichTextBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.BtnHWOP = new System.Windows.Forms.Button();
            this.BtnRTS = new System.Windows.Forms.Button();
            this.BtnCheckError = new System.Windows.Forms.Button();
            this.BtnSetEATT = new System.Windows.Forms.Button();
            this.NudEATT = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.BtnSetScenario = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBoxStandard = new System.Windows.Forms.ComboBox();
            this.BtnSetSignal = new System.Windows.Forms.Button();
            this.BtnSetFreq = new System.Windows.Forms.Button();
            this.BtnWlanInit = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.NudFrequency = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.BtnWlanState = new System.Windows.Forms.Button();
            this.NudEnPower = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.BtnWlanCur = new System.Windows.Forms.Button();
            this.comboBoxTriggerSrc = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.NudTriggerThreshold = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.BtnSetTrigger = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NudEATT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NudFrequency)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NudEnPower)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NudTriggerThreshold)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 116);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "CMW500 IP:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.richTextBoxTips);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(775, 94);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "测试说明";
            // 
            // richTextBoxTips
            // 
            this.richTextBoxTips.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBoxTips.Enabled = false;
            this.richTextBoxTips.Location = new System.Drawing.Point(7, 21);
            this.richTextBoxTips.Name = "richTextBoxTips";
            this.richTextBoxTips.Size = new System.Drawing.Size(762, 67);
            this.richTextBoxTips.TabIndex = 0;
            this.richTextBoxTips.Text = "";
            // 
            // textBoxCMW500IP
            // 
            this.textBoxCMW500IP.Location = new System.Drawing.Point(83, 113);
            this.textBoxCMW500IP.Name = "textBoxCMW500IP";
            this.textBoxCMW500IP.Size = new System.Drawing.Size(173, 21);
            this.textBoxCMW500IP.TabIndex = 2;
            this.textBoxCMW500IP.Text = "10.10.10.12";
            // 
            // BtnOpen
            // 
            this.BtnOpen.Location = new System.Drawing.Point(262, 113);
            this.BtnOpen.Name = "BtnOpen";
            this.BtnOpen.Size = new System.Drawing.Size(75, 21);
            this.BtnOpen.TabIndex = 3;
            this.BtnOpen.Text = "打开连接";
            this.BtnOpen.UseVisualStyleBackColor = true;
            this.BtnOpen.Click += new System.EventHandler(this.BtnOpen_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.BtnSend1);
            this.groupBox2.Controls.Add(this.comboBoxSCPICMD);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.BtnSend);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.textBoxSCPICMD);
            this.groupBox2.Location = new System.Drawing.Point(12, 140);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(776, 81);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "SCPI交互调试";
            // 
            // BtnLTEMultiEvaluation
            // 
            this.BtnLTEMultiEvaluation.Location = new System.Drawing.Point(143, 20);
            this.BtnLTEMultiEvaluation.Name = "BtnLTEMultiEvaluation";
            this.BtnLTEMultiEvaluation.Size = new System.Drawing.Size(131, 23);
            this.BtnLTEMultiEvaluation.TabIndex = 7;
            this.BtnLTEMultiEvaluation.Text = "初始化LTE Multi Evaluation";
            this.BtnLTEMultiEvaluation.UseVisualStyleBackColor = true;
            this.BtnLTEMultiEvaluation.Click += new System.EventHandler(this.BtnLTEMultiEvaluation_Click);
            // 
            // BtnLTESignal
            // 
            this.BtnLTESignal.Location = new System.Drawing.Point(6, 20);
            this.BtnLTESignal.Name = "BtnLTESignal";
            this.BtnLTESignal.Size = new System.Drawing.Size(131, 23);
            this.BtnLTESignal.TabIndex = 6;
            this.BtnLTESignal.Text = "初始化LTE Signal";
            this.BtnLTESignal.UseVisualStyleBackColor = true;
            this.BtnLTESignal.Click += new System.EventHandler(this.BtnLTESignal_Click);
            // 
            // BtnSend1
            // 
            this.BtnSend1.Location = new System.Drawing.Point(626, 47);
            this.BtnSend1.Name = "BtnSend1";
            this.BtnSend1.Size = new System.Drawing.Size(75, 23);
            this.BtnSend1.TabIndex = 5;
            this.BtnSend1.Text = "发送";
            this.BtnSend1.UseVisualStyleBackColor = true;
            this.BtnSend1.Click += new System.EventHandler(this.BtnSend1_Click);
            // 
            // comboBoxSCPICMD
            // 
            this.comboBoxSCPICMD.FormattingEnabled = true;
            this.comboBoxSCPICMD.Items.AddRange(new object[] {
            "*IDN?",
            "*RST;*CLS;*OPC?",
            "SYSTem:REMOTE:LOCal",
            "SYSTem:DISPlay:UPDate ON",
            "SYSTem:DISPlay:UPDate OFF",
            "TRACe:REMote:MODE:DISPlay:ENABle ANALysis",
            "TRACe:REMote:MODE:DISPlay:ENABle LIVE",
            "TRACe:REMote:MODE:DISPlay:ENABle OFF",
            "TRACe:REMote:MODE:DISPlay:CLEar",
            "SYSTem:PRESet:ALL",
            "SYSTem:RESet:ALL",
            "SYSTem:ERRor:ALL?",
            "",
            "CONFigure:WLAN:MEAS:RFSettings:EATTenuation 7.96",
            "ROUTe:WLAN:MEAS:SCENario:SALone RF1C,RX1",
            "CONFigure:WLAN:MEAS:ISIGnal:STANdard LOFDm;BWIDth BW20mhz",
            "CONFigure:WLAN:MEAS:ISIGnal:STANdard DSSS;BWIDth BW20mhz",
            "CONFigure:WLAN:MEAS:ISIGnal:STANdard HTOFdm;BWIDth BW20mhz",
            "CONFigure:WLAN:MEAS:ISIGnal:STANdard HTOFdm;BWIDth BW40mhz",
            "CONFigure:WLAN:MEAS:MEValuation:SCOunt:MOD 1;TSM 1;PVTime 1",
            "CONFigure:WLAN:MEAS:MEValuation:RESult:TSMask OFF",
            "CONFigure:WLAN:MEAS:MEValuation:RESult:TSMask ON",
            "CONFigure:WLAN:MEAS:RFSettings:FREQuency 2442.000000MHz;UMAR 0;ENPower",
            "TRIGger:WLAN:MEAS:MEValuation:THReshold -25",
            "CONFigure:WLAN:MEAS:ISIGnal:BTYPe MIXed",
            "INIT:WLAN:MEAS:MEValuation",
            "FETCh:WLAN:MEAS:MEValuation:STATe?",
            "FETCh:WLAN:MEAS:MEValuation:MODulation:CURR?",
            "FETCh:WLAN:MEAS:MEValuation:MODulation:AVERage?",
            "FETCh:WLAN:MEAS:MEValuation:MODulation:DSSS:AVERage?",
            "FETCh:WLAN:MEAS:MEValuation:TRACe:TSMask:AVERage?",
            "FETCh:WLAN:MEAS:MEValuation:TRACe:TSMask:MASK?",
            "",
            "ROUTe:LTE:SIGN:SCENario:SCELl RF1C,RX1,RF1C,TX1",
            "CONFigure:LTE:SIGN:RFSettings:EATTenuation:OUTPut 2",
            "CONFigure:LTE:SIGN:RFSettings:EATTenuation:INPut 2",
            "CONFigure:LTE:SIGN:DMODe:UCSPecific OFF",
            "CONFigure:LTE:SIGN:DMODe FDD",
            "CONFigure:LTE:SIGN:PCC:BAND OB7",
            "",
            "CONFigure:LTE:SIGN:RFSettings:PCC:CHANnel:DL 3000 ;UL?",
            "CONFigure:LTE:SIGN:RFSettings:PCC:CHANnel:UL? Hz",
            "CONFigure:LTE:SIGN:RFSettings:PCC:FOFFset:DL 100",
            "CONFigure:LTE:SIGN:RFSettings:PCC:FOFFset:UL -200",
            "",
            "SOURce:LTE:SIGN:CELL:STATe ON",
            "SOURce:LTE:SIGN:CELL:STATe OFF",
            "SOURce:LTE:SIGN:CELL:STATe:ALL?",
            "",
            "FETCh:LTE:SIGN:PSWitched:STATe?",
            "SENSe:LTE:SIGN:RRCState?",
            "CALL:LTE:SIGN:PSWitched:ACTion CONNect",
            "CALL:LTE:SIGN:PSWitched:ACTion DISConnect",
            "CALL:LTE:SIGN:PSWitched:ACTion DETach",
            "",
            "SENSe:LTE:SIGN:UEReport:PCC:RSRP?",
            "SENSe:LTE:SIGN:UEReport:PCC:RSRQ?",
            "",
            "SENSe:LTE:SIGN:UESinfo:IMEI?",
            "SENSe:LTE:SIGN:UESinfo:IMSI?",
            "SENSe:LTE:SIGN:UESinfo:VDPReference?",
            "SENSe:LTE:SIGN:UESinfo:UEUSage?",
            "SENSe:LTE:SIGN:UESinfo:UEADdress:IPV4?",
            "SENSe:LTE:SIGN:UESinfo:UEADdress:IPV6?",
            "SENSe:LTE:SIGN:UESinfo:UEADdress:DEDBearer:SEParate?",
            "",
            "ROUTe:LTE:MEAS:SCENario:CSPath",
            "ROUTe:LTE:MEAS:SCENario?",
            "ROUTe:LTE:MEAS?",
            "CONFigure:LTE:MEAS:RFSettings:EATTenuation 2",
            "",
            "INIT:LTE:MEAS:MEValuation",
            "STOP:LTE:MEAS:MEValuation",
            "ABORt:LTE:MEAS:MEValuation",
            "FETCh:LTE:MEAS:MEValuation:STATe?",
            "",
            "FETC:LTE:MEAS:MEValuation:MODulation:AVERage?",
            "FETC:LTE:MEAS:MEValuation:MODulation:CURRent?",
            "FETCh:LTE:MEAS:MEValuation:MODulation:EXTReme?",
            "",
            "FETCh:LTE:MEAS:MEValuation:SEMask:CURRent?",
            "FETCh:LTE:MEAS:MEValuation:SEMask:AVERage?",
            "FETCh:LTE:MEAS:MEValuation:SEMask:EXTReme?"});
            this.comboBoxSCPICMD.Location = new System.Drawing.Point(77, 47);
            this.comboBoxSCPICMD.Name = "comboBoxSCPICMD";
            this.comboBoxSCPICMD.Size = new System.Drawing.Size(543, 20);
            this.comboBoxSCPICMD.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "指令选择：";
            // 
            // BtnSend
            // 
            this.BtnSend.Location = new System.Drawing.Point(626, 18);
            this.BtnSend.Name = "BtnSend";
            this.BtnSend.Size = new System.Drawing.Size(75, 23);
            this.BtnSend.TabIndex = 2;
            this.BtnSend.Text = "发送";
            this.BtnSend.UseVisualStyleBackColor = true;
            this.BtnSend.Click += new System.EventHandler(this.BtnSend_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "指令：";
            // 
            // textBoxSCPICMD
            // 
            this.textBoxSCPICMD.Location = new System.Drawing.Point(53, 20);
            this.textBoxSCPICMD.Name = "textBoxSCPICMD";
            this.textBoxSCPICMD.Size = new System.Drawing.Size(567, 21);
            this.textBoxSCPICMD.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.BtnSetTrigger);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.NudTriggerThreshold);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.comboBoxTriggerSrc);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.BtnWlanCur);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.NudEnPower);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.BtnWlanState);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.NudFrequency);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.BtnWlanInit);
            this.groupBox3.Controls.Add(this.BtnSetFreq);
            this.groupBox3.Controls.Add(this.BtnSetSignal);
            this.groupBox3.Controls.Add(this.comboBoxStandard);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.BtnSetScenario);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.NudEATT);
            this.groupBox3.Controls.Add(this.BtnSetEATT);
            this.groupBox3.Location = new System.Drawing.Point(12, 227);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(776, 129);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "WLAN Measurement";
            // 
            // BtnIDN
            // 
            this.BtnIDN.Location = new System.Drawing.Point(343, 113);
            this.BtnIDN.Name = "BtnIDN";
            this.BtnIDN.Size = new System.Drawing.Size(96, 23);
            this.BtnIDN.TabIndex = 8;
            this.BtnIDN.Text = "读取设备信息";
            this.BtnIDN.UseVisualStyleBackColor = true;
            this.BtnIDN.Click += new System.EventHandler(this.BtnIDN_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.richTextBoxRun);
            this.groupBox4.Location = new System.Drawing.Point(12, 491);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(776, 176);
            this.groupBox4.TabIndex = 20;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "执行日志：";
            // 
            // richTextBoxRun
            // 
            this.richTextBoxRun.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxRun.Location = new System.Drawing.Point(3, 17);
            this.richTextBoxRun.Name = "richTextBoxRun";
            this.richTextBoxRun.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.richTextBoxRun.Size = new System.Drawing.Size(770, 156);
            this.richTextBoxRun.TabIndex = 0;
            this.richTextBoxRun.Text = "";
            this.richTextBoxRun.TextChanged += new System.EventHandler(this.richTextBoxRun_TextChanged);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.BtnLTEMultiEvaluation);
            this.groupBox5.Controls.Add(this.BtnLTESignal);
            this.groupBox5.Location = new System.Drawing.Point(12, 362);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(776, 123);
            this.groupBox5.TabIndex = 21;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "LTE Test";
            // 
            // BtnHWOP
            // 
            this.BtnHWOP.Location = new System.Drawing.Point(445, 113);
            this.BtnHWOP.Name = "BtnHWOP";
            this.BtnHWOP.Size = new System.Drawing.Size(100, 23);
            this.BtnHWOP.TabIndex = 9;
            this.BtnHWOP.Text = "读取硬件支持";
            this.BtnHWOP.UseVisualStyleBackColor = true;
            this.BtnHWOP.Click += new System.EventHandler(this.BtnHWOP_Click);
            // 
            // BtnRTS
            // 
            this.BtnRTS.Location = new System.Drawing.Point(551, 116);
            this.BtnRTS.Name = "BtnRTS";
            this.BtnRTS.Size = new System.Drawing.Size(98, 20);
            this.BtnRTS.TabIndex = 10;
            this.BtnRTS.Text = "设备重置";
            this.BtnRTS.UseVisualStyleBackColor = true;
            this.BtnRTS.Click += new System.EventHandler(this.BtnRTS_Click);
            // 
            // BtnCheckError
            // 
            this.BtnCheckError.Location = new System.Drawing.Point(655, 116);
            this.BtnCheckError.Name = "BtnCheckError";
            this.BtnCheckError.Size = new System.Drawing.Size(98, 20);
            this.BtnCheckError.TabIndex = 22;
            this.BtnCheckError.Text = "检查Error";
            this.BtnCheckError.UseVisualStyleBackColor = true;
            this.BtnCheckError.Click += new System.EventHandler(this.BtnCheckError_Click);
            // 
            // BtnSetEATT
            // 
            this.BtnSetEATT.Location = new System.Drawing.Point(122, 21);
            this.BtnSetEATT.Name = "BtnSetEATT";
            this.BtnSetEATT.Size = new System.Drawing.Size(98, 20);
            this.BtnSetEATT.TabIndex = 23;
            this.BtnSetEATT.Text = "设置线损";
            this.BtnSetEATT.UseVisualStyleBackColor = true;
            this.BtnSetEATT.Click += new System.EventHandler(this.BtnSetEATT_Click);
            // 
            // NudEATT
            // 
            this.NudEATT.DecimalPlaces = 2;
            this.NudEATT.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.NudEATT.Location = new System.Drawing.Point(53, 20);
            this.NudEATT.Name = "NudEATT";
            this.NudEATT.Size = new System.Drawing.Size(63, 21);
            this.NudEATT.TabIndex = 24;
            this.NudEATT.Value = new decimal(new int[] {
            15,
            0,
            0,
            65536});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 25;
            this.label4.Text = "EATT：";
            // 
            // BtnSetScenario
            // 
            this.BtnSetScenario.Location = new System.Drawing.Point(237, 21);
            this.BtnSetScenario.Name = "BtnSetScenario";
            this.BtnSetScenario.Size = new System.Drawing.Size(98, 20);
            this.BtnSetScenario.TabIndex = 26;
            this.BtnSetScenario.Text = "设置测试场景";
            this.BtnSetScenario.UseVisualStyleBackColor = true;
            this.BtnSetScenario.Click += new System.EventHandler(this.BtnSetScenario_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(344, 25);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 12);
            this.label5.TabIndex = 27;
            this.label5.Text = "Standard:";
            // 
            // comboBoxStandard
            // 
            this.comboBoxStandard.FormattingEnabled = true;
            this.comboBoxStandard.Items.AddRange(new object[] {
            "DSSS-802.11b/g(DSSS)",
            "LOFDM-802.11a/g(OFDM)",
            "HTOFdm-802.11n(OFDM)",
            "POFDm-802.11p",
            "VHTofdm-802.11ac"});
            this.comboBoxStandard.Location = new System.Drawing.Point(409, 19);
            this.comboBoxStandard.Name = "comboBoxStandard";
            this.comboBoxStandard.Size = new System.Drawing.Size(99, 20);
            this.comboBoxStandard.TabIndex = 28;
            // 
            // BtnSetSignal
            // 
            this.BtnSetSignal.Location = new System.Drawing.Point(514, 21);
            this.BtnSetSignal.Name = "BtnSetSignal";
            this.BtnSetSignal.Size = new System.Drawing.Size(98, 20);
            this.BtnSetSignal.TabIndex = 29;
            this.BtnSetSignal.Text = "设置信号标准";
            this.BtnSetSignal.UseVisualStyleBackColor = true;
            this.BtnSetSignal.Click += new System.EventHandler(this.BtnSetSignal_Click);
            // 
            // BtnSetFreq
            // 
            this.BtnSetFreq.Location = new System.Drawing.Point(409, 57);
            this.BtnSetFreq.Name = "BtnSetFreq";
            this.BtnSetFreq.Size = new System.Drawing.Size(129, 20);
            this.BtnSetFreq.TabIndex = 30;
            this.BtnSetFreq.Text = "设置频率和期望功率";
            this.BtnSetFreq.UseVisualStyleBackColor = true;
            this.BtnSetFreq.Click += new System.EventHandler(this.BtnSetFreq_Click);
            // 
            // BtnWlanInit
            // 
            this.BtnWlanInit.Location = new System.Drawing.Point(8, 94);
            this.BtnWlanInit.Name = "BtnWlanInit";
            this.BtnWlanInit.Size = new System.Drawing.Size(63, 20);
            this.BtnWlanInit.TabIndex = 31;
            this.BtnWlanInit.Text = "Init";
            this.BtnWlanInit.UseVisualStyleBackColor = true;
            this.BtnWlanInit.Click += new System.EventHandler(this.BtnWlanInit_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 60);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(89, 12);
            this.label6.TabIndex = 32;
            this.label6.Text = "RF Frequency：";
            // 
            // NudFrequency
            // 
            this.NudFrequency.DecimalPlaces = 6;
            this.NudFrequency.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.NudFrequency.Location = new System.Drawing.Point(100, 58);
            this.NudFrequency.Maximum = new decimal(new int[] {
            24720000,
            0,
            0,
            262144});
            this.NudFrequency.Minimum = new decimal(new int[] {
            24120000,
            0,
            0,
            262144});
            this.NudFrequency.Name = "NudFrequency";
            this.NudFrequency.Size = new System.Drawing.Size(99, 21);
            this.NudFrequency.TabIndex = 33;
            this.NudFrequency.Value = new decimal(new int[] {
            24120000,
            0,
            0,
            262144});
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(205, 61);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(23, 12);
            this.label7.TabIndex = 34;
            this.label7.Text = "MHz";
            // 
            // BtnWlanState
            // 
            this.BtnWlanState.Location = new System.Drawing.Point(77, 94);
            this.BtnWlanState.Name = "BtnWlanState";
            this.BtnWlanState.Size = new System.Drawing.Size(68, 20);
            this.BtnWlanState.TabIndex = 35;
            this.BtnWlanState.Text = "State";
            this.BtnWlanState.UseVisualStyleBackColor = true;
            this.BtnWlanState.Click += new System.EventHandler(this.BtnWlanState_Click);
            // 
            // NudEnPower
            // 
            this.NudEnPower.DecimalPlaces = 6;
            this.NudEnPower.Location = new System.Drawing.Point(301, 58);
            this.NudEnPower.Maximum = new decimal(new int[] {
            55,
            0,
            0,
            0});
            this.NudEnPower.Minimum = new decimal(new int[] {
            55,
            0,
            0,
            -2147483648});
            this.NudEnPower.Name = "NudEnPower";
            this.NudEnPower.Size = new System.Drawing.Size(73, 21);
            this.NudEnPower.TabIndex = 37;
            this.NudEnPower.Value = new decimal(new int[] {
            25,
            0,
            0,
            0});
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(236, 60);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(59, 12);
            this.label8.TabIndex = 36;
            this.label8.Text = "EnPower：";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(380, 61);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(23, 12);
            this.label9.TabIndex = 38;
            this.label9.Text = "dBm";
            // 
            // BtnWlanCur
            // 
            this.BtnWlanCur.Location = new System.Drawing.Point(151, 94);
            this.BtnWlanCur.Name = "BtnWlanCur";
            this.BtnWlanCur.Size = new System.Drawing.Size(69, 20);
            this.BtnWlanCur.TabIndex = 39;
            this.BtnWlanCur.Text = "Current";
            this.BtnWlanCur.UseVisualStyleBackColor = true;
            this.BtnWlanCur.Click += new System.EventHandler(this.BtnWlanCur_Click);
            // 
            // comboBoxTriggerSrc
            // 
            this.comboBoxTriggerSrc.FormattingEnabled = true;
            this.comboBoxTriggerSrc.Items.AddRange(new object[] {
            "IF Power",
            "Free Run"});
            this.comboBoxTriggerSrc.Location = new System.Drawing.Point(319, 94);
            this.comboBoxTriggerSrc.Name = "comboBoxTriggerSrc";
            this.comboBoxTriggerSrc.Size = new System.Drawing.Size(84, 20);
            this.comboBoxTriggerSrc.TabIndex = 41;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(242, 98);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(71, 12);
            this.label10.TabIndex = 40;
            this.label10.Text = "TriggerSrc:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(607, 98);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(17, 12);
            this.label11.TabIndex = 44;
            this.label11.Text = "dB";
            // 
            // NudTriggerThreshold
            // 
            this.NudTriggerThreshold.DecimalPlaces = 2;
            this.NudTriggerThreshold.Location = new System.Drawing.Point(528, 95);
            this.NudTriggerThreshold.Maximum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.NudTriggerThreshold.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            -2147483648});
            this.NudTriggerThreshold.Name = "NudTriggerThreshold";
            this.NudTriggerThreshold.Size = new System.Drawing.Size(73, 21);
            this.NudTriggerThreshold.TabIndex = 43;
            this.NudTriggerThreshold.Value = new decimal(new int[] {
            25,
            0,
            0,
            -2147483648});
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(407, 98);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(113, 12);
            this.label12.TabIndex = 42;
            this.label12.Text = "TriggerThreshold：";
            // 
            // BtnSetTrigger
            // 
            this.BtnSetTrigger.Location = new System.Drawing.Point(630, 94);
            this.BtnSetTrigger.Name = "BtnSetTrigger";
            this.BtnSetTrigger.Size = new System.Drawing.Size(129, 20);
            this.BtnSetTrigger.TabIndex = 45;
            this.BtnSetTrigger.Text = "设置Trigger";
            this.BtnSetTrigger.UseVisualStyleBackColor = true;
            this.BtnSetTrigger.Click += new System.EventHandler(this.BtnSetTrigger_Click);
            // 
            // FormCMW500
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 679);
            this.Controls.Add(this.BtnCheckError);
            this.Controls.Add(this.BtnRTS);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.BtnHWOP);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.BtnIDN);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.BtnOpen);
            this.Controls.Add(this.textBoxCMW500IP);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Name = "FormCMW500";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormCMW500";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormCMW500_FormClosing);
            this.Load += new System.EventHandler(this.FormCMW500_Load);
            this.Shown += new System.EventHandler(this.FormCMW500_Shown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.NudEATT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NudFrequency)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NudEnPower)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NudTriggerThreshold)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RichTextBox richTextBoxTips;
        private System.Windows.Forms.TextBox textBoxCMW500IP;
        private System.Windows.Forms.Button BtnOpen;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBoxSCPICMD;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button BtnSend;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxSCPICMD;
        private System.Windows.Forms.Button BtnSend1;
        private System.Windows.Forms.Button BtnLTESignal;
        private System.Windows.Forms.Button BtnLTEMultiEvaluation;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button BtnIDN;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RichTextBox richTextBoxRun;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button BtnHWOP;
        private System.Windows.Forms.Button BtnRTS;
        private System.Windows.Forms.Button BtnCheckError;
        private System.Windows.Forms.Button BtnSetEATT;
        private System.Windows.Forms.NumericUpDown NudEATT;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button BtnSetScenario;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBoxStandard;
        private System.Windows.Forms.Button BtnSetSignal;
        private System.Windows.Forms.Button BtnSetFreq;
        private System.Windows.Forms.Button BtnWlanInit;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown NudFrequency;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button BtnWlanState;
        private System.Windows.Forms.NumericUpDown NudEnPower;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button BtnWlanCur;
        private System.Windows.Forms.ComboBox comboBoxTriggerSrc;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown NudTriggerThreshold;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button BtnSetTrigger;
    }
}