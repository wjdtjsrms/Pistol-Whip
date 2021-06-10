using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmissionCyan : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject target_mat;


    void Start()
    {

    }
    void ColorSwitch()
    {
        MeshRenderer mr = target_mat.GetComponent<MeshRenderer>();
        Material mat = mr.material;

        mat.SetColor("_EmissionColor", Color.black);

        float emission = Mathf.PingPong(Time.time, 1.4f);//emission을 몇 초마다 변경할지 
        Color baseColor = Color.cyan * 0.8f;// 기본 컬러에 강도 2높임
        Color finalColor = baseColor * Mathf.LinearToGammaSpace(emission);

        mat.SetColor("_EmissionColor", finalColor);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        ColorSwitch();

    }
}

