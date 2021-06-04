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
                // �Ѿ��� 1~3 ���� �����Ѵ�.
                var TargetTime = Random.Range(1,3);
                // ��ġ + (���� * �ð�) = ���� ��ġ���� ���͸�ŭ�� �ӵ��� �������� �ð���ŭ �̵��� ��ǥ ��ġ
                var targetVec = GameManager.Instance.PlayerPos + (Vector3.forward * 3.0f) * TargetTime; // 3.0f �� �÷��̾��� �ӵ��̴�.
                // �ѱ��� ��ġ���� ��ǥ��ġ���� ��ǥ �ð��� �����ϴ� �ӵ��� ���Ѵ�.
                bullet.GetComponent<Bullet>().bulletspeed = Vector3.Distance(barrelLocation.position, targetVec) / TargetTime; // �ӵ� = �Ÿ� / �ð�
                // �Ѿ��� ��ǥ��ġ �������� ������.
                bullet.gameObject.transform.LookAt(targetVec);
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