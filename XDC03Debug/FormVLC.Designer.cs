namespace XDC03Debug
{
    partial class FormVLC
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.vlcControl1 = new Vlc.DotNet.Forms.VlcControl();
            this.labelRTSPURI = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.buttonNG = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.vlcControl1)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.vlcControl1);
            this.groupBox1.Location = new System.Drawing.Point(12, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(985, 584);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "实时影像";
            // 
            // vlcControl1
            // 
            this.vlcControl1.BackColor = System.Drawing.Color.Black;
            this.vlcControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vlcControl1.Location = new System.Drawing.Point(3, 21);
            this.vlcControl1.Name = "vlcControl1";
            this.vlcControl1.Size = new System.Drawing.Size(979, 560);
            this.vlcControl1.Spu = -1;
            this.vlcControl1.TabIndex = 0;
            this.vlcControl1.Text = "vlcControl1";
            this.vlcControl1.VlcLibDirectory = null;
            this.vlcControl1.VlcMediaplayerOptions = null;
            this.vlcControl1.VlcLibDirectoryNeeded += new System.EventHandler<Vlc.DotNet.Forms.VlcLibDirectoryNeededEventArgs>(this.vlcControl1_VlcLibDirectoryNeeded);
            // 
            // labelRTSPURI
            // 
            this.labelRTSPURI.AutoSize = true;
            this.labelRTSPURI.Location = new System.Drawing.Point(6, 21);
            this.labelRTSPURI.Name = "labelRTSPURI";
            this.labelRTSPURI.Size = new System.Drawing.Size(55, 15);
            this.labelRTSPURI.TabIndex = 4;
            this.labelRTSPURI.Text = "label2";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.buttonNG);
            this.groupBox2.Controls.Add(this.labelRTSPURI);
            this.groupBox2.Controls.Add(this.buttonOK);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(12, 603);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(985, 109);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "人工判断";
            // 
            // buttonNG
            // 
            this.buttonNG.Font = new System.Drawing.Font("Microsoft YaHei UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonNG.ForeColor = System.Drawing.Color.Red;
            this.buttonNG.Location = new System.Drawing.Point(529, 57);
            this.buttonNG.Name = "buttonNG";
            this.buttonNG.Size = new System.Drawing.Size(160, 40);
            this.buttonNG.TabIndex = 6;
            this.buttonNG.Text = "F12_NG";
            this.buttonNG.UseVisualStyleBackColor = true;
            this.buttonNG.Click += new System.EventHandler(this.buttonNG_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Font = new System.Drawing.Font("Microsoft YaHei UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonOK.ForeColor = System.Drawing.Color.Green;
            this.buttonOK.Location = new System.Drawing.Point(304, 57);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(141, 40);
            this.buttonOK.TabIndex = 5;
            this.buttonOK.Text = "F2_OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(349, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(287, 33);
            this.label1.TabIndex = 4;
            this.label1.Text = "是否正常出现影像";
            // 
            // FormVLC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1009, 724);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.KeyPreview = true;
            this.Name = "FormVLC";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "实时影像测试";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormVLC_FormClosing);
            this.Shown += new System.EventHandler(this.FormVLC_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormVLC_KeyDown);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.vlcControl1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private Vlc.DotNet.Forms.VlcControl vlcControl1;
        private System.Windows.Forms.Label labelRTSPURI;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button buttonNG;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Label label1;
    }
}