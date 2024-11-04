namespace RP_C_MK06
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
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxSerialPort = new System.Windows.Forms.ComboBox();
            this.BtnOpenPort = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.richTextBoxLog = new System.Windows.Forms.RichTextBox();
            this.BtnReadADC = new System.Windows.Forms.Button();
            this.numericUpDownDuration = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.BtnClosePort = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxADC1Min = new System.Windows.Forms.TextBox();
            this.textBoxADC1Max = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxADC2Min = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxADC2Max = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxADC1HFV = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxADC2HFV = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDuration)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(38, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "串口号:";
            // 
            // comboBoxSerialPort
            // 
            this.comboBoxSerialPort.FormattingEnabled = true;
            this.comboBoxSerialPort.Location = new System.Drawing.Point(91, 10);
            this.comboBoxSerialPort.Name = "comboBoxSerialPort";
            this.comboBoxSerialPort.Size = new System.Drawing.Size(121, 20);
            this.comboBoxSerialPort.TabIndex = 1;
            this.comboBoxSerialPort.DropDown += new System.EventHandler(this.comboBoxSerialPort_DropDown);
            // 
            // BtnOpenPort
            // 
            this.BtnOpenPort.Location = new System.Drawing.Point(227, 8);
            this.BtnOpenPort.Name = "BtnOpenPort";
            this.BtnOpenPort.Size = new System.Drawing.Size(75, 23);
            this.BtnOpenPort.TabIndex = 2;
            this.BtnOpenPort.Text = "打开串口";
            this.BtnOpenPort.UseVisualStyleBackColor = true;
            this.BtnOpenPort.Click += new System.EventHandler(this.BtnOpenPort_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.richTextBoxLog);
            this.groupBox1.Location = new System.Drawing.Point(13, 44);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(420, 187);
            this.groupBox1.TabIndex = 3;
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
            // BtnReadADC
            // 
            this.BtnReadADC.Location = new System.Drawing.Point(605, 58);
            this.BtnReadADC.Name = "BtnReadADC";
            this.BtnReadADC.Size = new System.Drawing.Size(75, 23);
            this.BtnReadADC.TabIndex = 4;
            this.BtnReadADC.Text = "读取ADC值";
            this.BtnReadADC.UseVisualStyleBackColor = true;
            this.BtnReadADC.Click += new System.EventHandler(this.BtnReadADC_Click);
            // 
            // numericUpDownDuration
            // 
            this.numericUpDownDuration.Location = new System.Drawing.Point(521, 59);
            this.numericUpDownDuration.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numericUpDownDuration.Name = "numericUpDownDuration";
            this.numericUpDownDuration.Size = new System.Drawing.Size(78, 21);
            this.numericUpDownDuration.TabIndex = 5;
            this.numericUpDownDuration.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(450, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "持续时间：";
            // 
            // BtnClosePort
            // 
            this.BtnClosePort.Location = new System.Drawing.Point(308, 8);
            this.BtnClosePort.Name = "BtnClosePort";
            this.BtnClosePort.Size = new System.Drawing.Size(75, 23);
            this.BtnClosePort.TabIndex = 7;
            this.BtnClosePort.Text = "关闭串口";
            this.BtnClosePort.UseVisualStyleBackColor = true;
            this.BtnClosePort.Click += new System.EventHandler(this.BtnClosePort_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(450, 99);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "ADC1 Min：";
            // 
            // textBoxADC1Min
            // 
            this.textBoxADC1Min.Location = new System.Drawing.Point(521, 96);
            this.textBoxADC1Min.Name = "textBoxADC1Min";
            this.textBoxADC1Min.Size = new System.Drawing.Size(100, 21);
            this.textBoxADC1Min.TabIndex = 9;
            // 
            // textBoxADC1Max
            // 
            this.textBoxADC1Max.Location = new System.Drawing.Point(521, 123);
            this.textBoxADC1Max.Name = "textBoxADC1Max";
            this.textBoxADC1Max.Size = new System.Drawing.Size(100, 21);
            this.textBoxADC1Max.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(450, 126);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 10;
            this.label4.Text = "ADC1 Max：";
            // 
            // textBoxADC2Min
            // 
            this.textBoxADC2Min.Location = new System.Drawing.Point(521, 190);
            this.textBoxADC2Min.Name = "textBoxADC2Min";
            this.textBoxADC2Min.Size = new System.Drawing.Size(100, 21);
            this.textBoxADC2Min.TabIndex = 13;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(450, 193);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 12;
            this.label5.Text = "ADC2 Min：";
            // 
            // textBoxADC2Max
            // 
            this.textBoxADC2Max.Location = new System.Drawing.Point(521, 217);
            this.textBoxADC2Max.Name = "textBoxADC2Max";
            this.textBoxADC2Max.Size = new System.Drawing.Size(100, 21);
            this.textBoxADC2Max.TabIndex = 15;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(450, 220);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 14;
            this.label6.Text = "ADC2 Max：";
            // 
            // textBoxADC1HFV
            // 
            this.textBoxADC1HFV.Location = new System.Drawing.Point(575, 150);
            this.textBoxADC1HFV.Name = "textBoxADC1HFV";
            this.textBoxADC1HFV.Size = new System.Drawing.Size(100, 21);
            this.textBoxADC1HFV.TabIndex = 17;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(450, 153);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(119, 12);
            this.label7.TabIndex = 16;
            this.label7.Text = "ADC1 MostFrequent：";
            // 
            // textBoxADC2HFV
            // 
            this.textBoxADC2HFV.Location = new System.Drawing.Point(575, 244);
            this.textBoxADC2HFV.Name = "textBoxADC2HFV";
            this.textBoxADC2HFV.Size = new System.Drawing.Size(100, 21);
            this.textBoxADC2HFV.TabIndex = 19;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(450, 247);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(119, 12);
            this.label8.TabIndex = 18;
            this.label8.Text = "ADC2 MostFrequent：";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(727, 297);
            this.Controls.Add(this.textBoxADC2HFV);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.textBoxADC1HFV);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textBoxADC2Max);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBoxADC2Min);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBoxADC1Max);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxADC1Min);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.BtnClosePort);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.numericUpDownDuration);
            this.Controls.Add(this.BtnReadADC);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.BtnOpenPort);
            this.Controls.Add(this.comboBoxSerialPort);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RP-C-MK06调试工具";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDuration)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxSerialPort;
        private System.Windows.Forms.Button BtnOpenPort;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RichTextBox richTextBoxLog;
        private System.Windows.Forms.Button BtnReadADC;
        private System.Windows.Forms.NumericUpDown numericUpDownDuration;
        private System.Windows.Forms.Label label2;
        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.Button BtnClosePort;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxADC1Min;
        private System.Windows.Forms.TextBox textBoxADC1Max;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxADC2Min;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxADC2Max;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxADC1HFV;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxADC2HFV;
        private System.Windows.Forms.Label label8;
    }
}

