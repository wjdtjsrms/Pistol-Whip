using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;
public partial class GunShoot : MonoBehaviour
{
    #region �� ���� �ʵ�
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
    private bool triggerButton = false; // �Ѿ� �ܹ� �߻�� �Ҹ���
    private int magCapacity = 15; // źâ �뷮
    private int magAmmo = 15; // ���� źâ�� ���� �ִ� ź��
    private float reloadTime = 1.0f; // ������ �ҿ� �ð�
    #endregion

    #region ���η����� ���� �ʵ�
    [SerializeField]
    private BulletLineRender lineFader;
    private List<BulletLineRender> lineFaders;
    private int index = 0; // ���� �׸� ���� �������� �ε���
    #endregion

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

        // �ִ� 4�� ���� ���ÿ� ���δ�.
        lineFaders = new List<BulletLineRender>();
        lineFaders.Add(Instantiate(lineFader));
        lineFaders.Add(Instantiate(lineFader));
        lineFaders.Add(Instantiate(lineFader));
        lineFaders.Add(Instantiate(lineFader));
    }

    private void Start()
    {
        childAnimator = GetComponentInChildren<Animator>();
        gunAudio = GetComponent<AudioSource>();
        magAmmo = magCapacity; // źâ�� ���� ä���.
        state = State.Ready; // ���� ���� ���¸� ���� �� �غ� �� ���·� ����
        muzzle.gameObject.transform.position = barrelLocation.position; // �߻� ����Ʈ�� ��ġ�� �ѱ��� ����     
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

        if (Physics.Raycast(barrelLocation.position, barrelLocation.forward, out hit, fireDistance))
        {
            // ���̰� �浹�� ���� ����
            hitPosition = hit.point;

            if (hit.transform.gameObject.CompareTag("Monster2"))
            {
                MonsterCtrl ailen = hit.transform.GetComponent<MonsterCtrl>();
                ailen?.GetDamage(attackAmount);
            }
            if (hit.transform.gameObject.CompareTag("Button"))
            {
                SceneManager.LoadScene("SampleScene");
            }

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
        lineFaders[++index % lineFaders.Count].StartRender(barrelLocation.position, hitposition); // ��ݽ� �����Ǵ� ���� ������ ��ο�
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