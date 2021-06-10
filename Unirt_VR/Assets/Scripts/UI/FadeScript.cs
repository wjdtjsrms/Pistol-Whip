using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public partial class FadeScript : MonoBehaviour
{
    #region �ʵ�
    [SerializeField]
    private Image BlackFade; // ������
    [SerializeField]
    private Image RedFade; // ������
    [SerializeField]
    private Image WhiteFade; // �㿬��

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
    private ParticleSystem Player_Hit_Effect; // �÷��̾� �ǰ� ����Ʈ
    [SerializeField]
    private AudioSource hit_Audio; //�÷��̾� ��Ʈ ���� ����� ����� �ҽ� 

    [SerializeField]
    private AudioClip[] player_hit_Clip; //�÷��̾� ��Ʈ �����Ŭ��

    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
        }

        hit_Audio = GameObject.Find("Canvas").GetComponent<AudioSource>(); // ����� ������Ʈ�� �����ɴϴ�.
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

    IEnumerator FadeLoad(string sceneName) // �ε尡 ���������� ���̵� �ƿ� �ϴ� �Լ�
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

        Player_Hit_Effect.Play(); // ��Ʈ ����Ʈ ����

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



