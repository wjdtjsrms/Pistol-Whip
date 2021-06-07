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
    #region �� ���� �ʵ�
    [SerializeField]
    private ParticleSystem muzzle; // ��� ����Ʈ
    [SerializeField]
    private Transform barrelLocation; // ��� ����Ʈ�� ������ ��ġ
    [SerializeField]
    private AudioClip fireClip; // �� �߻� ���� Ŭ��
    [SerializeField]
    private AudioClip reloadClip; // �� ������ ���� Ŭ��
    [SerializeField]
    private AudioClip emptydClip; // �� ������ ���� Ŭ��
    [SerializeField]
    private TextMeshPro bulletText; // źâ ���� �˷��ִ� �ؽ�Ʈ

    private float attackAmount = 35.0f; // �� ���ݷ�
    private float fireDistance = 35.0f; // �� �����Ÿ�
    private Animator childAnimator; // �� �ִϸ��̼� ������Ʈ �ڽĿ��Լ� �޾ƿ´�.
    private AudioSource gunAudio; // �߻� ���带 ����� ������ҽ� ������Ʈ
    private bool triggerButton = false; // �Ѿ� �ܹ� �߻�� �Ҹ���
    private int magCapacity = 15; // źâ �뷮
    private int magAmmo = 15; // ���� źâ�� ���� �ִ� ź��
    private float reloadTime = 0.5f; // ������ �ҿ� �ð�
    private int layerMask; // ���̿� �浹�� ��ü�� �����ϱ� ���� ���̾��ũ

    private enum State // ���� ���� ����
    {
        Ready, // �߻� �غ��
        Empty, // źâ�� ��
        Reloading // ���� ��
    }
    private State state;
    #endregion

    #region ���η����� ���� �ʵ�
    [SerializeField]
    private BulletLineRender[] lineRenders;
    private int index = 0; // ���� �׸� ���� �������� �ε���
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
        magAmmo = magCapacity; // źâ�� ���� ä���.
        state = State.Ready; // ���� ���� ���¸� ���� �� �غ� �� ���·� ����
        muzzle.gameObject.transform.position = barrelLocation.position; // �߻� ����Ʈ�� ��ġ�� �ѱ��� ����        
        layerMask = (1 << LayerMask.NameToLayer("UI")) + (1 << LayerMask.NameToLayer("Enemy")); // �� ���̴� Enemy �� UI ���̾ �浹�Ѵ�.
    }

    private void Update()
    {
        // ������ Ʈ���� ��ư�� ������ ����Ѵ�.
        if (CustomController.IsButtonPressed(CommonUsages.triggerButton, ref triggerButton, false))
        {
            if (state == State.Empty) // �Ѿ��� ���ٸ� �߻���� �ʰ� �� �� �Ҹ��� ����.
            {
                gunAudio?.PlayOneShot(emptydClip);
            }
            if (state == State.Ready)
            {
                Shot(); // ���� �߻� ó�� ����
            }
        }

        // ���� 90���� ������ ������ �ȴ�.
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
            // ���̰� �浹�� ���� ����
            hitPosition = hit.point;
            // �Ѱ� ���ͷ����� �ִ� Ÿ���̶�� �� ��ü�� �Լ��� ȣ���Ѵ�.
            IShotAble shotObject = hit.transform.gameObject.GetComponent<IShotAble>();
            shotObject?.OnShot(attackAmount, hit.point, hit.normal);
           
        }
        else
        {
            // ���̰� �ٸ� ��ü�� �浹���� �ʾҴٸ�
            // ź���� �ִ� �����Ÿ����� ���ư��� ���� ��ġ�� �浹 ��ġ�� ���
            hitPosition = barrelLocation.position + barrelLocation.forward * fireDistance;
        }
        //�߻� ����Ʈ ���
        ShotEffect(hitPosition);

        magAmmo--;
        bulletText.text = magAmmo.ToString(); // ���� �Ѿ� ����
        if (magAmmo <= 0)
        {
            state = State.Empty;
        }
    }

    private void ShotEffect(Vector3 hitposition)
    {
        muzzle.Play(); // ��� ����Ʈ ����
        childAnimator?.SetTrigger("Fire"); // ��� �ִϸ��̼� ����
        gunAudio?.PlayOneShot(fireClip); // ��� ���� ����
        lineRenders[++index % lineRenders.Length].StartRender(barrelLocation.position, hitposition); // ��ݽ� �����Ǵ� ���� ������ ��ο�
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
        state = State.Reloading;// ���¸� ������ ������ �ٲ۴�.
        gunAudio?.PlayOneShot(reloadClip); // ������ �Ҹ��� ����Ѵ�.

        yield return new WaitForSeconds(reloadTime); // ������ �ð���ŭ ��ٸ���.
        magAmmo = magCapacity; // �Ѿ��� �ִ�ġ���� ä���.
        bulletText.text = magAmmo.ToString(); // �Ѿ� ������ UI�� ����
        state = State.Ready; // ��� ���� ���·� �ٲ۴�.
    }
}