using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning_Floor : MonoBehaviour
{
    [SerializeField]
    private GameObject[] Lightning_F;
    [SerializeField]
    private int index = 0;

    void Start()
    {
        for (int i = 0; i < Lightning_F.Length; i++)
        {
            Lightning_F[i].SetActive(false);
        }
        Lightning_F[index].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(Lightning_F[index].transform.position.z < GameManager.Instance.PlayerPos.z)
        {
            Lightning_F[index++].gameObject.SetActive(false);
            Lightning_F[index].gameObject.SetActive(true);
        }
    }
}
