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
    [SerializeField]
    private ParticleSystem appear_Effect; // 등장 이팩트
    [SerializeField]
    private ParticleSystem disappear_Effect; // 사망시 이팩트
    [SerializeField]
    private Material enemyMaterial;

    private AudioSource audioSource;
    private TextMeshPro scoreText;
    private Animator animator;
    private int hp = 0;
    private int score = 0;
    private int layerMask; // 레이와 충돌할 객체를 검출하기 위한 레이어마스크

    private Transform targetPos; // 생성 후 이동할 위치
    private Vector3 moveTargetVec; // 이동할 목표 위치
    private Vector3 playerPos; // 플레이어의 위치   
    private float moveSpeed = 4.5f; // 이동 속도
    private bool isDie = false; // 현재 적의 상태
    private Vector3 sizeUI; // 점수 UI의 기본 크기

    // 코루틴 최적화를 위한 변수 선언
    private YieldInstruction waitShort = new WaitForSeconds(0.5f);
    private YieldInstruction waitTwoSecond = new WaitForSeconds(2.0f);
    private YieldInstruction waitAttackDely = new WaitForSeconds(5.0f);

    private bool isAim = false;

    #endregion

    private void Awake()
    {
        // 필요한 컴포넌트를 가져온다.
        animator = GetComponent<Animator>();
        scoreText = scoreUI.GetComponent<TextMeshPro>();
        audioSource = GetComponent<AudioSource>();

        // 기본 점수 UI의 크기를 기억한다.
        sizeUI = scoreUI.transform.localScale;
        layerMask = (1 << LayerMask.NameToLayer("Player")) + (1 << LayerMask.NameToLayer("Wall")); // 쏜 레이는 Player 와 Wall 레이어만 충돌한다.
    }

    private void Start()
    {
        // 필요한 이벤트 리스너들을 등록한다.
        GameManager.Instance.actPlayerDie += () => gameObject.SetActive(false);
        GameManager.Instance.actGamePause += () => gameObject.SetActive(false);
        GameManager.Instance.actGameRestart += () => 
        { if(isDie == false)
            {
                gameObject.SetActive(true);
            }       
        };
    }

    // 값을 다시 초기화 한다.
    private void OnEnable()
    {
        StopAllCoroutines();

        // 값들을 기본값으로 되돌린다.
        isDie = false;
        animator.enabled = true;
        animator.SetBool("IsRunning", true);
        GetComponent<Collider>().enabled = true;

        // 등장 이팩트를 보여준다.
        appear_Effect.Play();

        // 발사 이펙트의 위치를 총구로 변경    
        muzzle.gameObject.transform.position = barrelLocation.position;

        // 점수 UI를 초기화 한다.
        scoreUI.transform.localScale = sizeUI;
        scoreUI.SetActive(false);
        scoreText.enabled = true;

        // 목표 위치를 바라본다.
        moveTargetVec = targetPos.position;
        // moveTargetVec.y = 0; 만악의 근원
        transform.LookAt(moveTargetVec);

        // 목표 위치로 이동한다.
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
                // y값이 있다면 그 값만큼 위를 쳐다본다.
                playerPos.y += this.transform.position.y;
            }
            else
            {    // y값을 제외한 방향을 바라본다.
                playerPos.y = 0;
            }
            transform.LookAt(playerPos);
        }

        // 플레이어보다 뒤에 위치한 적은 꺼버린다.
        if (this.gameObject.activeSelf && GameManager.Instance.PlayerPos.z - limitDistance > this.transform.position.z)
        {
            this.gameObject.SetActive(false);
        }
    }
}

public partial class EnemyCtrl : MonoBehaviour, IShotAble
{
    // 생성될 위치와 이동할 위치를 설정해준다. EnemyManager에서 호출한다.
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
    private void drawWarningLine(Vector3 playerPos) // 경고선 출력
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
        animator.SetBool("IsAttack", true); // 공격 애니메이션 실행
        muzzle.Play(); // 공격 이펙트 실행
        audioSource.PlayOneShot(attackClip); // 공격 사운드 실행
        BulletPooling.Instance.Spawn(barrelLocation); // 총알 생성
    }

    // ybot@Shooting 애니메이션의 Event에서 실행된다. 참조 0개라고 삭제하면 큰일 난다.
    private void ResetAttack()
    {
        animator.SetBool("IsAttack", false);
    }

    // 적이 플레이어가 쏜 총알에 맞았을때 실행될 함수
    public void OnShot(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        // 이펙트를 hitPoint, hitNormal 방향으로 그린다.
        GameObject BloodObject = ObjectManager.Instance.Fire();
        BloodObject.transform.position = hitPoint;
        BloodObject.transform.rotation = Quaternion.LookRotation(hitNormal);

        if (--hp <= 0)
        {
            StopAllCoroutines();
            EnemyDamage();
            GetScore(); // 점수를 획득한다.
            disappear_Effect.Play();  // 사망 이펙트가 나온다.
        }

    }
    public void EnemyDamage()
    {
        animator.enabled = false; // 레그돌 활성화를 위해 애니메이터를 끈다
        isDie = true; // 얘는 이제 죽었다.

        score = Random.Range(80, 120);
        GameManager.Instance.GetScored(score); // 게임매니저에 점수를 추가한다.
       
        GetComponent<Collider>().enabled = false; // Enemy 히트박스를 끈다

        GameManager.Instance.EnemyDie(this); // 적 사망 이벤트를 실행한다.
        StartCoroutine(EnemyDieCoroutine()); // 사망 처리 코루틴을 실행시킨다.
    }

    // 점수를 보이게 하고 게임 매니저에 점수를 추가하는 함수
    private void GetScore()
    {
        var color = score > 100 ? Color.red : Color.black; // 100점 이상은 빨강, 이하는 검정색으로 표시된다.

        scoreUI.SetActive(true); // 점수 UI를 킨다.
        Vector3 vec = GameManager.Instance.PlayerPos - transform.position;
        vec.Normalize();
        Quaternion q = Quaternion.LookRotation(-vec);
        scoreUI.transform.rotation = q; // 점수 UI가 플레이어를 바라보게 한다.

        scoreText.text = score.ToString(); //  점수로 업데이트 한다.
        scoreText.color = color; // 색깔을 바꾼다.
       
    }

    // Enemy가 사망시 실행 될 코루틴
    IEnumerator EnemyDieCoroutine()
    {
        yield return waitShort; // 잠시 기다린다.

        // Lerp를 상태를 저장하기 위한 변수
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
        yield return waitShort;
        while (!isDie) // 죽기 전까지 계속 실행된다.
        {
            StartCoroutine(laserprint());
            yield return waitTwoSecond;
            EnemyAttack();

            yield return waitAttackDely;
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

