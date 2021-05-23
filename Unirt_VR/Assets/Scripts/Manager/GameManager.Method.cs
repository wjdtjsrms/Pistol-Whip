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
        player = playerGameObject.GetComponent<PlayerController>();
        actPlayerDie += EndGame;
    }

    void Update()
    {
        if (!isGameOver)
        {
            HP = (int)player?.HP;
        }

        if(true) // ������ ������ isGameOver�� �ٲ۴�.
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