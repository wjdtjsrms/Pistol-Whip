using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// SpawnManager���� �ʹ� ���� �񱳿����� �Ͼ�� ���� �����ϱ� ���� world ���� ��������. 
public class SpawnManagerControl : MonoBehaviour
{
    [SerializeField]
    private GameObject[] SpawnManagers; // �� ������ spawnManager�� �����Ѵ�.
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
        // world�� �Ѿ�� ������ spawnManager�� ��Ȱ��ȭ�ϰ� ���� world�� spawnManager�� Ȱ��ȭ ��Ų��.
        if (SpawnManagers[index].transform.position.z < GameManager.Instance.PlayerPos.z)
        {
            SpawnManagers[index++].gameObject.SetActive(false);
            SpawnManagers[index].gameObject.SetActive(true);
        }
    }
}
