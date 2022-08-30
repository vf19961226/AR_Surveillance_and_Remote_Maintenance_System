using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Xml;
using System.Xml.Serialization;
using System;
using System.Text;

using System.Data.SqlClient;
using System.IO;

public class TRS_model : MonoBehaviour
{
    public string model_type;
    public GameObject X_Axis;
    public GameObject Y_Axis;
    public GameObject Z_Axis;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void show_handle() //之後可利用GetHandleSize確保手把大小一致
    {
        if (model_manager2.model_id != null)
        {
            GameObject model = model_manager2.model_id;

            GameObject X = Instantiate(X_Axis, model.transform.position, model.transform.rotation);
            X.transform.localScale = model.transform.localScale;

            GameObject Y = Instantiate(Y_Axis, model.transform.position, model.transform.rotation);
            Y.transform.localScale = model.transform.localScale;

            GameObject Z = Instantiate(Z_Axis, model.transform.position, model.transform.rotation);
            Z.transform.localScale = model.transform.localScale;

            model_manager2.handle_show = true;
            model_manager2.X_show = X;
            model_manager2.Y_show = Y;
            model_manager2.Z_show = Z;

            model.transform.GetComponent<Collider>().enabled = false; //關閉該物體的碰撞器
        }
    }

    private void destroy_handle()
    {
        Destroy(model_manager2.X_show);
        Destroy(model_manager2.Y_show);
        Destroy(model_manager2.Z_show);
        model_manager2.X_show = null;
        model_manager2.Y_show = null;
        model_manager2.Z_show = null;

        model_manager2.handle_id = null;
    }

    public void btn_OnClick()
    {
        if (model_manager2.handle_show == false) //確認物件移動手把沒有被顯示
        {
            show_handle();
            if (model_type == "T") model_manager2.model_type = 1;
            else if (model_type == "R") model_manager2.model_type = 2;
            else if (model_type == "S") model_manager2.model_type = 3;
        }
        else //如果把手已經顯示
        {
            destroy_handle(); //銷毀上一個把手
            show_handle(); //重新繪製把手
            if (model_type == "T" && model_manager2.model_type != 1)
            {
                model_manager2.model_type = 1;
            }
            else if (model_type == "R" && model_manager2.model_type != 2)
            {
                model_manager2.model_type = 2;
            }
            else if (model_type == "S" && model_manager2.model_type != 3)
            {
                model_manager2.model_type = 3;
            }
        }

    }

    public void distory()
    {
        if (model_manager2.model_id != null)
        {
            Destroy(model_manager2.model_id);
            model_manager2.model_id = null;
            destroy_handle();
        }
    }

    public void send_model() //發送標註資訊
    {
        GameObject[] model_list;
        model_list = GameObject.FindGameObjectsWithTag("Annotation");

        XmlDocument send_data = new XmlDocument();
        XmlElement annotation_data = send_data.CreateElement("Annotation_Data");
        send_data.AppendChild(annotation_data);

        XmlElement information = send_data.CreateElement("Information");
        annotation_data.AppendChild(information);

        XmlElement creator = send_data.CreateElement("Creator");
        creator.InnerText = "Hong-Yuan Yang";
        information.AppendChild(creator);

        XmlElement time = send_data.CreateElement("Time");
        time.InnerText = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        information.AppendChild(time);

        XmlElement target = send_data.CreateElement("Target");
        target.InnerText = "NCKU IMILab CNC";
        information.AppendChild(target);

        XmlElement model_data = send_data.CreateElement("Model_Data");
        annotation_data.AppendChild(model_data);

        int i = 0;
        foreach (GameObject model in model_list)
        {
            string name = model.name;
            Vector3 position = model.transform.transform.position;
            Quaternion rotate = model.transform.rotation;
            Vector3 scale = model.transform.localScale;

            XmlElement number = send_data.CreateElement("Number");
            number.SetAttribute("Number", Convert.ToString(i));//設定屬性
            model_data.AppendChild(number);

            XmlElement type = send_data.CreateElement("Type");
            type.InnerText = model_name2type(name);
            number.AppendChild(type);

            XmlElement location = send_data.CreateElement("Location");
            location.InnerText = Convert.ToString(position);
            number.AppendChild(location);

            XmlElement size = send_data.CreateElement("Size");
            size.InnerText = Convert.ToString(scale);
            number.AppendChild(size);

            XmlElement rotation_angle = send_data.CreateElement("Rotation_Angle");
            rotation_angle.InnerText = Convert.ToString(rotate);
            number.AppendChild(rotation_angle);

            i++;
        }
        Directory.CreateDirectory("XML_File"); //創建儲存XML檔案的資料夾

        string file_name = "log_" + DateTime.Now.ToString("yyyyMMddHHmmss");
        send_data.Save("XML_File/" + file_name + ".xml"); //儲存XML檔
        xml2sql(file_name);

        byte[] send = Encoding.UTF8.GetBytes(send_data.OuterXml);
        model_manager2.clientSocket.Send(send);
    }

    private void xml2sql(string file_path)
    {
        Debug.Log(file_path);
        SqlConnection conn = model_manager2.clientSQL;

        string cmd = @"
                        USE Cloud_Database;
                        INSERT INTO Operate_Log(
                        [time], [file_name], [tag]
                        )
                        VALUES(
                        GETDATE(), '" + file_path + "', 'TEST');";

        SqlCommand dr = new SqlCommand(cmd, conn);
        dr.ExecuteNonQuery();
        dr.Cancel();
    }

    private string model_name2type(string name)
    {
        string type = "";
        switch (name)
        {
            case "Arrow Variant(Clone)":
                type = "0";
                break;

            case "hand2 Variant(Clone)":
                type = "1";
                break;

            case "Notice Variant(Clone)":
                type = "2";
                break;

            case "Whiteboard Variant(Clone)":
                type = "3";
                break;
        }
        return type;
    }
}
