using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public partial class EnemyCtrl : MonoBehaviour, IShotAble
{
    #region �ʵ�
    [SerializeField]
    private Bullet bullet; // ������ �ҷ��� ������
    [SerializeField]
    private GameObject scoreUI; // ����� ������ ���� UI
    [SerializeField]
    private AudioClip attackClip; // �� ���ݽ� ����� ����� Ŭ��
    [SerializeField]
    private ParticleSystem muzzle; // ���ݽ� ����� ����Ʈ
    [SerializeField]
    private LineRenderer line; // ���ݼ� ������
    [SerializeField]
    private Transform barrelLocation; // �Ѿ��� ���� ��ġ
    [SerializeField]
    private float limitDistance = 6.0f; // �󸶳� �������� �����������.
    private AudioSource audioSource;
    private TextMeshPro scoreText;
    private Animator animator;
    private Vector3 sizeUI;

    [SerializeField]
    private ParticleSystem appear_Effect;
    [SerializeField]
    private ParticleSystem disappear_Effect;

    private Transform targetPos; // ���� �� �̵��� ��ġ
    private Vector3 moveTargetVec; // �̵��� ��ǥ ��ġ
    private Vector3 playerPos; // �÷��̾��� ��ġ   
    private float moveSpeed = 4.0f; // �̵� �ӵ�
    private bool isDie = false; // ���� ���� ����

    // �ڷ�ƾ ����ȭ�� ���� ���� ����
    YieldInstruction waitShort = new WaitForSeconds(0.5f);
    YieldInstruction waitAttackDely = new WaitForSeconds(3.0f);

    //��� ������Ƽ
    bool laser;
    bool ShotWait
    {
        get => laser;
        set
        {
            laser = value;

            if (animator.GetBool("IsAttack") == laser) //IsAttack�� false�϶� ���η����� true, true�϶� false
            {
                drawWarningLine(GameManager.Instance.PlayerPos);
                line.enabled = !laser;
            }
        }
    }

    #endregion

    private void Awake()
    {
        // �ʿ��� ������Ʈ�� �����´�.
        animator = GetComponent<Animator>();
        scoreText = scoreUI.GetComponent<TextMeshPro>();
        audioSource = GetComponent<AudioSource>();

        //ShotWait = true;
        sizeUI = scoreUI.transform.localScale;
    }

    private void Start()
    {
        GameManager.Instance.actPlayerDie += () => gameObject.SetActive(false);
        GameManager.Instance.actGamePause += () => gameObject.SetActive(false);
        GameManager.Instance.actGameRestart += () => gameObject.SetActive(true);
    }
    // ���� �ٽ� �ʱ�ȭ �Ѵ�.
    private void OnEnable()
    {
        StopAllCoroutines();

        appear_Effect.Play();

        animator.enabled = true;
        animator.SetBool("IsRunning", true);

        GetComponent<Collider>().enabled = true;

        scoreUI.transform.localScale = sizeUI;
        scoreText.enabled = true;
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
        StopAllCoroutines();
    }
    private void Update()
    {
        if(this.gameObject.activeSelf && GameManager.Instance.PlayerPos.z - limitDistance > this.transform.position.z)
        {

            this.gameObject.SetActive(false);
        }
    }
}

public partial class EnemyCtrl : MonoBehaviour, IShotAble
{

    public void SetTransform(Transform startPos, Transform targetPos)
    {
        this.transform.position = startPos.position;
        this.targetPos = targetPos;
    }

    private void EnemyAim()
    {
        StopAllCoroutines();

        // y���� ������ ������ �ٶ󺻴�.
        playerPos = GameManager.Instance.PlayerPos;
        playerPos.y = 0;
        transform.LookAt(playerPos);

        //drawWarningLine(GameManager.Instance.PlayerPos); // ��� ��� �޼ҵ� ȣ��

        animator.SetBool("IsRunning", false);
        StartCoroutine(AttackCoroutine());

    }
    public void drawWarningLine(Vector3 playerPos) // ��� ���
    {  
        line.SetPosition(0, barrelLocation.position);
        line.SetPosition(1, playerPos); 
        line.enabled = true;
    }

    private void EnemyAttack()
    {
        animator.SetBool("IsAttack", true); // ���� �ִϸ��̼� ����
        muzzle.Play(); // ���� ����Ʈ ����
        line.enabled = false;
        audioSource.PlayOneShot(attackClip); // ���� ���� ����
        BulletPooling.Instance.Spawn(barrelLocation);

        //StartCoroutine(laserprint());
    }

