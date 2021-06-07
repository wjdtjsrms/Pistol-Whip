using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;
using TMPro;

public partial class GunShoot : MonoBehaviour
{
    #region 총 관련 필드
    [SerializeField]
    private ParticleSystem muzzle; // 사격 이펙트
    [SerializeField]
    private Transform barrelLocation; // 사격 이펙트가 생성될 위치
    [SerializeField]
    private AudioClip fireClip; // 총 발사 사운드 클립
    [SerializeField]
    private AudioClip reloadClip; // 총 재장전 사운드 클립
    [SerializeField]
    private AudioClip emptydClip; // 총 재장전 사운드 클립
    [SerializeField]
    private TextMeshPro bulletText; // 탄창 수를 알려주는 텍스트

    private float attackAmount = 35.0f; // 총 공격력
    private float fireDistance = 35.0f; // 총 사정거리
    private Animator childAnimator; // 총 애니메이션 컴포넌트 자식에게서 받아온다.
    private AudioSource gunAudio; // 발사 사운드를 재생할 오디오소스 컴포넌트
    private bool triggerButton = false; // 총알 단발 발사용 불리언
    private int magCapacity = 15; // 탄창 용량
    private int magAmmo = 15; // 현재 탄창에 남아 있는 탄알
    private float reloadTime = 0.5f; // 재장전 소요 시간
    private int layerMask; // 레이와 충돌할 객체를 검출하기 위한 레이어마스크

    private enum State // 총의 현재 상태
    {
        Ready, // 발사 준비됨
        Empty, // 탄창이 빔
        Reloading // 재장 중
    }
    private State state;
    #endregion

    #region 라인렌더러 관련 필드
    [SerializeField]
    private BulletLineRender[] lineRenders;
    private int index = 0; // 현재 그릴 라인 렌더러의 인덱스
    #endregion

    private void Awake()
    {
        if (barrelLocation == null)
        {
            barrelLocation = this.transform;
        }
    }

    private void Start()
    {
        childAnimator = GetComponentInChildren<Animator>();
        gunAudio = GetComponent<AudioSource>();
        magAmmo = magCapacity; // 탄창을 가득 채운다.
        state = State.Ready; // 총의 현재 상태를 총을 쏠 준비가 된 상태로 변경
        muzzle.gameObject.transform.position = barrelLocation.position; // 발사 이펙트의 위치를 총구로 변경        
        layerMask = (1 << LayerMask.NameToLayer("UI")) + (1 << LayerMask.NameToLayer("Enemy")); // 쏜 레이는 Enemy 와 UI 레이어만 충돌한다.
    }

    private void Update()
    {
        // 오른쪽 트리거 버튼을 누르면 사격한다.
        if (CustomController.IsButtonPressed(CommonUsages.triggerButton, ref triggerButton, false))
        {
            if (state == State.Empty) // 총알이 없다면 발사되지 않고 빈 총 소리가 난다.
            {
                gunAudio?.PlayOneShot(emptydClip);
            }
            if (state == State.Ready)
            {
                Shot(); // 실제 발사 처리 실행
            }
        }

        // 총을 90도로 꺾으면 재장전 된다.
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
        RaycastHit hit;
        Vector3 hitPosition;

        if (Physics.Raycast(barrelLocation.position, barrelLocation.forward, out hit, fireDistance, layerMask))
        {
            // 레이가 충돌한 지점 저장
            hitPosition = hit.point;
            // 총과 인터렉션이 있는 타입이라면 그 객체의 함수를 호출한다.
            IShotAble shotObject = hit.transform.gameObject.GetComponent<IShotAble>();
            shotObject?.OnShot(attackAmount, hit.point, hit.normal);
           
        }
        else
        {
            // 레이가 다른 물체와 충돌하지 않았다면
            // 탄알이 최대 사정거리까지 날아갔을 때의 위치를 충돌 위치를 사용
            hitPosition = barrelLocation.position + barrelLocation.forward * fireDistance;
        }
        //발사 이펙트 재생
        ShotEffect(hitPosition);

        magAmmo--;
        bulletText.text = magAmmo.ToString(); // 남은 총알 갱신
        if (magAmmo <= 0)
        {
            state = State.Empty;
        }
    }

    private void ShotEffect(Vector3 hitposition)
    {
        muzzle.Play(); // 사격 이펙트 실행
        childAnimator?.SetTrigger("Fire"); // 사격 애니메이션 실행
        gunAudio?.PlayOneShot(fireClip); // 사격 사운드 실행
        lineRenders[++index % lineRenders.Length].StartRender(barrelLocation.position, hitposition); // 사격시 생성되는 라인 렌더러 드로우
    }

    public bool Reloading()
    {
        if (state == State.Reloading || magAmmo >= magCapacity)
        {
            return false;
        }

        StopAllCoroutines();
        StartCoroutine(ReloadRoutine());
        return true;
    }

    private IEnumerator ReloadRoutine()
    {
        state = State.Reloading;// 상태를 재장전 중으로 바꾼다.
        gunAudio?.PlayOneShot(reloadClip); // 재장전 소리를 재생한다.

        yield return new WaitForSeconds(reloadTime); // 재장전 시간만큼 기다린다.
        magAmmo = magCapacity; // 총알을 최대치까지 채운다.
        bulletText.text = magAmmo.ToString(); // 총알 정보를 UI에 갱신
        state = State.Ready; // 사격 가능 상태로 바꾼다.
    }
}