using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
public partial class GunShoot : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem muzzle; // ��� ����Ʈ
    [SerializeField]
    private Transform barrelLocation; // ��� ����Ʈ�� ������ ��ġ
    [SerializeField]
    private float attackAmount = 35.0f; // �� ���ݷ�
    [SerializeField]
    private float fireDistance = 100.0f; // �� �����Ÿ�
    [SerializeField]
    private AudioClip fireClip; // �� �߻� ���� Ŭ��
    [SerializeField]
    private AudioClip reloadClip; // �� ������ ���� Ŭ��
    [SerializeField]
    private AudioClip emptydClip; // �� ������ ���� Ŭ��
    [SerializeField]
    private Text bulletText; // źâ ���� �˷��ִ� �ؽ�Ʈ

    private Animator childAnimator; // �� �ִϸ��̼� ������Ʈ �ڽĿ��Լ� �޾ƿ´�.
    private AudioSource gunAudio; // �߻� ���带 ����� ������ҽ� ������Ʈ
    private LineRenderer bulletLineRender; // �߻�� ���̸� �׸� 
    private bool triggerButton = false; // �Ѿ� �ܹ� �߻�� �Ҹ���
    private int magCapacity = 15; // źâ �뷮
    private int magAmmo = 15; // ���� źâ�� ���� �ִ� ź��
    private float reloadTime = 1.0f; // ������ �ҿ� �ð�

    private enum State // ���� ���� ����
    {
        Ready, // �߻� �غ��
        Empty, // źâ�� ��
        Reloading // ���� ��
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
        magAmmo = magCapacity; // źâ�� ���� ä���.
        state = State.Ready; // ���� ���� ���¸� ���� �� �غ� �� ���·� ����
        muzzle.gameObject.transform.position = barrelLocation.position;
    }

    private void Update()
    {
        // ������ Ʈ���� ��ư�� ������ ����Ѵ�.
        if (CustomController.IsButtonPressed(CommonUsages.triggerButton, ref triggerButton, false))
        {
            if(state == State.Empty)
            {
                gunAudio?.PlayOneShot(emptydClip);
            }
            if(state == State.Ready)
            {
                Shot(); // ���� �߻� ó�� ����
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
        muzzle.Play(); // �ڷ�ƾ���� ����Ʈ ȣ���ϸ� ���� ��

        RaycastHit hit;
        Vector3 hitPosition;

        if (Physics.Raycast(barrelLocation.position, barrelLocation.forward, out hit, fireDistance))
        {
            // ���̰� �浹�� ���� ����
            hitPosition = hit.point;

            if (hit.transform.gameObject.CompareTag("Monster2"))
            {
                MonsterCtrl ailen = hit.transform.GetComponent<MonsterCtrl>();
                ailen?.GetDamage(attackAmount);
            }
        }
        else
        {
            // ���̰� �ٸ� ��ü�� �浹���� �ʾҴٸ�
            // ź���� �ִ� �����Ÿ����� ���ư��� ���� ��ġ�� �浹 ��ġ�� ���
            hitPosition = barrelLocation.position + barrelLocation.forward * fireDistance;
        }

        //�߻� ����Ʈ ���
        StartCoroutine(ShotEffect(hitPosition));

        magAmmo--;
        bulletText.text = magAmmo.ToString(); // ���� �Ѿ� ����
        if (magAmmo <= 0)
        {
            state = State.Empty;        
        }
    }

    private IEnumerator ShotEffect(Vector3 hitposition)
    {
        childAnimator?.SetTrigger("Fire"); // ��� �ִϸ��̼� ����
        gunAudio?.PlayOneShot(fireClip); // ��� ���� ����

        // ���� �������� �ѱ��� ��ġ
        bulletLineRender.SetPosition(0, barrelLocation.position);
        // ���� ������ �Է����� ���� �浹 ��ġ
        bulletLineRender.SetPosition(1, hitposition);

        // ���� �������� Ȱ��ȭ �Ͽ� ź�� ������ �׸���.
        bulletLineRender.enabled = true;
        // ��� ���
        yield return new WaitForSeconds(0.03f);
        // ���� �������� Ȱ��ȭ �Ͽ� ź�� ������ �����.
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
        bulletText.text = magAmmo.ToString(); // ���� �Ѿ� ����
        state = State.Ready;
    }
}
