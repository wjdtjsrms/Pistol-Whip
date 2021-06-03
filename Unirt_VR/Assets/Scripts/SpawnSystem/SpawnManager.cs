using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private EnemyManager enemyManager;
    private SpawnPoint[] allSpawnPoint;
    private Vector3 playerPos;

    private List<SpawnPoint> musicPoint = new List<SpawnPoint>();
    private List<SpawnPoint> distancePoint = new List<SpawnPoint>();

    void Start()
    {
        allSpawnPoint = GetComponentsInChildren<SpawnPoint>();
        foreach (var child in allSpawnPoint)
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
        playerPos = GameManager.Instance.PlayerPos;

        foreach (var child in musicPoint)
        {
            if (child.IsUse == false && child.MusicTime <= Time.time)
            {
                SpawnChild(child);
            }
        }

        foreach (var child in distancePoint)
        {
            if (child.IsUse == false && child.Distance >= (child.transform.position - playerPos).sqrMagnitude)
            {
                SpawnChild(child);
            }
        }
    }

    private void SpawnChild(SpawnPoint spawnPoint)
    {
        spawnPoint.IsUse = true;
        if (spawnPoint.IsMove == true)
        {
            enemyManager.Spawn(spawnPoint.StartPoint, spawnPoint.EndPoint);
        }
        else
        {
            enemyManager.Spawn(spawnPoint.StartPoint, spawnPoint.StartPoint);
        }
    }

}