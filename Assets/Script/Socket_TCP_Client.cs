using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System.Net; //Socket
using System.Net.Sockets; //Socket
using System.Threading; //多執行緒
using System.Text;

using OpenCvSharp;

public class Socket_TCP_Client : MonoBehaviour
{
    public string Server_IP;
    public int Server_PORT;

    private Socket clientSocket; //Socket Client物件
    private Thread threadSocket; //Socket的執行緒

    private GameObject Show_Board;
    private bool get_img = false;
    Mat image = null;

    // Start is called before the first frame update
    void Start()
    {
        Show_Board = GameObject.Find("RawImage_FogShading");

        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); //創建一個Socket物件
        clientSocket.Connect(new IPEndPoint(IPAddress.Parse(Server_IP), Server_PORT)); //連線到Server
        model_manager2.clientSocket = clientSocket;

        threadSocket = new Thread(new ThreadStart(SocketReceive));
        threadSocket.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (get_img && image != null)
        {
            get_img = false;
            /* 查看影像是否正常輸入
            Cv2.ImShow("From Socket Server2", image);
            Cv2.WaitKey(0);
            */
            int height = image.Height;
            int width = image.Width;
            Texture2D renderedTexture = new Texture2D(width, height, TextureFormat.RGBA32, true, true);
            renderedTexture.LoadImage(image.ImEncode());

            RawImage show_img = Show_Board.GetComponent<RawImage>();
            show_img.texture = renderedTexture;
        }
    }

    private void SocketReceive() //接收Server（Fog Node）傳過來的影像
    {
        while (true)
        {
            byte[] data = new byte[2000000];
            int count = clientSocket.Receive(data);

            ImreadModes mode = ImreadModes.Color;
            image = Cv2.ImDecode(data, mode);
            /* 查看影像是否正常輸入
            Cv2.ImShow("From Socket Server", image);
            Cv2.WaitKey(0);
            */

            get_img = true;
        }
    }

    void OnApplicationQuit()
    {
        //關閉執行緒
        if (threadSocket != null)
        {
            threadSocket.Interrupt();
            threadSocket.Abort();
        }
        //最後關閉伺服器
        if (clientSocket != null) clientSocket.Close();
    }
}
