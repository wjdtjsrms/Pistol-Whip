using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyCtrl : MonoBehaviour, IShotAble
{
    [SerializeField]
    private Transform targetPos;
    [SerializeField]
    private Bullet bullet;
    [SerializeField]
    private GameObject scoreUI;
    [SerializeField]
    private AudioClip attackClip;
    private AudioSource audioSource;
    private TextMeshPro scoreText;
    [SerializeField]
    private ParticleSystem muzzle;
    [SerializeField]
    private Transform barrelLocation;
    private Vector3 moveTargetVec;
    private Vector3 playerPos;
    private Animator animator;
    private float moveSpeed = 4.0f;
    private bool isDie = false;

    YieldInstruction waitShort = new WaitForSeconds(1.0f);
    YieldInstruction waitAttackDely = new WaitForSeconds(2.0f);

    private void Awake()
    {
        animator = GetComponent<Animator>();
        scoreText = scoreUI.GetComponent<TextMeshPro>();
        audioSource = GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    private void OnEnable()
    {
        // 언제 스폰 될것인가.

        StopAllCoroutines();
        animator.SetBool("IsRunning", true);
        moveTargetVec = targetPos.position;
        moveTargetVec.y = 0;
        transform.LookAt(moveTargetVec);
        isDie = false;
        scoreUI.SetActive(false);
        muzzle.gameObject.transform.position = barrelLocation.position; // 발사 이펙트의 위치를 총구로 변경    
        StartCoroutine(MoveCoroutine());
    }
    private void OnDisable()
    {
        isDie = true;
        StopAllCoroutines();
        // 애니메이션을 킨다.
        // 레그돌을 킨다.
    }

    // Update is called once per frame
    void Update()
    {
    }

    void EnemyAim()
    {
        playerPos = GameManager.Instance.PlayerPos;
        playerPos.y = 0;
        transform.LookAt(playerPos);
        drawWarningLine(playerPos);

        animator.SetBool("IsRunning", false);
        StopAllCoroutines();
        StartCoroutine(AttackCoroutine());
    }
    public void drawWarningLine(Vector3 playerPos)
    {

    }

    void EnemyAttack()
    {
        animator.SetBool("IsAttack", true);
        muzzle.Play();
        audioSource.PlayOneShot(attackClip);
        Instantiate(bullet, barrelLocation.position, transform.rotation).gameObject.transform.LookAt(GameManager.Instance.PlayerPos);
    }

    // ybot@Shooting 애니메이션의 Event에서 실행된다.
    void ResetAttack()
    {
        animator.SetBool("IsAttack", false);
    }



    public void OnShot(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        StopAllCoroutines();
        GetComponent<Animator>().enabled = false;

        scoreUI.SetActive(true);
        scoreUI.transform.LookAt(new Vector3(0, GameManager.Instance.PlayerPos.y, 0));
        scoreUI.transform.Rotate(new Vector3(0, 180, 0));

        var score = Random.Range(80, 120);
        var color = score > 100 ? Color.red : Color.black;
        scoreText.text = score.ToString();
        scoreText.color = color;

        StartCoroutine(ScoreTextCoroutine());
        GameManager.Instance.EnemyDie(this);
        GameManager.Instance.GetScored(score);
        // 이펙트를 hitPoint, -hitNormal 방향으로 그린다.
        // 점수 나온다.
        // 점수가 게임 매니저에 추가된다.
        // 사망 이펙트가 나온다.

        Invoke("SetActiveFalse", 2f);

    }

    IEnumerator ScoreTextCoroutine()
    {
        yield return waitShort;

        float percent = 0;
        float speed = 0.5f;
        while (percent < 1)
        {
            percent += Time.deltaTime * speed;      
            Vector3 size = Vector3.Lerp(scoreUI.transform.localScale, Vector3.zero, percent);
            scoreUI.transform.localScale = size;
            yield return null;
        }
        yield break;

    }

    private void SetActiveFalse()
    {
        scoreText.enabled = false;
        this.gameObject.SetActive(false);
    }

    IEnumerator AttackCoroutine()
    {
        while (!isDie)
        {
            yield return waitAttackDely;
            EnemyAttack();
        }
        yield break;
    }
    IEnumerator MoveCoroutine()
    {
        while (Vector3.Distance(transform.position, moveTargetVec) > 0.5f)
        {
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
            yield return null;
        }
        // 목표 위치에 도착했다면 현재 플레이어 방향을 겨냥한다.
        EnemyAim();
        yield break;
    }
}
