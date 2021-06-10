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
    [SerializeField]
    private ParticleSystem appear_Effect; // ���� ����Ʈ
    [SerializeField]
    private ParticleSystem disappear_Effect; // ����� ����Ʈ
    [SerializeField]
    private Material enemyMaterial;

    private AudioSource audioSource;
    private TextMeshPro scoreText;
    private Animator animator;
    private int hp = 0;
    private int score = 0;
    private int layerMask; // ���̿� �浹�� ��ü�� �����ϱ� ���� ���̾��ũ

    private Transform targetPos; // ���� �� �̵��� ��ġ
    private Vector3 moveTargetVec; // �̵��� ��ǥ ��ġ
    private Vector3 playerPos; // �÷��̾��� ��ġ   
    private float moveSpeed = 4.5f; // �̵� �ӵ�
    private bool isDie = false; // ���� ���� ����
    private Vector3 sizeUI; // ���� UI�� �⺻ ũ��

    // �ڷ�ƾ ����ȭ�� ���� ���� ����
    private YieldInstruction waitShort = new WaitForSeconds(0.5f);
    private YieldInstruction waitTwoSecond = new WaitForSeconds(2.0f);
    private YieldInstruction waitAttackDely = new WaitForSeconds(5.0f);

    private bool isAim = false;

    #endregion

    private void Awake()
    {
        // �ʿ��� ������Ʈ�� �����´�.
        animator = GetComponent<Animator>();
        scoreText = scoreUI.GetComponent<TextMeshPro>();
        audioSource = GetComponent<AudioSource>();

        // �⺻ ���� UI�� ũ�⸦ ����Ѵ�.
        sizeUI = scoreUI.transform.localScale;
        layerMask = (1 << LayerMask.NameToLayer("Player")) + (1 << LayerMask.NameToLayer("Wall")); // �� ���̴� Player �� Wall ���̾ �浹�Ѵ�.
    }

    private void Start()
    {
        // �ʿ��� �̺�Ʈ �����ʵ��� ����Ѵ�.
        GameManager.Instance.actPlayerDie += () => gameObject.SetActive(false);
        GameManager.Instance.actGamePause += () => gameObject.SetActive(false);
        GameManager.Instance.actGameRestart += () => 
        { if(isDie == false)
            {
                gameObject.SetActive(true);
            }       
        };
    }

    // ���� �ٽ� �ʱ�ȭ �Ѵ�.
    private void OnEnable()
    {
        StopAllCoroutines();

        // ������ �⺻������ �ǵ�����.
        isDie = false;
        animator.enabled = true;
        animator.SetBool("IsRunning", true);
        GetComponent<Collider>().enabled = true;

        // ���� ����Ʈ�� �����ش�.
        appear_Effect.Play();

        // �߻� ����Ʈ�� ��ġ�� �ѱ��� ����    
        muzzle.gameObject.transform.position = barrelLocation.position;

        // ���� UI�� �ʱ�ȭ �Ѵ�.
        scoreUI.transform.localScale = sizeUI;
        scoreUI.SetActive(false);
        scoreText.enabled = true;

        // ��ǥ ��ġ�� �ٶ󺻴�.
        moveTargetVec = targetPos.position;
        // moveTargetVec.y = 0; ������ �ٿ�
        transform.LookAt(moveTargetVec);

        // ��ǥ ��ġ�� �̵��Ѵ�.
        StartCoroutine(MoveCoroutine());
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void Update()
    {

        if(animator.GetBool("IsRunning") == false)
        {
            playerPos = GameManager.Instance.PlayerPos;
            if (this.transform.position.y > 0)
            {
                // y���� �ִٸ� �� ����ŭ ���� �Ĵٺ���.
                playerPos.y += this.transform.position.y;
            }
            else
            {    // y���� ������ ������ �ٶ󺻴�.
                playerPos.y = 0;
            }
            transform.LookAt(playerPos);
        }

        // �÷��̾�� �ڿ� ��ġ�� ���� ��������.
        if (this.gameObject.activeSelf && GameManager.Instance.PlayerPos.z - limitDistance > this.transform.position.z)
        {
            this.gameObject.SetActive(false);
        }
    }
}

public partial class EnemyCtrl : MonoBehaviour, IShotAble
{
    // ������ ��ġ�� �̵��� ��ġ�� �������ش�. EnemyManager���� ȣ���Ѵ�.
    public void SetValue(Transform startPos, Transform targetPos, int hp)
    {
        this.transform.position = startPos.position;
        this.targetPos = targetPos;
        this.hp = hp;
    }

    private void EnemyAim()
    {
        StopAllCoroutines();  
        animator.SetBool("IsRunning", false);
        StartCoroutine(AttackCoroutine());
    }
    private void drawWarningLine(Vector3 playerPos) // ��� ���
    {     
        line.SetPosition(0, barrelLocation.position);
        line.SetPosition(1, playerPos);
        line.enabled = true;
    }

