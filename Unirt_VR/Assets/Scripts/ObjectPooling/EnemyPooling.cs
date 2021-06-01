using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPooling : MonoBehaviour
{
    private static EnemyPooling instance;
    public static EnemyPooling Instance { get { return instance; } }
    public GameObject Enemy;

    public float zPos; //  Sample code 적을 일정 거리 뒤에 생성시킨다.
    public static List<GameObject> EnemyPool = new List<GameObject>();
    private int EnemyCount = 20;
    private Vector3 RandomVetor;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        for (int i = 0; i < EnemyCount; i++)
        {
            GameObject prefabInstance = Instantiate(Enemy);
            prefabInstance.transform.SetParent(transform);

            prefabInstance.SetActive(false);
            EnemyPool.Add(prefabInstance);
        }
    }

    public GameObject Spawn()
    {
        foreach(GameObject move in EnemyPool)
        {
            if(!move.activeInHierarchy)
            {
                move.SetActive(true);
                return move;
            }
        }
        GameObject prefabInstance = Instantiate(Enemy);
        prefabInstance.transform.SetParent(transform);

        EnemyPool.Add(prefabInstance);
        return prefabInstance;

    }


    // pos z 축 15씩 올려준다. 

    IEnumerator EnemySpawn()
    {
        while(true)
        {
            if (EnemyPool.Count != 0)
            {
                zPos = Random.Range(1, 20);
                RandomVetor = new Vector3(0f, 0f, zPos);
                GameObject prefabInstance = Spawn();
                prefabInstance.transform.position = gameObject.transform.position + RandomVetor;
            }
        }
        yield return new WaitForSeconds(1f);
    }
}