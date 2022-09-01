using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Data.SqlClient;
//導入Socket
using System.Net.Sockets;
using System.Net;

namespace Surface_AR_Viewer
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        SqlConnection conn;
        Socket clientSocket;
        private void Form2_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection("data source=140.116.86.220; user id = sa; password = password");
            conn.Open();

            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); //創建一個Socket物件
            clientSocket.Connect(new IPEndPoint(IPAddress.Parse("140.116.86.220"), 7003)); //連線到Server

            set_CNC_Window();
            set_Log_Window();
        }

        private void set_CNC_Window()
        {
            //讀取
            string sql_cmd = @"
                              USE Fog_Database;
                              SELECT * FROM CNC_Sensing_Data;
                              ";

            SqlCommand cmd = new SqlCommand(sql_cmd, conn);
            SqlDataReader dr = cmd.ExecuteReader();

            string[] data_title = {"number", "time", "x_vibration", "y_vibration", "z_vibration", "s_current", "x_loc", "y_loc", "z_loc", 
                                   "x_velocity", "y_velocity", "z_velocity", "x_torque", "y_torque", "z_torque", "x_errcode", "y_errcode", "z_errcode", "Alarm"};
            while (dr.Read())
            {
                ListViewItem lvi = new ListViewItem();
                foreach (string title in data_title)
                {
                    lvi.SubItems.Add(dr[title].ToString());
                }
                CNC_Sensing_Data.Items.Add(lvi);
            }
            dr.Close();
        }

        private void set_Log_Window()
        {
            //讀取
            string sql_cmd = @"
                              USE Fog_Database;
                              SELECT * FROM Operate_Log;
                              ";

            SqlCommand cmd = new SqlCommand(sql_cmd, conn);
            SqlDataReader dr = cmd.ExecuteReader();

            string[] data_title = { "number", "time", "file_name", "tag" };
            while (dr.Read())
            {
                ListViewItem lvi = new ListViewItem();
                foreach (string title in data_title)
                {
                    lvi.SubItems.Add(dr[title].ToString());
                }
                Operate_Log.Items.Add(lvi);
            }
            dr.Close();
        }

        private void close_btn_Click(object sender, EventArgs e)
        {
            conn.Close(); //斷開跟SQL Server的連線
            clientSocket.Close(); //斷開跟Socket Server的連線
            this.Close(); //關閉這個視窗
        }

        private void Next_Page_btn_Click(object sender, EventArgs e)
        {
            change_page();
        }

        private void Previous_Page_bnt_Click(object sender, EventArgs e)
        {
            change_page();
        }

        private void change_page()
        {
            if (Page_Tag.Text == "CNC Sensing Data")
            {
                Page_Tag.Text = "Operate Log";
                CNC_Sensing_Data.Visible = false;
                Operate_Log.Visible = true;
            }
            else if (Page_Tag.Text == "Operate Log")
            {
                Page_Tag.Text = "CNC Sensing Data";
                CNC_Sensing_Data.Visible = true;
                Operate_Log.Visible = false;
            }
        }

        private void ok_btn_Click(object sender, EventArgs e)
        {
            if (Page_Tag.Text == "Operate Log") //如果有選到Operate Log的資料，案OK後就傳送檔名到Fog Node進行渲染
            {
                string xml_file = Operate_Log.FocusedItem.SubItems[3].Text;
                byte[] bytes = Encoding.UTF8.GetBytes(xml_file);
                clientSocket.Send(bytes);
                Console.WriteLine(xml_file);
            }
            conn.Close(); //斷開跟SQL Server的連線
            clientSocket.Close(); //斷開跟Socket Server的連線
            this.Close(); //關閉這個視窗
        }
    }
}
