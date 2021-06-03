using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueButton : MonoBehaviour, IShotAble
{
    public static bool GameIsPaused = false;

    public GameObject SettingWindow;

    public void OnShot(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        Continue();
    }
    // Update is called once per frame
    public void Continue()
    {
        SettingWindow.SetActive(false);
    }
    public void Resume()
    {

        Time.timeScale = 1f;
        GameIsPaused = false;
    }
}
