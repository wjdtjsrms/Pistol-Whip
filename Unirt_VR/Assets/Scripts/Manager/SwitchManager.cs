using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;


public class SwitchManager : MonoBehaviour
{
    [SerializeField]
    private GameObject childObject;

    bool YButton = false; // 버튼이 눌렸는지 확인하는 변수

    public void Update()
    {
        MenuUI();
    }

    private void MenuUI()
    {
        if (CustomController.IsButtonPressed(CommonUsages.secondaryButton, ref YButton, false))//오른쪽 컨트롤러를 누르면 설정창이 열립니다..
        {
            childObject.gameObject.SetActive(!childObject.activeSelf);//UI를 껐다 켠다.
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

