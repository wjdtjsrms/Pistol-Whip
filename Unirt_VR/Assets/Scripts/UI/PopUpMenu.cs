using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
public class PopUpMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject childObject;
    private bool isPressedB = false; // B ��ư�� ���ȴ��� Ȯ���ϴ� ����

    void Update()
    {
        if (CustomController.IsButtonPressed(CommonUsages.secondaryButton, ref isPressedB, false) && GameManager.Instance.isGameStart == true)
        {
            GameManager.Instance.GamePause(this); // �Ͻ����� �̺�Ʈ�� ȣ���Ѵ�.
            childObject.gameObject.SetActive(true); 
        }
    }
}
