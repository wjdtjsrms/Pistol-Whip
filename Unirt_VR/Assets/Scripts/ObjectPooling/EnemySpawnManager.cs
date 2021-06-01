using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    GameObject Enemy = EnemyPooling.Instance.Spawn();

    
    // Update is called once per frame
    void Update()
    {
        Enemy.transform.position = new Vector3(0, 0, 15);
        Enemy.transform.rotation = Quaternion.identity;
        Enemy.SetActive(true);
    }
}
