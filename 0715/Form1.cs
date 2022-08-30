using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EtherCATSeries;
using Automation.BDaq; //加入
using LBSoft.IndustrialCtrls.Buttons;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Globalization;
using System.Threading;

using System.IO.Ports;              // for SerialPort class

namespace _0715
{
    using Opc.UaFx;
    using Opc.UaFx.Client;
    public partial class Form1 : Form
	{
		bool hanmode = false;
		double dfXPos = 0;
		double dfYPos = 0;
		double dfZPos = 0;
		double dfUPos = 0;
		double dfVPos = 0;
		double dfWPos = 0;
		double dfAPos = 0;
		double dfBPos = 0;
		public int jogaxis=0, fun=0;
		//基本參數
		int setacctime = 300;
		int setdectime = 300;
		int feedspeed = 10;
		int overrideS = 10;
		int ptpspeed = 10;
		int jogpulse = 100;
		int jogless = -100;
		int MaxSpeed = 100;
		int handmodebtntime = 0;
		//目標位置參數
		int XG = 0, YG = 0, ZG = 0;

		bool a = false;
		byte cccw = 0;
		int texttime;
		string textfirst;
		//draw
		double XDpostionRead, YDpostionRead, Theta;
		Graphics g;
		static float XfFpostionText, YfFpostionText, XFpostionRead, YFpostionRead;
		float wid, hei;
		Pen pen;
		SolidBrush brush;

		//
		public static long nErrorCount = 0;
		public static long nInputCount0 = 0;
		bool bInit = false;
		byte bSpindle = 0;
		ushort homesin = 0;

		/// <summary>
		/// ///////////////////////
		/// </summary>
		private const uint LightStartBit = 0x001; //Y0
		private const uint LightStopBit = 0x002; //Y1
		private const uint AutoPowerBit = 0x008; //Y3
		private const uint AmpPowerBit = 0x010; //Y4
		private const uint SpindleClampBit = 0x020; //Y5
		private const uint SpindleAirBit = 0x040; //Y6
		private const uint SpindleDirBit = 0x100; //Y8

		private const int StartPBShift = 1;
		private const int StopBShift = 3;
		private const int ToolUnClmPBShift = 4;
		private const int EMGShift = 5;
		private const int InvAlmShift = 6;
		private const int ToolClmShift = 8;
		private const int ToolUnClmShift = 9;

		private uint DIStatus = 0;

		public InstantAiCtrl instantAiCtrl1 = new InstantAiCtrl();
		public FreqMeterCtrl FreqMeterCtrl1 = new FreqMeterCtrl();
		private void EnableDAC(byte byChannel)
		{
			IntPtr Data = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(ushort))); ;

			Marshal.WriteInt16(Data, 1);
			MCCL.MCC_EcatCoeSdoDownload(4, 0x2000, byChannel, Data, 2);

