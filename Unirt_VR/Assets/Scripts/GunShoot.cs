using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public partial class GunShoot : MonoBehaviour
{
    [SerializeField]
    private GameObject muzzleFlashPrefab; // 사격 이펙트
    [SerializeField]
    private Transform barrelLocation;
    [SerializeField]
    private float attackAmount = 35.0f;
    [SerializeField]
    private AudioClip fireClip; // 총 발사 사운드 클립

    //public bool isGrab = false;
    private Animator childAnimator; // 총 애니메이션 컴포넌트 자식에게서 받아온다.
    private AudioSource fireAudio; // 발사 사운드를 재생할 오디오소스 컴포넌트
    private LineRenderer bulletLineRender; // 발사될 레이를 그릴 

    public HandState currentGrab; // CustomController에서 정의한다.

    private void Awake()
    {
        if (barrelLocation == null)
        {
            barrelLocation = transform;
        }
        bulletLineRender = GetComponent<LineRenderer>();
        bulletLineRender.positionCount = 2;
        bulletLineRender.enabled = false;
    }

    void Start()
    {
        childAnimator = GetComponentInChildren<Animator>();
        fireAudio = GetComponent<AudioSource>();
    }
}

public partial class GunShoot : MonoBehaviour
{
    public void SetGrapState(HandState state)
    {
        currentGrab = state;
    }
    public void SetGrapNone()
    {
        if (!GetComponent<XRGrabInteractable>().isSelected)
        {
            currentGrab = HandState.NONE;
        }
    }
    public void SetGrapLeft()
    {
        currentGrab = HandState.LEFT;
    }
    public void SetGrapRight()
    {
        currentGrab = HandState.RIGHT;
    }

    public void Shoot()
    {
        GameObject tempFlash;
        tempFlash = Instantiate(muzzleFlashPrefab, barrelLocation.position, barrelLocation.rotation); // 사격 이펙트 생성
        childAnimator?.SetTrigger("Fire"); // 사격 애니메이션 실행
        fireAudio?.PlayOneShot(fireClip); // 사격 사운드 실행
        ShootRay();
    }

    private void ShootRay() // 임시 코드
    {
        bulletLineRender.enabled = true;
        RaycastHit hitInfo;
        bool hasHit = Physics.Raycast(barrelLocation.position, barrelLocation.forward, out hitInfo, 100);
        bulletLineRender.SetPosition(0, barrelLocation.position);
        bulletLineRender.SetPosition(1, barrelLocation.position + barrelLocation.forward * 100);
        if (hasHit)
        {
            if (hitInfo.transform.gameObject.CompareTag("Monster2"))
            {
                MonsterCtrl ailen = hitInfo.transform.GetComponent<MonsterCtrl>();
                ailen?.GetDamage(attackAmount);
            }
        }   
    }
}
