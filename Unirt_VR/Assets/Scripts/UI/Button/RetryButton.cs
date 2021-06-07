using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryButton : MonoBehaviour, IShotAble
{
    [SerializeField]
    private FadeScript fadeSceneLoad;

    public void OnShot(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        fadeSceneLoad.FadeLoadPlay();
    }
}
