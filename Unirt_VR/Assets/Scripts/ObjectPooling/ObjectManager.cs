using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectManager : MonoBehaviour
{
    #region 필드
    [SerializeField]
    private GameObject Blood_Prefab;
    [SerializeField]
    private int BloodCount = 5;
    private static ObjectManager instance;
    private static List<GameObject> Blood_Pool = null;
    #endregion  

    public static ObjectManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        // 필요한 갯수만큼 미리 생성해둔다.
        Blood_Pool = new List<GameObject>(BloodCount);
        for (int i = 0; i < BloodCount; i++)
        {
            GameObject prefabInstance = Instantiate(Blood_Prefab);
            prefabInstance.transform.SetParent(transform);
            prefabInstance.SetActive(false);
            Blood_Pool.Add(prefabInstance);
        }
    }

    public GameObject Fire()
    {
        foreach (GameObject Blood in Blood_Pool)
        {
            if (!Blood.activeInHierarchy)
            {
                Blood.SetActive(true);
                return Blood;
            }
        }

        // 메모리풀을 전부 사용중이라면 새로 생성한다.
        GameObject prefabInstance = Instantiate(Blood_Prefab);
        prefabInstance.transform.SetParent(transform);
        Blood_Pool.Add(prefabInstance);
        return prefabInstance;
    }


}