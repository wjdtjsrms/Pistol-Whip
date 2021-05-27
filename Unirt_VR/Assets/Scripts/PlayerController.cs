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
        if (other.gameObject.CompareTag("Punch"))
        {
            GameManager.Instance.playerDamage(this);
        }
        else if(other.gameObject.CompareTag("Button"))// 버튼 총으로 맞출때 버튼 클릭 
        {
            Debug.Log("Enter");
            SceneManager.LoadScene("SampleScene");
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
