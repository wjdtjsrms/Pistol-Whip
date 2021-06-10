using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning_Floor_Manager : MonoBehaviour
{
    [SerializeField]
    private EnemyCtrl enemy; // ����� Enemy ������
    [SerializeField]
    private int BottomCount = 50; // ������ ����
    private List<GameObject> Bottompool = new List<GameObject>(); // ������ ��ü�� ���� ����Ʈ

    private void Awake()
    {
        // �ʿ��� ������ŭ �̸� �����صд�.
        for (int i = 0; i < BottomCount; i++)
        {
            GameObject prefabInstance = Instantiate(enemy.gameObject);
            prefabInstance.transform.SetParent(transform);
            prefabInstance.SetActive(false);
            Bottompool.Add(prefabInstance);
        }
    } 
}
    //public GameObject Spawn(Transform startPos, Transform targetPos)
    //{
    //    foreach (GameObject enemy in Bottompool)
    //    {
    //        if (!enemy.activeInHierarchy)
    //        {
    //            // ���� ��ġ�� ���� ��ġ�� ������ �� ������Ų��.
    //            enemy.GetComponent<EnemyCtrl>().SetTransform(startPos, targetPos);
    //            enemy.SetActive(true);
    //            return enemy;
    //        }
    //    }

    //    // �޸�Ǯ�� ���� ������̶�� ���� �����Ѵ�.
    //    GameObject enemyInstance = Instantiate(enemy.gameObject);
    //    enemyInstance.transform.SetParent(transform);
    //    enemyPool.Add(enemyInstance);
    //    return new GameObject();
