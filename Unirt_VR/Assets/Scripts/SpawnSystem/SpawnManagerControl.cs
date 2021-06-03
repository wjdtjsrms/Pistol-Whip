using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManagerControl : MonoBehaviour
{
    [SerializeField]
    private GameObject[] SpawnManagers;
    private int index = 0;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < SpawnManagers.Length;i++)
        {
            SpawnManagers[i].SetActive(false);
        }

        SpawnManagers[index].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(SpawnManagers[index].transform.position.z < GameManager.Instance.PlayerPos.z)
        {
            SpawnManagers[index++].gameObject.SetActive(false);
            SpawnManagers[index].gameObject.SetActive(true);
        }
    }
}
