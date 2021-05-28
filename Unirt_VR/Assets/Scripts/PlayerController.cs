using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
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
