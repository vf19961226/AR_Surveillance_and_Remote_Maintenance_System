using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Net.Sockets; //Socket
using System.Data.SqlClient; //MS SQL Server

public class model_manager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

static class model_manager2
{
    //public static bool camera_transform = true;
    public static int model_type = 0; //目前的移動模式（0：移動世界座標、1：模型平移、2：模型旋轉、3：模型縮放）
    public static GameObject model_id = null; //目前鎖定的標註物件
    public static GameObject handle_id = null; //目前鎖定的手把物件
    public static bool handle_show = false; //是否已經顯示物件移動手把
    //public static ArrayList model_list = new ArrayList();

    public static GameObject X_show = null; //目前顯示的手把X軸物件
    public static GameObject Y_show = null; //目前顯示的手把Y軸物件
    public static GameObject Z_show = null; //目前顯示的手把Z軸物件

    public static Socket clientSocket; //Socket Client物件

    public static SqlConnection clientSQL = null; //MS SQL Server Client物件
    public static string select_xml_file = null; //DB表格選擇的XML檔案
    public static bool DB_Panel_Show = false; //目前DB_Show_Panel有沒有顯示

    public static float[] loc = { 0, 0, 0 }; //目前三軸的位置
}