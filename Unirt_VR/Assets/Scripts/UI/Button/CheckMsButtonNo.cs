using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckMsButtonNo : MonoBehaviour, IShotAble
{
    [SerializeField]
    private GameObject SettingWindow;
    [SerializeField]
    private GameObject CheckMsBox;

    public void OnShot(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        SettingWindow.SetActive(true);
        CheckMsBox.SetActive(false);
    }
}
