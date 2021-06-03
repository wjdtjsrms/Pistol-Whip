using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPooling : MonoBehaviour
{
    [SerializeField]
    private Bullet bullet;
    [SerializeField]
    private int bulletCount = 10;
    private List<GameObject> bulletPool = new List<GameObject>();
    private static BulletPooling instance = null;
    public static BulletPooling Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }

    private void Awake()
    {
        if(null == instance)
        {
            instance = this;
        }

        for (int i = 0; i < bulletCount; i++)
        {
            GameObject prefabInstance = Instantiate(bullet.gameObject);
            prefabInstance.transform.SetParent(transform);
            prefabInstance.SetActive(false);
            bulletPool.Add(prefabInstance);
        }
    }

    public GameObject Spawn(Transform barrelLocation)
    {
        foreach (GameObject bullet in bulletPool)
        {
            if (!bullet.activeInHierarchy)
            {
                bullet.transform.position = barrelLocation.position;
                Vector3 temp = GameManager.Instance.PlayerPos + Vector3.forward * 3.0f; // �÷��̾ 1�ʵ� ������ ��ġ
                bullet.gameObject.transform.LookAt(temp);
                bullet.SetActive(true);
                return bullet;
            }
        }

        // �޸�Ǯ�� ���� ������϶��� ���
        GameObject bulletInstance = Instantiate(bullet.gameObject);
        bulletInstance.transform.SetParent(transform);
        bulletPool.Add(bulletInstance);
        return bulletInstance;

    }
}
