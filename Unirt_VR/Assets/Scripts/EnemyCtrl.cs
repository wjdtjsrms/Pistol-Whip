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
        // ���� ���� �ɰ��ΰ�.

        StopAllCoroutines();
        animator.SetBool("IsRunning", true);
        moveTargetVec = targetPos.position;
        moveTargetVec.y = 0;
        transform.LookAt(moveTargetVec);
        isDie = false;
        scoreUI.SetActive(false);
        muzzle.gameObject.transform.position = barrelLocation.position; // �߻� ����Ʈ�� ��ġ�� �ѱ��� ����    
        StartCoroutine(MoveCoroutine());
    }
    private void OnDisable()
    {
        isDie = true;
        StopAllCoroutines();
        // �ִϸ��̼��� Ų��.
        // ���׵��� Ų��.
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

    // ybot@Shooting �ִϸ��̼��� Event���� ����ȴ�.
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
        // ����Ʈ�� hitPoint, -hitNormal �������� �׸���.
        // ���� ���´�.
        // ������ ���� �Ŵ����� �߰��ȴ�.
        // ��� ����Ʈ�� ���´�.

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
        // ��ǥ ��ġ�� �����ߴٸ� ���� �÷��̾� ������ �ܳ��Ѵ�.
        EnemyAim();
        yield break;
    }
}
