using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
public partial class GunShoot : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem muzzle; // 사격 이펙트
    [SerializeField]
    private Transform barrelLocation; // 사격 이펙트가 생성될 위치
    [SerializeField]
    private float attackAmount = 35.0f; // 총 공격력
    [SerializeField]
    private float fireDistance = 100.0f; // 총 사정거리
    [SerializeField]
    private AudioClip fireClip; // 총 발사 사운드 클립
    [SerializeField]
    private AudioClip reloadClip; // 총 재장전 사운드 클립
    [SerializeField]
    private AudioClip emptydClip; // 총 재장전 사운드 클립
    [SerializeField]
    private Text bulletText; // 탄창 수를 알려주는 텍스트

    private Animator childAnimator; // 총 애니메이션 컴포넌트 자식에게서 받아온다.
    private AudioSource gunAudio; // 발사 사운드를 재생할 오디오소스 컴포넌트
    private LineRenderer bulletLineRender; // 발사될 레이를 그릴 
    private bool triggerButton = false; // 총알 단발 발사용 불리언
    private int magCapacity = 15; // 탄창 용량
    private int magAmmo = 15; // 현재 탄창에 남아 있는 탄알
    private float reloadTime = 1.0f; // 재장전 소요 시간

    private enum State // 총의 현재 상태
    {
        Ready, // 발사 준비됨
        Empty, // 탄창이 빔
        Reloading // 재장 중
    }
    private State state;

    private void Awake()
    {
        if (barrelLocation == null)
        {
            barrelLocation = this.transform;
        }
        bulletLineRender = GetComponent<LineRenderer>();
        bulletLineRender.positionCount = 2;
        bulletLineRender.enabled = false;
    }

    private void Start()
    {
        childAnimator = GetComponentInChildren<Animator>();
        gunAudio = GetComponent<AudioSource>();
        magAmmo = magCapacity; // 탄창을 가득 채운다.
        state = State.Ready; // 총의 현재 상태를 총을 쏠 준비가 된 상태로 변경
        muzzle.gameObject.transform.position = barrelLocation.position;
    }

    private void Update()
    {
        // 오른쪽 트리거 버튼을 누르면 사격한다.
        if (CustomController.IsButtonPressed(CommonUsages.triggerButton, ref triggerButton, false))
        {
            if(state == State.Empty)
            {
                gunAudio?.PlayOneShot(emptydClip);
            }
            if(state == State.Ready)
            {
                Shot(); // 실제 발사 처리 실행
            }         
        }
        if (Vector3.Angle(transform.up, Vector3.up) > 100)
        {
            Reloading();
        }
    }
}

public partial class GunShoot : MonoBehaviour
{
    private void Shot()
    {
        muzzle.Play(); // 코루틴에서 이펙트 호출하면 오류 남

        RaycastHit hit;
        Vector3 hitPosition;

        if (Physics.Raycast(barrelLocation.position, barrelLocation.forward, out hit, fireDistance))
        {
            // 레이가 충돌한 지점 저장
            hitPosition = hit.point;

            if (hit.transform.gameObject.CompareTag("Monster2"))
            {
                MonsterCtrl ailen = hit.transform.GetComponent<MonsterCtrl>();
                ailen?.GetDamage(attackAmount);
            }
        }
        else
        {
            // 레이가 다른 물체와 충돌하지 않았다면
            // 탄알이 최대 사정거리까지 날아갔을 때의 위치를 충돌 위치를 사용
            hitPosition = barrelLocation.position + barrelLocation.forward * fireDistance;
        }

        //발사 이펙트 재생
        StartCoroutine(ShotEffect(hitPosition));

        magAmmo--;
        bulletText.text = magAmmo.ToString(); // 남은 총알 갱신
        if (magAmmo <= 0)
        {
            state = State.Empty;        
        }
    }

    private IEnumerator ShotEffect(Vector3 hitposition)
    {
        childAnimator?.SetTrigger("Fire"); // 사격 애니메이션 실행
        gunAudio?.PlayOneShot(fireClip); // 사격 사운드 실행

        // 선의 시작점은 총구의 위치
        bulletLineRender.SetPosition(0, barrelLocation.position);
        // 선의 끝점은 입력으로 들어온 충돌 위치
        bulletLineRender.SetPosition(1, hitposition);

        // 라인 렌더러를 활성화 하여 탄알 궤적을 그린다.
        bulletLineRender.enabled = true;
        // 잠시 대기
        yield return new WaitForSeconds(0.03f);
        // 라인 렌더러를 활성화 하여 탄알 궤적을 지운다.
        bulletLineRender.enabled = false;
    }

    public bool Reloading()
    {
        if(state == State.Reloading || magAmmo >=  magCapacity)
        {
            return false;
        }

        StartCoroutine(ReloadRoutine());
        return true;
    }

    private IEnumerator ReloadRoutine()
    {
        state = State.Reloading;
        gunAudio?.PlayOneShot(reloadClip);

        yield return new WaitForSeconds(reloadTime);
        magAmmo = magCapacity;
        bulletText.text = magAmmo.ToString(); // 남은 총알 갱신
        state = State.Ready;
    }
}
