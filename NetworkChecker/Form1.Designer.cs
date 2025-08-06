namespace NetworkChecker
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
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxIP = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.numericUpDownPort = new System.Windows.Forms.NumericUpDown();
            this.buttonCheckIPPort = new System.Windows.Forms.Button();
            this.buttonCheckIP = new System.Windows.Forms.Button();
            this.labelIPStatus = new System.Windows.Forms.Label();
            this.labelPortStatus = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPort)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 39);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "IP";
            // 
            // textBoxIP
            // 
            this.textBoxIP.Location = new System.Drawing.Point(62, 35);
            this.textBoxIP.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxIP.Name = "textBoxIP";
            this.textBoxIP.Size = new System.Drawing.Size(216, 26);
            this.textBoxIP.TabIndex = 1;
            this.textBoxIP.Text = "192.168.1.1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 84);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "Port";
            // 
            // numericUpDownPort
            // 
            this.numericUpDownPort.Location = new System.Drawing.Point(62, 82);
            this.numericUpDownPort.Maximum = new decimal(new int[] {
            6546,
            0,
            0,
            0});
            this.numericUpDownPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownPort.Name = "numericUpDownPort";
            this.numericUpDownPort.Size = new System.Drawing.Size(104, 26);
            this.numericUpDownPort.TabIndex = 3;
            this.numericUpDownPort.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // buttonCheckIPPort
            // 
            this.buttonCheckIPPort.Location = new System.Drawing.Point(307, 84);
            this.buttonCheckIPPort.Name = "buttonCheckIPPort";
            this.buttonCheckIPPort.Size = new System.Drawing.Size(82, 26);
            this.buttonCheckIPPort.TabIndex = 4;
            this.buttonCheckIPPort.Text = "检测端口";
            this.buttonCheckIPPort.UseVisualStyleBackColor = true;
            this.buttonCheckIPPort.Click += new System.EventHandler(this.buttonCheckIPPort_Click);
            // 
            // buttonCheckIP
            // 
            this.buttonCheckIP.Location = new System.Drawing.Point(307, 34);
            this.buttonCheckIP.Name = "buttonCheckIP";
            this.buttonCheckIP.Size = new System.Drawing.Size(82, 26);
            this.buttonCheckIP.TabIndex = 5;
            this.buttonCheckIP.Text = "检测IP";
            this.buttonCheckIP.UseVisualStyleBackColor = true;
            this.buttonCheckIP.Click += new System.EventHandler(this.buttonCheckIP_Click);
            // 
            // labelIPStatus
            // 
            this.labelIPStatus.AutoSize = true;
            this.labelIPStatus.Location = new System.Drawing.Point(408, 39);
            this.labelIPStatus.Name = "labelIPStatus";
            this.labelIPStatus.Size = new System.Drawing.Size(87, 16);
            this.labelIPStatus.TabIndex = 6;
            this.labelIPStatus.Text = "IP检测结果";
            // 
            // labelPortStatus
            // 
            this.labelPortStatus.AutoSize = true;
            this.labelPortStatus.Location = new System.Drawing.Point(408, 89);
            this.labelPortStatus.Name = "labelPortStatus";
            this.labelPortStatus.Size = new System.Drawing.Size(87, 16);
            this.labelPortStatus.TabIndex = 7;
            this.labelPortStatus.Text = "IP检测结果";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(636, 152);
            this.Controls.Add(this.labelPortStatus);
            this.Controls.Add(this.labelIPStatus);
            this.Controls.Add(this.buttonCheckIP);
            this.Controls.Add(this.buttonCheckIPPort);
            this.Controls.Add(this.numericUpDownPort);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxIP);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("宋体", 12F);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "网络检测";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPort)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxIP;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericUpDownPort;
        private System.Windows.Forms.Button buttonCheckIPPort;
        private System.Windows.Forms.Button buttonCheckIP;
        private System.Windows.Forms.Label labelIPStatus;
        private System.Windows.Forms.Label labelPortStatus;
    }
}

