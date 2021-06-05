using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// SpawnManager에서 너무 많은 비교연산이 일어나는 것을 방지하기 위해 world 별로 나누었다. 
public class SpawnManagerControl : MonoBehaviour
{
    [SerializeField]
    private GameObject[] SpawnManagers; // 각 월드의 spawnManager를 저장한다.
    private int index = 0;

    void Start()
    {
        for (int i = 0; i < SpawnManagers.Length; i++)
        {
            SpawnManagers[i].SetActive(false);
        }

        SpawnManagers[index].SetActive(true);
    }


    void Update()
    {
        // world를 넘어가면 현재의 spawnManager를 비활성화하고 다음 world의 spawnManager를 활성화 시킨다.
        if (SpawnManagers[index].transform.position.z < GameManager.Instance.PlayerPos.z)
        {
            SpawnManagers[index++].gameObject.SetActive(false);
            SpawnManagers[index].gameObject.SetActive(true);
        }
    }
}
