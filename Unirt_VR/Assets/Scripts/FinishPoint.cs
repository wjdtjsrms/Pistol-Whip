using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishPoint : MonoBehaviour
{
    [SerializeField]
    private GameObject ClearUI;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.GameEnd(this);
            GameManager.Instance.SaveOperation();
            ClearUI.SetActive(false);


        }
    }
}
