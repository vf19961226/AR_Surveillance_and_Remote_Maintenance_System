using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Net; //Socket
using System.Net.Sockets; //Socket
using System.Threading; //多執行緒
using System.Text;
using System;

public class CNC_Loc_Sync : MonoBehaviour
{
    public string Server_IP;
    public int Server_PORT;

    public GameObject X_Axis; //沿X軸移動（在Unity中）
    public GameObject Y_Axis; //沿Z軸移動（在Unity中）
    public GameObject Z_Axis; //沿Y軸移動（在Unity中）

    private Socket clientSocket; //Socket Client物件
    private Thread threadSocket; //Socket的執行緒

    private float[] locs = { 0, 0, 0}; //紀錄CNC三軸位置
    private float[] before_locs = { 0, 0, 0 };
    private float[] before_locs_show = { 0, 0, 0 };

    private byte[] data = new byte[100];

    // Start is called before the first frame update
    void Start()
    {
        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); //創建一個Socket物件
        clientSocket.Connect(new IPEndPoint(IPAddress.Parse(Server_IP), Server_PORT)); //連線到Server

        threadSocket = new Thread(new ThreadStart(SocketReceive));
        threadSocket.Start();
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            locs = model_manager2.loc;
            X_Axis.transform.position = new Vector3(-(locs[0] * 0.001f - 0.42f), 0f, 0f);
            Y_Axis.transform.position = new Vector3(0f, 0f, -(locs[1] * 0.001f - 0.275f));
            Z_Axis.transform.position = new Vector3(-(locs[0] * 0.001f - 0.42f), -locs[2] * 0.001f, 0f);
        }
        catch
        {

        }

        //Debug.Log(data.Length);
        //string result = Encoding.ASCII.GetString(data);
        //Debug.Log("有收到值" + result);

        //string[] results = result.Split(' ');
        //if (results.Length == 3)
        //{
        //    Debug.Log(results[0] + ", " + results[1] + ", " + results[2]);
        //    try
        //    {
        //        locs[0] = float.Parse(results[0]);
        //        locs[1] = float.Parse(results[1]);
        //        locs[2] = float.Parse(results[2]);
        //        //before_locs = locs;

        //        X_Axis.transform.position = new Vector3(-(locs[0] * 0.001f - 0.42f), 0f, 0f);
        //        Y_Axis.transform.position = new Vector3(0f, 0f, -(locs[1] * 0.001f - 0.275f));
        //        Z_Axis.transform.position = new Vector3(-(locs[0] * 0.001f - 0.42f), -locs[2] * 0.001f, 0f);
        //    }
        //    catch
        //    {
        //        //locs = before_locs;
        //    }
        //    /*
        //    X_Axis.transform.position = new Vector3(-(locs[0] * 0.001f - 0.42f), 0f, 0f);
        //    Y_Axis.transform.position = new Vector3(0f, 0f, -(locs[1] * 0.001f - 0.275f));
        //    Z_Axis.transform.position = new Vector3(-(locs[0] * 0.001f - 0.42f), -locs[2] * 0.001f, 0f);
        //    */
        //}
        /*
        X_Axis.transform.position = new Vector3(-(locs[0] * 0.001f - 0.42f), 0f, 0f);
        Y_Axis.transform.position = new Vector3(0f, 0f, -(locs[1] * 0.001f - 0.275f));
        Z_Axis.transform.position = new Vector3(-(locs[0] * 0.001f - 0.42f), -locs[2] * 0.001f, 0f);
        /*
        if ((locs[0] - before_locs_show[0]) * 0.001f <= 0.09) //如果數值跳動太大則不移動（在Unity中的行程0.9）
        {
            X_Axis.transform.position = new Vector3(-(locs[0] * 0.001f - 0.42f), 0f, 0f);
            before_locs_show[0] = locs[0];

            if ((locs[2] - before_locs_show[2]) * 0.001f <= 0.03) //如果數值跳動太大則不移動（在Unity中的行程0.29）
            {
                Z_Axis.transform.position = new Vector3(-(locs[0] * 0.001f - 0.42f), -locs[2] * 0.001f, 0f);
                before_locs_show[2] = locs[2];
            }
        }

        if ((locs[1] - before_locs_show[1]) * 0.001f <= 0.05) //如果數值跳動太大則不移動（在Unity中的行程0.55）
        {
            Y_Axis.transform.position = new Vector3(0f, 0f, -(locs[1] * 0.001f - 0.275f));
            before_locs_show[1] = locs[1];
        }
        
        if (locs == new float[]{ 0f, 0f, 0f})
        {
            X_Axis.transform.position = new Vector3(0.42f, 0f, 0f);
            Y_Axis.transform.position = new Vector3(0f, 0f, 0.275f);
            Z_Axis.transform.position = new Vector3(0.42f, 0f, 0f);
            before_locs_show = locs;
        }
        */
    }

    private void SocketReceive() //接收Server（Fog Node）傳過來的影像
    {
        while (true)
        {
            data = new byte[100];
            int count = clientSocket.Receive(data);

            string result = Encoding.ASCII.GetString(data);
            string[] results = result.Split(' ');
            //Debug.Log(results);
            if (results.Length == 3)
            {
                try
                {
                    model_manager2.loc[0] = float.Parse(results[0]);
                    model_manager2.loc[1] = float.Parse(results[1]);
                    model_manager2.loc[2] = float.Parse(results[2]);
                }
                catch
                {

                }
                
            }

            //Debug.Log("有收到值" + result);
            //string result = Encoding.ASCII.GetString(data);
            //Debug.Log(result);
            /*
            string[] results = result.Split(',');
            if (results.Length != 3) continue;
            //Debug.Log(results.Length);
            /*
            int i = 0;
            foreach (string L in results)
            {
                //Debug.Log(L);
                try
                {
                    locs[i] = float.Parse(L);
                }
                catch
                {
                    Debug.Log("數字型別出錯了" + L);
                    locs[i] = before_locs[i];
                }
                i++;
            }
            這裡原本有註解
            try
            {
                locs[0] = float.Parse(results[0]);
                locs[1] = float.Parse(results[1]);
                locs[2] = float.Parse(results[2]);
                before_locs = locs;
            }
            catch
            {
                locs = before_locs;
            }
            */
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
