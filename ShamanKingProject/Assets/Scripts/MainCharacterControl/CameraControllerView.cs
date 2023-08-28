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

    [Header("最大攝影機角度")]
    [Tooltip("預設75度")]
    [SerializeField]
    int maxHeadAngle_ = 75;

    [Tooltip("最小攝影機角度")]
    [Header("預設-60度")]    
    [SerializeField]
    int minHeadAngle_ = -60;

    [SerializeField]
    CMCameraController cmCameraController_;

    [Header("攝影機")]
    [SerializeField]
    float headRotateSpeedLimitValue_ = 0.7f;


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
        var sensitiveRotateValue = getSensitiveRotateValue();
        
        var rotationX = sensitiveRotateValue.x;
        var rotationY = sensitiveRotateValue.y;


        var finalAngle = cameraFollowedObject_.transform.rotation;
       finalAngle *= Quaternion.Euler(rotationX, rotationY, 0f);

        var mainCameraEulerAngles = finalAngle.eulerAngles;
        var aimCameraEulerAngles = finalAngle.eulerAngles;

        mainCameraEulerAngles = clampMainCameraRotateAngle(mainCameraEulerAngles);
        aimCameraEulerAngles = clampAimCameraRotateAngle(aimCameraEulerAngles);

        cameraFollowedObject_.transform.eulerAngles = mainCameraEulerAngles;        
        aimCameraFollowedObject_.transform.eulerAngles = aimCameraEulerAngles;
    }
    Vector3 getSensitiveRotateValue()
    {
        if (cameraFollowedObject_.transform.rotation.eulerAngles.x>=0 && cameraFollowedObject_.transform.rotation.eulerAngles.x <=76)
        {
            var y_limiter = 1 - headRotateSpeedLimitValue_ / maxHeadAngle_ * cameraFollowedObject_.transform.rotation.eulerAngles.x;
            var sensitiveRotateValue = new Vector3(rotateValue_.x * Time.deltaTime * rotateSpeed_Y_, rotateValue_.y * Time.deltaTime * rotateSpeed_X_*y_limiter, 0);
            return sensitiveRotateValue;
        }
        else
        {
            var sensitiveRotateValue = new Vector3(rotateValue_.x * Time.deltaTime * rotateSpeed_Y_, rotateValue_.y * Time.deltaTime * rotateSpeed_X_, 0);
            return sensitiveRotateValue;
        }
    }
    Vector3 clampMainCameraRotateAngle(Vector3 target)
    {
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
        if (target.x > 180f)
        {
            target.x -= 360f;
        }
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
