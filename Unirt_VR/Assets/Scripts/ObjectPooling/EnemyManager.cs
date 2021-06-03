using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    private EnemyCtrl enemy;
    [SerializeField]
    private int enemyCount = 20;
    private List<GameObject> enemyPool = new List<GameObject>();

    private void Awake()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            GameObject prefabInstance = Instantiate(enemy.gameObject);
            prefabInstance.transform.SetParent(transform);
            prefabInstance.SetActive(false);
            enemyPool.Add(prefabInstance);
        }
    }

    public GameObject Spawn(Transform startPos, Transform targetPos)
    {
        foreach (GameObject enemy in enemyPool)
        {
            if (!enemy.activeInHierarchy)
            {
                enemy.GetComponent<EnemyCtrl>().SetTransform(startPos, targetPos);
                enemy.SetActive(true);
                return enemy;
            }
        }

        // 메모리풀을 전부 사용중일때만 사용
        GameObject enemyInstance = Instantiate(enemy.gameObject);
        enemyInstance.transform.SetParent(transform);
        enemyPool.Add(enemyInstance);
        return new GameObject();

    }

}