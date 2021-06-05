using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckMsButtonYes : MonoBehaviour, IShotAble
{
    public void OnShot(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        SceneManager.LoadScene("StartScoreScene");
    }
}
