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
                // 총알은 2.5 ~ 3.5 초후 도착한다.
                var TargetTime = Random.Range(2.5f,3.5f);
                // 위치 + (벡터 * 시간) = 현재 위치에서 벡터만큼의 속도와 방향으로 시간만큼 이동한 목표 위치
                var targetVec = GameManager.Instance.PlayerPos + (Vector3.forward * 3.0f) * TargetTime; // 3.0f 는 플레이어의 속도이다.
                // 총구의 위치에서 목표위치까지 목표 시간에 도달하는 속도를 구한다.
                bullet.GetComponent<Bullet>().bulletspeed = Vector3.Distance(barrelLocation.position, targetVec) / TargetTime; // 속도 = 거리 / 시간
                // 총알을 목표위치 방향으로 돌린다.
                bullet.gameObject.transform.LookAt(targetVec);
                bullet.SetActive(true);
                return bullet;
            }
        }

        // 메모리풀을 전부 사용중일때만 사용
        GameObject bulletInstance = Instantiate(bullet.gameObject);
        bulletInstance.transform.SetParent(transform);
        bulletPool.Add(bulletInstance);
        return bulletInstance;

    }
}
