using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//導入Socket
using System.Net.Sockets;
using System.Net;
//導入OpenCV
using OpenCvSharp;
//導入NAudio
using NAudio.Wave;
using System.IO;

namespace Surface_AR_Viewer
{
    public partial class AR_viewer : Form
    {
        public AR_viewer()
        {
            InitializeComponent();
        }

        Socket clientSocket;
        Socket clientSocket2;
        public WaveIn waveSource = null;
        VideoCapture cap;

        private void get_cam_img_DoWork(object sender, DoWorkEventArgs e)
        {
            Mat img = new Mat();
            while (true)
            {
                cap.Read(img);
                if (img.Empty())
                {
                    continue;
                }
                /* 測試相機
                Window cam = new Window("Camera", img);
                if (Cv2.WaitKey(1)==27 || Cv2.WaitKey(1) == 32)
                {
                    break;
                }
                */
                byte[] send_img = img.ImEncode();
                clientSocket.Send(send_img);
            }
        }

        private void conn2server_btn_Click(object sender, EventArgs e)
        {
            cap = new VideoCapture(0);
            cap.Open(1, VideoCaptureAPIs.DSHOW);
            cap.FrameHeight = 720;
            cap.FrameWidth = 1280;
            cap.Fps = 30;
            
            var v = OpenCvSharp.FourCC.FromString("MJPG");
            cap.Set(VideoCaptureProperties.FourCC, v.Value);
            
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); //創建一個Socket物件
            clientSocket.Connect(new IPEndPoint(IPAddress.Parse("140.116.86.220"), 7000)); //連線到Server

            get_cam_img.RunWorkerAsync();
            show_img.RunWorkerAsync();
        }

        private void disconn2server_btn_Click(object sender, EventArgs e)
        {
            get_cam_img.CancelAsync();
            show_img.CancelAsync();

            clientSocket.Close();
            Application.Exit();
        }

        private void show_img_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                try
                {
                    byte[] date = new byte[2000000];
                    int count = clientSocket.Receive(date);
                    ImreadModes mode = ImreadModes.Color;
                    Mat image = Cv2.ImDecode(date, mode);
                    Bitmap bit_img = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(image);
                    pictureBox1.Image = bit_img;
                }
                catch
                {

                }
            }
        }

        private bool help = false;
        private void help_btn_Click(object sender, EventArgs e)
        {
            if (!help)
            {
                clientSocket2 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); //創建一個Socket物件
                clientSocket2.Connect(new IPEndPoint(IPAddress.Parse("140.116.86.220"), 7002)); //連線到Server

                play_voice.RunWorkerAsync();

                StartRec();
                help_btn.Text = "Exit Help";
                help = true;
            }
            else if(help)
            {
                play_voice.CancelAsync();

                StopRec();
                help_btn.Text = "Help";
                help = false;
            }
        }

        public void StartRec()
        {
            try
            {
                waveSource = new WaveIn();//保證電腦有麥克接入否則報錯。
                waveSource.WaveFormat = new WaveFormat(16000, 16, 1); // 16KHz，16bit,單聲道的錄音格式

                waveSource.DataAvailable += new EventHandler<WaveInEventArgs>(waveSource_DataAvailable);

                waveSource.StartRecording();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void StopRec()
        {
            if (waveSource != null)
            {
                waveSource.Dispose();
                waveSource = null;
            }

            play_voice.CancelAsync();
            clientSocket2.Close();
        }

        private void waveSource_DataAvailable(object sender, WaveInEventArgs e)
        { 
            clientSocket2.Send(e.Buffer);
        }

        private void play_voice_DoWork(object sender, DoWorkEventArgs e)
        {
            WaveOut waveOut = new WaveOut();
            while (true)
            {
                try
                {
                    byte[] date = new byte[3200];
                    int count = clientSocket2.Receive(date);

                    MemoryStream ms = new MemoryStream(date);
                    IWaveProvider reader = new RawSourceWaveStream(ms, new WaveFormat(16000, 16, 1));
                    waveOut.Init(reader);
                    waveOut.Play();
                }
                catch
                {
                    clientSocket2.Close();
                    break;
                }
                
            }
        }

        private void data_btn_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            f2.Visible = true;
        }
    }
}
