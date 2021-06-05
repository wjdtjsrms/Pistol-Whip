using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public partial class CustomController : MonoBehaviour
{
    #region 컨트롤러 초기화 변수
    [SerializeField]
    private InputDeviceCharacteristics characteristics; // device 특성값을 담는 변수
    [SerializeField]
    private List<GameObject> controllerModels; // 사용 가능한 모델 리스트들
    private GameObject controllerInstance; // 생성된 컨트롤러 인스턴스를 참조하는 변수
    private InputDevice availableDevice; // 현재 사용중인 컨트롤러
    private static InputDevice rightInputDevice; // 오른손 컨트롤러 인풋 디바이스
    private static InputDevice leftInputDevice; // 왼손 컨트롤러 인풋 디바이스
    #endregion

    #region 핸드 모델 초기화 변수
    [SerializeField]
    private bool renderController; // Hand와 Controller 사이를 변경할 변수
    [SerializeField]
    private GameObject handModel; // 핸드 모델 prefab
    private GameObject handInstance; // 생성된 핸드 인스턴스를 참조하는 변수
    private Animator handModelAnimator; // 핸드 모델 애니메이션 변수

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
        TryInitiaiize(); // 컨트롤러 세팅

    }
    void Update()
    {
        // 모종의 이유로 퀘스트가 셋팅이 되지 않았다면 다시 초기화를 진행한다.
        if (!availableDevice.isValid)
        {
            TryInitiaiize();
        }
        if (renderController) // 컨트롤러를 렌더한다.
        {
            handInstance.SetActive(false);
            controllerInstance.SetActive(true);
        }
        else // 핸드를 렌더한다.
        {
            handInstance.SetActive(true);
            controllerInstance.SetActive(false);
            UpdateHandAnimation(); // 핸드 애니메이션은 여기서만 수행한다.
        }
    }
}

// 버튼 클릭 체크 함수
public partial class CustomController : MonoBehaviour 
{
    // 확인할 버튼, 중복 클릭 방지용 불리언, 왼쪽 버튼인지 오른쪽 버튼인지 확인하는 불리언
    public static bool IsButtonPressed(InputFeatureUsage<bool> inputFeature, ref bool isPressed, bool isLeft) // 버튼이 중복으로 눌리지 않게 해주는 함수
    {
        bool ButtonValue;
        InputDevice inputDevice = isLeft ? leftInputDevice : rightInputDevice; // 왼손 버튼인지 오른손 버튼인지 판독
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

// 초기화용 함수
public partial class CustomController : MonoBehaviour
{
    void TryInitiaiize()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(characteristics, devices); // 설정한 값에 부합하는 디바이스를 가져온다.

        foreach (var device in devices)
        {
            Debug.Log($"Available Device Name : {device.name}, Characteristic  {device.characteristics}");
        }
        if (devices.Count > 0)
        {
            availableDevice = devices[0];
            GameObject currentControllerModel = null;
            SetControllerModel(ref currentControllerModel); // 컨트롤러 모델 설정 
            InstantiateModel(ref currentControllerModel); // 설정한 컨트롤러의 객체 생성

            handInstance = Instantiate(handModel, transform); // 핸드 인스턴스 추가
            handModelAnimator = handInstance.GetComponent<Animator>(); // 핸드 인스턴스에 추가되어 있는 애니메이터를 가져온다.
        }
    }

    // 왼손 오른손 설정
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

    // 설정한 모델 생성
    void InstantiateModel(ref GameObject currentControllerModel)
    {
        if (currentControllerModel)
        {
            controllerInstance = Instantiate(currentControllerModel, transform);
        }
        else
        {
            // 적당한 객체를 찾지 못하면 기본 객체를 생성한다.
            Debug.LogError("Didn't get sutiable controller model");
            controllerInstance = Instantiate(controllerModels[0], transform);
        }
    }

    // 핸드 애니메이션 설정
    void UpdateHandAnimation()
    {
        // 컨트롤러의 trigger 값을 애니메이터의 trigger 값에 대입한다.
        if (availableDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            handModelAnimator.SetFloat("Trigger", triggerValue);
        }
        else
        {
            handModelAnimator.SetFloat("Trigger", 0);
        }

        // 컨트롤러의 grip 값을 애니메이터의 grip 값에 대입한다.
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