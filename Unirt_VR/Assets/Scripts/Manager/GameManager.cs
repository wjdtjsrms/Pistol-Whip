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
    private bool menumButtonPressed = false; // 메뉴 버튼 클릭용 불리언
    public  List<DataFormat> Scoreinfo = new List<DataFormat>(); // 플레이어 번호와 점수를 저장할 List
    public List<DataFormat> Saveinfo = new List<DataFormat>();

    public bool isGameOver
    {
        get;
        private set;
    }
    public int Score // 플레이어 점수
    {
        get ;
        private set;
    }
    public int Count // 플레이 count
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
