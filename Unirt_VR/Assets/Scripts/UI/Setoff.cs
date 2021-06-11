using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setoff : MonoBehaviour,IShotAble
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject gameobjectOFF;

    // Update is called once per frame
    public void OnShot(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        gameobjectOFF.SetActive(false);
    }
   
}
