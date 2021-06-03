using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    #region 필드
    public enum CheckOption { Music = 0, Distance }
    [SerializeField]
    private Transform startPoint; // 스폰할 위치
    [SerializeField]
    private Transform endPoint; // 스폰 후 움직일 위치
    [SerializeField]
    private bool isMove = true; // 스폰 후 이동할 것인지 이 경우 startPoint에서 가만히 있는다.
    [SerializeField]
    private CheckOption checkOption = CheckOption.Distance; // 스폰 조건, 플레이어와의 거리 비교 후 스폰 또는 움악의 시간과 비교 후 스폰
    [SerializeField]
    private int musicTime = 0; // CheckOption.Music 인 경우 음악의 실행 되고 언제 스폰 될것인가?
    [SerializeField]
    private float distance = 0f;// CheckOption.Distance 인 경우 플레이어와 거리가 언제일때 스폰 될것인가?
    #endregion
    #region 프로퍼티
    public Transform StartPoint // 스폰할 위치
    {
        get
        {
            return startPoint;
        }
    }
    public Transform EndPoint // 스폰 후 움직일 위치
    {
        get
        {
            return endPoint;
        }
    }
    public bool IsMove // 스폰 후 이동할 것인지 이 경우 startPoint에서 가만히 있는다.
    {
        get
        {
            return isMove;
        }
    }
    public CheckOption OptionCheck // 스폰 조건, 플레이어와의 거리 비교 후 스폰 또는 움악의 시간과 비교 후 스폰
    {
        get
        {
            return checkOption;
        }
    }
    public int MusicTime // CheckOption.Music 인 경우 음악의 실행 되고 언제 스폰 될것인가?
    {
        get
        {
            return musicTime;
        }
    }
    public float Distance // CheckOption.Distance 인 경우 플레이어와 거리가 언제일때 스폰 될것인가?
    {
        get
        {
            return distance;
        }
    }

    public bool IsUse // CheckOption.Distance 인 경우 플레이어와 거리가 언제일때 스폰 될것인가?
    {
        get;set;
    }
    #endregion

    private void Awake()
    {
        startPoint = transform.GetChild(0);
        endPoint = transform.GetChild(1);
    }
}
