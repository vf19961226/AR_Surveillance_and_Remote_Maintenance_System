using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class model_transform : MonoBehaviour
{
    /*
    public GameObject translate;
    public GameObject rotate;
    public GameObject scale;
    */
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) //檢測滑鼠左鍵點擊以及是否在UI上
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;  

            if (Physics.Raycast(ray, out hitInfo)) //射線有檢測到物體碰撞
            {
                GameObject check = hitInfo.collider.gameObject;
                if (check.tag != "Transform" && model_manager2.model_id != null) model_manager2.model_id.transform.GetComponent<Collider>().enabled = true; //開啟該物體的碰撞器

                if (check != model_manager2.model_id && check.tag != "Transform") //如果點擊的物件跟前一個不一樣
                {
                    model_manager2.model_type = 0;

                    Destroy(model_manager2.X_show);
                    Destroy(model_manager2.Y_show);
                    Destroy(model_manager2.Z_show);
                    model_manager2.X_show = null;
                    model_manager2.Y_show = null;
                    model_manager2.Z_show = null;

                    model_manager2.handle_show = false;

                    model_manager2.model_id = check;
                }

                if (check.tag == "Transform") //如果是選到手把的話
                {
                    model_manager2.handle_id = check;
                }
                
                /*
                GameObject gameobj = hitInfo.collider.gameObject;
                if (model_manager2.model_type == 0)
                {
                    Instantiate(translate, gameobj.transform);
                }
                */
            }
            else //射線沒有檢測到物體
            {
                if (model_manager2.model_id != null) model_manager2.model_id.transform.GetComponent<Collider>().enabled = true; //開啟該物體的碰撞器

                model_manager2.model_type = 0;
                model_manager2.model_id = null;
                if (model_manager2.handle_show) //如果手把有顯示就將手把銷毀
                {
                    Destroy(model_manager2.X_show);
                    Destroy(model_manager2.Y_show);
                    Destroy(model_manager2.Z_show);
                    model_manager2.X_show = null;
                    model_manager2.Y_show = null;
                    model_manager2.Z_show = null;

                    model_manager2.handle_show = false;
                }

                if (model_manager2.DB_Panel_Show)
                {
                    GameObject.Find("Canvas/DB_Show_Panel").SetActive(false);
                    model_manager2.select_xml_file = null;
                    model_manager2.DB_Panel_Show = false;
                }
            }
        }
    }
}
