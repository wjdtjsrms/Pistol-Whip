using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueButton : MonoBehaviour, IShotAble
{
    [SerializeField]
    private GameObject SettingWindow;

    public void OnShot(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        GameManager.Instance.GameRestart(this);
        SettingWindow.SetActive(false);
    }
}
