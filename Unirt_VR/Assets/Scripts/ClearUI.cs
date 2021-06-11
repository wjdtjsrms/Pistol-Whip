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
            Clear.SetActive(true); // �ݶ��̴��� ������ Game Clear �۾� ���ֱ�
        }
    }
}
