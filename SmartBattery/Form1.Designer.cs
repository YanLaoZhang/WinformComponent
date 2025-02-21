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
            this.BtnCMDPanel = new System.Windows.Forms.Button();
            this.BtnAFIFlash = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.BtnSelectExePath = new System.Windows.Forms.Button();
            this.textBoxExePath = new System.Windows.Forms.TextBox();
            this.textBoxAFIPath = new System.Windows.Forms.TextBox();
            this.BtnSelectAFIPath = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.NUDDiffer = new System.Windows.Forms.NumericUpDown();
            this.BtnCheckUI = new System.Windows.Forms.Button();
            this.BtnAFIFlashVD12D = new System.Windows.Forms.Button();
            this.BtnScanAll = new System.Windows.Forms.Button();
            this.BtnVoltageCalibrateVD12D = new System.Windows.Forms.Button();
            this.BtnActive = new System.Windows.Forms.Button();
            this.BtnCMDPanelVD12D = new System.Windows.Forms.Button();
            this.BtnAFIFlashFlaUI = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.BtnScanAllFlaUI = new System.Windows.Forms.Button();
            this.BtnOffsetCalibrateFlaUI = new System.Windows.Forms.Button();
            this.BtnVoltageCalibrateFlaUI = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.NUDDiffer)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // BtnOffsetCalibrate
            // 
            this.BtnOffsetCalibrate.Location = new System.Drawing.Point(166, 160);
            this.BtnOffsetCalibrate.Name = "BtnOffsetCalibrate";
            this.BtnOffsetCalibrate.Size = new System.Drawing.Size(118, 31);
            this.BtnOffsetCalibrate.TabIndex = 0;
            this.BtnOffsetCalibrate.Text = "OffsetCalibrate";
            this.BtnOffsetCalibrate.UseVisualStyleBackColor = true;
            this.BtnOffsetCalibrate.Click += new System.EventHandler(this.BtnOffsetCalibrate_Click);
            // 
            // BtnVoltageCalibrate
            // 
            this.BtnVoltageCalibrate.Location = new System.Drawing.Point(316, 160);
            this.BtnVoltageCalibrate.Name = "BtnVoltageCalibrate";
            this.BtnVoltageCalibrate.Size = new System.Drawing.Size(148, 31);
            this.BtnVoltageCalibrate.TabIndex = 1;
            this.BtnVoltageCalibrate.Text = "VoltageCalibrateEone";
            this.BtnVoltageCalibrate.UseVisualStyleBackColor = true;
            this.BtnVoltageCalibrate.Click += new System.EventHandler(this.BtnVoltageCalibrate_Click);
            // 
            // BtnCurrentCalibrate
            // 
            this.BtnCurrentCalibrate.Location = new System.Drawing.Point(502, 160);
            this.BtnCurrentCalibrate.Name = "BtnCurrentCalibrate";
            this.BtnCurrentCalibrate.Size = new System.Drawing.Size(118, 31);
            this.BtnCurrentCalibrate.TabIndex = 2;
            this.BtnCurrentCalibrate.Text = "CurrentCalibrate";
            this.BtnCurrentCalibrate.UseVisualStyleBackColor = true;
            this.BtnCurrentCalibrate.Click += new System.EventHandler(this.BtnCurrentCalibrate_Click);
            // 
            // BtnCMDPanel
            // 
            this.BtnCMDPanel.Location = new System.Drawing.Point(658, 160);
            this.BtnCMDPanel.Name = "BtnCMDPanel";
            this.BtnCMDPanel.Size = new System.Drawing.Size(118, 31);
            this.BtnCMDPanel.TabIndex = 3;
            this.BtnCMDPanel.Text = "CMD Panel Eone";
            this.BtnCMDPanel.UseVisualStyleBackColor = true;
            this.BtnCMDPanel.Click += new System.EventHandler(this.BtnCMDPanel_Click);
            // 
            // BtnAFIFlash
            // 
            this.BtnAFIFlash.Location = new System.Drawing.Point(208, 93);
            this.BtnAFIFlash.Name = "BtnAFIFlash";
            this.BtnAFIFlash.Size = new System.Drawing.Size(118, 31);
            this.BtnAFIFlash.TabIndex = 4;
            this.BtnAFIFlash.Text = "AFI Flash Eone";
            this.BtnAFIFlash.UseVisualStyleBackColor = true;
            this.BtnAFIFlash.Click += new System.EventHandler(this.BtnAFIFlash_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(136, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "烧录工具：";
            // 
            // BtnSelectExePath
            // 
            this.BtnSelectExePath.Location = new System.Drawing.Point(12, 12);
            this.BtnSelectExePath.Name = "BtnSelectExePath";
            this.BtnSelectExePath.Size = new System.Drawing.Size(118, 31);
            this.BtnSelectExePath.TabIndex = 6;
            this.BtnSelectExePath.Text = "选择烧录工具路径";
            this.BtnSelectExePath.UseVisualStyleBackColor = true;
            this.BtnSelectExePath.Click += new System.EventHandler(this.BtnSelectExePath_Click);
            // 
            // textBoxExePath
            // 
            this.textBoxExePath.Enabled = false;
            this.textBoxExePath.Location = new System.Drawing.Point(208, 18);
            this.textBoxExePath.Name = "textBoxExePath";
            this.textBoxExePath.Size = new System.Drawing.Size(580, 21);
            this.textBoxExePath.TabIndex = 7;
            // 
            // textBoxAFIPath
            // 
            this.textBoxAFIPath.Enabled = false;
            this.textBoxAFIPath.Location = new System.Drawing.Point(208, 55);
            this.textBoxAFIPath.Name = "textBoxAFIPath";
            this.textBoxAFIPath.Size = new System.Drawing.Size(580, 21);
            this.textBoxAFIPath.TabIndex = 10;
            // 
            // BtnSelectAFIPath
            // 
            this.BtnSelectAFIPath.Location = new System.Drawing.Point(12, 49);
            this.BtnSelectAFIPath.Name = "BtnSelectAFIPath";
            this.BtnSelectAFIPath.Size = new System.Drawing.Size(118, 31);
            this.BtnSelectAFIPath.TabIndex = 9;
            this.BtnSelectAFIPath.Text = "选择AFI文件路径";
            this.BtnSelectAFIPath.UseVisualStyleBackColor = true;
            this.BtnSelectAFIPath.Click += new System.EventHandler(this.BtnSelectAFIPath_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(136, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "AFI文件：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 93);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 11;
            this.label3.Text = "误差：";
            // 
            // NUDDiffer
            // 
            this.NUDDiffer.Location = new System.Drawing.Point(59, 91);
            this.NUDDiffer.Name = "NUDDiffer";
            this.NUDDiffer.Size = new System.Drawing.Size(87, 21);
            this.NUDDiffer.TabIndex = 12;
            // 
            // BtnCheckUI
            // 
            this.BtnCheckUI.Location = new System.Drawing.Point(12, 256);
            this.BtnCheckUI.Name = "BtnCheckUI";
            this.BtnCheckUI.Size = new System.Drawing.Size(118, 31);
            this.BtnCheckUI.TabIndex = 13;
            this.BtnCheckUI.Text = "CheckUI";
            this.BtnCheckUI.UseVisualStyleBackColor = true;
            this.BtnCheckUI.Click += new System.EventHandler(this.BtnCheckUI_Click);
            // 
            // BtnAFIFlashVD12D
            // 
            this.BtnAFIFlashVD12D.Location = new System.Drawing.Point(373, 93);
            this.BtnAFIFlashVD12D.Name = "BtnAFIFlashVD12D";
            this.BtnAFIFlashVD12D.Size = new System.Drawing.Size(118, 31);
            this.BtnAFIFlashVD12D.TabIndex = 14;
            this.BtnAFIFlashVD12D.Text = "AFI Flash VD12D";
            this.BtnAFIFlashVD12D.UseVisualStyleBackColor = true;
            this.BtnAFIFlashVD12D.Click += new System.EventHandler(this.BtnAFIFlashVD12D_Click);
            // 
            // BtnScanAll
            // 
            this.BtnScanAll.Location = new System.Drawing.Point(12, 160);
            this.BtnScanAll.Name = "BtnScanAll";
            this.BtnScanAll.Size = new System.Drawing.Size(118, 31);
            this.BtnScanAll.TabIndex = 15;
            this.BtnScanAll.Text = "ScanAll";
            this.BtnScanAll.UseVisualStyleBackColor = true;
            this.BtnScanAll.Click += new System.EventHandler(this.BtnScanAll_Click);
            // 
            // BtnVoltageCalibrateVD12D
            // 
            this.BtnVoltageCalibrateVD12D.Location = new System.Drawing.Point(316, 208);
            this.BtnVoltageCalibrateVD12D.Name = "BtnVoltageCalibrateVD12D";
            this.BtnVoltageCalibrateVD12D.Size = new System.Drawing.Size(148, 31);
            this.BtnVoltageCalibrateVD12D.TabIndex = 16;
            this.BtnVoltageCalibrateVD12D.Text = "VoltageCalibrateVD12D";
            this.BtnVoltageCalibrateVD12D.UseVisualStyleBackColor = true;
            this.BtnVoltageCalibrateVD12D.Click += new System.EventHandler(this.BtnVoltageCalibrateVD12D_Click);
            // 
            // BtnActive
            // 
            this.BtnActive.Location = new System.Drawing.Point(12, 208);
            this.BtnActive.Name = "BtnActive";
            this.BtnActive.Size = new System.Drawing.Size(118, 31);
            this.BtnActive.TabIndex = 17;
            this.BtnActive.Text = "Active";
            this.BtnActive.UseVisualStyleBackColor = true;
            this.BtnActive.Click += new System.EventHandler(this.BtnActive_Click);
            // 
            // BtnCMDPanelVD12D
            // 
            this.BtnCMDPanelVD12D.Location = new System.Drawing.Point(658, 208);
            this.BtnCMDPanelVD12D.Name = "BtnCMDPanelVD12D";
            this.BtnCMDPanelVD12D.Size = new System.Drawing.Size(118, 31);
            this.BtnCMDPanelVD12D.TabIndex = 18;
            this.BtnCMDPanelVD12D.Text = "CMD Panel VD12D";
            this.BtnCMDPanelVD12D.UseVisualStyleBackColor = true;
            this.BtnCMDPanelVD12D.Click += new System.EventHandler(this.BtnCMDPanelVD12D_Click);
            // 
            // BtnAFIFlashFlaUI
            // 
            this.BtnAFIFlashFlaUI.Location = new System.Drawing.Point(6, 20);
            this.BtnAFIFlashFlaUI.Name = "BtnAFIFlashFlaUI";
            this.BtnAFIFlashFlaUI.Size = new System.Drawing.Size(118, 31);
            this.BtnAFIFlashFlaUI.TabIndex = 19;
            this.BtnAFIFlashFlaUI.Text = "AFI Flash";
            this.BtnAFIFlashFlaUI.UseVisualStyleBackColor = true;
            this.BtnAFIFlashFlaUI.Click += new System.EventHandler(this.BtnAFIFlashFlaUI_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.BtnVoltageCalibrateFlaUI);
            this.groupBox1.Controls.Add(this.BtnOffsetCalibrateFlaUI);
            this.groupBox1.Controls.Add(this.BtnScanAllFlaUI);
            this.groupBox1.Controls.Add(this.BtnAFIFlashFlaUI);
            this.groupBox1.Location = new System.Drawing.Point(12, 307);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(776, 131);
            this.groupBox1.TabIndex = 20;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "FlaUI.UIA3";
            // 
            // BtnScanAllFlaUI
            // 
            this.BtnScanAllFlaUI.Location = new System.Drawing.Point(130, 20);
            this.BtnScanAllFlaUI.Name = "BtnScanAllFlaUI";
            this.BtnScanAllFlaUI.Size = new System.Drawing.Size(118, 31);
            this.BtnScanAllFlaUI.TabIndex = 21;
            this.BtnScanAllFlaUI.Text = "ScanAll";
            this.BtnScanAllFlaUI.UseVisualStyleBackColor = true;
            this.BtnScanAllFlaUI.Click += new System.EventHandler(this.BtnScanAllFlaUI_Click);
            // 
            // BtnOffsetCalibrateFlaUI
            // 
            this.BtnOffsetCalibrateFlaUI.Location = new System.Drawing.Point(254, 20);
            this.BtnOffsetCalibrateFlaUI.Name = "BtnOffsetCalibrateFlaUI";
            this.BtnOffsetCalibrateFlaUI.Size = new System.Drawing.Size(118, 31);
            this.BtnOffsetCalibrateFlaUI.TabIndex = 21;
            this.BtnOffsetCalibrateFlaUI.Text = "OffsetCalibrate";
            this.BtnOffsetCalibrateFlaUI.UseVisualStyleBackColor = true;
            this.BtnOffsetCalibrateFlaUI.Click += new System.EventHandler(this.BtnOffsetCalibrateFlaUI_Click);
            // 
            // BtnVoltageCalibrateFlaUI
            // 
            this.BtnVoltageCalibrateFlaUI.Location = new System.Drawing.Point(378, 20);
            this.BtnVoltageCalibrateFlaUI.Name = "BtnVoltageCalibrateFlaUI";
            this.BtnVoltageCalibrateFlaUI.Size = new System.Drawing.Size(148, 31);
            this.BtnVoltageCalibrateFlaUI.TabIndex = 22;
            this.BtnVoltageCalibrateFlaUI.Text = "VoltageCalibrateEone";
            this.BtnVoltageCalibrateFlaUI.UseVisualStyleBackColor = true;
            this.BtnVoltageCalibrateFlaUI.Click += new System.EventHandler(this.BtnVoltageCalibrateFlaUI_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.BtnCMDPanelVD12D);
            this.Controls.Add(this.BtnActive);
            this.Controls.Add(this.BtnVoltageCalibrateVD12D);
            this.Controls.Add(this.BtnScanAll);
            this.Controls.Add(this.BtnAFIFlashVD12D);
            this.Controls.Add(this.BtnCheckUI);
            this.Controls.Add(this.NUDDiffer);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxAFIPath);
            this.Controls.Add(this.BtnSelectAFIPath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxExePath);
            this.Controls.Add(this.BtnSelectExePath);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BtnAFIFlash);
            this.Controls.Add(this.BtnCMDPanel);
            this.Controls.Add(this.BtnCurrentCalibrate);
            this.Controls.Add(this.BtnVoltageCalibrate);
            this.Controls.Add(this.BtnOffsetCalibrate);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.NUDDiffer)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnOffsetCalibrate;
        private System.Windows.Forms.Button BtnVoltageCalibrate;
        private System.Windows.Forms.Button BtnCurrentCalibrate;
        private System.Windows.Forms.Button BtnCMDPanel;
        private System.Windows.Forms.Button BtnAFIFlash;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button BtnSelectExePath;
        private System.Windows.Forms.TextBox textBoxExePath;
        private System.Windows.Forms.TextBox textBoxAFIPath;
        private System.Windows.Forms.Button BtnSelectAFIPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown NUDDiffer;
        private System.Windows.Forms.Button BtnCheckUI;
        private System.Windows.Forms.Button BtnAFIFlashVD12D;
        private System.Windows.Forms.Button BtnScanAll;
        private System.Windows.Forms.Button BtnVoltageCalibrateVD12D;
        private System.Windows.Forms.Button BtnActive;
        private System.Windows.Forms.Button BtnCMDPanelVD12D;
        private System.Windows.Forms.Button BtnAFIFlashFlaUI;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button BtnScanAllFlaUI;
        private System.Windows.Forms.Button BtnOffsetCalibrateFlaUI;
        private System.Windows.Forms.Button BtnVoltageCalibrateFlaUI;
    }
}

