using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch : MonoBehaviour
{
    [SerializeField]
    private CustomController customController; // Grip 값을 가져올 콘트롤러

    [SerializeField]
    private AudioClip melleClip;

    private float gripValue = 0f;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        gripValue = customController.GripValue;
    }

    void OnTriggerEnter(Collider other)
    {
        if (gripValue > 0.7f)
        {
            if (other.CompareTag("Hit"))
            {
                FadeScript.Instance.FadeWhite();
                other.GetComponent<EnemyCtrl>().EnemyDamage();
                audioSource.PlayOneShot(melleClip);

            }
        }
    }
}