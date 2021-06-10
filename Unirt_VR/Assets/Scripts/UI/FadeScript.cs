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

    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
        }
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