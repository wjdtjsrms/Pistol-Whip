using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    private EnemyCtrl enemy; // 사용할 Enemy 프리펩
    [SerializeField]
    private int enemyCount = 20; // 생성할 갯수
    private List<GameObject> enemyPool = new List<GameObject>(); // 생성된 객체를 담을 리스트


    private void Awake()
    {
        // 필요한 갯수만큼 미리 생성해둔다.
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
                // 시작 위치와 도착 위치를 설정한 후 스폰시킨다.
                enemy.GetComponent<EnemyCtrl>().SetTransform(startPos, targetPos);
                enemy.SetActive(true);
                return enemy;
            }
        }

        // 메모리풀을 전부 사용중이라면 새로 생성한다.
        GameObject enemyInstance = Instantiate(enemy.gameObject);
        enemyInstance.transform.SetParent(transform);
        enemyPool.Add(enemyInstance);
        return new GameObject();
    }
}