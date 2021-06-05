using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    public float bulletspeed = 6f;
    public float attackAmount = 50.0f;
    private Rigidbody bulletRigidbody;
    [SerializeField]
    private GameObject[] ringObjects;
    private float startRange = -0.01f;
    private float endRange = -0.07f;
    private YieldInstruction waitSecond = new WaitForSeconds(3.0f);
    private void Awake()
    {
        bulletRigidbody = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        GameManager.Instance.actPlayerDie += () => gameObject.SetActive(false);
        GameManager.Instance.actGamePause += () => gameObject.SetActive(false);
        GameManager.Instance.actGameRestart += () => gameObject.SetActive(true);
    }
    private void OnEnable()
    {
        bulletRigidbody.velocity = transform.forward * bulletspeed;
        for (int i = 0; i < ringObjects.Length; i++)
        {
            ringObjects[i].transform.localPosition = new Vector3(0f, 0f, startRange * (i + 1));
        }

        StartCoroutine(RingMoveCoroutine());
        StartCoroutine(waitActiveFalse());
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            PlayerController playercontroller= other.GetComponent<PlayerController>();          
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
            float range = Mathf.Lerp(startRange, endRange * bulletspeed, percent);
            for (int i = 0; i < ringObjects.Length; i++)
            {
                ringObjects[i].transform.localPosition = new Vector3(0f, 0f, range * (i + 1));
            }
            yield return null;
        }
        yield break;
    }
    IEnumerator waitActiveFalse()
    {
        yield return waitSecond;
        this.gameObject.SetActive(false);
        yield break;
    }
}
