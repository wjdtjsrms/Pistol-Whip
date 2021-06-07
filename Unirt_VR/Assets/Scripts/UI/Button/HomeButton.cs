using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeButton : MonoBehaviour, IShotAble
{
    [SerializeField]
    private GameObject SettingWindow;
    [SerializeField]
    private GameObject CheckMsBox;

    public void OnShot(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        CheckMsBox.SetActive(true);
        SettingWindow.SetActive(false);
    }
}