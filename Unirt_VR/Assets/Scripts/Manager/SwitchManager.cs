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
        if (!audioSource.isPlaying)// ������÷��̰� ������
        {
            SceneManager.LoadScene("StartScoreScene");// ��ŸƮ���ھ� ���� �ε��Ѵ�.0.0
            MusicCanavs.SetActive(false);// ���Ǽ��� â�� �ݾ��ش�.
            ScoreCanavs.SetActive(true);// ���ھ� â�� �����ش�.

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
