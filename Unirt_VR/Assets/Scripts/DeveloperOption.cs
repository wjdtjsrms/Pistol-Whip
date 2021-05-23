using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using System;

// 개발시에만 필요한 기능을 모아둘 클래스
// 이 클래스를 삭제해도 다른 컴포넌트에 지장이 없게 만들어야 함.

public class DeveloperOption : MonoBehaviour
{
    #region UI 필드
    [SerializeField]
    private Button setGoFront;
    [SerializeField]
    private Button setFreeMove;
    [SerializeField]
    private static Text debugText;
    [SerializeField]
    private GameObject childObject;
    #endregion
    bool isPressedXA = false; // 버튼이 눌렸는지 확인하는 변수
    [SerializeField]
    private MovementProvider movementProvider; // 이동 설정 변경을 위해 선언
    [SerializeField]
    private CustomController customController; // X,A 버튼 입력값을 받아오기 위해 선언

    private void Start()
    {
        // 버튼에 이동 설정 변경 이벤트 추가
        setFreeMove.onClick.AddListener(() => movementProvider.moveType = MovementProvider.MoveType.FreeMove);
        setGoFront.onClick.AddListener(() => movementProvider.moveType = MovementProvider.MoveType.GoFront);
    }
    private void Update()
    {
        SetUI();
    }

    private void SetUI() // 콘트롤러의 x,a 버튼이 눌렸으면 UI를 껏다 킨다. 
    {    
        if (CustomController.IsButtonPressed(CommonUsages.primaryButton, ref isPressedXA,false))
        {
            childObject.gameObject.SetActive(!childObject.activeSelf);
        }
    }
}