    // ybot@Shooting �ִϸ��̼��� Event���� ����ȴ�. �����ϸ� �ƹ�ư ū�� ����.
    void ResetAttack()
    {
        animator.SetBool("IsAttack", false);
    }

    // ���� �÷��̾ �� �Ѿ˿� �¾����� ����� �Լ�
    public void OnShot(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        StopAllCoroutines();
        EnemyDamage();

        animator.enabled = false; // ���׵� Ȱ��ȭ�� ���� �ִϸ����͸� ����
        isDie = true; // ��� ���� �׾���.
     
        GetScore(); // ������ ȹ���Ѵ�.
        GameManager.Instance.EnemyDie(this); // �� ��� �̺�Ʈ�� �����Ѵ�.
        StartCoroutine(EnemyDieCoroutine()); // ��� ó�� �ڷ�ƾ�� �����Ų��.
        GetComponent<Collider>().enabled = false;


        // ����Ʈ�� hitPoint, hitNormal �������� �׸���.
        GameObject BloodObject = ObjectManager.Instance.Fire();
        BloodObject.transform.position = hitPoint;
        BloodObject.transform.rotation = Quaternion.LookRotation(hitNormal);

        // ��� ����Ʈ�� ���´�.
        disappear_Effect.Play();
    }
    public void EnemyDamage()
    {
        animator.enabled = false; // ���׵� Ȱ��ȭ�� ���� �ִϸ����͸� ����
        isDie = true; // ��� ���� �׾���.
        GetScore(); // ������ ȹ���Ѵ�.
        GameManager.Instance.EnemyDie(this); // �� ��� �̺�Ʈ�� �����Ѵ�.
        StartCoroutine(EnemyDieCoroutine()); // ��� ó�� �ڷ�ƾ�� �����Ų��.
        GetComponent<Collider>().enabled = false;
    }


    // ������ ���̰� �ϰ� ���� �Ŵ����� ������ �߰��ϴ� �Լ�
    private void GetScore()
    {
        var score = Random.Range(80, 120); // ������ �켱 �������� å���Ѵ�.
        var color = score > 100 ? Color.red : Color.black; // 100�� �̻��� ����, ���ϴ� ���������� ǥ�õȴ�.

        scoreUI.SetActive(true); // ���� UI�� Ų��.
        Vector3 vec = GameManager.Instance.PlayerPos - transform.position;
        vec.Normalize();
        Quaternion q = Quaternion.LookRotation(-vec);
        scoreUI.transform.rotation = q; // ���� UI�� �÷��̾ �ٶ󺸰� �Ѵ�.

        scoreText.text = score.ToString(); //  ������ ������Ʈ �Ѵ�.
        scoreText.color = color; // ������ �ٲ۴�.
        GameManager.Instance.GetScored(score); // ���ӸŴ����� ������ �߰��Ѵ�.
    }

    // Enemy�� ����� ���� �� �ڷ�ƾ
    IEnumerator EnemyDieCoroutine()
    {
        yield return waitShort; // ��� ��ٸ���.

        // Lerp�� ����ϱ� ���� ����
        float percent = 0;
        float speed = 0.5f;

        while (percent < 1)
        {
            percent += Time.deltaTime * speed;
            Vector3 size = Vector3.Lerp(scoreUI.transform.localScale, Vector3.zero, percent);
            scoreUI.transform.localScale = size; // ���� UI ����� ���������� 0���� ������.
            yield return null;
        }

        scoreText.enabled = false; // ���� UI�� ����.
        this.gameObject.SetActive(false); // �� ������Ʈ�� ����.
        yield break;

    }

    // Enemy�� ���ݽ� ���� �� �ڷ�ƾ
    IEnumerator AttackCoroutine()
    {      
        while (!isDie) // �ױ� ������ ��� ����ȴ�.
        {
            yield return waitAttackDely;
            
            EnemyAttack();
        }
        yield break;
    }

    // Enemy�� �̵��� ���� �� �ڷ�ƾ
    IEnumerator MoveCoroutine()
    {
        // ��ǥ ��ġ�� ���������� �̵��Ѵ�.
        while (Vector3.Distance(transform.position, moveTargetVec) > 0.5f)
        {
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
            yield return null;
        }
        // ��ǥ ��ġ�� �����ߴٸ� ���� �÷��̾� ������ �ܳ��Ѵ�.
        EnemyAim();
        yield break;
    }

    // ��� ��� �ڷ�ƾ
    IEnumerator laserprint()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f); // 0.1�ʸ��� ����� ���� �Ѱ�
            ShotWait = !ShotWait;
        }
    }
}

