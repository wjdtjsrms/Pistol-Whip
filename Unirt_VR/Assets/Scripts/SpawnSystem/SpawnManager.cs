using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private EnemyManager enemyManager; // enemy를 스폰할 메모리풀
    private SpawnPoint[] allSpawnPoint; // 월드에 있는 모든 SpawnPoint를 저장하는 배열
    private Vector3 playerPos;
    // 음악이 스폰 기준인 SpawnPoint를 저장하는 리스트 
    private List<SpawnPoint> musicPoint = new List<SpawnPoint>();
    // 거리가 스폰 기준인 SpawnPoint를 저장하는 리스트 
    private List<SpawnPoint> distancePoint = new List<SpawnPoint>();

    void Start()
    {
        // 모든 SpawnPoint를 가져와서 스폰 기준으로 나누어서 새로 저장한다.
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

    void Update()
    {
        playerPos = GameManager.Instance.PlayerPos;

        foreach (var child in musicPoint)
        {
            // 음악 시간이 지정된 시간을 넘어가면 스폰
            if (child.IsUse == false && child.MusicTime <= GameManager.Instance.MusicPlayTime)
            {
                SpawnChild(child);
            }
        }

        foreach (var child in distancePoint)
        {
            // 플레이어와 스폰 지점의 거리가 지정된 거리보다 짧아지면 스폰
            if (child.IsUse == false && child.Distance >= (child.transform.position - playerPos).sqrMagnitude)
            {
                SpawnChild(child);
            }
        }
    }

    private void SpawnChild(SpawnPoint spawnPoint)
    {
        // IsMove 값에 따라 이동 위치가 바뀐다.
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