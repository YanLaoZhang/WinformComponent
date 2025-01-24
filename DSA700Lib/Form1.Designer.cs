namespace DSA700Lib
{
    partial class Form1
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
            this.labelDevices = new System.Windows.Forms.Label();
            this.comboBoxDevices = new System.Windows.Forms.ComboBox();
            this.comboBoxCMD = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.BtnSendCMD = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.BtnWRTIE = new System.Windows.Forms.Button();
            this.BtnSetFreq = new System.Windows.Forms.Button();
            this.BtnMaxHold = new System.Windows.Forms.Button();
            this.BtnTrackingOFF = new System.Windows.Forms.Button();
            this.BtnMaxPeakSearch = new System.Windows.Forms.Button();
            this.BtnPeakSearch = new System.Windows.Forms.Button();
            this.BtnReadPower = new System.Windows.Forms.Button();
            this.BtnReadFreq = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelDevices
            // 
            this.labelDevices.AutoSize = true;
            this.labelDevices.Location = new System.Drawing.Point(13, 15);
            this.labelDevices.Name = "labelDevices";
            this.labelDevices.Size = new System.Drawing.Size(59, 12);
            this.labelDevices.TabIndex = 0;
            this.labelDevices.Text = "可用设备:";
            this.labelDevices.Click += new System.EventHandler(this.labelDevices_Click);
            // 
            // comboBoxDevices
            // 
            this.comboBoxDevices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDevices.FormattingEnabled = true;
            this.comboBoxDevices.Location = new System.Drawing.Point(78, 12);
            this.comboBoxDevices.Name = "comboBoxDevices";
            this.comboBoxDevices.Size = new System.Drawing.Size(300, 20);
            this.comboBoxDevices.TabIndex = 1;
            // 
            // comboBoxCMD
            // 
            this.comboBoxCMD.FormattingEnabled = true;
            this.comboBoxCMD.Items.AddRange(new object[] {
            ":SENSe:FREQuency:CENTer 433950000",
            ":SENSe:SIGCapture:STATe ON",
            ":SENSe:SIGCapture:STATe?",
            ":CALCulate:MARKer1:CPEak:STATe ON",
            ":SENSe:SIGCapture:2FSK:STATe ON",
            ":SENSe:SIGCapture:2FSK:MAXHold:STATe ON",
            ":TRACe1:MODE MAXHold",
            ":TRACe1:MODE WRITe",
            ":CALCulate:MARKer:TRACking:STATe ON",
            ":CALCulate:MARKer:TRACking:STATe OFF",
            ":CALCulate:MARKer1:PEAK:SEARch:MODE MAXimum",
            ":CALCulate:MARKer1:MAXimum:MAX",
            ":CALCulate:MARKer1:X?",
            ":CALCulate:MARKer1:Y?",
            ":SENSe:SIGCapture:2FSK:RESet"});
            this.comboBoxCMD.Location = new System.Drawing.Point(78, 68);
            this.comboBoxCMD.Name = "comboBoxCMD";
            this.comboBoxCMD.Size = new System.Drawing.Size(300, 20);
            this.comboBoxCMD.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "发送指令:";
            // 
            // BtnSendCMD
            // 
            this.BtnSendCMD.Location = new System.Drawing.Point(384, 68);
            this.BtnSendCMD.Name = "BtnSendCMD";
            this.BtnSendCMD.Size = new System.Drawing.Size(75, 23);
            this.BtnSendCMD.TabIndex = 4;
            this.BtnSendCMD.Text = "发送";
            this.BtnSendCMD.UseVisualStyleBackColor = true;
            this.BtnSendCMD.Click += new System.EventHandler(this.BtnSendCMD_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(78, 95);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.richTextBox1.Size = new System.Drawing.Size(480, 123);
            this.richTextBox1.TabIndex = 5;
            this.richTextBox1.Text = "";
            // 
            // BtnWRTIE
            // 
            this.BtnWRTIE.Location = new System.Drawing.Point(175, 253);
            this.BtnWRTIE.Name = "BtnWRTIE";
            this.BtnWRTIE.Size = new System.Drawing.Size(91, 23);
            this.BtnWRTIE.TabIndex = 6;
            this.BtnWRTIE.Text = "清除写入";
            this.BtnWRTIE.UseVisualStyleBackColor = true;
            this.BtnWRTIE.Click += new System.EventHandler(this.BtnWRTIE_Click);
            // 
            // BtnSetFreq
            // 
            this.BtnSetFreq.Location = new System.Drawing.Point(78, 224);
            this.BtnSetFreq.Name = "BtnSetFreq";
            this.BtnSetFreq.Size = new System.Drawing.Size(91, 23);
            this.BtnSetFreq.TabIndex = 7;
            this.BtnSetFreq.Text = "设置中心频率";
            this.BtnSetFreq.UseVisualStyleBackColor = true;
            this.BtnSetFreq.Click += new System.EventHandler(this.BtnSetFreq_Click);
            // 
            // BtnMaxHold
            // 
            this.BtnMaxHold.Location = new System.Drawing.Point(78, 253);
            this.BtnMaxHold.Name = "BtnMaxHold";
            this.BtnMaxHold.Size = new System.Drawing.Size(91, 23);
            this.BtnMaxHold.TabIndex = 8;
            this.BtnMaxHold.Text = "最大保持";
            this.BtnMaxHold.UseVisualStyleBackColor = true;
            this.BtnMaxHold.Click += new System.EventHandler(this.BtnMaxHold_Click);
            // 
            // BtnTrackingOFF
            // 
            this.BtnTrackingOFF.Location = new System.Drawing.Point(175, 224);
            this.BtnTrackingOFF.Name = "BtnTrackingOFF";
            this.BtnTrackingOFF.Size = new System.Drawing.Size(91, 23);
            this.BtnTrackingOFF.TabIndex = 9;
            this.BtnTrackingOFF.Text = "关闭信号追踪";
            this.BtnTrackingOFF.UseVisualStyleBackColor = true;
            this.BtnTrackingOFF.Click += new System.EventHandler(this.BtnTrackingOFF_Click);
            // 
            // BtnMaxPeakSearch
            // 
            this.BtnMaxPeakSearch.Location = new System.Drawing.Point(78, 282);
            this.BtnMaxPeakSearch.Name = "BtnMaxPeakSearch";
            this.BtnMaxPeakSearch.Size = new System.Drawing.Size(105, 23);
            this.BtnMaxPeakSearch.TabIndex = 10;
            this.BtnMaxPeakSearch.Text = "最大值峰值搜索";
            this.BtnMaxPeakSearch.UseVisualStyleBackColor = true;
            this.BtnMaxPeakSearch.Click += new System.EventHandler(this.BtnMaxPeakSearch_Click);
            // 
            // BtnPeakSearch
            // 
            this.BtnPeakSearch.Location = new System.Drawing.Point(189, 282);
            this.BtnPeakSearch.Name = "BtnPeakSearch";
            this.BtnPeakSearch.Size = new System.Drawing.Size(131, 23);
            this.BtnPeakSearch.TabIndex = 11;
            this.BtnPeakSearch.Text = "执行一次峰值搜索";
            this.BtnPeakSearch.UseVisualStyleBackColor = true;
            this.BtnPeakSearch.Click += new System.EventHandler(this.BtnPeakSearch_Click);
            // 
            // BtnReadPower
            // 
            this.BtnReadPower.Location = new System.Drawing.Point(78, 311);
            this.BtnReadPower.Name = "BtnReadPower";
            this.BtnReadPower.Size = new System.Drawing.Size(105, 23);
            this.BtnReadPower.TabIndex = 12;
            this.BtnReadPower.Text = "读取光标1功率";
            this.BtnReadPower.UseVisualStyleBackColor = true;
            this.BtnReadPower.Click += new System.EventHandler(this.BtnReadPower_Click);
            // 
            // BtnReadFreq
            // 
            this.BtnReadFreq.Location = new System.Drawing.Point(189, 311);
            this.BtnReadFreq.Name = "BtnReadFreq";
            this.BtnReadFreq.Size = new System.Drawing.Size(105, 23);
            this.BtnReadFreq.TabIndex = 13;
            this.BtnReadFreq.Text = "读取光标1频率";
            this.BtnReadFreq.UseVisualStyleBackColor = true;
            this.BtnReadFreq.Click += new System.EventHandler(this.BtnReadFreq_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(588, 350);
            this.Controls.Add(this.BtnReadFreq);
            this.Controls.Add(this.BtnReadPower);
            this.Controls.Add(this.BtnPeakSearch);
            this.Controls.Add(this.BtnMaxPeakSearch);
            this.Controls.Add(this.BtnTrackingOFF);
            this.Controls.Add(this.BtnMaxHold);
            this.Controls.Add(this.BtnSetFreq);
            this.Controls.Add(this.BtnWRTIE);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.BtnSendCMD);
            this.Controls.Add(this.comboBoxCMD);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxDevices);
            this.Controls.Add(this.labelDevices);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelDevices;
        private System.Windows.Forms.ComboBox comboBoxDevices;
        private System.Windows.Forms.ComboBox comboBoxCMD;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button BtnSendCMD;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button BtnWRTIE;
        private System.Windows.Forms.Button BtnSetFreq;
        private System.Windows.Forms.Button BtnMaxHold;
        private System.Windows.Forms.Button BtnTrackingOFF;
        private System.Windows.Forms.Button BtnMaxPeakSearch;
        private System.Windows.Forms.Button BtnPeakSearch;
        private System.Windows.Forms.Button BtnReadPower;
        private System.Windows.Forms.Button BtnReadFreq;
    }
}