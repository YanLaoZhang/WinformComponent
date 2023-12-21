namespace VLCForm
{
    partial class FormVLC
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.vlcControl1 = new Vlc.DotNet.Forms.VlcControl();
            this.labelRTSPURI = new System.Windows.Forms.Label();
            this.updateTimer = new System.Windows.Forms.Timer(this.components);
            this.labelError = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.vlcControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.vlcControl1);
            this.groupBox1.Location = new System.Drawing.Point(11, 11);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(1006, 710);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "实时影像";
            // 
            // vlcControl1
            // 
            this.vlcControl1.BackColor = System.Drawing.SystemColors.Window;
            this.vlcControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vlcControl1.ForeColor = System.Drawing.SystemColors.Window;
            this.vlcControl1.Location = new System.Drawing.Point(2, 16);
            this.vlcControl1.Margin = new System.Windows.Forms.Padding(2);
            this.vlcControl1.Name = "vlcControl1";
            this.vlcControl1.Size = new System.Drawing.Size(1002, 692);
            this.vlcControl1.Spu = -1;
            this.vlcControl1.TabIndex = 0;
            this.vlcControl1.Text = "vlcControl1";
            this.vlcControl1.VlcLibDirectory = null;
            this.vlcControl1.VlcMediaplayerOptions = null;
            this.vlcControl1.VlcLibDirectoryNeeded += new System.EventHandler<Vlc.DotNet.Forms.VlcLibDirectoryNeededEventArgs>(this.vlcControl1_VlcLibDirectoryNeeded);
            this.vlcControl1.EncounteredError += new System.EventHandler<Vlc.DotNet.Core.VlcMediaPlayerEncounteredErrorEventArgs>(this.vlcControl1_EncounteredError);
            // 
            // labelRTSPURI
            // 
            this.labelRTSPURI.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelRTSPURI.AutoSize = true;
            this.labelRTSPURI.Location = new System.Drawing.Point(11, 723);
            this.labelRTSPURI.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelRTSPURI.Name = "labelRTSPURI";
            this.labelRTSPURI.Size = new System.Drawing.Size(41, 12);
            this.labelRTSPURI.TabIndex = 5;
            this.labelRTSPURI.Text = "label2";
            // 
            // updateTimer
            // 
            this.updateTimer.Tick += new System.EventHandler(this.updateTimer_Tick);
            // 
            // labelError
            // 
            this.labelError.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelError.AutoSize = true;
            this.labelError.Location = new System.Drawing.Point(757, 723);
            this.labelError.Name = "labelError";
            this.labelError.Size = new System.Drawing.Size(29, 12);
            this.labelError.TabIndex = 6;
            this.labelError.Text = "Tips";
            this.labelError.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // FormVLC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1028, 744);
            this.Controls.Add(this.labelError);
            this.Controls.Add(this.labelRTSPURI);
            this.Controls.Add(this.groupBox1);
            this.Name = "FormVLC";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "VLC视频检查";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormVLC_FormClosing);
            this.Shown += new System.EventHandler(this.FormVLC_Shown);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.vlcControl1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private Vlc.DotNet.Forms.VlcControl vlcControl1;
        private System.Windows.Forms.Label labelRTSPURI;
        private System.Windows.Forms.Timer updateTimer;
        private System.Windows.Forms.Label labelError;
    }
}

