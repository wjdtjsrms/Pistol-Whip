using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmissionYellow : MonoBehaviour
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

        mat.SetColor("_EmissionColor", Color.white);

        float emission = Mathf.PingPong(Time.time, 1.2f);//emission�� �� �ʸ��� �������� 
        Color baseColor = Color.yellow *0.8f;// �⺻ �÷��� ���� 2����
        Color finalColor = baseColor * Mathf.LinearToGammaSpace(emission);

        mat.SetColor("_EmissionColor", finalColor);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        ColorSwitch();

    }
}

