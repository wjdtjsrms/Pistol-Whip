using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
public partial class GameManager : MonoBehaviour
{
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        Score = 0;
        isGameOver = false;
    }

    void Start()
    {
        actPlayerDie += EndGame;
    }

    void Update()
    {
        if (!isGameOver) // 데미지를 입을 때만 호출되게 변경
        {
            HP = (int)playerController?.HP;
        }

        if (true) // 개발이 끝나면 isGameOver로 바꾼다.
        {
            if (CustomController.IsButtonPressed(CommonUsages.menuButton, ref menumButtonPressed, true))
            {
                RestartGame();
            }
        }

    }
}
public partial class GameManager : MonoBehaviour
{
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            else
            {
                return instance;
            }
        }
    }

    public void GetScored(int value)
    {
        Score += value;
    }
    private void EndGame()
    {
        isGameOver = true;
    }
    public void RestartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
}