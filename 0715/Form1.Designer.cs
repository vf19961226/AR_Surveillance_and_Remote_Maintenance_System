
namespace _0715
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.clearpicture = new System.Windows.Forms.Button();
            this.stopspbtn = new System.Windows.Forms.Button();
            this.workposbtn = new System.Windows.Forms.Button();
            this.test2 = new System.Windows.Forms.Button();
            this.test = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.okbtn = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.Xalmcode = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.TorqueZ = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.TorqueY = new System.Windows.Forms.TextBox();
            this.Vtb3 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Vtb0 = new System.Windows.Forms.TextBox();
            this.Vtb1 = new System.Windows.Forms.TextBox();
            this.TorqueX = new System.Windows.Forms.TextBox();
            this.Vtb2 = new System.Windows.Forms.TextBox();
            this.Yalmcode = new System.Windows.Forms.TextBox();
            this.VelocityZ = new System.Windows.Forms.TextBox();
            this.Zalmcode = new System.Windows.Forms.TextBox();
            this.VelocityY = new System.Windows.Forms.TextBox();
            this.VelocityX = new System.Windows.Forms.TextBox();
            this.labelTorque = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.XtextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.ZtextBox = new System.Windows.Forms.TextBox();
            this.YtextBox = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.jogspeedtextBox = new System.Windows.Forms.TextBox();
            this.joglessbtn = new System.Windows.Forms.Button();
            this.jogpulsebtn = new System.Windows.Forms.Button();
            this.axiscomboBox = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.gohomebtn = new System.Windows.Forms.Button();
            this.jogmodebtn = new System.Windows.Forms.Button();
            this.startbtn = new System.Windows.Forms.Button();
            this.feedtextBox = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Interval = 200;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.Location = new System.Drawing.Point(12, 169);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(810, 400);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 168;
            this.pictureBox1.TabStop = false;
            // 
            // clearpicture
            // 
            this.clearpicture.BackColor = System.Drawing.Color.White;
            this.clearpicture.Location = new System.Drawing.Point(768, 514);
            this.clearpicture.Margin = new System.Windows.Forms.Padding(2);
            this.clearpicture.Name = "clearpicture";
            this.clearpicture.Size = new System.Drawing.Size(53, 54);
            this.clearpicture.TabIndex = 174;
            this.clearpicture.Text = "清空圖面";
            this.clearpicture.UseVisualStyleBackColor = false;
            this.clearpicture.Click += new System.EventHandler(this.clearpicture_Click);
            // 
            // stopspbtn
            // 
            this.stopspbtn.Location = new System.Drawing.Point(7, 94);
            this.stopspbtn.Margin = new System.Windows.Forms.Padding(2);
            this.stopspbtn.Name = "stopspbtn";
            this.stopspbtn.Size = new System.Drawing.Size(64, 29);
            this.stopspbtn.TabIndex = 287;
            this.stopspbtn.Text = "STOP";
            this.stopspbtn.UseVisualStyleBackColor = true;
            this.stopspbtn.Click += new System.EventHandler(this.stopspbtn_Click);
            // 
            // workposbtn
            // 
            this.workposbtn.Location = new System.Drawing.Point(7, 57);
            this.workposbtn.Margin = new System.Windows.Forms.Padding(2);
            this.workposbtn.Name = "workposbtn";
            this.workposbtn.Size = new System.Drawing.Size(64, 29);
            this.workposbtn.TabIndex = 286;
            this.workposbtn.Text = "教點";
            this.workposbtn.UseVisualStyleBackColor = true;
            this.workposbtn.Click += new System.EventHandler(this.workposbtn_Click);
            // 
            // test2
            // 
            this.test2.Location = new System.Drawing.Point(75, 94);
            this.test2.Margin = new System.Windows.Forms.Padding(2);
            this.test2.Name = "test2";
            this.test2.Size = new System.Drawing.Size(64, 29);
            this.test2.TabIndex = 285;
            this.test2.Text = "test2";
            this.test2.UseVisualStyleBackColor = true;
            this.test2.Click += new System.EventHandler(this.test2_Click);
            // 
            // test
            // 
            this.test.Location = new System.Drawing.Point(75, 57);
            this.test.Margin = new System.Windows.Forms.Padding(2);
            this.test.Name = "test";
            this.test.Size = new System.Drawing.Size(64, 29);
            this.test.TabIndex = 284;
            this.test.Text = "test1";
            this.test.UseVisualStyleBackColor = true;
            this.test.Click += new System.EventHandler(this.test_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.okbtn);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.Xalmcode);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.TorqueZ);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.TorqueY);
            this.groupBox3.Controls.Add(this.Vtb3);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.Vtb0);
            this.groupBox3.Controls.Add(this.Vtb1);
            this.groupBox3.Controls.Add(this.TorqueX);
            this.groupBox3.Controls.Add(this.Vtb2);
            this.groupBox3.Controls.Add(this.Yalmcode);
            this.groupBox3.Controls.Add(this.VelocityZ);
            this.groupBox3.Controls.Add(this.Zalmcode);
            this.groupBox3.Controls.Add(this.VelocityY);
            this.groupBox3.Controls.Add(this.VelocityX);
            this.groupBox3.Controls.Add(this.labelTorque);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.XtextBox);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.ZtextBox);
            this.groupBox3.Controls.Add(this.YtextBox);
            this.groupBox3.Location = new System.Drawing.Point(297, 11);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox3.Size = new System.Drawing.Size(380, 154);
            this.groupBox3.TabIndex = 282;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "資訊";
            // 
            // okbtn
            // 
            this.okbtn.Location = new System.Drawing.Point(305, 11);
            this.okbtn.Margin = new System.Windows.Forms.Padding(2);
            this.okbtn.Name = "okbtn";
            this.okbtn.Size = new System.Drawing.Size(63, 29);
            this.okbtn.TabIndex = 290;
            this.okbtn.Text = "Connect";
            this.okbtn.UseVisualStyleBackColor = true;
            this.okbtn.Click += new System.EventHandler(this.okbtn_Click);
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(275, 122);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(27, 18);
            this.label9.TabIndex = 315;
            this.label9.Text = "SPI:  ";
            // 
            // Xalmcode
            // 
            this.Xalmcode.Location = new System.Drawing.Point(65, 62);
            this.Xalmcode.Margin = new System.Windows.Forms.Padding(2);
            this.Xalmcode.Name = "Xalmcode";
            this.Xalmcode.Size = new System.Drawing.Size(61, 22);
            this.Xalmcode.TabIndex = 304;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(275, 96);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(27, 19);
            this.label4.TabIndex = 314;
            this.label4.Text = "V3:  ";
            // 
            // TorqueZ
            // 
            this.TorqueZ.Location = new System.Drawing.Point(208, 121);
            this.TorqueZ.Margin = new System.Windows.Forms.Padding(2);
            this.TorqueZ.Name = "TorqueZ";
            this.TorqueZ.Size = new System.Drawing.Size(61, 22);
            this.TorqueZ.TabIndex = 316;
            this.TorqueZ.TextChanged += new System.EventHandler(this.TorqueZ_TextChanged);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(275, 71);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(27, 20);
            this.label3.TabIndex = 313;
            this.label3.Text = "V2:  ";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(4, 65);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(70, 14);
            this.label8.TabIndex = 311;
            this.label8.Text = " Error Code: ";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(275, 47);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(27, 18);
            this.label2.TabIndex = 290;
            this.label2.Text = "V1:  ";
            // 
            // TorqueY
            // 
            this.TorqueY.Location = new System.Drawing.Point(137, 121);
            this.TorqueY.Margin = new System.Windows.Forms.Padding(2);
            this.TorqueY.Name = "TorqueY";
            this.TorqueY.Size = new System.Drawing.Size(61, 22);
            this.TorqueY.TabIndex = 315;
            // 
            // Vtb3
            // 
            this.Vtb3.Location = new System.Drawing.Point(306, 121);
            this.Vtb3.Margin = new System.Windows.Forms.Padding(2);
            this.Vtb3.Name = "Vtb3";
            this.Vtb3.Size = new System.Drawing.Size(61, 22);
            this.Vtb3.TabIndex = 309;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(82, 13);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(169, 19);
            this.label1.TabIndex = 305;
            this.label1.Text = "  X:                     Y:                    Z:";
            // 
            // Vtb0
            // 
            this.Vtb0.Location = new System.Drawing.Point(306, 44);
            this.Vtb0.Margin = new System.Windows.Forms.Padding(2);
            this.Vtb0.Name = "Vtb0";
            this.Vtb0.Size = new System.Drawing.Size(61, 22);
            this.Vtb0.TabIndex = 310;
            // 
            // Vtb1
            // 
            this.Vtb1.Location = new System.Drawing.Point(306, 69);
            this.Vtb1.Margin = new System.Windows.Forms.Padding(2);
            this.Vtb1.Name = "Vtb1";
            this.Vtb1.Size = new System.Drawing.Size(61, 22);
            this.Vtb1.TabIndex = 311;
            // 
            // TorqueX
            // 
            this.TorqueX.Location = new System.Drawing.Point(65, 121);
            this.TorqueX.Margin = new System.Windows.Forms.Padding(2);
            this.TorqueX.Name = "TorqueX";
            this.TorqueX.Size = new System.Drawing.Size(61, 22);
            this.TorqueX.TabIndex = 314;
            // 
            // Vtb2
            // 
            this.Vtb2.Location = new System.Drawing.Point(306, 95);
            this.Vtb2.Margin = new System.Windows.Forms.Padding(2);
            this.Vtb2.Name = "Vtb2";
            this.Vtb2.Size = new System.Drawing.Size(61, 22);
            this.Vtb2.TabIndex = 312;
            // 
            // Yalmcode
            // 
            this.Yalmcode.Location = new System.Drawing.Point(137, 63);
            this.Yalmcode.Margin = new System.Windows.Forms.Padding(2);
            this.Yalmcode.Name = "Yalmcode";
            this.Yalmcode.Size = new System.Drawing.Size(61, 22);
            this.Yalmcode.TabIndex = 306;
            // 
            // VelocityZ
            // 
            this.VelocityZ.Location = new System.Drawing.Point(208, 92);
            this.VelocityZ.Margin = new System.Windows.Forms.Padding(2);
            this.VelocityZ.Name = "VelocityZ";
            this.VelocityZ.Size = new System.Drawing.Size(61, 22);
            this.VelocityZ.TabIndex = 313;
            // 
            // Zalmcode
            // 
            this.Zalmcode.Location = new System.Drawing.Point(208, 63);
            this.Zalmcode.Margin = new System.Windows.Forms.Padding(2);
            this.Zalmcode.Name = "Zalmcode";
            this.Zalmcode.Size = new System.Drawing.Size(61, 22);
            this.Zalmcode.TabIndex = 307;
            // 
            // VelocityY
            // 
            this.VelocityY.Location = new System.Drawing.Point(137, 92);
            this.VelocityY.Margin = new System.Windows.Forms.Padding(2);
            this.VelocityY.Name = "VelocityY";
            this.VelocityY.Size = new System.Drawing.Size(61, 22);
            this.VelocityY.TabIndex = 312;
            // 
            // VelocityX
            // 
            this.VelocityX.Location = new System.Drawing.Point(65, 92);
            this.VelocityX.Margin = new System.Windows.Forms.Padding(2);
            this.VelocityX.Name = "VelocityX";
            this.VelocityX.Size = new System.Drawing.Size(61, 22);
            this.VelocityX.TabIndex = 308;
            // 
            // labelTorque
            // 
            this.labelTorque.AutoSize = true;
            this.labelTorque.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.labelTorque.Location = new System.Drawing.Point(8, 124);
            this.labelTorque.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelTorque.Name = "labelTorque";
            this.labelTorque.Size = new System.Drawing.Size(42, 12);
            this.labelTorque.TabIndex = 310;
            this.labelTorque.Text = "Torque:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label11.Location = new System.Drawing.Point(8, 95);
            this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(47, 12);
            this.label11.TabIndex = 309;
            this.label11.Text = "Velocity:";
            // 
            // XtextBox
            // 
            this.XtextBox.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.XtextBox.Location = new System.Drawing.Point(65, 34);
            this.XtextBox.Margin = new System.Windows.Forms.Padding(2);
            this.XtextBox.Name = "XtextBox";
            this.XtextBox.Size = new System.Drawing.Size(61, 22);
            this.XtextBox.TabIndex = 157;
            this.XtextBox.Text = "0";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(8, 36);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(44, 20);
            this.label5.TabIndex = 153;
            this.label5.Text = "Postion :";
            // 
            // ZtextBox
            // 
            this.ZtextBox.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.ZtextBox.Location = new System.Drawing.Point(208, 34);
            this.ZtextBox.Margin = new System.Windows.Forms.Padding(2);
            this.ZtextBox.Name = "ZtextBox";
            this.ZtextBox.Size = new System.Drawing.Size(61, 22);
            this.ZtextBox.TabIndex = 150;
            this.ZtextBox.Text = "0";
            // 
            // YtextBox
            // 
            this.YtextBox.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.YtextBox.Location = new System.Drawing.Point(137, 34);
            this.YtextBox.Margin = new System.Windows.Forms.Padding(2);
            this.YtextBox.Name = "YtextBox";
            this.YtextBox.Size = new System.Drawing.Size(61, 22);
            this.YtextBox.TabIndex = 149;
            this.YtextBox.Text = "0";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.jogspeedtextBox);
            this.groupBox2.Controls.Add(this.joglessbtn);
            this.groupBox2.Controls.Add(this.jogpulsebtn);
            this.groupBox2.Controls.Add(this.axiscomboBox);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Location = new System.Drawing.Point(10, 85);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox2.Size = new System.Drawing.Size(277, 80);
            this.groupBox2.TabIndex = 281;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "JOG模式區";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(184, 46);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(36, 12);
            this.label6.TabIndex = 175;
            this.label6.Text = "Speed:";
            // 
            // jogspeedtextBox
            // 
            this.jogspeedtextBox.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.jogspeedtextBox.Location = new System.Drawing.Point(229, 43);
            this.jogspeedtextBox.Margin = new System.Windows.Forms.Padding(2);
            this.jogspeedtextBox.Name = "jogspeedtextBox";
            this.jogspeedtextBox.Size = new System.Drawing.Size(38, 22);
            this.jogspeedtextBox.TabIndex = 174;
            this.jogspeedtextBox.Text = "20.0";
            this.jogspeedtextBox.TextChanged += new System.EventHandler(this.jogspeedtextBox_TextChanged_1);
            // 
            // joglessbtn
            // 
            this.joglessbtn.BackColor = System.Drawing.Color.White;
            this.joglessbtn.Location = new System.Drawing.Point(94, 18);
            this.joglessbtn.Margin = new System.Windows.Forms.Padding(2);
            this.joglessbtn.Name = "joglessbtn";
            this.joglessbtn.Size = new System.Drawing.Size(86, 55);
            this.joglessbtn.TabIndex = 163;
            this.joglessbtn.Text = "JOG-";
            this.joglessbtn.UseVisualStyleBackColor = false;
            this.joglessbtn.MouseDown += new System.Windows.Forms.MouseEventHandler(this.joglessbtn_MouseDown);
            this.joglessbtn.MouseUp += new System.Windows.Forms.MouseEventHandler(this.joglessbtn_MouseUp);
            // 
            // jogpulsebtn
            // 
            this.jogpulsebtn.BackColor = System.Drawing.Color.White;
            this.jogpulsebtn.Location = new System.Drawing.Point(4, 18);
            this.jogpulsebtn.Margin = new System.Windows.Forms.Padding(2);
            this.jogpulsebtn.Name = "jogpulsebtn";
            this.jogpulsebtn.Size = new System.Drawing.Size(86, 55);
            this.jogpulsebtn.TabIndex = 161;
            this.jogpulsebtn.Text = "JOG+";
            this.jogpulsebtn.UseVisualStyleBackColor = false;
            this.jogpulsebtn.MouseDown += new System.Windows.Forms.MouseEventHandler(this.jogpulsebtn_MouseDown);
            this.jogpulsebtn.MouseUp += new System.Windows.Forms.MouseEventHandler(this.jogpulsebtn_MouseUp);
            // 
            // axiscomboBox
            // 
            this.axiscomboBox.BackColor = System.Drawing.SystemColors.Window;
            this.axiscomboBox.ForeColor = System.Drawing.SystemColors.WindowText;
            this.axiscomboBox.Items.AddRange(new object[] {
            "X",
            "Y",
            "Z"});
            this.axiscomboBox.Location = new System.Drawing.Point(229, 18);
            this.axiscomboBox.Margin = new System.Windows.Forms.Padding(2);
            this.axiscomboBox.Name = "axiscomboBox";
            this.axiscomboBox.Size = new System.Drawing.Size(38, 20);
            this.axiscomboBox.TabIndex = 161;
            this.axiscomboBox.Text = "X";
            this.axiscomboBox.SelectedIndexChanged += new System.EventHandler(this.axiscomboBox_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(186, 20);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 12);
            this.label7.TabIndex = 162;
            this.label7.Text = "Axis:";
            // 
            // gohomebtn
            // 
            this.gohomebtn.BackColor = System.Drawing.Color.White;
            this.gohomebtn.Location = new System.Drawing.Point(105, 18);
            this.gohomebtn.Margin = new System.Windows.Forms.Padding(2);
            this.gohomebtn.Name = "gohomebtn";
            this.gohomebtn.Size = new System.Drawing.Size(86, 55);
            this.gohomebtn.TabIndex = 162;
            this.gohomebtn.Text = "三軸原點復歸";
            this.gohomebtn.UseVisualStyleBackColor = false;
            this.gohomebtn.Click += new System.EventHandler(this.gohomebtn_Click_1);
            // 
            // jogmodebtn
            // 
            this.jogmodebtn.BackColor = System.Drawing.Color.White;
            this.jogmodebtn.Location = new System.Drawing.Point(195, 18);
            this.jogmodebtn.Margin = new System.Windows.Forms.Padding(2);
            this.jogmodebtn.Name = "jogmodebtn";
            this.jogmodebtn.Size = new System.Drawing.Size(86, 55);
            this.jogmodebtn.TabIndex = 157;
            this.jogmodebtn.Text = "移至換刀點";
            this.jogmodebtn.UseVisualStyleBackColor = false;
            this.jogmodebtn.Click += new System.EventHandler(this.jogmodebtn_Click);
            // 
            // startbtn
            // 
            this.startbtn.BackColor = System.Drawing.Color.White;
            this.startbtn.Location = new System.Drawing.Point(14, 18);
            this.startbtn.Margin = new System.Windows.Forms.Padding(2);
            this.startbtn.Name = "startbtn";
            this.startbtn.Size = new System.Drawing.Size(86, 55);
            this.startbtn.TabIndex = 154;
            this.startbtn.Text = "System Start";
            this.startbtn.UseVisualStyleBackColor = false;
            this.startbtn.Click += new System.EventHandler(this.startbtn_Click);
            // 
            // feedtextBox
            // 
            this.feedtextBox.Location = new System.Drawing.Point(85, 27);
            this.feedtextBox.Margin = new System.Windows.Forms.Padding(2);
            this.feedtextBox.Name = "feedtextBox";
            this.feedtextBox.Size = new System.Drawing.Size(39, 22);
            this.feedtextBox.TabIndex = 289;
            this.feedtextBox.Text = "10";
            this.feedtextBox.TextChanged += new System.EventHandler(this.feedtextBox_TextChanged_1);
            // 
            // label19
            // 
            this.label19.Location = new System.Drawing.Point(11, 30);
            this.label19.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(71, 18);
            this.label19.TabIndex = 288;
            this.label19.Text = "SetFeedSpee:  ";
            this.label19.Click += new System.EventHandler(this.label19_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label19);
            this.groupBox4.Controls.Add(this.feedtextBox);
            this.groupBox4.Controls.Add(this.test2);
            this.groupBox4.Controls.Add(this.stopspbtn);
            this.groupBox4.Controls.Add(this.test);
            this.groupBox4.Controls.Add(this.workposbtn);
            this.groupBox4.Location = new System.Drawing.Point(681, 11);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox4.Size = new System.Drawing.Size(146, 154);
            this.groupBox4.TabIndex = 304;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Text";
            // 
            // timer2
            // 
            this.timer2.Interval = 150;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(834, 579);
            this.Controls.Add(this.startbtn);
            this.Controls.Add(this.jogmodebtn);
            this.Controls.Add(this.gohomebtn);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.clearpicture);
            this.Controls.Add(this.pictureBox1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "EMP-CNC";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load_1);
            this.Click += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button clearpicture;
        private System.Windows.Forms.Button stopspbtn;
        private System.Windows.Forms.Button workposbtn;
        private System.Windows.Forms.Button test2;
        private System.Windows.Forms.Button test;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox XtextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox ZtextBox;
        private System.Windows.Forms.TextBox YtextBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox jogspeedtextBox;
        private System.Windows.Forms.Button joglessbtn;
        private System.Windows.Forms.Button jogpulsebtn;
        private System.Windows.Forms.ComboBox axiscomboBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button gohomebtn;
        private System.Windows.Forms.Button jogmodebtn;
        private System.Windows.Forms.Button startbtn;
        private System.Windows.Forms.TextBox feedtextBox;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox Xalmcode;
        private System.Windows.Forms.TextBox TorqueZ;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox TorqueY;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TorqueX;
        private System.Windows.Forms.TextBox Yalmcode;
        private System.Windows.Forms.TextBox VelocityZ;
        private System.Windows.Forms.TextBox Zalmcode;
        private System.Windows.Forms.TextBox VelocityY;
        private System.Windows.Forms.TextBox VelocityX;
        private System.Windows.Forms.Label labelTorque;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button okbtn;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox Vtb3;
        private System.Windows.Forms.TextBox Vtb0;
        private System.Windows.Forms.TextBox Vtb1;
        private System.Windows.Forms.TextBox Vtb2;
    }
}

