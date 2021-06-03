using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckMsButtonNo : MonoBehaviour, IShotAble
{

    public GameObject SettingWindow;
    public GameObject CheckMsBox;
    // Start is called before the first frame update
    public void OnShot(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        CheckMsNo();
    }
  
    // Update is called once per frame
    public void CheckMsNo()// 홈 버튼 클릭시 
    {

        CheckMsBox.SetActive(false);
        if(CheckMsBox ==null)
        {
            SettingWindow.SetActive(false);
            CheckMsBox.SetActive(false);
        }
    }
}
