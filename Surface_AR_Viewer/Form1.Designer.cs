
namespace Surface_AR_Viewer
{
    partial class AR_viewer
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.conn2server_btn = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.disconn2server_btn = new System.Windows.Forms.Button();
            this.get_cam_img = new System.ComponentModel.BackgroundWorker();
            this.show_img = new System.ComponentModel.BackgroundWorker();
            this.help_btn = new System.Windows.Forms.Button();
            this.play_voice = new System.ComponentModel.BackgroundWorker();
            this.data_btn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // conn2server_btn
            // 
            this.conn2server_btn.Location = new System.Drawing.Point(12, 12);
            this.conn2server_btn.Name = "conn2server_btn";
            this.conn2server_btn.Size = new System.Drawing.Size(75, 23);
            this.conn2server_btn.TabIndex = 4;
            this.conn2server_btn.Text = "Connect";
            this.conn2server_btn.UseVisualStyleBackColor = true;
            this.conn2server_btn.Click += new System.EventHandler(this.conn2server_btn_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1280, 720);
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // disconn2server_btn
            // 
            this.disconn2server_btn.Location = new System.Drawing.Point(93, 12);
            this.disconn2server_btn.Name = "disconn2server_btn";
            this.disconn2server_btn.Size = new System.Drawing.Size(75, 23);
            this.disconn2server_btn.TabIndex = 6;
            this.disconn2server_btn.Text = "Disconnect";
            this.disconn2server_btn.UseVisualStyleBackColor = true;
            this.disconn2server_btn.Click += new System.EventHandler(this.disconn2server_btn_Click);
            // 
            // get_cam_img
            // 
            this.get_cam_img.WorkerSupportsCancellation = true;
            this.get_cam_img.DoWork += new System.ComponentModel.DoWorkEventHandler(this.get_cam_img_DoWork);
            // 
            // show_img
            // 
            this.show_img.WorkerSupportsCancellation = true;
            this.show_img.DoWork += new System.ComponentModel.DoWorkEventHandler(this.show_img_DoWork);
            // 
            // help_btn
            // 
            this.help_btn.Location = new System.Drawing.Point(174, 12);
            this.help_btn.Name = "help_btn";
            this.help_btn.Size = new System.Drawing.Size(75, 23);
            this.help_btn.TabIndex = 7;
            this.help_btn.Text = "Help";
            this.help_btn.UseVisualStyleBackColor = true;
            this.help_btn.Click += new System.EventHandler(this.help_btn_Click);
            // 
            // play_voice
            // 
            this.play_voice.WorkerSupportsCancellation = true;
            this.play_voice.DoWork += new System.ComponentModel.DoWorkEventHandler(this.play_voice_DoWork);
            // 
            // data_btn
            // 
            this.data_btn.Location = new System.Drawing.Point(255, 12);
            this.data_btn.Name = "data_btn";
            this.data_btn.Size = new System.Drawing.Size(75, 23);
            this.data_btn.TabIndex = 8;
            this.data_btn.Text = "Data";
            this.data_btn.UseVisualStyleBackColor = true;
            this.data_btn.Click += new System.EventHandler(this.data_btn_Click);
            // 
            // AR_viewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1280, 720);
            this.Controls.Add(this.data_btn);
            this.Controls.Add(this.help_btn);
            this.Controls.Add(this.disconn2server_btn);
            this.Controls.Add(this.conn2server_btn);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AR_viewer";
            this.Text = "Suface AR Viewer";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button conn2server_btn;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button disconn2server_btn;
        private System.ComponentModel.BackgroundWorker get_cam_img;
        private System.ComponentModel.BackgroundWorker show_img;
        private System.Windows.Forms.Button help_btn;
        private System.ComponentModel.BackgroundWorker play_voice;
        private System.Windows.Forms.Button data_btn;
    }
}

