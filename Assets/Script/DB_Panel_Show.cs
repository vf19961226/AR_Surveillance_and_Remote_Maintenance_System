using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System.Data;
using System.Data.SqlClient;

using System.Xml;

public class DB_Panel_Show : MonoBehaviour
{
    public GameObject Panel;
    public GameObject Page_Tag;
    public GameObject DB_Panel1;
    public GameObject DB_Panel2;
    public GameObject DB_Table1;
    public GameObject DB_Table2;
    public GameObject Row1;
    public GameObject Row2;

    public GameObject model0;
    public GameObject model1;
    public GameObject model2;
    public GameObject model3;

    public string SQL_Server_IP;
    public string User_ID;
    public string Password;

    private SqlConnection conn;
    public List<GameObject> cnc_table = new List<GameObject>(); //紀錄資料庫顯示資料的預制體
    public List<GameObject> operate_table = new List<GameObject>(); //紀錄資料庫顯示資料的預制體

    // Start is called before the first frame update
    void Start()
    {
        if (model_manager2.clientSQL == null)
        {
            conn = new SqlConnection("data source=" + SQL_Server_IP + "; user id =" + User_ID + "; password =" + Password);
            conn.Open();

            model_manager2.clientSQL = conn;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Panel_Show()
    {
        Panel.SetActive(true);
        model_manager2.DB_Panel_Show = true;
        model_manager2.model_type = -1; //相機移動功能關閉

        //讀取
        string sql_cmd = @"
                          USE Cloud_Database;
                          SELECT * FROM CNC_Sensing_Data;
                          ";
        SqlCommand cmd = new SqlCommand(sql_cmd, conn);
        SqlDataReader dr = cmd.ExecuteReader();

        while (dr.Read())
        {
            GameObject row = GameObject.Instantiate(Row1, DB_Table1.transform.position, DB_Table1.transform.rotation);
            row.transform.SetParent(DB_Table1.transform);
            row.transform.localScale = Vector3.one;

            row.transform.Find("Cell0").GetComponent<Text>().text = dr["number"].ToString();
            row.transform.Find("Cell1").GetComponent<Text>().text = dr["time"].ToString();
            row.transform.Find("Cell2").GetComponent<Text>().text = dr["x_vibration"].ToString();
            row.transform.Find("Cell3").GetComponent<Text>().text = dr["y_vibration"].ToString();
            row.transform.Find("Cell4").GetComponent<Text>().text = dr["z_vibration"].ToString();
            row.transform.Find("Cell5").GetComponent<Text>().text = dr["s_current"].ToString();
            row.transform.Find("Cell6").GetComponent<Text>().text = dr["x_loc"].ToString();
            row.transform.Find("Cell7").GetComponent<Text>().text = dr["y_loc"].ToString();
            row.transform.Find("Cell8").GetComponent<Text>().text = dr["z_loc"].ToString();
            row.transform.Find("Cell9").GetComponent<Text>().text = dr["x_velocity"].ToString();
            row.transform.Find("Cell10").GetComponent<Text>().text = dr["y_velocity"].ToString();
            row.transform.Find("Cell11").GetComponent<Text>().text = dr["z_velocity"].ToString();
            row.transform.Find("Cell12").GetComponent<Text>().text = dr["x_torque"].ToString();
            row.transform.Find("Cell13").GetComponent<Text>().text = dr["y_torque"].ToString();
            row.transform.Find("Cell14").GetComponent<Text>().text = dr["z_torque"].ToString();
            row.transform.Find("Cell15").GetComponent<Text>().text = dr["x_errcode"].ToString();
            row.transform.Find("Cell16").GetComponent<Text>().text = dr["y_errcode"].ToString();
            row.transform.Find("Cell17").GetComponent<Text>().text = dr["z_errcode"].ToString();
            row.transform.Find("Cell18").GetComponent<Text>().text = dr["Alarm"].ToString();

            cnc_table.Add(row);
        }
        dr.Close();

        //讀取Operate_Log
        sql_cmd = @"
                          USE Cloud_Database;
                          SELECT * FROM Operate_Log;
                          ";
        cmd = new SqlCommand(sql_cmd, conn);
        dr = cmd.ExecuteReader();

        while (dr.Read())
        {
            GameObject row = GameObject.Instantiate(Row2, DB_Table2.transform.position, DB_Table2.transform.rotation);
            row.transform.SetParent(DB_Table2.transform);
            row.transform.localScale = Vector3.one;

            row.transform.Find("Cell0").GetComponent<Text>().text = dr["number"].ToString();
            row.transform.Find("Cell1").GetComponent<Text>().text = dr["time"].ToString();
            row.transform.Find("Cell2").GetComponent<Text>().text = dr["file_name"].ToString();
            row.transform.Find("Cell3").GetComponent<InputField>().text = dr["tag"].ToString();

            string file_name = dr["file_name"].ToString();
            for (int i = 0; i <= 3; i++)
            {
                Button image_button = row.transform.Find("Cell" + i + "/RawImage" + i).GetComponent<Button>();
                image_button.onClick.AddListener(delegate { select_data(file_name); });
            }

            operate_table.Add(row);
        }
        dr.Close();
    }

    public void Clear_Table() //清除Table裡面的資料
    {
        foreach(GameObject obj in cnc_table)
        {
            Destroy(obj);
        }

        foreach (GameObject obj in operate_table)
        {
            Destroy(obj);
        }
    }

    public void Panel_Close()
    {
        /*
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Operate_Log_DB");
        foreach (GameObject obj in objs)
        {
            Destroy(obj);
        }
        */
        model_manager2.select_xml_file = null;
        model_manager2.DB_Panel_Show = false;
        Clear_Table();
        Panel.SetActive(false);
    }

    public void Panel_OK() //清除場上所有模型，並導入所選擇的模型檔案資訊
    {
        model_manager2.DB_Panel_Show = false;
        Panel.SetActive(false);
        if (Page_Tag.GetComponent<Text>().text == "Operate Log")
        {
            //Debug.Log("到時候直接開啟已經準備好的XML檔!!"); //到時候用select_data選的檔案開啟
            xml_import(); //匯入XML檔紀錄的模型資訊
        }
        Clear_Table();
    }

    private void xml_import() //匯入XML檔紀錄的模型資訊
    {
        string file_path = model_manager2.select_xml_file;

        if (file_path != null)
        {
            string xml_path = "XML_File/" + file_path + ".xml";

            XmlDocument doc = new XmlDocument();
            doc.Load(xml_path);
            XmlNode xmlRoot = doc.DocumentElement;

            foreach (XmlNode node in xmlRoot.SelectNodes("Model_Data/Number")) //讀取XML檔中所有Model_Data/Number節點
            {
                string Location = node.SelectSingleNode("Location").InnerText;
                string Size = node.SelectSingleNode("Size").InnerText;
                string Rotation_Angle = node.SelectSingleNode("Rotation_Angle").InnerText;

                switch (node.SelectSingleNode("Type").InnerText)
                {
                    case "0":
                        load_model(model0, Location, Size, Rotation_Angle);
                        break;
                    case "1":
                        load_model(model1, Location, Size, Rotation_Angle);
                        break;
                    case "2":
                        load_model(model2, Location, Size, Rotation_Angle);
                        break;
                    case "3":
                        load_model(model3, Location, Size, Rotation_Angle);
                        break;
                }
            }

            model_manager2.select_xml_file = null;
        }
        
    }

    private void load_model(GameObject obj, string loca, string size, string rotate)
    {
        loca = loca.Replace("(", "").Replace(")", "");
        string[] locas = loca.Split(','); //注意""(string)和''(char)不一樣

        size = size.Replace("(", "").Replace(")", "");
        string[] sizes = size.Split(',');

        rotate = rotate.Replace("(", "").Replace(")", "");
        string[] rotates = rotate.Split(',');

        GameObject new_obj = Instantiate(obj, new Vector3(float.Parse(locas[0]), float.Parse(locas[1]), float.Parse(locas[2])), new Quaternion(float.Parse(rotates[0]), float.Parse(rotates[1]), float.Parse(rotates[2]), float.Parse(rotates[3])));
        new_obj.transform.localScale = new Vector3(float.Parse(sizes[0]), float.Parse(sizes[1]), float.Parse(sizes[2]));
    }

    public void Change_Page()
    {
        Text now_page = Page_Tag.GetComponent<Text>();

        if (now_page.text == "CNC Sensing Data")
        {
            now_page.text = "Operate Log";
            DB_Panel1.SetActive(false);
            DB_Panel2.SetActive(true);
        }
        else if (now_page.text == "Operate Log")
        {
            now_page.text = "CNC Sensing Data";
            DB_Panel2.SetActive(false);
            DB_Panel1.SetActive(true);
        }
    }

    public void select_data(string file_name)
    {
        Debug.Log(file_name);
        model_manager2.select_xml_file = file_name;
    }
}
