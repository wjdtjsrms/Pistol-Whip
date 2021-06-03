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
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        actPlayerDie += () => isGameOver = true;
        actPlayerDie += EndGame;
    }

    void Update()
    {
        if (true) // 디버그용 모드
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
    public void RestartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void SetMusic()
    {
        if(!audioSource.isPlaying)
        {
            audioSource.Play();
        }
        else
        {
            audioSource.Pause();
        }
    }
    public void EndGame() 
    {
        isGameOver = true;
    }
    private void LoadStartScene()
    {
        SceneManager.LoadScene("StartScoreScene");
    }
}