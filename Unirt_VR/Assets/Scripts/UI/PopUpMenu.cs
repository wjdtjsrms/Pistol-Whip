using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
public class PopUpMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject childObject;
    private bool isPressedB = false; // B 버튼이 눌렸는지 확인하는 변수

    void Update()
    {
        if (CustomController.IsButtonPressed(CommonUsages.secondaryButton, ref isPressedB, false))
        {
            GameManager.Instance.GamePause(this);
            childObject.gameObject.SetActive(true);
        }
    }
}
