using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SwitchManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject StartBtn;
    public GameObject ReStartBtn;
    public GameObject SelectBtn;
    public GameObject MusicCanavs;
    public GameObject ScoreCanavs;

    public AudioClip otherClip;
    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!audioSource.isPlaying)// 오디오플레이가 끝나면
        {
            SceneManager.LoadScene("StartScoreScene");// 스타트스코어 씬을 로드한다.0.0
            MusicCanavs.SetActive(false);// 음악선택 창을 닫아준다.
            ScoreCanavs.SetActive(true);// 스코어 창을 열어준다.

        }
    }
    // Start is called before the first frame update
    
    public void Click()
    {
         SceneManager.LoadScene("SampleScene");
    }
    public void SelectCilck()
    {
        MusicCanavs.SetActive(true);
        ScoreCanavs.SetActive(false);
    }
}
