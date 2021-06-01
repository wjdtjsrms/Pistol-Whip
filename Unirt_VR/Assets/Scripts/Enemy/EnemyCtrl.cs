using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public partial class EnemyCtrl : MonoBehaviour, IShotAble
{
    #region �ʵ�
    [SerializeField]
    private Transform targetPos; // ���� �� �̵��� ��ġ
    [SerializeField]
    private Bullet bullet; // ������ �ҷ��� ������
    [SerializeField]
    private GameObject scoreUI; // ����� ������ ���� UI
    [SerializeField]
    private AudioClip attackClip; // �� ���ݽ� ����� ����� Ŭ��
    [SerializeField]
    private ParticleSystem muzzle; // ���ݽ� ����� ����Ʈ
    [SerializeField]
    private Transform barrelLocation; // �Ѿ��� ���� ��ġ

    private AudioSource audioSource;
    private TextMeshPro scoreText;
    private Animator animator;

    private Vector3 moveTargetVec; // �̵��� ��ǥ ��ġ
    private Vector3 playerPos; // �÷��̾��� ��ġ   
    private float moveSpeed = 4.0f; // �̵� �ӵ�
    private bool isDie = false; // ���� ���� ����

    // �ڷ�ƾ ����ȭ�� ���� ���� ����
    YieldInstruction waitShort = new WaitForSeconds(1.0f);
    YieldInstruction waitAttackDely = new WaitForSeconds(2.0f);
    #endregion

    private void Awake()
    {
        // �ʿ��� ������Ʈ�� �����´�.
        animator = GetComponent<Animator>();
        scoreText = scoreUI.GetComponent<TextMeshPro>();
        audioSource = GetComponent<AudioSource>();
    }

    // ���� �ٽ� �ʱ�ȭ �Ѵ�.
    private void OnEnable()
    {
        StopAllCoroutines();
        animator.SetBool("IsRunning", true);

        // y���� ������ ������ �ٶ󺻴�.
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
}

public partial class EnemyCtrl : MonoBehaviour, IShotAble
{
    void EnemyAim()
    {
        StopAllCoroutines();

        // y���� ������ ������ �ٶ󺻴�.
        playerPos = GameManager.Instance.PlayerPos;
        playerPos.y = 0;
        transform.LookAt(playerPos);

        drawWarningLine(GameManager.Instance.PlayerPos);
        animator.SetBool("IsRunning", false);
        StartCoroutine(AttackCoroutine());
    }
    public void drawWarningLine(Vector3 playerPos)
    {
        // �̰� ���� �����ϳ�
    }

    void EnemyAttack()
    {
        animator.SetBool("IsAttack", true); // ���� �ִϸ��̼� ����
        muzzle.Play(); // ���� ����Ʈ ����
        audioSource.PlayOneShot(attackClip); // ���� ���� ����
        Instantiate(bullet, barrelLocation.position, transform.rotation).gameObject.transform.LookAt(GameManager.Instance.PlayerPos);
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

        animator.enabled = false; // ���׵� Ȱ��ȭ�� ���� �ִϸ����͸� ����
        isDie = true; // ��� ���� �׾���.
        GetScore(); // ������ ȹ���Ѵ�.
        GameManager.Instance.EnemyDie(this); // �� ��� �̺�Ʈ�� �����Ѵ�.
        StartCoroutine(EnemyDieCoroutine()); // ��� ó�� �ڷ�ƾ�� �����Ų��.

        GameObject BloodObject = ObjectManager.Instance.Fire();
        BloodObject.transform.position = hitPoint;
        BloodObject.transform.rotation = Quaternion.LookRotation(hitNormal);

        // ����Ʈ�� hitPoint, -hitNormal �������� �׸���.
        // ��� ����Ʈ�� ���´�.
    }

    // ������ ���̰� �ϰ� ���� �Ŵ����� ������ �߰��ϴ� �Լ�
    private void GetScore()
    {
        var score = Random.Range(80, 120); // ������ �켱 �������� å���Ѵ�.
        var color = score > 100 ? Color.red : Color.black; // 100�� �̻��� ����, ���ϴ� ���������� ǥ�õȴ�.

        scoreUI.SetActive(true); // ���� UI�� Ų��.
        scoreUI.transform.LookAt(new Vector3(0, GameManager.Instance.PlayerPos.y, 0)); // �÷��̾� ������ �ٶ󺸰� �Ѵ�.
        scoreUI.transform.Rotate(new Vector3(0, 180, 0));

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
}

