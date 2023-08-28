using Gamemanager;
using UnityEngine;

public class CameraControllerView : MonoBehaviour
{
    [SerializeField]
    GameObject movementVirtualCamera_;

    [SerializeField]
    GameObject aimVirtualCamera_;


    Vector2 nowRotateGamepadValue_ = new Vector2();
    [field: SerializeField]
    public GameObject CameraFollowedObject { get; private set; }
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

    CameraControllerStateMachine stateMachine_;

    Vector3 rotateValue_ => new Vector3(nowRotateGamepadValue_.y, nowRotateGamepadValue_.x, 0);

    public Quaternion FinalQuaternion_ { get; private set; }

    private void Awake()
    {
        stateMachine_ = new CameraControllerStateMachine(this);
        cmCameraController_.SetVirtualCamera();
        stateMachine_.StageManagerInit();
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
        stateMachine_.StageManagerUpdate();
    }

    void rotateCameraFollowedObject()
    {
        var sensitiveRotateValue = getSensitiveRotateValue();

        var rotationX = sensitiveRotateValue.x;
        var rotationY = sensitiveRotateValue.y;


        FinalQuaternion_ = CameraFollowedObject.transform.rotation;
        FinalQuaternion_ *= Quaternion.Euler(rotationX, rotationY, 0f);


        var aimCameraEulerAngles = FinalQuaternion_.eulerAngles;

        aimCameraEulerAngles = clampAimCameraRotateAngle(aimCameraEulerAngles);

        aimCameraFollowedObject_.transform.eulerAngles = aimCameraEulerAngles;
    }
    Vector3 getSensitiveRotateValue()
    {
        if (CameraFollowedObject.transform.rotation.eulerAngles.x >= 0 && CameraFollowedObject.transform.rotation.eulerAngles.x <= 76)
        {
            var y_limiter = 1 - headRotateSpeedLimitValue_ / maxHeadAngle_ * CameraFollowedObject.transform.rotation.eulerAngles.x;
            var sensitiveRotateValue = new Vector3(rotateValue_.x * Time.deltaTime * rotateSpeed_Y_, rotateValue_.y * Time.deltaTime * rotateSpeed_X_ * y_limiter, 0);
            return sensitiveRotateValue;
        }
        else
        {
            var sensitiveRotateValue = new Vector3(rotateValue_.x * Time.deltaTime * rotateSpeed_Y_, rotateValue_.y * Time.deltaTime * rotateSpeed_X_, 0);
            return sensitiveRotateValue;
        }
    }
    public Vector3 ClampMainCameraRotateAngle(Vector3 target)
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
