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
        // 필요한 이벤트 리스너 등록
        actPlayerDie += () => isGameOver = true;
        actPlayerDie += () => audioSource.Pause();
        actGamePause += () => audioSource.Pause();
        actGameRestart += () => audioSource.Play();

        StartCoroutine(ReadyGame());
    }

    void Update()
    {
        if (true) // 개발자 모드
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

    private IEnumerator ReadyGame()
    {
        yield return waitOneSecond;

        actGamePause?.Invoke();

        for(int i = 5; i > 0; i--)
        {
            readyText.text = i.ToString();
            yield return waitOneSecond;
        }
        readyText.fontSize = 45;
        readyText.text = "MUSIC START";
        yield return waitOneSecond;

        float percent = 0;
        float speed = 0.5f;
        
        while (percent < 1)
        {
            percent += Time.deltaTime * speed;
            Vector3 size = Vector3.Lerp(readyText.transform.localScale, Vector3.zero, percent);
            readyText.transform.localScale = size;
            yield return null;
        }

        actGameRestart?.Invoke();
    }



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

    // 점수 추가 함수
    public void GetScored(int value)
    {
        Score += value;
    }

    // 플레이 횟수(랭킹 관련 집계)
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