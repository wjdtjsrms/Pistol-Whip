using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch : MonoBehaviour
{
    [SerializeField]
    private CustomController customController; // Grip 값을 가져올 콘트롤러
    [SerializeField]
    private GameObject appleWatch; // 보여줄 UI 오브젝트
    private float gripValue = 0f;

    private void Update()
    {
        gripValue = customController.GripValue;

        if (gripValue > 0.7f)
        {
            appleWatch.gameObject.SetActive(false);
        }
        else
        {
            appleWatch.gameObject.SetActive(true);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (gripValue > 0.7f)
        {
            if (other.CompareTag("Hit"))
            {
                other.GetComponent<EnemyCtrl>().EnemyDamage();
            }
        }
    }
}
