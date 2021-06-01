using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private GameObject gameobject;

    private float hp = 100.0f;
    private float damage = 10f;

    public float HP
    {
        get
        {
            return hp;
        }
    }
   
    void Start()
    {
        GameManager.Instance.actPlayerDie += Die;
        GameManager.Instance.actPlayerDamage += GetDamage;
    }

    private void OnTriggerEnter(Collider other)
    {
        // 총알 및 무엇이 되었든 플레이어에게 데미지를 줄 수 있는 존재는 Monster Tag를 가진다.
        if (other.gameObject.CompareTag("Monster"))
        {
            GameManager.Instance.playerDamage(this);
        }
    }

    public void GetDamage()
    {
        hp -= damage;
        if(hp<0)
        {
            GameManager.Instance.playerDie(this);
        }
    }

    public void Die()
    {
        gameObject.SetActive(false);
    }    
}
