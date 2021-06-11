using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private GameObject[] ringObjects; // bullet을 꾸며줄 링들
    private float startRange = -0.01f; // 링들이 시작될 위치
    private float endRange = -0.07f; // 링들이 도달할 위치

    private Rigidbody bulletRigidbody;
    private YieldInstruction waitSecond = new WaitForSeconds(3.0f); // 총알이 유지될 시간

    public float bulletspeed // 총알의 속도, BulletPooling에서 spawn시에 변경된다.
    {
        get;
        set;
    }

    private void Awake()
    {
        bulletRigidbody = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        // 필요한 이벤트 리스너들을 등록한다.
        GameManager.Instance.actPlayerDie += () => gameObject.SetActive(false);
        GameManager.Instance.actGamePause += () => gameObject.SetActive(false);
        GameManager.Instance.actGameRestart += () => gameObject.SetActive(true);
    }
    private void OnEnable()
    {
        bulletRigidbody.velocity = transform.forward * bulletspeed; // 총알을 앞으로 이동시킨다. 방향은 BulletPooling에서 정해준다.

        for (int i = 0; i < ringObjects.Length; i++) // 링들을 초기 위치로 옮겨준다.
        {
            ringObjects[i].transform.localPosition = new Vector3(0f, 0f, startRange * (i + 1));
        }

        StartCoroutine(RingMoveCoroutine());
        StartCoroutine(waitActiveFalse());

    }
    private void OnTriggerStay(Collider other)
    {
        if(other.tag != "Player")
        {
            gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player")
        {
            PlayerController playercontroller = other.GetComponent<PlayerController>();
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    IEnumerator RingMoveCoroutine() // 이 형식을 많이 쓰는데 델리게이트로는 안될까?
    {
        // Lerp를 사용하기 위한 변수
        float percent = 0;
        float speed = 1.0f;

        while (percent < 1)
        {
            percent += Time.deltaTime * speed;
            float range = Mathf.Lerp(startRange, endRange * bulletspeed, percent); // 총알의 속도에 맞추어 링들의 최대 이동거리가 결정된다.
            for (int i = 0; i < ringObjects.Length; i++)
            {
                ringObjects[i].transform.localPosition = new Vector3(0f, 0f, range * (i + 1)); // 링들을 서서히 이동시킨다.
            }
            yield return null;
        }
        yield break;
    }

    // 총알의 유지시간이 끝났다면 꺼버린다.
    IEnumerator waitActiveFalse()
    {
        yield return waitSecond;
        this.gameObject.SetActive(false);
        yield break;
    }
}
