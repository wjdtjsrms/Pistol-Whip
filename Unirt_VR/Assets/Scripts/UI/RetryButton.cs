using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryButton : MonoBehaviour, IShotAble
{
    public GameObject SettingWindow;
    public GameObject CheckMsBox;       
    public void OnShot(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        Invoke("LoadPlayScene", 1f);
        retryButton();
    }
    public void retryButton()// Ȩ ��ư Ŭ���� 
    {
        SceneManager.LoadScene("SampleScene");
    }

}
