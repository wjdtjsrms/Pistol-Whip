using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    #region �ʵ�
    public enum CheckOption { Music = 0, Distance }
    [SerializeField]
    private Transform startPoint; // ������ ��ġ
    [SerializeField]
    private Transform endPoint; // ���� �� ������ ��ġ
    [SerializeField]
    private bool isMove = true; // ���� �� �̵��� ������ �� ��� startPoint���� ������ �ִ´�.
    [SerializeField]
    private CheckOption checkOption = CheckOption.Distance; // ���� ����, �÷��̾���� �Ÿ� �� �� ���� �Ǵ� ����� �ð��� �� �� ����
    [SerializeField]
    private int musicTime = 0; // CheckOption.Music �� ��� ������ ���� �ǰ� ���� ���� �ɰ��ΰ�?
    [SerializeField]
    private float distance = 0f;// CheckOption.Distance �� ��� �÷��̾�� �Ÿ��� �����϶� ���� �ɰ��ΰ�?
    #endregion
    #region ������Ƽ
    public Transform StartPoint // ������ ��ġ
    {
        get
        {
            return startPoint;
        }
    }
    public Transform EndPoint // ���� �� ������ ��ġ
    {
        get
        {
            return endPoint;
        }
    }
    public bool IsMove // ���� �� �̵��� ������ �� ��� startPoint���� ������ �ִ´�.
    {
        get
        {
            return isMove;
        }
    }
    public CheckOption OptionCheck // ���� ����, �÷��̾���� �Ÿ� �� �� ���� �Ǵ� ����� �ð��� �� �� ����
    {
        get
        {
            return checkOption;
        }
    }
    public int MusicTime // CheckOption.Music �� ��� ������ ���� �ǰ� ���� ���� �ɰ��ΰ�?
    {
        get
        {
            return musicTime;
        }
    }
    public float Distance // CheckOption.Distance �� ��� �÷��̾�� �Ÿ��� �����϶� ���� �ɰ��ΰ�?
    {
        get
        {
            return distance;
        }
    }

    public bool IsUse // CheckOption.Distance �� ��� �÷��̾�� �Ÿ��� �����϶� ���� �ɰ��ΰ�?
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