    IEnumerator drawWarningLine()
    {
        float size = 0f;
        float dir = 0.0f;
        RaycastHit hit;
        Vector3 dirVec;
        Vector3 ForwardVec;
        while (isAim == true)
        {
            dirVec = (GameManager.Instance.PlayerPos - barrelLocation.position).normalized;
            ForwardVec = GameManager.Instance.PlayerTransform.forward;

            float dot = Vector3.Dot(dirVec, ForwardVec);
            size = Mathf.Lerp(0.08f, 0, Mathf.Abs(dot) + 0.1f);

            Vector3 cross = Vector3.Cross(dirVec, ForwardVec);
            dir = Vector3.Dot(cross, Vector3.up) > 0 ? 1.0f : -1.0f;


            line.SetPosition(0, barrelLocation.position);
            Vector3 temp = GameManager.Instance.PlayerPos;
            temp.x += 0.15f * dir;

            line.SetPosition(1, temp);
            line.startWidth = size;
            line.endWidth = size;
            line.enabled = true;


            yield return null;
        }
        yield break;
    }

    private void EnemyAttack()
    {
        animator.SetBool("IsAttack", true); // ���� �ִϸ��̼� ����
        muzzle.Play(); // ���� ����Ʈ ����
        audioSource.PlayOneShot(attackClip); // ���� ���� ����
        BulletPooling.Instance.Spawn(barrelLocation); // �Ѿ� ����
    }

    // ybot@Shooting �ִϸ��̼��� Event���� ����ȴ�. ���� 0����� �����ϸ� ū�� ����.
    private void ResetAttack()
    {
        animator.SetBool("IsAttack", false);
    }

    // ���� �÷��̾ �� �Ѿ˿� �¾����� ����� �Լ�
    public void OnShot(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        // ����Ʈ�� hitPoint, hitNormal �������� �׸���.
        GameObject BloodObject = ObjectManager.Instance.Fire();
        BloodObject.transform.position = hitPoint;
        BloodObject.transform.rotation = Quaternion.LookRotation(hitNormal);

        if (--hp <= 0)
        {
            StopAllCoroutines();
            EnemyDamage();
            GetScore(); // ������ ȹ���Ѵ�.
            disappear_Effect.Play();  // ��� ����Ʈ�� ���´�.
        }

    }
    public void EnemyDamage()
    {
        animator.enabled = false; // ���׵� Ȱ��ȭ�� ���� �ִϸ����͸� ����
        isDie = true; // ��� ���� �׾���.

        score = Random.Range(80, 120);
        GameManager.Instance.GetScored(score); // ���ӸŴ����� ������ �߰��Ѵ�.
       
        GetComponent<Collider>().enabled = false; // Enemy ��Ʈ�ڽ��� ����

        GameManager.Instance.EnemyDie(this); // �� ��� �̺�Ʈ�� �����Ѵ�.
        StartCoroutine(EnemyDieCoroutine()); // ��� ó�� �ڷ�ƾ�� �����Ų��.
    }

    // ������ ���̰� �ϰ� ���� �Ŵ����� ������ �߰��ϴ� �Լ�
    private void GetScore()
    {
        var color = score > 100 ? Color.red : Color.black; // 100�� �̻��� ����, ���ϴ� ���������� ǥ�õȴ�.

        scoreUI.SetActive(true); // ���� UI�� Ų��.
        Vector3 vec = GameManager.Instance.PlayerPos - transform.position;
        vec.Normalize();
        Quaternion q = Quaternion.LookRotation(-vec);
        scoreUI.transform.rotation = q; // ���� UI�� �÷��̾ �ٶ󺸰� �Ѵ�.

        scoreText.text = score.ToString(); //  ������ ������Ʈ �Ѵ�.
        scoreText.color = color; // ������ �ٲ۴�.
       
    }

    // Enemy�� ����� ���� �� �ڷ�ƾ
    IEnumerator EnemyDieCoroutine()
    {
        yield return waitShort; // ��� ��ٸ���.

        // Lerp�� ���¸� �����ϱ� ���� ����
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
        yield return waitShort;
        while (!isDie) // �ױ� ������ ��� ����ȴ�.
        {
            StartCoroutine(laserprint());
            yield return waitTwoSecond;
            EnemyAttack();

            yield return waitAttackDely;
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
        RaycastHit hit;
        isAim = true;
        if (Physics.Raycast(barrelLocation.position, (GameManager.Instance.PlayerPos - barrelLocation.position).normalized, out hit, 20f, layerMask))
        {

            if (hit.transform.tag == "Player")
            {
                StartCoroutine(drawWarningLine());
            }

        }

        yield return waitTwoSecond;
        isAim = false; 
        line.enabled = false;
        yield break;
    }
}

