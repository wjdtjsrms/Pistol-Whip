using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public partial class GunShoot : MonoBehaviour
{
    [SerializeField]
    private GameObject muzzleFlashPrefab; // ��� ����Ʈ
    [SerializeField]
    private Transform barrelLocation;
    [SerializeField]
    private float attackAmount = 35.0f;
    [SerializeField]
    private AudioClip fireClip; // �� �߻� ���� Ŭ��

    //public bool isGrab = false;
    private Animator childAnimator; // �� �ִϸ��̼� ������Ʈ �ڽĿ��Լ� �޾ƿ´�.
    private AudioSource fireAudio; // �߻� ���带 ����� ������ҽ� ������Ʈ
    private LineRenderer bulletLineRender; // �߻�� ���̸� �׸� 

    public HandState currentGrab; // CustomController���� �����Ѵ�.

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
        tempFlash = Instantiate(muzzleFlashPrefab, barrelLocation.position, barrelLocation.rotation); // ��� ����Ʈ ����
        childAnimator?.SetTrigger("Fire"); // ��� �ִϸ��̼� ����
        fireAudio?.PlayOneShot(fireClip); // ��� ���� ����
        ShootRay();
    }

    private void ShootRay() // �ӽ� �ڵ�
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
