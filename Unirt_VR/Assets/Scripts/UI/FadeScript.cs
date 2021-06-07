using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField]
    private ParticleSystem Player_hit_Effect;  // 플레이어 피격 이펙트

    //[SerializeField]
    //private GameObject Player_recovery_Effect;  //플레이어 회복 이펙트




    public void FadeBlack()
    {
        StopAllCoroutines();
        StartCoroutine(FadeFollow());
    }
    public void FadeRed()
    {
        StopAllCoroutines();
        StartCoroutine(DamageFade());
    }


    IEnumerator FadeFollow()
    {
        BlackFade.gameObject.SetActive(true);
        time = 0f;
        Color alpha = BlackFade.color;
        while(alpha.a < 1f)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(0,1, time);
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
            Player_hit_Effect.Play(); // 플레이어 피격 파티클 생성
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
        GameManager.Instance.actGameStart += FadeBlack;
        //GameManager.Instance.actPlayerDie += FadeBlack;
        GameManager.Instance.actGameEnd += () => Invoke("FadeBlack", 3f);
        GameManager.Instance.actPlayerDamage += FadeRed;
    }
}
