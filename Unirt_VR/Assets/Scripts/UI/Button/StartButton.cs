using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StartButton : MonoBehaviour, IShotAble
{
    [SerializeField]
    private GameObject parentObj;
    public void OnShot(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if(parentObj !=null)
        {
            parentObj.SetActive(false);
        }

        GameManager.Instance.GameStart(this);
    }
}
