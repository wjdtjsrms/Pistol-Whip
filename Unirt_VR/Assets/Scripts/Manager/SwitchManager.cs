using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.SceneManagement;

public class SwitchManager : MonoBehaviour
{
    [SerializeField]
    private GameObject childObject;
    [SerializeField]
    [Range(0.01f, 10f)]
    private float fadeTime;// fadeSpeed���� 10�̸� 1�� ���� Ŭ���� ����
    [SerializeField]
    private Image image; // ���̵� ȿ���� ���Ǵ� ���� �����̹���

    bool YButton = false; // ��ư�� ���ȴ��� Ȯ���ϴ� ����
    //public GameObject StartBtn;
    //public GameObject ReStartBtn;
    //public GameObject GamePlayBtn;
    //public GameObject CustomeBtn;
    //public GameObject AudioBtn;
    //public GameObject HomeBtn;
    public GameObject MusicCanavs;
    public GameObject ScoreCanavs;
    public GameObject SettingWindow;
    public GameObject GamePlayWindow;//����ĵ���� �ȿ��ִ� Ŀ����������
    public GameObject AudioWindow;//����ĵ���� �ȿ��ִ� ����� ���� ĵ����

    public Scene SampleScene;
    public AudioClip otherClip;
    AudioSource audioSource;

    private void Awake()
    {
        image = GetComponent<Image>();
        //Fade In. ����� ���İ��� 1���� 0���� ( ȭ���� ���� �������.)
        StartCoroutine(Fade(1, 0));
    }
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        SetMenu();
        IsnotPlay();
    }

    public void IsnotPlay()
    {
        if (!audioSource.isPlaying)// ������÷��̰� ������
        {
            //Fade Out. ����� ���İ��� 0���� 1���� (ȭ���� ���� ��ο�����.)
            StartCoroutine(Fade(0, 1));

            SceneManager.LoadScene("StartScoreScene");// ��ŸƮ���ھ� ���� �ε��Ѵ�.0.0

            //Fade In. ����� ���İ��� 1���� 0���� ( ȭ���� ���� �������.)
            StartCoroutine(Fade(1, 0));
            MusicCanavs.SetActive(false);// ���Ǽ��� â�� �ݾ��ش�.
            ScoreCanavs.SetActive(true);// ���ھ� â�� �����ش�.
        }
    }
    private void SetMenu()
    {
        if (CustomController.IsButtonPressed(CommonUsages.secondaryButton, ref YButton, false))//���� ��Ʈ�ѷ��� ������ ����â�� �����ϴ�..
        {
            childObject.gameObject.SetActive(!childObject.activeSelf);//UI�� ���� �Ҵ�.
        }

    }

    // Start is called before the first frame update

    public void playClick()// ���� ��ư�� ������ �Ǹ�
    {
         SceneManager.LoadScene("SampleScene");// ���� ���� �ҷ��´�.
    }
    public void HomeCilck()// Ȩ ��ư Ŭ���� 
    {
       
        //Fade Out. ����� ���İ��� 0���� 1���� (ȭ���� ���� ��ο�����.)
        StartCoroutine(Fade(0, 1));

        SceneManager.LoadScene("StartScoreScene");// ��ŸƮ���ھ� ���� �ε��Ѵ�.0.0

        //Fade In. ����� ���İ��� 1���� 0���� ( ȭ���� ���� �������.)
        StartCoroutine(Fade(1, 0));
        MusicCanavs.SetActive(true);// ���� ����Ʈ ĵ������ Ȱ��ȭ
        ScoreCanavs.SetActive(false);// ���ھ� ĵ������ ��Ȱ��ȭ
    }public void GamePlayCilck()// Ȩ ��ư Ŭ���� 
    {
        SettingWindow.SetActive(true);
        GamePlayWindow.SetActive(false);
        AudioWindow.SetActive(false);
    }
    public void CostomCilck()// Ŀ���ҹ�ư Ŭ���� 
    {
        SettingWindow.SetActive(false);
        GamePlayWindow.SetActive(true);
        AudioWindow.SetActive(false);
    } 

    public void AudioCilck()// ����� ��ư Ŭ���� 
    {
        SettingWindow.SetActive(false);
        GamePlayWindow.SetActive(false);
        AudioWindow.SetActive(true);
    } 
     public void CloseCilck()// �ݱ� ��ư Ŭ���� 
    {
        SettingWindow.SetActive(true);
        GamePlayWindow.SetActive(false);
        AudioWindow.SetActive(false);
    } 
    
    private IEnumerator Fade(float start, float end)
    {
        float currentTime = 0.0f;
        float percent = 0.0f;

        while (percent < 1)
        {
            //fadeTime���� ����� fadeTime ����
            //percent ���� 0���� 1�� �����ϵ��� ��
            currentTime += Time.deltaTime;
            percent = currentTime / fadeTime;

            //a ���� start���� end���� fadeTime���� ��ȭ��Ų��.
            Color color = image.color;
            color.a = Mathf.Lerp(start, end, percent);
            image.color = color;

            yield return null;

        }
    }
}
