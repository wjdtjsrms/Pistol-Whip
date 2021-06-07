using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    private EnemyCtrl enemy; // ����� Enemy ������
    [SerializeField]
    private int enemyCount = 20; // ������ ����
    private List<GameObject> enemyPool = new List<GameObject>(); // ������ ��ü�� ���� ����Ʈ


    private void Awake()
    {
        // �ʿ��� ������ŭ �̸� �����صд�.
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
                // ���� ��ġ�� ���� ��ġ�� ������ �� ������Ų��.
                enemy.GetComponent<EnemyCtrl>().SetTransform(startPos, targetPos);
                enemy.SetActive(true);
                return enemy;
            }
        }

        // �޸�Ǯ�� ���� ������̶�� ���� �����Ѵ�.
        GameObject enemyInstance = Instantiate(enemy.gameObject);
        enemyInstance.transform.SetParent(transform);
        enemyPool.Add(enemyInstance);
        return new GameObject();
    }
}