using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCanvas : MonoBehaviour
{
    public GameObject obj;
    GameObject temp;


    // Start is called before the first frame update
    void Start()
    {
        temp = GameObject.Find("IsGameEnd");
        if (temp.GetComponent<DontDestroy>().isGameEnd)
        {
            obj.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        GameManager.Instance.LoadOperation();
        //GameManager.Instance.LoadRanking();
    }
}
