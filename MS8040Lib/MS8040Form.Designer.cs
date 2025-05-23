﻿namespace MS8040Lib
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
            this.labelRefreshPort = new System.Windows.Forms.Label();
            this.comboBoxCurPort = new System.Windows.Forms.ComboBox();
            this.serialPortMS8040 = new System.IO.Ports.SerialPort(this.components);
            this.labelUnit = new System.Windows.Forms.Label();
            this.textBoxRead = new System.Windows.Forms.TextBox();
            this.BtnRead = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.richTextBoxMessage = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDownDuration = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.BtnStart = new System.Windows.Forms.Button();
            this.BtnStop = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDuration)).BeginInit();
            this.SuspendLayout();
            // 
            // labelRefreshPort
            // 
            this.labelRefreshPort.AutoSize = true;
            this.labelRefreshPort.Location = new System.Drawing.Point(13, 32);
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
            this.comboBoxCurPort.Location = new System.Drawing.Point(50, 23);
            this.comboBoxCurPort.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxCurPort.Name = "comboBoxCurPort";
            this.comboBoxCurPort.Size = new System.Drawing.Size(160, 27);
            this.comboBoxCurPort.TabIndex = 36;
            this.comboBoxCurPort.DropDown += new System.EventHandler(this.comboBoxCurPort_DropDown);
            // 
            // labelUnit
            // 
            this.labelUnit.AutoSize = true;
            this.labelUnit.Location = new System.Drawing.Point(681, 35);
            this.labelUnit.Name = "labelUnit";
            this.labelUnit.Size = new System.Drawing.Size(11, 12);
            this.labelUnit.TabIndex = 41;
            this.labelUnit.Text = "-";
            // 
            // textBoxRead
            // 
            this.textBoxRead.Font = new System.Drawing.Font("宋体", 14F);
            this.textBoxRead.Location = new System.Drawing.Point(540, 23);
            this.textBoxRead.Name = "textBoxRead";
            this.textBoxRead.Size = new System.Drawing.Size(135, 29);
            this.textBoxRead.TabIndex = 40;
            // 
            // BtnRead
            // 
            this.BtnRead.Font = new System.Drawing.Font("宋体", 10F);
            this.BtnRead.Location = new System.Drawing.Point(389, 20);
            this.BtnRead.Name = "BtnRead";
            this.BtnRead.Size = new System.Drawing.Size(145, 35);
            this.BtnRead.TabIndex = 39;
            this.BtnRead.Text = "读取表的测量值";
            this.BtnRead.UseVisualStyleBackColor = true;
            this.BtnRead.Click += new System.EventHandler(this.BtnRead_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("宋体", 12F);
            this.label12.Location = new System.Drawing.Point(46, 61);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(87, 16);
            this.label12.TabIndex = 43;
            this.label12.Text = "执行情况：";
            // 
            // richTextBoxMessage
            // 
            this.richTextBoxMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.richTextBoxMessage.BackColor = System.Drawing.Color.Black;
            this.richTextBoxMessage.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBoxMessage.ForeColor = System.Drawing.SystemColors.Window;
            this.richTextBoxMessage.Location = new System.Drawing.Point(139, 61);
            this.richTextBoxMessage.Name = "richTextBoxMessage";
            this.richTextBoxMessage.ReadOnly = true;
            this.richTextBoxMessage.Size = new System.Drawing.Size(560, 213);
            this.richTextBoxMessage.TabIndex = 42;
            this.richTextBoxMessage.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(218, 32);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 44;
            this.label1.Text = "读取时长";
            // 
            // numericUpDownDuration
            // 
            this.numericUpDownDuration.DecimalPlaces = 3;
            this.numericUpDownDuration.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.numericUpDownDuration.Location = new System.Drawing.Point(278, 28);
            this.numericUpDownDuration.Name = "numericUpDownDuration";
            this.numericUpDownDuration.Size = new System.Drawing.Size(81, 21);
            this.numericUpDownDuration.TabIndex = 45;
            this.numericUpDownDuration.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(365, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(11, 12);
            this.label2.TabIndex = 46;
            this.label2.Text = "s";
            // 
            // BtnStart
            // 
            this.BtnStart.AccessibleRole = System.Windows.Forms.AccessibleRole.SplitButton;
            this.BtnStart.Font = new System.Drawing.Font("宋体", 10F);
            this.BtnStart.Location = new System.Drawing.Point(12, 102);
            this.BtnStart.Name = "BtnStart";
            this.BtnStart.Size = new System.Drawing.Size(121, 35);
            this.BtnStart.TabIndex = 47;
            this.BtnStart.Text = "开始采集";
            this.BtnStart.UseVisualStyleBackColor = true;
            this.BtnStart.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // BtnStop
            // 
            this.BtnStop.AccessibleRole = System.Windows.Forms.AccessibleRole.SplitButton;
            this.BtnStop.Font = new System.Drawing.Font("宋体", 10F);
            this.BtnStop.Location = new System.Drawing.Point(12, 143);
            this.BtnStop.Name = "BtnStop";
            this.BtnStop.Size = new System.Drawing.Size(121, 35);
            this.BtnStop.TabIndex = 48;
            this.BtnStop.Text = "结束采集";
            this.BtnStop.UseVisualStyleBackColor = true;
            this.BtnStop.Click += new System.EventHandler(this.BtnStop_Click);
            // 
            // MS8040Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(759, 286);
            this.Controls.Add(this.BtnStop);
            this.Controls.Add(this.BtnStart);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.numericUpDownDuration);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.richTextBoxMessage);
            this.Controls.Add(this.labelUnit);
            this.Controls.Add(this.textBoxRead);
            this.Controls.Add(this.BtnRead);
            this.Controls.Add(this.labelRefreshPort);
            this.Controls.Add(this.comboBoxCurPort);
            this.Name = "MS8040Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MS8040电流表";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.MS8040Form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDuration)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label labelRefreshPort;
        private System.Windows.Forms.ComboBox comboBoxCurPort;
        private System.IO.Ports.SerialPort serialPortMS8040;
        private System.Windows.Forms.Label labelUnit;
        private System.Windows.Forms.TextBox textBoxRead;
        private System.Windows.Forms.Button BtnRead;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.RichTextBox richTextBoxMessage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDownDuration;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button BtnStart;
        private System.Windows.Forms.Button BtnStop;
    }
}

