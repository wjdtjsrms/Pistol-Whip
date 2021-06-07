using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialButton : MonoBehaviour, IShotAble
{
    // Start is called before the first frame update
    public void OnShot(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        OnTutorial();
    }
    private void OnTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }
}
