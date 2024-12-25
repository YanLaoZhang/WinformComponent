namespace TES136Debug
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
            this.BtnSearch = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.BtnSend = new System.Windows.Forms.Button();
            this.BtnSend1 = new System.Windows.Forms.Button();
            this.comboBoxHIDDevices = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // BtnSearch
            // 
            this.BtnSearch.Location = new System.Drawing.Point(13, 13);
            this.BtnSearch.Name = "BtnSearch";
            this.BtnSearch.Size = new System.Drawing.Size(75, 23);
            this.BtnSearch.TabIndex = 0;
            this.BtnSearch.Text = "查找设备";
            this.BtnSearch.UseVisualStyleBackColor = true;
            this.BtnSearch.Click += new System.EventHandler(this.BtnSearch_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox1.Location = new System.Drawing.Point(12, 70);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(1248, 368);
            this.richTextBox1.TabIndex = 1;
            this.richTextBox1.Text = "";
            // 
            // BtnSend
            // 
            this.BtnSend.Location = new System.Drawing.Point(906, 18);
            this.BtnSend.Name = "BtnSend";
            this.BtnSend.Size = new System.Drawing.Size(75, 23);
            this.BtnSend.TabIndex = 2;
            this.BtnSend.Text = "发送";
            this.BtnSend.UseVisualStyleBackColor = true;
            this.BtnSend.Click += new System.EventHandler(this.BtnSend_Click);
            // 
            // BtnSend1
            // 
            this.BtnSend1.Location = new System.Drawing.Point(94, 13);
            this.BtnSend1.Name = "BtnSend1";
            this.BtnSend1.Size = new System.Drawing.Size(75, 23);
            this.BtnSend1.TabIndex = 3;
            this.BtnSend1.Text = "发送1";
            this.BtnSend1.UseVisualStyleBackColor = true;
            this.BtnSend1.Click += new System.EventHandler(this.BtnSend1_Click);
            // 
            // comboBoxHIDDevices
            // 
            this.comboBoxHIDDevices.FormattingEnabled = true;
            this.comboBoxHIDDevices.Location = new System.Drawing.Point(467, 20);
            this.comboBoxHIDDevices.Name = "comboBoxHIDDevices";
            this.comboBoxHIDDevices.Size = new System.Drawing.Size(433, 20);
            this.comboBoxHIDDevices.TabIndex = 4;
            this.comboBoxHIDDevices.DropDown += new System.EventHandler(this.comboBoxHIDDevices_DropDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(402, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "HID设备：";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1272, 450);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxHIDDevices);
            this.Controls.Add(this.BtnSend1);
            this.Controls.Add(this.BtnSend);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.BtnSearch);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnSearch;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button BtnSend;
        private System.Windows.Forms.Button BtnSend1;
        private System.Windows.Forms.ComboBox comboBoxHIDDevices;
        private System.Windows.Forms.Label label1;
    }
}

