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
    public int HP
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
}
