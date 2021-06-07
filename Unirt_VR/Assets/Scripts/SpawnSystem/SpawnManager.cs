using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private EnemyManager enemyManager; // enemy�� ������ �޸�Ǯ
    private SpawnPoint[] allSpawnPoint; // ���忡 �ִ� ��� SpawnPoint�� �����ϴ� �迭
    private Vector3 playerPos;
    // ������ ���� ������ SpawnPoint�� �����ϴ� ����Ʈ 
    private List<SpawnPoint> musicPoint = new List<SpawnPoint>();
    // �Ÿ��� ���� ������ SpawnPoint�� �����ϴ� ����Ʈ 
    private List<SpawnPoint> distancePoint = new List<SpawnPoint>();

    void Start()
    {
        // ��� SpawnPoint�� �����ͼ� ���� �������� ����� ���� �����Ѵ�.
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
            // ���� �ð��� ������ �ð��� �Ѿ�� ����
            if (child.IsUse == false && child.MusicTime <= GameManager.Instance.MusicPlayTime)
            {
                SpawnChild(child);
            }
        }

        foreach (var child in distancePoint)
        {
            // �÷��̾�� ���� ������ �Ÿ��� ������ �Ÿ����� ª������ ����
            if (child.IsUse == false && child.Distance >= (child.transform.position - playerPos).sqrMagnitude)
            {
                SpawnChild(child);
            }
        }
    }

    private void SpawnChild(SpawnPoint spawnPoint)
    {
        // IsMove ���� ���� �̵� ��ġ�� �ٲ��.
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