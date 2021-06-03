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

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }


    private GameObject DeadUI;

    void Start()
    {
        GameManager.Instance.actPlayerDie += () => gameObject.SetActive(false);
        GameManager.Instance.actPlayerDamage += () => audioSource.PlayOneShot(damageClip);
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

    public void Die()
    {
        DeadUI.SetActive(true);
    }    
}
