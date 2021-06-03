using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;


public class SwitchManager : MonoBehaviour
{
    [SerializeField]
    private GameObject childObject;

    bool YButton = false; // ��ư�� ���ȴ��� Ȯ���ϴ� ����

    public void Update()
    {
        MenuUI();
    }

    private void MenuUI()
    {
        if (CustomController.IsButtonPressed(CommonUsages.secondaryButton, ref YButton, false))//������ ��Ʈ�ѷ��� ������ ����â�� �����ϴ�..
        {
            childObject.gameObject.SetActive(!childObject.activeSelf);//UI�� ���� �Ҵ�.
            //if (childObject.gameObject != null)
            //{
            //    Time.timeScale = 0.1f;
            //    return;
            //}
            //else
            //{
            //    Time.timeScale = 1f;
            //}
        }

    }




}
    // Start is called before the first frame update

