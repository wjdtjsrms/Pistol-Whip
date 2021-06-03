using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    private EnemyCtrl Enemy;
    [SerializeField]
    private int EnemyCount = 20;
    private List<GameObject> EnemyPool = new List<GameObject>();

    private void Awake()
    {
        for (int i = 0; i < EnemyCount; i++)
        {
            GameObject prefabInstance = Instantiate(Enemy.gameObject);
            prefabInstance.transform.SetParent(transform);
            prefabInstance.SetActive(false);
            EnemyPool.Add(prefabInstance);
        }
    }

    public GameObject Spawn(Transform startPos, Transform targetPos)
    {
        foreach (GameObject enemy in EnemyPool)
        {
            if (!enemy.activeInHierarchy)
            {
                enemy.GetComponent<EnemyCtrl>().SetTransform(startPos, targetPos);
                enemy.SetActive(true);
                return enemy;
            }
        }

        // 메모리풀을 전부 사용중일때만 사용
        GameObject enemyInstance = Instantiate(Enemy.gameObject);
        enemyInstance.transform.SetParent(transform);
        EnemyPool.Add(enemyInstance);
        return new GameObject();

    }

}