namespace SmartBattery
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
            this.BtnOffsetCalibrate = new System.Windows.Forms.Button();
            this.BtnVoltageCalibrate = new System.Windows.Forms.Button();
            this.BtnCurrentCalibrate = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // BtnOffsetCalibrate
            // 
            this.BtnOffsetCalibrate.Location = new System.Drawing.Point(12, 33);
            this.BtnOffsetCalibrate.Name = "BtnOffsetCalibrate";
            this.BtnOffsetCalibrate.Size = new System.Drawing.Size(118, 31);
            this.BtnOffsetCalibrate.TabIndex = 0;
            this.BtnOffsetCalibrate.Text = "OffsetCalibrate";
            this.BtnOffsetCalibrate.UseVisualStyleBackColor = true;
            this.BtnOffsetCalibrate.Click += new System.EventHandler(this.BtnOffsetCalibrate_Click);
            // 
            // BtnVoltageCalibrate
            // 
            this.BtnVoltageCalibrate.Location = new System.Drawing.Point(12, 83);
            this.BtnVoltageCalibrate.Name = "BtnVoltageCalibrate";
            this.BtnVoltageCalibrate.Size = new System.Drawing.Size(118, 31);
            this.BtnVoltageCalibrate.TabIndex = 1;
            this.BtnVoltageCalibrate.Text = "VoltageCalibrate";
            this.BtnVoltageCalibrate.UseVisualStyleBackColor = true;
            this.BtnVoltageCalibrate.Click += new System.EventHandler(this.BtnVoltageCalibrate_Click);
            // 
            // BtnCurrentCalibrate
            // 
            this.BtnCurrentCalibrate.Location = new System.Drawing.Point(12, 135);
            this.BtnCurrentCalibrate.Name = "BtnCurrentCalibrate";
            this.BtnCurrentCalibrate.Size = new System.Drawing.Size(118, 31);
            this.BtnCurrentCalibrate.TabIndex = 2;
            this.BtnCurrentCalibrate.Text = "CurrentCalibrate";
            this.BtnCurrentCalibrate.UseVisualStyleBackColor = true;
            this.BtnCurrentCalibrate.Click += new System.EventHandler(this.BtnCurrentCalibrate_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.BtnCurrentCalibrate);
            this.Controls.Add(this.BtnVoltageCalibrate);
            this.Controls.Add(this.BtnOffsetCalibrate);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BtnOffsetCalibrate;
        private System.Windows.Forms.Button BtnVoltageCalibrate;
        private System.Windows.Forms.Button BtnCurrentCalibrate;
    }
}

