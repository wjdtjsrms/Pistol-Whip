using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckMsButtonYes : MonoBehaviour, IShotAble
{
    [SerializeField]
    private FadeScript fadeSceneLoad;

    [SerializeField]
    private GameObject parentObj;


    public void OnShot(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        parentObj.SetActive(false);
        fadeSceneLoad.FadeLoadStart();
    }
}
