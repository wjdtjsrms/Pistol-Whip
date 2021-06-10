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
        Count = 0;
        isGameOver = false;
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        // �ʿ��� �̺�Ʈ ������ ���
        actPlayerDie += () => isGameOver = true;
        actPlayerDie += () => audioSource.Pause();
        actGamePause += () => audioSource.Pause();
        actGameRestart += () => audioSource.Play();
        //actGameEnd += () => Invoke("LoadStartScene", 3.5f);
    }

    void Update()
    {
        if (true) // ������ ���
        {
            if (CustomController.IsButtonPressed(CommonUsages.menuButton, ref menumButtonPressed, true))
            {
                SceneManager.LoadScene("SampleScene");
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

    // ���� �߰� �Լ�
    public void GetScored(int value)
    {
        Score += value;
    }

    // �÷��� Ƚ��(��ŷ ���� ����)
    public void GetCount(int value)
    {
        Count += value;
    }

    public void SetMusic()
    {
        if (!audioSource.isPlaying)
        {   
            audioSource.Play();
        }
        else
        {
            audioSource.Pause();
        }
    }
    private void LoadStartScene()
    {
        SceneManager.LoadSceneAsync("StartScoreScene");
    }
}