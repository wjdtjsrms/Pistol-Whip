using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public  class ObjectManager : MonoBehaviour 
{
    #region 

    private static ObjectManager instance;
    public static ObjectManager Instance { get { return instance; } }
    public float fireTime = 0.01f;
    [SerializeField]
    private GameObject Blood_Prefab;
    [SerializeField]
    private int BloodCount = 5;
    public static List<GameObject> Blood_Pool = null;

    #endregion  // ObjectManager 의 모든 변수들

    public void Start()
    {
        instance = this;

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
        foreach(GameObject Blood in Blood_Pool)
        {
            if (!Blood.activeInHierarchy)
            {
                Blood.SetActive(true);
                return Blood;

            }
        }

        GameObject prefabInstance = Instantiate(Blood_Prefab);
        prefabInstance.transform.SetParent(transform);
        
        Blood_Pool.Add(prefabInstance);
        return prefabInstance;
    }


}