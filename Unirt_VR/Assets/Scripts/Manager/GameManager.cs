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
    public bool isGameOver
    {
        get;
        private set;
    }
    public int Score
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
    public Transform PlayerTransform
    {
        get
        {
            return playerController.transform;
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
