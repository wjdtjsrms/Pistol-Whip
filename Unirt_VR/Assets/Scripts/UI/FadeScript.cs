using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeScript : MonoBehaviour
{
    [SerializeField]
    private Image BlackFade;
    [SerializeField]
    private Image RedFade;
    private float time = 0f;
    private float F_time = 1f;
    private float F_time_Red = 0.4f;
    private YieldInstruction waitOneSecond = new WaitForSeconds(1f);

    public void FadeBlack()
    {
        StopAllCoroutines();
        StartCoroutine(FadeFollow());
    }
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
        StartCoroutine(DamageFade());
    }
    IEnumerator FadeLoad(string sceneName)
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

    IEnumerator FadeFollow()
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
        time = 0f;
        yield return waitOneSecond;
        while (alpha.a > 0f)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(1, 0, time);
            BlackFade.color = alpha;
            yield return null;
        }
        BlackFade.gameObject.SetActive(false);
        yield break;
    }

    IEnumerator DamageFade()
    {
        RedFade.gameObject.SetActive(true);
        time = 0f;
        Color alpha = RedFade.color;
        while (alpha.a < 1f)
        {
            time += Time.deltaTime / F_time_Red;
            alpha.a = Mathf.Lerp(0, 1, time);
            RedFade.color = alpha;
            yield return null;
        }
        time = 0f;
        while (alpha.a > 0f)
        {
            time += Time.deltaTime / F_time_Red;
            alpha.a = Mathf.Lerp(1, 0, time);
            RedFade.color = alpha;
            yield return null;
        }
        RedFade.gameObject.SetActive(false);
        yield break;
    }

    void Start()
    {
        GameManager.Instance.actGameStart += FadeLoadPlay;
        GameManager.Instance.actGameEnd += FadeLoadStart;
        GameManager.Instance.actPlayerDamage += FadeRed;

    }
}
