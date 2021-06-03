using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckMsButtonYes : MonoBehaviour, IShotAble
{
    // Start is called before the first frame update
    public void OnShot(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        Invoke("StartScoreScene", 1f);
        GamecheckBoxYes();
    }
   
    // Update is called once per frame
    public void GamecheckBoxYes()// 홈 버튼 클릭시 
    {
        SceneManager.LoadScene("StartScoreScene");
    }
}
