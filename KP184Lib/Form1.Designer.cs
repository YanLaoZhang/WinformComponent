namespace KP184Lib
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
            this.BtnLoadON = new System.Windows.Forms.Button();
            this.BtnLoadOFF = new System.Windows.Forms.Button();
            this.BtnLoadMode = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxMode = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.numericUpDownVol = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.BtnCVSetting = new System.Windows.Forms.Button();
            this.BtnCCSetting = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.numericUpDownCur = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.BtnCRSetting = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.numericUpDownResis = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.BtnCWSetting = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.numericUpDownPower = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.BtnReadVolAndCur = new System.Windows.Forms.Button();
            this.textBoxVol = new System.Windows.Forms.TextBox();
            this.textBoxCur = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.comboBoxSerialPort = new System.Windows.Forms.ComboBox();
            this.comboBoxBaudRate = new System.Windows.Forms.ComboBox();
            this.label16 = new System.Windows.Forms.Label();
            this.numericUpDownADD = new System.Windows.Forms.NumericUpDown();
            this.label17 = new System.Windows.Forms.Label();
            this.BtnReadVol = new System.Windows.Forms.Button();
            this.BtnReadCur = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownVol)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCur)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownResis)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPower)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownADD)).BeginInit();
            this.SuspendLayout();
            // 
            // BtnLoadON
            // 
            this.BtnLoadON.Location = new System.Drawing.Point(12, 63);
            this.BtnLoadON.Name = "BtnLoadON";
            this.BtnLoadON.Size = new System.Drawing.Size(124, 38);
            this.BtnLoadON.TabIndex = 0;
            this.BtnLoadON.Text = "打开";
            this.BtnLoadON.UseVisualStyleBackColor = true;
            this.BtnLoadON.Click += new System.EventHandler(this.BtnLoadON_Click);
            // 
            // BtnLoadOFF
            // 
            this.BtnLoadOFF.Location = new System.Drawing.Point(142, 63);
            this.BtnLoadOFF.Name = "BtnLoadOFF";
            this.BtnLoadOFF.Size = new System.Drawing.Size(124, 38);
            this.BtnLoadOFF.TabIndex = 1;
            this.BtnLoadOFF.Text = "关闭";
            this.BtnLoadOFF.UseVisualStyleBackColor = true;
            this.BtnLoadOFF.Click += new System.EventHandler(this.BtnLoadOFF_Click);
            // 
            // BtnLoadMode
            // 
            this.BtnLoadMode.Location = new System.Drawing.Point(418, 63);
            this.BtnLoadMode.Name = "BtnLoadMode";
            this.BtnLoadMode.Size = new System.Drawing.Size(124, 38);
            this.BtnLoadMode.TabIndex = 2;
            this.BtnLoadMode.Text = "设置模式";
            this.BtnLoadMode.UseVisualStyleBackColor = true;
            this.BtnLoadMode.Click += new System.EventHandler(this.BtnLoadMode_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(289, 76);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "模式：";
            // 
            // comboBoxMode
            // 
            this.comboBoxMode.FormattingEnabled = true;
            this.comboBoxMode.Items.AddRange(new object[] {
            "0-CV",
            "1-CC",
            "2-CR",
            "3-CW"});
            this.comboBoxMode.Location = new System.Drawing.Point(336, 73);
            this.comboBoxMode.Name = "comboBoxMode";
            this.comboBoxMode.Size = new System.Drawing.Size(76, 20);
            this.comboBoxMode.TabIndex = 4;
            this.comboBoxMode.Text = "0-CV";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 132);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "电压值：";
            // 
            // numericUpDownVol
            // 
            this.numericUpDownVol.Location = new System.Drawing.Point(71, 130);
            this.numericUpDownVol.Maximum = new decimal(new int[] {
            150000,
            0,
            0,
            0});
            this.numericUpDownVol.Name = "numericUpDownVol";
            this.numericUpDownVol.Size = new System.Drawing.Size(120, 21);
            this.numericUpDownVol.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(197, 139);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "mV";
            // 
            // BtnCVSetting
            // 
            this.BtnCVSetting.Location = new System.Drawing.Point(233, 119);
            this.BtnCVSetting.Name = "BtnCVSetting";
            this.BtnCVSetting.Size = new System.Drawing.Size(124, 38);
            this.BtnCVSetting.TabIndex = 8;
            this.BtnCVSetting.Text = "设置CV电压";
            this.BtnCVSetting.UseVisualStyleBackColor = true;
            this.BtnCVSetting.Click += new System.EventHandler(this.BtnCVSetting_Click);
            // 
            // BtnCCSetting
            // 
            this.BtnCCSetting.Location = new System.Drawing.Point(233, 172);
            this.BtnCCSetting.Name = "BtnCCSetting";
            this.BtnCCSetting.Size = new System.Drawing.Size(124, 38);
            this.BtnCCSetting.TabIndex = 12;
            this.BtnCCSetting.Text = "设置CC电流";
            this.BtnCCSetting.UseVisualStyleBackColor = true;
            this.BtnCCSetting.Click += new System.EventHandler(this.BtnCCSetting_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(197, 192);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 12);
            this.label4.TabIndex = 11;
            this.label4.Text = "mA";
            // 
            // numericUpDownCur
            // 
            this.numericUpDownCur.Location = new System.Drawing.Point(71, 183);
            this.numericUpDownCur.Maximum = new decimal(new int[] {
            30000,
            0,
            0,
            0});
            this.numericUpDownCur.Name = "numericUpDownCur";
            this.numericUpDownCur.Size = new System.Drawing.Size(120, 21);
            this.numericUpDownCur.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 185);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 9;
            this.label5.Text = "电流值：";
            // 
            // BtnCRSetting
            // 
            this.BtnCRSetting.Location = new System.Drawing.Point(233, 230);
            this.BtnCRSetting.Name = "BtnCRSetting";
            this.BtnCRSetting.Size = new System.Drawing.Size(124, 38);
            this.BtnCRSetting.TabIndex = 16;
            this.BtnCRSetting.Text = "设置CR电阻";
            this.BtnCRSetting.UseVisualStyleBackColor = true;
            this.BtnCRSetting.Click += new System.EventHandler(this.BtnCRSetting_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(197, 250);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(17, 12);
            this.label6.TabIndex = 15;
            this.label6.Text = "Ω";
            // 
            // numericUpDownResis
            // 
            this.numericUpDownResis.Location = new System.Drawing.Point(71, 241);
            this.numericUpDownResis.Maximum = new decimal(new int[] {
            80000,
            0,
            0,
            0});
            this.numericUpDownResis.Name = "numericUpDownResis";
            this.numericUpDownResis.Size = new System.Drawing.Size(120, 21);
            this.numericUpDownResis.TabIndex = 14;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 243);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 13;
            this.label7.Text = "电阻值：";
            // 
            // BtnCWSetting
            // 
            this.BtnCWSetting.Location = new System.Drawing.Point(233, 289);
            this.BtnCWSetting.Name = "BtnCWSetting";
            this.BtnCWSetting.Size = new System.Drawing.Size(124, 38);
            this.BtnCWSetting.TabIndex = 20;
            this.BtnCWSetting.Text = "设置CW功率";
            this.BtnCWSetting.UseVisualStyleBackColor = true;
            this.BtnCWSetting.Click += new System.EventHandler(this.BtnCWSetting_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(197, 309);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 12);
            this.label8.TabIndex = 19;
            this.label8.Text = "0.1W";
            // 
            // numericUpDownPower
            // 
            this.numericUpDownPower.Location = new System.Drawing.Point(71, 300);
            this.numericUpDownPower.Maximum = new decimal(new int[] {
            2500,
            0,
            0,
            0});
            this.numericUpDownPower.Name = "numericUpDownPower";
            this.numericUpDownPower.Size = new System.Drawing.Size(120, 21);
            this.numericUpDownPower.TabIndex = 18;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 302);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 12);
            this.label9.TabIndex = 17;
            this.label9.Text = "功率值：";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(450, 230);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(113, 12);
            this.label10.TabIndex = 21;
            this.label10.Text = "读取实际电压电流：";
            // 
            // BtnReadVolAndCur
            // 
            this.BtnReadVolAndCur.Location = new System.Drawing.Point(452, 172);
            this.BtnReadVolAndCur.Name = "BtnReadVolAndCur";
            this.BtnReadVolAndCur.Size = new System.Drawing.Size(124, 38);
            this.BtnReadVolAndCur.TabIndex = 22;
            this.BtnReadVolAndCur.Text = "读取电压电流";
            this.BtnReadVolAndCur.UseVisualStyleBackColor = true;
            this.BtnReadVolAndCur.Click += new System.EventHandler(this.BtnReadVolAndCur_Click);
            // 
            // textBoxVol
            // 
            this.textBoxVol.Location = new System.Drawing.Point(509, 259);
            this.textBoxVol.Name = "textBoxVol";
            this.textBoxVol.Size = new System.Drawing.Size(100, 21);
            this.textBoxVol.TabIndex = 23;
            // 
            // textBoxCur
            // 
            this.textBoxCur.Location = new System.Drawing.Point(509, 300);
            this.textBoxCur.Name = "textBoxCur";
            this.textBoxCur.Size = new System.Drawing.Size(100, 21);
            this.textBoxCur.TabIndex = 24;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(450, 262);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(53, 12);
            this.label11.TabIndex = 25;
            this.label11.Text = "电压值：";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(450, 303);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(53, 12);
            this.label12.TabIndex = 26;
            this.label12.Text = "电流值：";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(615, 268);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(17, 12);
            this.label13.TabIndex = 27;
            this.label13.Text = "mV";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(615, 309);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(17, 12);
            this.label14.TabIndex = 28;
            this.label14.Text = "mA";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(12, 22);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(41, 12);
            this.label15.TabIndex = 29;
            this.label15.Text = "串口：";
            // 
            // comboBoxSerialPort
            // 
            this.comboBoxSerialPort.FormattingEnabled = true;
            this.comboBoxSerialPort.Location = new System.Drawing.Point(59, 19);
            this.comboBoxSerialPort.Name = "comboBoxSerialPort";
            this.comboBoxSerialPort.Size = new System.Drawing.Size(121, 20);
            this.comboBoxSerialPort.TabIndex = 30;
            // 
            // comboBoxBaudRate
            // 
            this.comboBoxBaudRate.FormattingEnabled = true;
            this.comboBoxBaudRate.Items.AddRange(new object[] {
            "2400",
            "4800",
            "9600",
            "18200",
            "38400",
            "57600",
            "115200"});
            this.comboBoxBaudRate.Location = new System.Drawing.Point(268, 19);
            this.comboBoxBaudRate.Name = "comboBoxBaudRate";
            this.comboBoxBaudRate.Size = new System.Drawing.Size(121, 20);
            this.comboBoxBaudRate.TabIndex = 32;
            this.comboBoxBaudRate.Text = "115200";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(209, 22);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(53, 12);
            this.label16.TabIndex = 31;
            this.label16.Text = "波特率：";
            // 
            // numericUpDownADD
            // 
            this.numericUpDownADD.Location = new System.Drawing.Point(491, 19);
            this.numericUpDownADD.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.numericUpDownADD.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownADD.Name = "numericUpDownADD";
            this.numericUpDownADD.Size = new System.Drawing.Size(120, 21);
            this.numericUpDownADD.TabIndex = 34;
            this.numericUpDownADD.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(420, 24);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(65, 12);
            this.label17.TabIndex = 33;
            this.label17.Text = "设备地址：";
            // 
            // BtnReadVol
            // 
            this.BtnReadVol.Location = new System.Drawing.Point(452, 339);
            this.BtnReadVol.Name = "BtnReadVol";
            this.BtnReadVol.Size = new System.Drawing.Size(124, 38);
            this.BtnReadVol.TabIndex = 35;
            this.BtnReadVol.Text = "读取电压";
            this.BtnReadVol.UseVisualStyleBackColor = true;
            this.BtnReadVol.Visible = false;
            this.BtnReadVol.Click += new System.EventHandler(this.BtnReadVol_Click);
            // 
            // BtnReadCur
            // 
            this.BtnReadCur.Location = new System.Drawing.Point(582, 339);
            this.BtnReadCur.Name = "BtnReadCur";
            this.BtnReadCur.Size = new System.Drawing.Size(124, 38);
            this.BtnReadCur.TabIndex = 36;
            this.BtnReadCur.Text = "读取电流";
            this.BtnReadCur.UseVisualStyleBackColor = true;
            this.BtnReadCur.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 427);
            this.Controls.Add(this.BtnReadCur);
            this.Controls.Add(this.BtnReadVol);
            this.Controls.Add(this.numericUpDownADD);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.comboBoxBaudRate);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.comboBoxSerialPort);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.textBoxCur);
            this.Controls.Add(this.textBoxVol);
            this.Controls.Add(this.BtnReadVolAndCur);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.BtnCWSetting);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.numericUpDownPower);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.BtnCRSetting);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.numericUpDownResis);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.BtnCCSetting);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.numericUpDownCur);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.BtnCVSetting);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.numericUpDownVol);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBoxMode);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BtnLoadMode);
            this.Controls.Add(this.BtnLoadOFF);
            this.Controls.Add(this.BtnLoadON);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "KUNKIN KP184直流电子负载";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownVol)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCur)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownResis)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPower)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownADD)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnLoadON;
        private System.Windows.Forms.Button BtnLoadOFF;
        private System.Windows.Forms.Button BtnLoadMode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxMode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericUpDownVol;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button BtnCVSetting;
        private System.Windows.Forms.Button BtnCCSetting;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numericUpDownCur;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button BtnCRSetting;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numericUpDownResis;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button BtnCWSetting;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown numericUpDownPower;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button BtnReadVolAndCur;
        private System.Windows.Forms.TextBox textBoxVol;
        private System.Windows.Forms.TextBox textBoxCur;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ComboBox comboBoxSerialPort;
        private System.Windows.Forms.ComboBox comboBoxBaudRate;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.NumericUpDown numericUpDownADD;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Button BtnReadVol;
        private System.Windows.Forms.Button BtnReadCur;
    }
}

