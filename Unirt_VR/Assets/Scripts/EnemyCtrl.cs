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
    private TextMeshPro scoreText;

    [SerializeField]
    private Transform gunBarrel;
    private Vector3 moveTargetVec;
    private Vector3 playerPos;
    private Animator animator;
    private float moveSpeed = 4.0f;
    private bool isDie = false;

    YieldInstruction waitAttackDely = new WaitForSeconds(2.0f);

    private void Awake()
    {
        animator = GetComponent<Animator>();
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
        scoreText.enabled = false;
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
        Vector3 shotPosition = gunBarrel.position;
        Instantiate(bullet, gunBarrel.position, transform.rotation).gameObject.transform.LookAt(GameManager.Instance.PlayerPos);
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
        Vector3 temp = hitNormal;
        temp.y = 0;
        GetComponent<Rigidbody>().velocity = temp * 10;
        scoreText.enabled = true;
        // ����Ʈ�� hitPoint, -hitNormal �������� �׸���.
        // ���� ���´�.
        // ������ ���� �Ŵ����� �߰��ȴ�.
        // ��� ����Ʈ�� ���´�.

        Invoke("SetActiveFalse", 2f);

    }
    private void SetActiveFalse()
    {
        scoreText.enabled = false;
        this.gameObject.SetActive(false);
        StopAllCoroutines();
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
