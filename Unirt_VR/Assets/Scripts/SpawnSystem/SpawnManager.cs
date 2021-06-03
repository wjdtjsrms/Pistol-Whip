using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private EnemyManager enemyManager;
    private SpawnPoint[] allSpawnPoint;

    private List<SpawnPoint> musicPoint = new List<SpawnPoint>();
    private List<SpawnPoint> distancePoint = new List<SpawnPoint>();

    void Start()
    {
        allSpawnPoint = GetComponentsInChildren<SpawnPoint>();
        foreach (SpawnPoint child in allSpawnPoint)
        {
            if (child.OptionCheck == SpawnPoint.CheckOption.Music)
            {
                musicPoint.Add(child);
            }
            else if (child.OptionCheck == SpawnPoint.CheckOption.Distance)
            {
                distancePoint.Add(child);
            }
        }


    }

    // Update is called once per frame
    void Update()
    {
        foreach (SpawnPoint child in musicPoint)
        {
            if (child.MusicTime <= Time.time && child.IsUse == false)
            {
                child.IsUse = true;
                enemyManager.Spawn(child.StartPoint, child.EndPoint);
            }

        }

        foreach (SpawnPoint child in distancePoint)
        {
            if (child.Distance >= Vector3.Distance(child.transform.position, GameManager.Instance.PlayerPos) && child.IsUse == false)
            {
                child.IsUse = true;
                enemyManager.Spawn(child.StartPoint, child.EndPoint);
            }
        }
    }
}