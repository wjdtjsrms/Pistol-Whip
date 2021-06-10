using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;


public class SwitchManager : MonoBehaviour
{
    [SerializeField]
    private GameObject settingMenu;

    bool YButton = false; // ��ư�� ���ȴ��� Ȯ���ϴ� ����

    public void Update()
    {
        MenuUI();
    }

    private void MenuUI()
    {
        if (CustomController.IsButtonPressed(CommonUsages.secondaryButton, ref YButton, true))//������ ��Ʈ�ѷ��� ������ ����â�� �����ϴ�..
        {
           
           settingMenu.gameObject.SetActive(!settingMenu.activeSelf);//UI�� ���� �Ҵ�
   
        }

    }




}
    // Start is called before the first frame update

