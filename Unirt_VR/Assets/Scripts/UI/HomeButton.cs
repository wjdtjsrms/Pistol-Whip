using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeButton : MonoBehaviour, IShotAble
{
    public GameObject SettingWindow;
    public GameObject CheckMsBox;

    public void OnShot(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        homebutton();
    }


  
    public void homebutton()
    {
        SettingWindow.SetActive(false);
      
        if(SettingWindow != null) 
        {
            CheckMsBox.SetActive(true);
           SettingWindow.SetActive(false);
        }
    }
}