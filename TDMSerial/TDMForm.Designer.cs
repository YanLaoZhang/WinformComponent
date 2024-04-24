namespace TDMSerialLib
{
    partial class TDMForm
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
            this.components = new System.ComponentModel.Container();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDownN = new System.Windows.Forms.NumericUpDown();
            this.textBoxValue = new System.Windows.Forms.TextBox();
            this.BtnRead = new System.Windows.Forms.Button();
            this.BtnInit = new System.Windows.Forms.Button();
            this.BtnOpenPort = new System.Windows.Forms.Button();
            this.labelRefreshPort = new System.Windows.Forms.Label();
            this.comboBoxCurPort = new System.Windows.Forms.ComboBox();
            this.serialPortRelay = new System.IO.Ports.SerialPort(this.components);
            this.groupBox8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownN)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.label1);
            this.groupBox8.Controls.Add(this.numericUpDownN);
            this.groupBox8.Controls.Add(this.textBoxValue);
            this.groupBox8.Controls.Add(this.BtnRead);
            this.groupBox8.Controls.Add(this.BtnInit);
            this.groupBox8.Controls.Add(this.BtnOpenPort);
            this.groupBox8.Controls.Add(this.labelRefreshPort);
            this.groupBox8.Controls.Add(this.comboBoxCurPort);
            this.groupBox8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox8.Font = new System.Drawing.Font("宋体", 14F);
            this.groupBox8.Location = new System.Drawing.Point(0, 0);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(547, 308);
            this.groupBox8.TabIndex = 1;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "TDM表调试";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(224, 43);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 19);
            this.label1.TabIndex = 77;
            this.label1.Text = "小数位数";
            // 
            // numericUpDownN
            // 
            this.numericUpDownN.Location = new System.Drawing.Point(316, 38);
            this.numericUpDownN.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.numericUpDownN.Name = "numericUpDownN";
            this.numericUpDownN.Size = new System.Drawing.Size(62, 29);
            this.numericUpDownN.TabIndex = 76;
            this.numericUpDownN.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // textBoxValue
            // 
            this.textBoxValue.Font = new System.Drawing.Font("宋体", 18F);
            this.textBoxValue.Location = new System.Drawing.Point(178, 160);
            this.textBoxValue.Name = "textBoxValue";
            this.textBoxValue.Size = new System.Drawing.Size(124, 35);
            this.textBoxValue.TabIndex = 75;
            // 
            // BtnRead
            // 
            this.BtnRead.Location = new System.Drawing.Point(62, 155);
            this.BtnRead.Name = "BtnRead";
            this.BtnRead.Size = new System.Drawing.Size(110, 44);
            this.BtnRead.TabIndex = 74;
            this.BtnRead.Text = "读取数据";
            this.BtnRead.UseVisualStyleBackColor = true;
            this.BtnRead.Click += new System.EventHandler(this.BtnRead_Click);
            // 
            // BtnInit
            // 
            this.BtnInit.Location = new System.Drawing.Point(62, 91);
            this.BtnInit.Name = "BtnInit";
            this.BtnInit.Size = new System.Drawing.Size(110, 44);
            this.BtnInit.TabIndex = 73;
            this.BtnInit.Text = "重置";
            this.BtnInit.UseVisualStyleBackColor = true;
            this.BtnInit.Click += new System.EventHandler(this.BtnInit_Click);
            // 
            // BtnOpenPort
            // 
            this.BtnOpenPort.Location = new System.Drawing.Point(396, 33);
            this.BtnOpenPort.Margin = new System.Windows.Forms.Padding(4);
            this.BtnOpenPort.Name = "BtnOpenPort";
            this.BtnOpenPort.Size = new System.Drawing.Size(138, 39);
            this.BtnOpenPort.TabIndex = 72;
            this.BtnOpenPort.Text = "打开串口";
            this.BtnOpenPort.UseVisualStyleBackColor = true;
            this.BtnOpenPort.Click += new System.EventHandler(this.BtnOpenPort_Click);
            // 
            // labelRefreshPort
            // 
            this.labelRefreshPort.AutoSize = true;
            this.labelRefreshPort.Location = new System.Drawing.Point(7, 43);
            this.labelRefreshPort.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelRefreshPort.Name = "labelRefreshPort";
            this.labelRefreshPort.Size = new System.Drawing.Size(47, 19);
            this.labelRefreshPort.TabIndex = 71;
            this.labelRefreshPort.Text = "串口";
            this.labelRefreshPort.Click += new System.EventHandler(this.labelRefreshPort_Click);
            // 
            // comboBoxCurPort
            // 
            this.comboBoxCurPort.FormattingEnabled = true;
            this.comboBoxCurPort.Location = new System.Drawing.Point(62, 40);
            this.comboBoxCurPort.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxCurPort.Name = "comboBoxCurPort";
            this.comboBoxCurPort.Size = new System.Drawing.Size(154, 27);
            this.comboBoxCurPort.TabIndex = 70;
            this.comboBoxCurPort.DropDown += new System.EventHandler(this.comboBoxCurPort_DropDown);
            // 
            // TDMForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(547, 308);
            this.Controls.Add(this.groupBox8);
            this.Name = "TDMForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RelayForm";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.RelayForm_Load);
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownN)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.Button BtnOpenPort;
        private System.Windows.Forms.Label labelRefreshPort;
        private System.Windows.Forms.ComboBox comboBoxCurPort;
        private System.IO.Ports.SerialPort serialPortRelay;
        private System.Windows.Forms.Button BtnInit;
        private System.Windows.Forms.Button BtnRead;
        private System.Windows.Forms.TextBox textBoxValue;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDownN;
    }
}