using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emission : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject target_mat;


    void Start()
    {
        
    }
    private void emission()
    {
        SkinnedMeshRenderer mr = target_mat.GetComponent<SkinnedMeshRenderer>();
        Material mat = mr.material;


        mat.SetColor("_EmissionColor", Color.white);

        float emission = Mathf.PingPong(Time.time, 1f);//emission�� �� �ʸ��� �������� 
        Color baseColor = Color.gray * 1.5f;// �⺻ �÷��� ���� 2����
        Color finalColor = baseColor * Mathf.LinearToGammaSpace(emission);

        mat.SetColor("_EmissionColor", finalColor);


    }
    // Update is called once per frame
    void FixedUpdate()
    {
        emission();
    }
}

