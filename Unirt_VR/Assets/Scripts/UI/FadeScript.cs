using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public partial class FadeScript : MonoBehaviour
{
    #region 필드
    [SerializeField]
    private Image BlackFade; // 검은거
    [SerializeField]
    private Image RedFade; // 빨간거
    [SerializeField]
    private Image WhiteFade; // 허연거

    private float time = 0f;
    private float F_time = 1f;
    private static FadeScript instance;
    public static FadeScript Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }
    #endregion


    [SerializeField]
    private ParticleSystem Player_Hit_Effect; // 플레이어 피격 이펙트
    [SerializeField]
    private AudioSource hit_Audio; //플레이어 히트 사운드 재생할 오디오 소스 

    [SerializeField]
    private AudioClip[] player_hit_Clip; //플레이어 히트 오디오클립

    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
        }

        hit_Audio = GameObject.Find("Canvas").GetComponent<AudioSource>(); // 오디오 컴포넌트를 가져옵니다.
    }

    void Start()
    {
        GameManager.Instance.actGameStart += FadeLoadPlay;
        GameManager.Instance.actGameEnd += FadeLoadStart;
        GameManager.Instance.actPlayerDamage += FadeRed;
    }
}

public partial class FadeScript : MonoBehaviour
{
    public void FadeLoadStart()
    {
        StopAllCoroutines();
        StartCoroutine(FadeLoad("StartScoreScene"));
    }
    public void FadeLoadPlay()
    {
        StopAllCoroutines();
        StartCoroutine(FadeLoad("SampleScene"));
    }
    public void FadeRed()
    {
        StopAllCoroutines();
        StartCoroutine(FadeFollow(RedFade, 0.3f));
    }
    public void FadeWhite()
    {
        StopAllCoroutines();
        StartCoroutine(FadeFollow(WhiteFade, 0.3f));
    }

    IEnumerator FadeLoad(string sceneName) // 로드가 끝날때까지 페이드 아웃 하는 함수
    {
        BlackFade.gameObject.SetActive(true);
        time = 0f;
        Color alpha = BlackFade.color;
        while (alpha.a < 1f)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(0, 1, time);
            BlackFade.color = alpha;
            yield return null;
        }
        AsyncOperation asyncOper = SceneManager.LoadSceneAsync(sceneName);
        asyncOper.allowSceneActivation = true;
        yield break;
    }

    IEnumerator FadeFollow(Image fadeImage, float fTime)
    {

        Player_Hit_Effect.Play(); // 히트 이펙트 실행

        //int []_temp = new AudioClip [3];
        //hit_Audio.clip = player_hit_Clip[_temp];
        //hit_Audio.Play();

        fadeImage.gameObject.SetActive(true);

        time = 0f;
        Color alpha = fadeImage.color;
        while (alpha.a < 1f)
        {
            time += Time.deltaTime / fTime;
            alpha.a = Mathf.Lerp(0, 1, time);
            fadeImage.color = alpha;
            yield return null;
        }
        time = 0f;
        while (alpha.a > 0f)
        {
            time += Time.deltaTime / fTime;
            alpha.a = Mathf.Lerp(1, 0, time);
            fadeImage.color = alpha;
            yield return null;
        }
        fadeImage.gameObject.SetActive(false);
        yield break;
    }
}



