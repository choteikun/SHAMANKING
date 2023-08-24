using Cinemachine;
using Gamemanager;
using UnityEngine;

public class CameraControllerView : MonoBehaviour
{
    [SerializeField]
    GameObject movementVirtualCamera_;

    [SerializeField]
    GameObject aimVirtualCamera_;

    
    Vector2 nowRotateGamepadValue_ = new Vector2();
    [SerializeField]
    GameObject cameraFollowedObject_;
    [SerializeField]
    GameObject aimCameraFollowedObject_;
    [SerializeField]
    float rotateSpeed_X_ = 50f;
    [SerializeField]
    float rotateSpeed_Y_ = 10f;

    [Tooltip("建議角度為75度")]
    [Header("最大角度")]
    [SerializeField]
    int maxHeadAngle_ = 75;

    [Tooltip("建議角度為-60度,請勿小於-89度")]
    [Header("最小角度")]    
    [SerializeField]
    int minHeadAngle_ = -60;

    [SerializeField]
    CMCameraController cmCameraController_;
    Vector3 rotateValue_ => new Vector3(nowRotateGamepadValue_.y, nowRotateGamepadValue_.x, 0);
    private void Awake()
    {
        cmCameraController_.SetVirtualCamera();
    }
    private void Start()
    {
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerCameraRotate, changeRotateValue);
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnAimingButtonTrigger, changeCamera);
    }

    void changeRotateValue(PlayerControllerCameraRotateCommand command)
    {
        nowRotateGamepadValue_ = command.RotateValue;
    }
    private void Update()
    {
        rotateCameraFollowedObject();
        cmCameraController_.CMTVUpdater(maxHeadAngle_);
    }

    void rotateCameraFollowedObject()
    {
        var sensitiveRotateValue = new Vector3(rotateValue_.x * Time.deltaTime * rotateSpeed_Y_, rotateValue_.y * Time.deltaTime * rotateSpeed_X_, 0);
        // 根據 rotateValue_ 的 x 和 y 分量來計算旋轉角度
        float rotationX = sensitiveRotateValue.x;
        float rotationY = sensitiveRotateValue.y;


        var finalAngle = cameraFollowedObject_.transform.rotation;
        // 根據計算出的旋轉角度來旋轉物體
       finalAngle *= Quaternion.Euler(rotationX, rotationY, 0f);

        // 取得物體的歐拉角度
        var mainCameraEulerAngles = finalAngle.eulerAngles;
        var aimCameraEulerAngles = finalAngle.eulerAngles;

        mainCameraEulerAngles = clampMainCameraRotateAngle(mainCameraEulerAngles);
        aimCameraEulerAngles = clampAimCameraRotateAngle(aimCameraEulerAngles);
        // 將歐拉角度設定回物體的旋轉
        cameraFollowedObject_.transform.eulerAngles = mainCameraEulerAngles;        
        aimCameraFollowedObject_.transform.eulerAngles = aimCameraEulerAngles;
    }
    Vector3 clampMainCameraRotateAngle(Vector3 target)
    {
        // 將旋轉角度限制在 75 到 -90 的範圍內
        if (target.x > 180f)
        {
            target.x -= 360f;
        }
        target.x = Mathf.Clamp(target.x, minHeadAngle_, maxHeadAngle_);
        target.z = 0;
        return target;
    }
    Vector3 clampAimCameraRotateAngle(Vector3 target)
    {
        target.x = Mathf.Clamp(target.x, -25, 25);
        target.z = 0;
        return target;
    }
    void changeCamera(PlayerAimingButtonCommand command)
    {
        if (command.AimingButtonIsPressed)
        {
            movementVirtualCamera_.SetActive(false);
            aimVirtualCamera_.SetActive(true);
        }
        else
        {
            movementVirtualCamera_.SetActive(true);
            aimVirtualCamera_.SetActive(false);
        }
    }
}
