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
    private float fadeTime;// fadeSpeed값이 10이면 1초 값은 클수록 빠름
    [SerializeField]
    private Image image; // 페이드 효과에 사용되는 검은 바탕이미지

    bool YButton = false; // 버튼이 눌렸는지 확인하는 변수
    //public GameObject StartBtn;
    //public GameObject ReStartBtn;
    //public GameObject GamePlayBtn;
    //public GameObject CustomeBtn;
    //public GameObject AudioBtn;
    //public GameObject HomeBtn;
    public GameObject MusicCanavs;
    public GameObject ScoreCanavs;
    public GameObject SettingWindow;
    public GameObject GamePlayWindow;//세팅캔버스 안에있는 커스텀윈도우
    public GameObject AudioWindow;//세팅캔버스 안에있는 오디오 설정 캔버스

    public Scene SampleScene;
    public AudioClip otherClip;
    AudioSource audioSource;

    private void Awake()
    {
        image = GetComponent<Image>();
        //Fade In. 배경의 알파값이 1에서 0으로 ( 화면이 점점 밝아진다.)
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
        if (!audioSource.isPlaying)// 오디오플레이가 끝나면
        {
            //Fade Out. 배경의 알파값이 0에서 1으로 (화면이 점점 어두워진다.)
            StartCoroutine(Fade(0, 1));

            SceneManager.LoadScene("StartScoreScene");// 스타트스코어 씬을 로드한다.0.0

            //Fade In. 배경의 알파값이 1에서 0으로 ( 화면이 점점 밝아진다.)
            StartCoroutine(Fade(1, 0));
            MusicCanavs.SetActive(false);// 음악선택 창을 닫아준다.
            ScoreCanavs.SetActive(true);// 스코어 창을 열어준다.
        }
    }
    private void SetMenu()
    {
        if (CustomController.IsButtonPressed(CommonUsages.secondaryButton, ref YButton, false))//왼쪽 컨트롤러를 누르면 설정창이 열립니다..
        {
            childObject.gameObject.SetActive(!childObject.activeSelf);//UI를 껐다 켠다.
        }

    }

    // Start is called before the first frame update

    public void playClick()// 시작 버튼을 누르게 되면
    {
         SceneManager.LoadScene("SampleScene");// 샘플 씬을 불러온다.
    }
    public void HomeCilck()// 홈 버튼 클릭시 
    {
       
        //Fade Out. 배경의 알파값이 0에서 1으로 (화면이 점점 어두워진다.)
        StartCoroutine(Fade(0, 1));

        SceneManager.LoadScene("StartScoreScene");// 스타트스코어 씬을 로드한다.0.0

        //Fade In. 배경의 알파값이 1에서 0으로 ( 화면이 점점 밝아진다.)
        StartCoroutine(Fade(1, 0));
        MusicCanavs.SetActive(true);// 뮤직 셀렉트 캔버스는 활성화
        ScoreCanavs.SetActive(false);// 스코어 캔버스는 비활성화
    }public void GamePlayCilck()// 홈 버튼 클릭시 
    {
        SettingWindow.SetActive(true);
        GamePlayWindow.SetActive(false);
        AudioWindow.SetActive(false);
    }
    public void CostomCilck()// 커스텀버튼 클릭시 
    {
        SettingWindow.SetActive(false);
        GamePlayWindow.SetActive(true);
        AudioWindow.SetActive(false);
    } 

    public void AudioCilck()// 오디오 버튼 클릭시 
    {
        SettingWindow.SetActive(false);
        GamePlayWindow.SetActive(false);
        AudioWindow.SetActive(true);
    } 
     public void CloseCilck()// 닫기 버튼 클릭시 
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
            //fadeTime으로 나누어서 fadeTime 동안
            //percent 값이 0에서 1로 증가하도록 함
            currentTime += Time.deltaTime;
            percent = currentTime / fadeTime;

            //a 값을 start에서 end까지 fadeTime동안 변화시킨다.
            Color color = image.color;
            color.a = Mathf.Lerp(start, end, percent);
            image.color = color;

            yield return null;

        }
    }
}
