namespace BleSolution
{
    partial class Form2
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
            this.btnSearch = new System.Windows.Forms.Button();
            this.cmbServer = new System.Windows.Forms.ComboBox();
            this.cmbFeatures = new System.Windows.Forms.ComboBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnServer = new System.Windows.Forms.Button();
            this.btnFeatures = new System.Windows.Forms.Button();
            this.btnOpteron = new System.Windows.Forms.Button();
            this.btnReader = new System.Windows.Forms.Button();
            this.btnSend = new System.Windows.Forms.Button();
            this.listboxBleDevice = new System.Windows.Forms.ListBox();
            this.listboxMessage = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(13, 13);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 0;
            this.btnSearch.Text = "搜索";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // cmbServer
            // 
            this.cmbServer.FormattingEnabled = true;
            this.cmbServer.Location = new System.Drawing.Point(93, 42);
            this.cmbServer.Name = "cmbServer";
            this.cmbServer.Size = new System.Drawing.Size(440, 20);
            this.cmbServer.TabIndex = 1;
            // 
            // cmbFeatures
            // 
            this.cmbFeatures.FormattingEnabled = true;
            this.cmbFeatures.Location = new System.Drawing.Point(93, 71);
            this.cmbFeatures.Name = "cmbFeatures";
            this.cmbFeatures.Size = new System.Drawing.Size(440, 20);
            this.cmbFeatures.TabIndex = 2;
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(112, 13);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 3;
            this.btnConnect.Text = "匹配";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnServer
            // 
            this.btnServer.Location = new System.Drawing.Point(12, 42);
            this.btnServer.Name = "btnServer";
            this.btnServer.Size = new System.Drawing.Size(75, 23);
            this.btnServer.TabIndex = 4;
            this.btnServer.Text = "获取服务";
            this.btnServer.UseVisualStyleBackColor = true;
            this.btnServer.Click += new System.EventHandler(this.btnServer_Click);
            // 
            // btnFeatures
            // 
            this.btnFeatures.Location = new System.Drawing.Point(12, 71);
            this.btnFeatures.Name = "btnFeatures";
            this.btnFeatures.Size = new System.Drawing.Size(75, 23);
            this.btnFeatures.TabIndex = 5;
            this.btnFeatures.Text = "获取特征";
            this.btnFeatures.UseVisualStyleBackColor = true;
            this.btnFeatures.Click += new System.EventHandler(this.btnFeatures_Click);
            // 
            // btnOpteron
            // 
            this.btnOpteron.Location = new System.Drawing.Point(13, 100);
            this.btnOpteron.Name = "btnOpteron";
            this.btnOpteron.Size = new System.Drawing.Size(75, 23);
            this.btnOpteron.TabIndex = 6;
            this.btnOpteron.Text = "获取操作";
            this.btnOpteron.UseVisualStyleBackColor = true;
            this.btnOpteron.Click += new System.EventHandler(this.btnOpteron_Click);
            // 
            // btnReader
            // 
            this.btnReader.Location = new System.Drawing.Point(94, 100);
            this.btnReader.Name = "btnReader";
            this.btnReader.Size = new System.Drawing.Size(75, 23);
            this.btnReader.TabIndex = 7;
            this.btnReader.Text = "读取";
            this.btnReader.UseVisualStyleBackColor = true;
            this.btnReader.Click += new System.EventHandler(this.btnReader_Click);
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(175, 100);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.TabIndex = 8;
            this.btnSend.Text = "发送";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // listboxBleDevice
            // 
            this.listboxBleDevice.FormattingEnabled = true;
            this.listboxBleDevice.ItemHeight = 12;
            this.listboxBleDevice.Location = new System.Drawing.Point(13, 135);
            this.listboxBleDevice.Name = "listboxBleDevice";
            this.listboxBleDevice.Size = new System.Drawing.Size(775, 256);
            this.listboxBleDevice.TabIndex = 9;
            // 
            // listboxMessage
            // 
            this.listboxMessage.FormattingEnabled = true;
            this.listboxMessage.ItemHeight = 12;
            this.listboxMessage.Location = new System.Drawing.Point(12, 396);
            this.listboxMessage.Name = "listboxMessage";
            this.listboxMessage.Size = new System.Drawing.Size(776, 256);
            this.listboxMessage.TabIndex = 10;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 664);
            this.Controls.Add(this.listboxMessage);
            this.Controls.Add(this.listboxBleDevice);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.btnReader);
            this.Controls.Add(this.btnOpteron);
            this.Controls.Add(this.btnFeatures);
            this.Controls.Add(this.btnServer);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.cmbFeatures);
            this.Controls.Add(this.cmbServer);
            this.Controls.Add(this.btnSearch);
            this.Name = "Form2";
            this.Text = "Form2";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.ComboBox cmbServer;
        private System.Windows.Forms.ComboBox cmbFeatures;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnServer;
        private System.Windows.Forms.Button btnFeatures;
        private System.Windows.Forms.Button btnOpteron;
        private System.Windows.Forms.Button btnReader;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.ListBox listboxBleDevice;
        private System.Windows.Forms.ListBox listboxMessage;
    }
}