			MCCL.MCC_EcatCoeSdoDownload(4, 0x2001, byChannel, Data, 2);
		}
		private void EnableSpindle(int bEnable)
		{
			uint uCurValue = 0;

			MCCL.MCC_EcatGetOutput(6, ref uCurValue);

			if (bEnable == 1)
				uCurValue |= SpindleDirBit;
			else
				uCurValue &= ~SpindleDirBit;

			MCCL.MCC_EcatSetOutput(6, uCurValue);
		}

		private void changeTool(int bEnable)
		{

			uint uCurValue = 0;
			MCCL.MCC_EcatGetOutput(6, ref uCurValue);
			if (bEnable == 1)
			{
				uCurValue |= SpindleClampBit;
				MCCL.MCC_EcatSetOutput(6, uCurValue);
				Thread.Sleep(100);
				uCurValue |= SpindleAirBit;
				MCCL.MCC_EcatSetOutput(6, uCurValue);
			}
			else
			{
				uCurValue &= ~SpindleAirBit;
				MCCL.MCC_EcatSetOutput(6, uCurValue);
				Thread.Sleep(100);
				uCurValue &= ~SpindleClampBit;
				MCCL.MCC_EcatSetOutput(6, uCurValue);
			}
		}


		private void LightStop(int bEnable)
		{
			uint uCurValue = 0;

			MCCL.MCC_EcatGetOutput(6, ref uCurValue);

			if (bEnable == 1)
				uCurValue |= LightStopBit;
			else
				uCurValue &= ~LightStopBit;

			MCCL.MCC_EcatSetOutput(6, uCurValue);
		}

		private void LightStart(int bEnable)
		{
			uint uCurValue = 0;

			MCCL.MCC_EcatGetOutput(6, ref uCurValue);

			if (bEnable == 1)
				uCurValue |= LightStartBit;
			else
				uCurValue &= ~LightStartBit;

			MCCL.MCC_EcatSetOutput(6, uCurValue);
		}
		private void DetectDIStatus(uint nInput)
		{
			DIStatus = nInput >> StartPBShift;
			DIStatus &= 0x1;
			if (DIStatus == 1)
			{
				Debug.WriteLine("StartPB");
			}

			DIStatus = nInput >> StopBShift;
			DIStatus &= 0x1;
			if (DIStatus == 1)
			{
				Debug.WriteLine("StopPB");
			}

			DIStatus = nInput >> ToolUnClmPBShift;
			DIStatus &= 0x1;
			if (DIStatus == 1)
			{
				changeTool(1);

				Debug.WriteLine("ToolUnClmPB");
			}
			else
			{
				changeTool(0);
			}

		}

		private bool Taggle = true;
		public Form1()
		{
			InitializeComponent();

			MCCL.MCC_StartEcServer();
			jogpulsebtn.Enabled = false;
			joglessbtn.Enabled = false;
			jogmodebtn.Enabled = false;
		}
		private void Form1_Load(object sender, System.EventArgs e)
		{
			
		}
		private SYS_MAC_PARAM stMacParam = new SYS_MAC_PARAM();
		private SYS_ENCODER_CONFIG stENCConfig = new SYS_ENCODER_CONFIG();
		private SYS_CARD_CONFIG stCardConfig = new SYS_CARD_CONFIG();
		int MAX_NUM_OF_DRIVERS = 3;

		int g_nGroupIndex0 = 0;
		int MAX_NUM_OF_GROUP = 1;
		private void startbtn_Click(object sender, EventArgs e)
		{
            instantAiCtrl1.SelectedDevice = new DeviceInformation(0);
            FreqMeterCtrl1.SelectedDevice = new DeviceInformation(0);
            if (!instantAiCtrl1.Initialized || !FreqMeterCtrl1.Initialized)
            {
                MessageBox.Show("\n連線失敗，請檢查設備是否妥善連接", "連線失敗", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            channel.Add(0);
            channel.Add(1);
            channel.Add(2);
            channel.Add(3);
            //instantAiCtrl1.Channels[3].ValueRange = ValueRange.V_Neg5To5;
            instantAiCtrl1.Channels[0].ValueRange = ValueRange.V_Neg5To5;
            instantAiCtrl1.Channels[1].ValueRange = ValueRange.V_Neg5To5;
            instantAiCtrl1.Channels[2].ValueRange = ValueRange.V_Neg5To5;
            instantAiCtrl1.Channels[3].ValueRange = ValueRange.V_Neg5To5;
            //    instantAiCtrl1.Channels[i].ValueRange = ValueRange.V_Neg10To10;
            //    instantAiCtrl1.Channels[i].ValueRange = ValueRange.V_Neg15To15;
            if (!bInit)
			{
				int ret = MCCL.MCC_RtxInit(3);
				if (ret != 0)
				{
					MessageBox.Show("No EcServer!", "Error");
				}
				MCCL.MCC_EcatAutoGenerateENI(1);
				ushort i = 0;
				int nRet = 0;

				if (nRet == MCCL.NO_ERR)
				{
					stMacParam.wPosToEncoderDir = 0;

					stMacParam.dwPPR = 100000;
					stMacParam.wRPM = 3000;
					stMacParam.dfPitch = 5.0;
					stMacParam.dfGearRatio = 1.0;
					stMacParam.dfHighLimit = 900.0;
					stMacParam.dfLowLimit = 0;
					stMacParam.dfHighLimitOffset = 0;
					stMacParam.dfLowLimitOffset = 0;
					stMacParam.wPulseMode = 0;
					stMacParam.wPulseWidth = 100;
					stMacParam.wCommandMode = 0;
					stMacParam.wOverTravelUpSensorMode = 1;//  not checking
					stMacParam.wOverTravelDownSensorMode = 1;


					for (ushort wChannel = 0; wChannel < MAX_NUM_OF_DRIVERS; wChannel++)
					{
						if (wChannel == 2)
						{
							stMacParam.dfPitch = 10.0;
							stMacParam.dfHighLimit = 290.0;
						}
						if (wChannel == 1)
							stMacParam.dfHighLimit = 550.0;
						MCCL.MCC_SetMacParam(ref stMacParam, wChannel);      //  mechanism parameters are the same for all axes
						MCCL.MCC_SetEncoderConfig(ref stENCConfig, wChannel);//  encoder configures are the same for all axes
					}
					g_nGroupIndex0 = MCCL.MCC_CreateGroup(0, 1, 2, -1, -1, -1, -1, -1);

					stCardConfig.wCardType = 4;

					nRet = MCCL.MCC_InitSystem(1);               //  only use one card
																 //nRet = MCCL.MCC_InitSimulation(1);

					if (nRet < 0)
					{
						MessageBox.Show("Other Error");
						return;
					}
					for (i = 0; i < MAX_NUM_OF_GROUP; i++)
					{
						MCCL.MCC_SetAbsolute(i);
						MCCL.MCC_SetAccTime(500, i);//  set accleration time to be 300 ms
						MCCL.MCC_SetDecTime(500, i);//  set decleration time to be 300 ms
						MCCL.MCC_SetFeedSpeed(feedspeed, i);//  set line, arc and circle motion's feed rate (unit : mm/sec)
						MCCL.MCC_SetPtPAccTime(300, 300, 300, 0, 0, 0, 0, 0, 0);
						MCCL.MCC_SetPtPDecTime(300, 300, 300, 100, 0, 0, 0, 0, 0);
					}

					if (nRet == 0)
					{
						//  set channel 0 servv on 
						//Sleep(50000);
					}

					//Thread.Sleep(3000);

					MCCL.MCC_EcatSetOutput(6, AmpPowerBit);
					Thread.Sleep(1000);
					for (i = 0; i < MAX_NUM_OF_DRIVERS; i++)
						MCCL.MCC_SetServoOn(i);

					timer1.Start();
					bInit = true;
					EnableDAC(1);
					jogmodebtn.Enabled = true;
				}
			}
			else
			{
				bInit = false;
				MCCL.MCC_EcatSetOutput(6, 0);
				Thread.Sleep(1000);
				MCCL.MCC_CloseSystem();
				timer1.Stop();
				//startbtn.Enabled = false;
			}
			
		}

		int nStatus = 0;
		int home = 0;

		double v1 = 0.0;
		double v2 = 0;
		double v3 = 0;
		double spi = 0;
		List<int> channel = new List<int>();

        private void timer_GetData_Tick()
        {
            instantAiCtrl1.Read(channel[0], out v1);
            instantAiCtrl1.Read(channel[1], out v2);
            instantAiCtrl1.Read(channel[2], out v3);
            instantAiCtrl1.Read(channel[3], out spi);
            Vtb0.Text = Convert.ToString(v1);
            Vtb1.Text = Convert.ToString(v2);
            Vtb2.Text = Convert.ToString(v3);
            Vtb3.Text = Convert.ToString(spi);
        }
        ushort errortype = 1;
		ushort errorbufferX = 0x603F;
		ushort errorbufferY = 0x603F;
		ushort errorbufferZ = 0x603F;
		int velX = 0, velY = 0, velZ = 0;
		int torX = 0, torY = 0, torZ = 0;
		private void timer1_Tick(object sender, System.EventArgs e)
		{

			uint nInput = 0;
			MCCL.MCC_GetCurPos(ref dfXPos, ref dfYPos, ref dfZPos, ref dfUPos, ref dfVPos, ref dfWPos, ref dfAPos, ref dfBPos, 0);
			XtextBox.Text = dfXPos.ToString();
			YtextBox.Text = dfYPos.ToString();
			ZtextBox.Text = dfZPos.ToString();
			pen = new Pen(Color.Black);
			brush = new SolidBrush(pen.Color);
			wid = pictureBox1.Width;
			hei = pictureBox1.Height;
			nStatus = MCCL.MCC_EcatGetGoHomeStatusEx(1); // 0:homing,  1:done
			if (nStatus == 0)
			{
				gohomebtn.Text = "原點復歸中";
				home = 1;
			}
			else if (nStatus == 1 && home==1  )
			{
				gohomebtn.Text = "三軸原點復歸";
				home = 2;
				jogmodebtn.Enabled = true;
			}
			XDpostionRead = dfXPos;
			YDpostionRead = dfYPos;
			XfFpostionText = float.Parse(XtextBox.Text);
			YfFpostionText = float.Parse(YtextBox.Text);

			g = pictureBox1.CreateGraphics();
			pen = new Pen(Color.Red, 5);

			g.DrawLine(pen, -1000, 0, 1000, 0);
			g.DrawLine(pen, 0, -1000, 0, 1000);
			if ( home==2)
			{
				PointF b0 = new PointF(XfFpostionText, YfFpostionText);
				PointF b1 = new PointF(XFpostionRead, YFpostionRead);
				g.DrawLine(pen, b0, b1);
				XFpostionRead = Convert.ToSingle(XDpostionRead);
				YFpostionRead = Convert.ToSingle(YDpostionRead);

			}
			MCCL.MCC_EcatGetInput(5, ref nInput);
			//Console.WriteLine("0x" + nInput);
			DetectDIStatus(nInput);

			if (nInput != 65535)
				Debug.WriteLine("Diff");

			//int ErrorStatus = MCCL.MCC_GetErrorCode();
			MCCL.MCC_EcatGetMotorVelocity(ref velX, 0);
			MCCL.MCC_EcatGetMotorVelocity(ref velY, 1);
			MCCL.MCC_EcatGetMotorVelocity(ref velZ, 2);

			VelocityX.Text = velX.ToString();
			VelocityY.Text = velY.ToString();
			VelocityZ.Text = velZ.ToString();


			MCCL.MCC_EcatGetMotorTorque(ref torX, 0);
			MCCL.MCC_EcatGetMotorTorque(ref torY, 1);
			MCCL.MCC_EcatGetMotorTorque(ref torZ, 2);

			TorqueX.Text = torX.ToString();
			TorqueY.Text = torY.ToString();
			TorqueZ.Text = torZ.ToString();


			MCCL.MCC_EcatGetSlaveErrorCode(ref errortype, ref errorbufferX, 0);
			MCCL.MCC_EcatGetSlaveErrorCode(ref errortype, ref errorbufferY, 1);
			MCCL.MCC_EcatGetSlaveErrorCode(ref errortype, ref errorbufferZ, 2);
			Xalmcode.Text = errorbufferX.ToString();
			Yalmcode.Text = errorbufferY.ToString();
			Zalmcode.Text = errorbufferZ.ToString();

			ushort wInput0 = 0;
			ushort wOutput = 0xFFFF; // IO output
			timer_GetData_Tick();
		}

		private void feedtextBox_TextChanged_1(object sender, EventArgs e)
		{
            if (feedtextBox.Text != "") { 
			feedspeed = Convert.ToInt32(feedtextBox.Text);
			MCCL.MCC_SetFeedSpeed(feedspeed, 0);}
		}
        private void clearerrorbtn_Click(object sender, EventArgs e)
        {
			MCCL.MCC_ClearError();
		}

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
			uint uCurValue = 0;
			MCCL.MCC_EcatSetOutput(6, uCurValue);
			MCCL.MCC_RtxClose();
		}


		public double jogspeed=20.0;

		private void jogpulsebtn_MouseDown(object sender, MouseEventArgs e)
        {
            byte a;
            uint b;
            b = Convert.ToUInt32(jogaxis);
            a = Convert.ToByte(b);
            MCCL.MCC_JogConti(1, jogspeed, a, 0);

        }

		private void jogpulsebtn_MouseUp(object sender, MouseEventArgs e)
		{
			MCCL.MCC_AbortMotionEx(200, 0);
		}

		private void joglessbtn_MouseDown(object sender, MouseEventArgs e)
        {
            byte c;
            uint d;
            d = Convert.ToUInt32(jogaxis);
            c = Convert.ToByte(d);
            MCCL.MCC_JogConti(-1, jogspeed, c, 0);

        }
		private void joglessbtn_MouseUp(object sender, MouseEventArgs e)
		{
			MCCL.MCC_AbortMotionEx(200, 0);

		}
		public double workx ;
		public double worky ;
		public double workz ;

        private void jogspeedtextBox_TextChanged_1(object sender, EventArgs e)
        {

			jogspeed = Convert.ToDouble(jogspeedtextBox.Text);
		}

        private void axiscomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
			jogaxis = axiscomboBox.SelectedIndex;
		}


        OpcClient fog_server = new OpcClient("opc.tcp://140.116.86.220:4840/");
		bool haveconnect = false;

        private void TorqueZ_TextChanged(object sender, EventArgs e)
        {

        }

        private void okbtn_Click(object sender, EventArgs e)
		{
			if (haveconnect == false)
			{
				fog_server.Connect();
				timer2.Enabled = true;
			}else if (haveconnect == true)
            {
				fog_server.Disconnect();
				timer2.Enabled = false;
            }
		}
		private void timer2_Tick(object sender, EventArgs e)
        {
            //Console.WriteLine("Connect to fog server~~");
            OpcStatus x = fog_server.WriteNode("ns=3;i=1", v1);
            OpcStatus y = fog_server.WriteNode("ns=3;i=2", v2);
            OpcStatus z = fog_server.WriteNode("ns=3;i=3", v3);
            OpcStatus x0 = fog_server.WriteNode("ns=3;i=4", spi);
            OpcStatus y0 = fog_server.WriteNode("ns=4;i=1", dfXPos);
            OpcStatus z0 = fog_server.WriteNode("ns=4;i=2", dfYPos);
            OpcStatus x1 = fog_server.WriteNode("ns=4;i=3", dfZPos);
            OpcStatus y1 = fog_server.WriteNode("ns=5;i=1", velX);
            OpcStatus z1 = fog_server.WriteNode("ns=5;i=2", velY);
            OpcStatus x2 = fog_server.WriteNode("ns=5;i=3", velZ);
            OpcStatus y2 = fog_server.WriteNode("ns=6;i=1", torX);
            OpcStatus z2 = fog_server.WriteNode("ns=6;i=2", torY);
            OpcStatus x3 = fog_server.WriteNode("ns=6;i=3", torZ);
            OpcStatus y3 = fog_server.WriteNode("ns=7;i=1", errorbufferX);
            OpcStatus z3 = fog_server.WriteNode("ns=7;i=2", errorbufferY);
            OpcStatus z4 = fog_server.WriteNode("ns=7;i=3", errorbufferZ);
			//Console.WriteLine("Send value to fog server~~");
			//Console.WriteLine(dfXPos);
            //fog_server.Disconnect();
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {

        }

        private void clearpicture_Click(object sender, EventArgs e)
        {
			pictureBox1.Image = null;
		}

		bool havehome =false;
        private void gohomebtn_Click_1(object sender, EventArgs e)
        {
			
			if (havehome == false)
			{
				jogpulsebtn.Enabled = false;
				joglessbtn.Enabled = false;
				jogmodebtn.Enabled = false;
				MCCL.MCC_EcatSetHomeMode(1, 0);
				MCCL.MCC_EcatSetHomeSwitchSpeed(10000, 0);
				MCCL.MCC_EcatSetHomeZeroSpeed(5000, 0);
				MCCL.MCC_EcatHomeEx(0);

				//Y
				MCCL.MCC_EcatSetHomeMode(1, 1);
				MCCL.MCC_EcatSetHomeSwitchSpeed(10000, 1);
				MCCL.MCC_EcatSetHomeZeroSpeed(5000, 1);
				MCCL.MCC_EcatHomeEx(1);

				//Z
				MCCL.MCC_EcatSetHomeMode(1, 2);
				MCCL.MCC_EcatSetHomeSwitchSpeed(5000, 2);
				MCCL.MCC_EcatSetHomeZeroSpeed(2500, 2);
				MCCL.MCC_EcatHomeEx(2);
				havehome = true;
			}
			else
			{
				MCCL.MCC_Line(0, 0, 0 , 0, 0, 0, 0, 0);
			}
		}


		/// /////////////////////////////////////////////////
		private void workposbtn_Click(object sender, EventArgs e)
		{
			workx = dfXPos;
			worky = dfYPos;
			workz = dfZPos;
		}

		private void stopspbtn_Click(object sender, EventArgs e)
        {
				EnableSpindle(0);
				MCCL.MCC_AbortMotionEx(0.2,0);
				LightStop(1);
				LightStart(0);
		}

		private void label19_Click(object sender, EventArgs e)
        {

        }

        private void test_Click(object sender, EventArgs e)
        {
			LightStop(0);
			workposbtn.Enabled = false;
			MCCL.MCC_Line(workx, worky, workz - 10, 0, 0, 0, 0, 0);
			MCCL.MCC_LineZ(workz);
			uint uCurValue = 0;
			MCCL.MCC_EcatSetDacOutputValue(4, 0, 1.5f);
			MCCL.MCC_EcatGetOutput(6, ref uCurValue);
			uCurValue |= SpindleDirBit;
			uCurValue |= LightStartBit;
			MCCL.MCC_EcatSetOutputEnqueue(6, uCurValue, 0, 0);

			int k = 0;

			if (k == 0)
			{

				for (int i0 = 1; i0 <= 2; i0++)
				{
					MCCL.MCC_LineZ(workz + i0);
					for (int i = 0; i < 15; i++)
					{
						MCCL.MCC_LineX(workx + 15);
						MCCL.MCC_LineY(worky + i);
						MCCL.MCC_LineX(workx);
						MCCL.MCC_LineY(worky + i);
					}
					if (i0 != 2)
					{
						MCCL.MCC_LineZ(workz);
						MCCL.MCC_LineX(workx);
						MCCL.MCC_LineY(worky);
					}
				}
				k = 1;
			}
			if (k == 1)
			{
				MCCL.MCC_LineZ(workz - 10);
				MCCL.MCC_LineY(worky + 30);
				MCCL.MCC_LineZ(workz + 1);
				k = 2;
			}
			if (k == 2)
			{
				for (int i1 = 1; i1 <= 2; i1++)
				{
					MCCL.MCC_LineZ(workz + i1);
					for (int i = 0; i < 15; i++)
					{

						MCCL.MCC_ArcThetaXY(workx, worky, -90, 0);
						MCCL.MCC_ArcThetaXY(workx, worky, 90, 0);
						MCCL.MCC_LineY(worky + 30 + i);
					}
					if (i1 != 2)
					{
						MCCL.MCC_LineZ(workz);
						MCCL.MCC_LineX(workx);
						MCCL.MCC_LineY(worky + 30);
					}
				}
				k = 3;
			}
			if (k == 3)
			{
				MCCL.MCC_LineZ(workz + 1);
				for (int i = 1; i <= 2; i++)
				{
					MCCL.MCC_LineY(worky + 60);
					MCCL.MCC_LineX(workx + 60);
					MCCL.MCC_LineY(worky);
					MCCL.MCC_LineX(workx);
					MCCL.MCC_LineZ(workz + 1 + i);
				}
				k = 4;
			}
			if (k == 4)
			{
                uint uCurValue0 = 0;
                MCCL.MCC_EcatGetOutput(6, ref uCurValue0);
                uCurValue0 &= ~SpindleDirBit;
                uCurValue0 &= ~LightStartBit;
                uCurValue0 |= LightStopBit;
                MCCL.MCC_EcatSetOutputEnqueue(6, uCurValue0, 0, 0);
                MCCL.MCC_LineZ(workz - 20);
            }
            workposbtn.Enabled = true;
        }
		
		private void test2_Click(object sender, EventArgs e)
        {
			workposbtn.Enabled = false;
			int k = 0;
			MCCL.MCC_Line(workx, worky, workz - 10, 0, 0, 0, 0, 0);
			MCCL.MCC_LineZ(workz);
			LightStop(0);
			uint uCurValue = 0;
			MCCL.MCC_EcatGetOutput(6, ref uCurValue);
			uCurValue |= SpindleDirBit;
			uCurValue |= LightStartBit;
			MCCL.MCC_EcatSetOutputEnqueue(6, uCurValue,0,0);

			MCCL.MCC_EcatSetDacOutputValue(4, 0, 1.5f);
			if (k == 0)
			{
				for (int i0 = 1; i0 <= 2; i0++)
				{
					MCCL.MCC_LineZ(workz + i0);
					for (int i = 0; i < 10; i++)
					{
						MCCL.MCC_LineX(workx + 60);
						MCCL.MCC_LineY(worky + i);
						MCCL.MCC_LineX(workx);
						MCCL.MCC_LineY(worky + i);
					}
					if (i0 != 2)
					{
						MCCL.MCC_LineZ(workz);
						MCCL.MCC_LineX(workx);
						MCCL.MCC_LineY(worky);
					}
				}
				k = 1;
			}
			if (k == 1)
			{
				MCCL.MCC_LineZ(workz - 10);
				MCCL.MCC_LineY(worky + 20);
				MCCL.MCC_LineZ(workz);
				k = 2;
			}
			if (k == 2)
			{
				for (int i1 = 1; i1 <= 2; i1++)
				{
					MCCL.MCC_LineZ(workz + i1);
					for (int i = 0; i < 10; i++)
					{
						MCCL.MCC_LineX(workx + 60);
						MCCL.MCC_LineY(worky + 20 + i);
						MCCL.MCC_LineX(workx);
						MCCL.MCC_LineY(worky + 20 + i);
					}
					if (i1 != 2)
					{
						MCCL.MCC_LineZ(workz);
						MCCL.MCC_LineX(workx);
						MCCL.MCC_LineY(worky + 20);
					}
				}
				k = 3;
			}
			if (k == 3)
			{
				MCCL.MCC_LineZ(workz - 10);
				MCCL.MCC_LineX(workx + 25);
				k = 4;
			}
			if (k == 4)
			{
				for (int i2 = 1; i2 <= 2; i2++)
				{
					MCCL.MCC_LineZ(workz + i2);
					for (int i = 0; i < 20; i++)
					{
						MCCL.MCC_LineX(workx + 25);
						MCCL.MCC_LineY(worky + 30 + i);
						MCCL.MCC_LineX(workx + 35);
						MCCL.MCC_LineY(worky + 30 + i);
					}
					if (i2 != 2)
					{
						MCCL.MCC_LineZ(workz);
						MCCL.MCC_LineX(workx + 25);
						MCCL.MCC_LineY(worky + 30);
					}
				}
				k = 5;
			}
			if (k == 5)
			{
				MCCL.MCC_LineZ(workz - 10);
				MCCL.MCC_LineX(workx);
				k = 6;
			}
			if (k == 6)
			{
				for (int i3 = 1; i3 <= 2; i3++)
				{
					MCCL.MCC_LineZ(workz + i3);
					for (int i = 0; i < 10; i++)
					{
						MCCL.MCC_LineX(workx + 60);
						MCCL.MCC_LineY(worky + 50 + i);
						MCCL.MCC_LineX(workx);
						MCCL.MCC_LineY(worky + 50 + i);
					}
					if (i3 != 2)
					{
						MCCL.MCC_LineZ(workz);
						MCCL.MCC_LineX(workx);
						MCCL.MCC_LineY(worky + 50);
					}
				}
				k = 7;
			}
			if (k == 7)
			{
				MCCL.MCC_EcatGetOutput(6, ref uCurValue);
				uCurValue &= ~SpindleDirBit;
				uCurValue &= ~LightStartBit;
				uCurValue |= LightStopBit;
				MCCL.MCC_EcatSetOutputEnqueue(6, uCurValue, 0, 0);

				MCCL.MCC_LineZ(workz-20);
				MCCL.MCC_Line(400, 0, 0, 0, 0, 0, 0, 0);

				k = 8;
			}
			workposbtn.Enabled = true;
		}
			

        /// ///////////////////////////////////////////////////

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }
		
		private void jogmodebtn_Click(object sender, EventArgs e)
		{
			jogmodebtn.Enabled = false;
			axiscomboBox.Enabled = true;
			jogpulsebtn.Enabled = true;
			joglessbtn.Enabled = true;
		}
	}
}

