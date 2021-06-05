using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public partial class CustomController : MonoBehaviour
{
    #region ��Ʈ�ѷ� �ʱ�ȭ ����
    [SerializeField]
    private InputDeviceCharacteristics characteristics; // device Ư������ ��� ����
    [SerializeField]
    private List<GameObject> controllerModels; // ��� ������ �� ����Ʈ��
    private GameObject controllerInstance; // ������ ��Ʈ�ѷ� �ν��Ͻ��� �����ϴ� ����
    private InputDevice availableDevice; // ���� ������� ��Ʈ�ѷ�
    private static InputDevice rightInputDevice; // ������ ��Ʈ�ѷ� ��ǲ ����̽�
    private static InputDevice leftInputDevice; // �޼� ��Ʈ�ѷ� ��ǲ ����̽�
    #endregion

    #region �ڵ� �� �ʱ�ȭ ����
    [SerializeField]
    private bool renderController; // Hand�� Controller ���̸� ������ ����
    [SerializeField]
    private GameObject handModel; // �ڵ� �� prefab
    private GameObject handInstance; // ������ �ڵ� �ν��Ͻ��� �����ϴ� ����
    private Animator handModelAnimator; // �ڵ� �� �ִϸ��̼� ����

    public float GripValue
    {
        get
        {
            if(handModelAnimator != null)
            {
                return handModelAnimator.GetFloat("Grip");
            }
            return 0f;
        }
    }

    #endregion

    void Start()
    {
        TryInitiaiize(); // ��Ʈ�ѷ� ����

    }
    void Update()
    {
        // ������ ������ ����Ʈ�� ������ ���� �ʾҴٸ� �ٽ� �ʱ�ȭ�� �����Ѵ�.
        if (!availableDevice.isValid)
        {
            TryInitiaiize();
        }
        if (renderController) // ��Ʈ�ѷ��� �����Ѵ�.
        {
            handInstance.SetActive(false);
            controllerInstance.SetActive(true);
        }
        else // �ڵ带 �����Ѵ�.
        {
            handInstance.SetActive(true);
            controllerInstance.SetActive(false);
            UpdateHandAnimation(); // �ڵ� �ִϸ��̼��� ���⼭�� �����Ѵ�.
        }
    }
}

// ��ư Ŭ�� üũ �Լ�
public partial class CustomController : MonoBehaviour 
{
    // Ȯ���� ��ư, �ߺ� Ŭ�� ������ �Ҹ���, ���� ��ư���� ������ ��ư���� Ȯ���ϴ� �Ҹ���
    public static bool IsButtonPressed(InputFeatureUsage<bool> inputFeature, ref bool isPressed, bool isLeft) // ��ư�� �ߺ����� ������ �ʰ� ���ִ� �Լ�
    {
        bool ButtonValue;
        InputDevice inputDevice = isLeft ? leftInputDevice : rightInputDevice; // �޼� ��ư���� ������ ��ư���� �ǵ�
        if (inputDevice.TryGetFeatureValue(inputFeature, out ButtonValue) && ButtonValue)
        {
            if (isPressed == false)
            {
                isPressed = true;
                return true;
            }
        }
        else
        {
            isPressed = false;
        }
        return false;
    }
}

// �ʱ�ȭ�� �Լ�
public partial class CustomController : MonoBehaviour
{
    void TryInitiaiize()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(characteristics, devices); // ������ ���� �����ϴ� ����̽��� �����´�.

        foreach (var device in devices)
        {
            Debug.Log($"Available Device Name : {device.name}, Characteristic  {device.characteristics}");
        }
        if (devices.Count > 0)
        {
            availableDevice = devices[0];
            GameObject currentControllerModel = null;
            SetControllerModel(ref currentControllerModel); // ��Ʈ�ѷ� �� ���� 
            InstantiateModel(ref currentControllerModel); // ������ ��Ʈ�ѷ��� ��ü ����

            handInstance = Instantiate(handModel, transform); // �ڵ� �ν��Ͻ� �߰�
            handModelAnimator = handInstance.GetComponent<Animator>(); // �ڵ� �ν��Ͻ��� �߰��Ǿ� �ִ� �ִϸ����͸� �����´�.
        }
    }

    // �޼� ������ ����
    void SetControllerModel(ref GameObject currentControllerModel)
    {
        if (availableDevice.name.Contains("Left"))
        {
            currentControllerModel = controllerModels[1];
            leftInputDevice = availableDevice;

        }
        else if (availableDevice.name.Contains("Right"))
        {
            currentControllerModel = controllerModels[2];
            rightInputDevice = availableDevice;

        }
        else
        {
            currentControllerModel = null;
        }
    }

    // ������ �� ����
    void InstantiateModel(ref GameObject currentControllerModel)
    {
        if (currentControllerModel)
        {
            controllerInstance = Instantiate(currentControllerModel, transform);
        }
        else
        {
            // ������ ��ü�� ã�� ���ϸ� �⺻ ��ü�� �����Ѵ�.
            Debug.LogError("Didn't get sutiable controller model");
            controllerInstance = Instantiate(controllerModels[0], transform);
        }
    }

    // �ڵ� �ִϸ��̼� ����
    void UpdateHandAnimation()
    {
        // ��Ʈ�ѷ��� trigger ���� �ִϸ������� trigger ���� �����Ѵ�.
        if (availableDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            handModelAnimator.SetFloat("Trigger", triggerValue);
        }
        else
        {
            handModelAnimator.SetFloat("Trigger", 0);
        }

        // ��Ʈ�ѷ��� grip ���� �ִϸ������� grip ���� �����Ѵ�.
        if (availableDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
        {
            handModelAnimator.SetFloat("Grip", gripValue);
        }
        else
        {
            handModelAnimator.SetFloat("Grip", 0);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (handModelAnimator.GetFloat("Grip") >= 0.7F)
        {
            if (other.CompareTag("Hit"))
            {
                other.GetComponent<EnemyCtrl>().EnemyDamage();
            }
        }
    }
}