using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{

    public  bool isGameEnd = false;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);      
    }

    void Start()
    {
        GameManager.Instance.actGameEnd += () => isGameEnd = true;
    }

}
