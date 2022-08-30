using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class control_camera_view : MonoBehaviour
{
    public Vector3 mousePos1;                           //記錄滑鼠點下去瞬間的位置
    public Vector3 mousePos2;                           //記錄滑鼠任何時刻的位置
    public Quaternion start_qua;                        //角度使用四元數
    public Vector3 start_pos;                           //位置座標
                                                        // Use this for initialization
    void Start()
    {
        //記錄相機開始的角度與位置
        start_qua = transform.rotation;
        start_pos = transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        //記錄滑鼠實時移動的點
        mousePos2 = Input.mousePosition;
    }
    void OnGUI()
    {
        if (model_manager2.model_type == 0)
        {
            //滑鼠左鍵
            if (Input.GetMouseButton(0))
            {
                mousePos1 = Input.mousePosition;            //記錄滑鼠點選瞬間的點
                Vector3 offset = mousePos1 - mousePos2;     //記錄滑鼠移動的距離
                                                            //上下與左右 旋轉分開，絕對值比較
                if (Mathf.Abs(offset.x) > Mathf.Abs(offset.y))
                {
                    //以物體上方為旋轉軸（Vector3.up == new Vector3(0, 1.0f, 0)），物體左右旋轉角度與滑鼠橫向移動距離相關，變化速率30
                    transform.RotateAround(new Vector3(0, 0, 0), Vector3.up, Time.deltaTime * offset.x * 30);
                }
                else
                {
                    //以世界座標右方為旋轉軸（transform.right，是會變化的量），物體上下旋轉角度與滑鼠縱向移動距離相關，變化速率30
                    transform.RotateAround(new Vector3(0, 0, 0), transform.right, -Time.deltaTime * offset.y * 30);
                }
                //列印資料transform.right變數
                //Debug.Log("pos: " + transform.right);
            }
            //滑鼠中鍵，物體恢復原來的角度與位置
            if (Input.GetMouseButton(2))
            {
                transform.rotation = start_qua;
                transform.position = start_pos;
            }
            //滑鼠中鍵滑動，物體縮放，攝像頭前後移動距離範圍在2f~5f，變化速率3f
            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                transform.Translate(Vector3.forward * Time.deltaTime * Input.GetAxis("Mouse ScrollWheel") * 20);
            }
            if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                transform.Translate(Vector3.back * Time.deltaTime * (-Input.GetAxis("Mouse ScrollWheel")) * 20);
            }
        }
        else if (model_manager2.model_type == 1 && model_manager2.handle_id != null) //虛擬物件平移模式
        {

            //滑鼠左鍵
            if (Input.GetMouseButton(0))
            {
                mousePos1 = Input.mousePosition;            //記錄滑鼠點選瞬間的點
                Vector3 offset = mousePos1 - mousePos2;     //記錄滑鼠移動的距離

                if (model_manager2.handle_id == model_manager2.X_show)
                {
                    if (mousePos1.sqrMagnitude > mousePos2.sqrMagnitude)
                    {
                        model_translate(new Vector3(offset.sqrMagnitude * 0.0005f, 0, 0));
                    }
                    else if (mousePos1.sqrMagnitude < mousePos2.sqrMagnitude)
                    {
                        model_translate(new Vector3(-offset.sqrMagnitude * 0.0005f, 0, 0));
                    }
                }
                else if (model_manager2.handle_id == model_manager2.Y_show)
                {
                    if (mousePos1.sqrMagnitude > mousePos2.sqrMagnitude)
                    {
                        model_translate(new Vector3(0, offset.sqrMagnitude * 0.0005f, 0));
                    }
                    else if (mousePos1.sqrMagnitude < mousePos2.sqrMagnitude)
                    {
                        model_translate(new Vector3(0, -offset.sqrMagnitude * 0.0005f, 0));
                    }
                }
                else if (model_manager2.handle_id == model_manager2.Z_show)
                {
                    if (mousePos1.sqrMagnitude > mousePos2.sqrMagnitude)
                    {
                        model_translate(new Vector3(0, 0, -offset.sqrMagnitude * 0.0005f));
                    }
                    else if (mousePos1.sqrMagnitude < mousePos2.sqrMagnitude)
                    {
                        model_translate(new Vector3(0, 0, offset.sqrMagnitude * 0.0005f));
                    }
                }
            }
        }
        else if (model_manager2.model_type == 2 && model_manager2.handle_id != null) //虛擬物件旋轉模式
        {

            //滑鼠左鍵
            if (Input.GetMouseButton(0))
            {
                mousePos1 = Input.mousePosition;            //記錄滑鼠點選瞬間的點
                Vector3 offset = mousePos1 - mousePos2;     //記錄滑鼠移動的距離

                if (model_manager2.handle_id == model_manager2.X_show)
                {
                    if (mousePos1.sqrMagnitude > mousePos2.sqrMagnitude)
                    {
                        model_rotate(new Vector3(offset.sqrMagnitude * 0.5f, 0, 0));
                    }
                    else if (mousePos1.sqrMagnitude < mousePos2.sqrMagnitude)
                    {
                        model_rotate(new Vector3(-offset.sqrMagnitude * 0.5f, 0, 0));
                    }
                }
                else if (model_manager2.handle_id == model_manager2.Y_show)
                {
                    if (mousePos1.sqrMagnitude > mousePos2.sqrMagnitude)
                    {
                        model_rotate(new Vector3(0, offset.sqrMagnitude * 0.5f, 0));
                    }
                    else if (mousePos1.sqrMagnitude < mousePos2.sqrMagnitude)
                    {
                        model_rotate(new Vector3(0, -offset.sqrMagnitude * 0.5f, 0));
                    }
                }
                else if (model_manager2.handle_id == model_manager2.Z_show)
                {
                    if (mousePos1.sqrMagnitude > mousePos2.sqrMagnitude)
                    {
                        model_rotate(new Vector3(0, 0, -offset.sqrMagnitude * 0.5f));
                    }
                    else if (mousePos1.sqrMagnitude < mousePos2.sqrMagnitude)
                    {
                        model_rotate(new Vector3(0, 0, offset.sqrMagnitude * 0.5f));
                    }
                }
            }
        }
        else if (model_manager2.model_type == 3 && model_manager2.handle_id != null) //虛擬物件縮放模式
        {

            //滑鼠左鍵
            if (Input.GetMouseButton(0))
            {
                mousePos1 = Input.mousePosition;            //記錄滑鼠點選瞬間的點
                Vector3 offset = mousePos1 - mousePos2;     //記錄滑鼠移動的距離

                if (model_manager2.handle_id == model_manager2.X_show)
                {
                    if (mousePos1.sqrMagnitude > mousePos2.sqrMagnitude)
                    {
                        model_scale(new Vector3(-offset.sqrMagnitude * 0.005f, 0, 0));
                    }
                    else if (mousePos1.sqrMagnitude < mousePos2.sqrMagnitude)
                    {
                        model_scale(new Vector3(offset.sqrMagnitude * 0.005f, 0, 0));
                    }
                }
                else if (model_manager2.handle_id == model_manager2.Y_show)
                {
                    if (mousePos1.sqrMagnitude > mousePos2.sqrMagnitude)
                    {
                        model_scale(new Vector3(0, offset.sqrMagnitude * 0.005f, 0));
                    }
                    else if (mousePos1.sqrMagnitude < mousePos2.sqrMagnitude)
                    {
                        model_scale(new Vector3(0, -offset.sqrMagnitude * 0.005f, 0));
                    }
                }
                else if (model_manager2.handle_id == model_manager2.Z_show)
                {
                    if (mousePos1.sqrMagnitude > mousePos2.sqrMagnitude)
                    {
                        model_scale(new Vector3(0, 0, -offset.sqrMagnitude * 0.005f));
                    }
                    else if (mousePos1.sqrMagnitude < mousePos2.sqrMagnitude)
                    {
                        model_scale(new Vector3(0, 0, offset.sqrMagnitude * 0.005f));
                    }
                }
            }
        }
    }

    private void model_translate(Vector3 move)
    {
        model_manager2.model_id.transform.Translate(move);
        model_manager2.X_show.transform.Translate(move);
        model_manager2.Y_show.transform.Translate(move);
        model_manager2.Z_show.transform.Translate(move);
    }

    private void model_rotate(Vector3 rotate)
    {
        model_manager2.model_id.transform.Rotate(rotate);
        model_manager2.X_show.transform.Rotate(rotate);
        model_manager2.Y_show.transform.Rotate(rotate);
        model_manager2.Z_show.transform.Rotate(rotate);
    }

    private void model_scale(Vector3 scale)
    {
        model_manager2.model_id.transform.localScale += scale;
        model_manager2.X_show.transform.localScale += scale;
        model_manager2.Y_show.transform.localScale += scale;
        model_manager2.Z_show.transform.localScale += scale;
    }
}
