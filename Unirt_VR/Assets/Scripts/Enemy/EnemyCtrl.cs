using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public partial class EnemyCtrl : MonoBehaviour, IShotAble
{
    #region 필드
    [SerializeField]
    private Bullet bullet; // 생성할 불렛의 프리펩
    [SerializeField]
    private GameObject scoreUI; // 사망시 등장할 점수 UI
    [SerializeField]
    private AudioClip attackClip; // 적 공격시 사용할 오디오 클립
    [SerializeField]
    private ParticleSystem muzzle; // 공격시 사용할 이펙트
    [SerializeField]
    private LineRenderer line; // 공격선 레이저
    [SerializeField]
    private Transform barrelLocation; // 총알이 나올 위치
    [SerializeField]
    private float limitDistance = 6.0f; // 얼마나 떨어지면 사라지것인지.
    private AudioSource audioSource;
    private TextMeshPro scoreText;
    private Animator animator;
    private Vector3 sizeUI;

    [SerializeField]
    private ParticleSystem appear_Effect;
    [SerializeField]
    private ParticleSystem disappear_Effect;

    private Transform targetPos; // 생성 후 이동할 위치
    private Vector3 moveTargetVec; // 이동할 목표 위치
    private Vector3 playerPos; // 플레이어의 위치   
    private float moveSpeed = 4.0f; // 이동 속도
    private bool isDie = false; // 현재 적의 상태

    // 코루틴 최적화를 위한 변수 선언
    YieldInstruction waitShort = new WaitForSeconds(0.5f);
    YieldInstruction waitAttackDely = new WaitForSeconds(3.0f);

    //경고선 프로퍼티
    bool laser;
    bool ShotWait
    {
        get => laser;
        set
        {
            laser = value;

            if (animator.GetBool("IsAttack") == laser) //IsAttack이 false일땐 라인렌더러 true, true일땐 false
            {
                drawWarningLine(GameManager.Instance.PlayerPos);
                line.enabled = !laser;
            }
        }
    }

    #endregion

    private void Awake()
    {
        // 필요한 컴포넌트를 가져온다.
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
    // 값을 다시 초기화 한다.
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
        muzzle.gameObject.transform.position = barrelLocation.position; // 발사 이펙트의 위치를 총구로 변경    

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

        // y값을 제외한 방향을 바라본다.
        playerPos = GameManager.Instance.PlayerPos;
        playerPos.y = 0;
        transform.LookAt(playerPos);

        //drawWarningLine(GameManager.Instance.PlayerPos); // 경고선 출력 메소드 호출

        animator.SetBool("IsRunning", false);
        StartCoroutine(AttackCoroutine());

    }
    public void drawWarningLine(Vector3 playerPos) // 경고선 출력
    {  
        line.SetPosition(0, barrelLocation.position);
        line.SetPosition(1, playerPos); 
        line.enabled = true;
    }

    private void EnemyAttack()
    {
        animator.SetBool("IsAttack", true); // 공격 애니메이션 실행
        muzzle.Play(); // 공격 이펙트 실행
        line.enabled = false;
        audioSource.PlayOneShot(attackClip); // 공격 사운드 실행
        BulletPooling.Instance.Spawn(barrelLocation);

        //StartCoroutine(laserprint());
    }

    // ybot@Shooting 애니메이션의 Event에서 실행된다. 삭제하면 아무튼 큰일 난다.
    void ResetAttack()
    {
        animator.SetBool("IsAttack", false);
    }

    // 적이 플레이어가 쏜 총알에 맞아을때 실행될 함수
    public void OnShot(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        StopAllCoroutines();
        EnemyDamage();

        animator.enabled = false; // 레그돌 활성화를 위해 애니메이터를 끈다
        isDie = true; // 얘는 이제 죽었다.
     
        GetScore(); // 점수를 획득한다.
        GameManager.Instance.EnemyDie(this); // 적 사망 이벤트를 실행한다.
        StartCoroutine(EnemyDieCoroutine()); // 사망 처리 코루틴을 실행시킨다.
        GetComponent<Collider>().enabled = false;


        // 이펙트를 hitPoint, hitNormal 방향으로 그린다.
        GameObject BloodObject = ObjectManager.Instance.Fire();
        BloodObject.transform.position = hitPoint;
        BloodObject.transform.rotation = Quaternion.LookRotation(hitNormal);

        // 사망 이펙트가 나온다.
        disappear_Effect.Play();
    }
    public void EnemyDamage()
    {
        animator.enabled = false; // 레그돌 활성화를 위해 애니메이터를 끈다
        isDie = true; // 얘는 이제 죽었다.
        GetScore(); // 점수를 획득한다.
        GameManager.Instance.EnemyDie(this); // 적 사망 이벤트를 실행한다.
        StartCoroutine(EnemyDieCoroutine()); // 사망 처리 코루틴을 실행시킨다.
        GetComponent<Collider>().enabled = false;
    }


    // 점수를 보이게 하고 게임 매니저에 점수를 추가하는 함수
    private void GetScore()
    {
        var score = Random.Range(80, 120); // 점수는 우선 랜덤으로 책정한다.
        var color = score > 100 ? Color.red : Color.black; // 100점 이상은 빨강, 이하는 검정색으로 표시된다.

        scoreUI.SetActive(true); // 점수 UI를 킨다.
        Vector3 vec = GameManager.Instance.PlayerPos - transform.position;
        vec.Normalize();
        Quaternion q = Quaternion.LookRotation(-vec);
        scoreUI.transform.rotation = q; // 점수 UI가 플레이어를 바라보게 한다.

        scoreText.text = score.ToString(); //  점수로 업데이트 한다.
        scoreText.color = color; // 색깔을 바꾼다.
        GameManager.Instance.GetScored(score); // 게임매니저에 점수를 추가한다.
    }

    // Enemy가 사망시 실행 될 코루틴
    IEnumerator EnemyDieCoroutine()
    {
        yield return waitShort; // 잠시 기다린다.

        // Lerp를 사용하기 위한 변수
        float percent = 0;
        float speed = 0.5f;

        while (percent < 1)
        {
            percent += Time.deltaTime * speed;
            Vector3 size = Vector3.Lerp(scoreUI.transform.localScale, Vector3.zero, percent);
            scoreUI.transform.localScale = size; // 점수 UI 사이즈를 점진적으로 0으로 보낸다.
            yield return null;
        }

        scoreText.enabled = false; // 점수 UI를 끈다.
        this.gameObject.SetActive(false); // 이 오브젝트를 끈다.
        yield break;

    }

    // Enemy가 공격시 실행 될 코루틴
    IEnumerator AttackCoroutine()
    {      
        while (!isDie) // 죽기 전까지 계속 실행된다.
        {
            yield return waitAttackDely;
            
            EnemyAttack();
        }
        yield break;
    }

    // Enemy가 이동시 실행 될 코루틴
    IEnumerator MoveCoroutine()
    {
        // 목표 위치로 점진적으로 이동한다.
        while (Vector3.Distance(transform.position, moveTargetVec) > 0.5f)
        {
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
            yield return null;
        }
        // 목표 위치에 도착했다면 현재 플레이어 방향을 겨냥한다.
        EnemyAim();
        yield break;
    }

    // 경고선 출력 코루틴
    IEnumerator laserprint()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f); // 0.1초마다 경고선을 끄고 켜게
            ShotWait = !ShotWait;
        }
    }
}

