using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public partial class GameManager : MonoBehaviour
{

    [SerializeField]
    private PlayerController playerController;
    private AudioSource audioSource;
    private static GameManager instance;
    private bool menumButtonPressed = false; // �޴� ��ư Ŭ���� �Ҹ���
    public  List<DataFormat> Scoreinfo = new List<DataFormat>(); // �÷��̾� ��ȣ�� ������ ������ List
    public List<DataFormat> Saveinfo = new List<DataFormat>();

    public bool isGameOver
    {
        get;
        private set;
    }
    public int Score // �÷��̾� ����
    {
        get ;
        private set;
    }
    public int Count // �÷��� count
    {
        get;
        private set;
    }

    public Vector3 PlayerPos
    {
        get
        {
            return playerController.transform.position;
        }
    }
    public float MusicPlayTime
    {
        get
        {
            if (audioSource.clip != null)
            {
                return audioSource.time;
            }
            else
            {
                return 0f;
            }
        }
    }
}
