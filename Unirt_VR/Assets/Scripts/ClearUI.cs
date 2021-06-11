using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearUI : MonoBehaviour
{
    [SerializeField]
    private GameObject Clear;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {           
            Clear.SetActive(true); // 콜라이더에 닿으면 Game Clear 글씨 켜주기
        }
    }
}
