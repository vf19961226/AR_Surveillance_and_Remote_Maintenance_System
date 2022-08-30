using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using NAudio.Wave;
using System.IO;

//導入Socket
using System.Net.Sockets;
using System.Net;

namespace Voice_Chat2
{
    public partial class Form1 : Form
    {
        public WaveIn waveSource = null;
        //public WaveFileWriter waveFile = null;

        Socket clientSocket;

        public Form1()
        {
            InitializeComponent();
        }

        public void StartRec()
        {
            try
            {
                waveSource = new WaveIn();//保證電腦有麥克接入否則報錯。
                waveSource.WaveFormat = new WaveFormat(16000, 16, 1); // 16KHz，16bit,單聲道的錄音格式

                waveSource.DataAvailable += new EventHandler<WaveInEventArgs>(waveSource_DataAvailable);
                waveSource.RecordingStopped += new EventHandler<StoppedEventArgs>(waveSource_RecordingStopped);

                //waveFile = new WaveFileWriter(fileName, waveSource.WaveFormat);

                waveSource.StartRecording();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private void waveSource_DataAvailable(object sender, WaveInEventArgs e)
        {
            /*
            if (waveFile != null)
            {
                //waveFile.Write(e.Buffer, 0, e.BytesRecorded);
                //waveFile.Flush();

                Console.WriteLine(e.Buffer);
            }
            Console.WriteLine(e.BytesRecorded);

            MemoryStream ms = new MemoryStream(e.Buffer);
            IWaveProvider reader = new RawSourceWaveStream(ms, new WaveFormat(16000, 16, 1));
            WaveOut waveOut = new WaveOut();
            waveOut.Init(reader);
            waveOut.Play();
            */
            try
            {
                clientSocket.Send(e.Buffer);
            }
            catch
            {

            }
        }

        private void waveSource_RecordingStopped(object sender, StoppedEventArgs e)
        {
            if (waveSource != null)
            {
                waveSource.Dispose();
                waveSource = null;
            }
            /*
            if (waveFile != null)
            {
                waveFile.Dispose();
                waveFile = null;
            }
            */
            play_voice.CancelAsync();
            clientSocket.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); //創建一個Socket物件
            clientSocket.Connect(new IPEndPoint(IPAddress.Parse("140.116.86.220"), 7002)); //連線到Server

            play_voice.RunWorkerAsync();
            StartRec();
            button1.Text = "Calling...";
        }

        private void play_voice_DoWork(object sender, DoWorkEventArgs e)
        {
            WaveOut waveOut = new WaveOut();
            while (true)
            {
                try
                {
                    byte[] date = new byte[3200];
                    int count = clientSocket.Receive(date);

                    MemoryStream ms = new MemoryStream(date);
                    IWaveProvider reader = new RawSourceWaveStream(ms, new WaveFormat(16000, 16, 1));
                    waveOut.Init(reader);
                    waveOut.Play();
                }
                catch
                {
                    clientSocket.Close();
                    break;
                }
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            //button1.PerformClick();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (waveSource != null)
            {
                waveSource.Dispose();
                waveSource = null;
            }

            play_voice.CancelAsync();
            clientSocket.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (waveSource != null)
            {
                waveSource.Dispose();
                waveSource = null;
            }

            button1.Text = "Call";
            play_voice.CancelAsync();
            clientSocket.Close();
            Application.Exit();
        }
    }
}
