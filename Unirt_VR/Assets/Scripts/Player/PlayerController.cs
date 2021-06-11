using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private AudioClip damageClip;
    private AudioSource audioSource;

    [SerializeField]
    public AudioClip Break_Sound; // 플레이어 히트 유리 깨짐 소리
    [SerializeField]
    public AudioClip Breath_Sound; // 플레이어 숨소리.


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        AudioSource Player_Hit  = GetComponent<AudioSource>();
        AudioSource Player_Hit2 = GetComponent<AudioSource>();

        AudioSource Player_Restore = GetComponent<AudioSource>();

    }

    void Start()
    {
        GameManager.Instance.actPlayerDie += () => gameObject.SetActive(false);
        GameManager.Instance.actPlayerDamage += () => audioSource.PlayOneShot(damageClip);
        GameManager.Instance.actPlayerDamage += () => audioSource.PlayOneShot(Break_Sound);
        GameManager.Instance.actPlayerDamage += () => audioSource.PlayOneShot(Breath_Sound);
    }
    
    private void OnTriggerEnter(Collider other)
    {

        // 총알 및 무엇이 되었든 플레이어에게 데미지를 줄 수 있는 존재는 Monster Tag를 가진다.
        if (other.gameObject.CompareTag("Monster"))
        {
            other.gameObject.SetActive(false);
            GameManager.Instance.playerDamage(this);
        }
    }
}
