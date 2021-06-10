using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicCanvas : MonoBehaviour
{
    GameObject temp;
    // Start is called before the first frame update
    void Start()
    {
        temp = GameObject.Find("IsGameEnd");
        if (temp.GetComponent<DontDestroy>().isGameEnd)
        {
            gameObject.SetActive(false);
        }

    }

    // Update is called once per frame
    void Update()
    {
    }
}
