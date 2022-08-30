using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class model_btn_script : MonoBehaviour
{
    public GameObject model;
    /*
    public GameObject X_Axis;
    public GameObject Y_Axis;
    public GameObject Z_Axis;
    */
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void btn_OnClick()
    {
        if (model_manager2.model_id != null) model_manager2.model_id.transform.GetComponent<Collider>().enabled = true; //開啟前一個鎖定物體的碰撞器

        GameObject checked_model = Instantiate(model, model.transform.position, Quaternion.identity); //生成新的標註物件
        model_manager2.model_id = checked_model; //鎖定生成的物件
        /*
        Button Translate_btn = GameObject.Find("Translate_btn").GetComponent<Button>();
        Translate_btn.interactable = true;
        Button Rotate_btn = GameObject.Find("Rotate_btn").GetComponent<Button>();
        Rotate_btn.interactable = true;
        Button Scale_btn = GameObject.Find("Scale_btn").GetComponent<Button>();
        Scale_btn.interactable = true;
        Button Delete_btn = GameObject.Find("Delete_btn").GetComponent<Button>();
        Delete_btn.interactable = true;
        Button Send_btn = GameObject.Find("Send_btn").GetComponent<Button>();
        Send_btn.interactable = true;
        
        model_manager2.model_type = 1; //改為模型平移模式

        GameObject X = Instantiate(X_Axis, checked_model.transform.position, Quaternion.identity);
        X.transform.localScale = checked_model.transform.localScale;

        GameObject Y = Instantiate(Y_Axis, checked_model.transform.position, Quaternion.identity);
        Y.transform.localScale = checked_model.transform.localScale;

        GameObject Z = Instantiate(Z_Axis, checked_model.transform.position, Quaternion.identity);
        Z.transform.localScale = checked_model.transform.localScale;

        model_manager2.handle_show = true;
        */
        if (model_manager2.handle_show) //如果把手有顯示就銷毀把手
        {
            Destroy(model_manager2.X_show);
            Destroy(model_manager2.Y_show);
            Destroy(model_manager2.Z_show);
            model_manager2.X_show = null;
            model_manager2.Y_show = null;
            model_manager2.Z_show = null;
        }
    }
}
