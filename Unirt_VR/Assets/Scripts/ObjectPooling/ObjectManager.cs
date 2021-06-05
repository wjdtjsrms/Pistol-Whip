using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectManager : MonoBehaviour
{
    #region �ʵ�
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

        // �ʿ��� ������ŭ �̸� �����صд�.
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

        // �޸�Ǯ�� ���� ������̶�� ���� �����Ѵ�.
        GameObject prefabInstance = Instantiate(Blood_Prefab);
        prefabInstance.transform.SetParent(transform);
        Blood_Pool.Add(prefabInstance);
        return prefabInstance;
    }


}