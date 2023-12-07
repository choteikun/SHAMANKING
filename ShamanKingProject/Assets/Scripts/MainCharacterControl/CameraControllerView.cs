using Gamemanager;
using UniRx;
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
    [field:SerializeField]
    public GameObject AimCameraFollowedObject_ { get; private set; }
    [field: SerializeField]
    public GameObject TargetCameraFollowedObject_ { get; private set; }
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

    [Header("瞄準角度限制")]
    [SerializeField]
    float aimClampAngle_;

    CameraControllerStateMachine stateMachine_;

    Vector3 rotateValue_ => new Vector3(nowRotateGamepadValue_.y, nowRotateGamepadValue_.x, 0);

    public Quaternion MainCamQuaternion_ { get; private set; }
    public Quaternion AimQuaternion { get; private set; }

    public bool canMove_ = true;

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
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnSystemGetTarget, cmd => { stateMachine_.TransitionState("Target"); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnSystemResetTarget, cmd => { stateMachine_.TransitionState("MainGame"); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerLaunchGhost, launchCancelCameraRotate);
        GameManager.Instance.MainGameEvent.OnPlayerLaunchActionFinish.Where(cmd => cmd.Hit && (cmd.HitObjecctTag == HitObjecctTag.Biteable||cmd.HitObjecctTag == HitObjecctTag.Enemy)).Subscribe(cmd =>backToMainGame());
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnSystemCallFirstSceneCameraTransfer, cmd => { canMove_ = false; nowRotateGamepadValue_ = Vector2.zero;  });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnGameStandingConversationStart, cmd => { canMove_ = false; nowRotateGamepadValue_ = Vector2.zero; });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnSystemCallCameraTransferBack, cmd => { canMove_ = true; });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnGameStandingConversationEnd, cmd => { canMove_ = true; });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnGameConversationEnd, cmd => { canMove_ = true; });
    }

    void changeRotateValue(PlayerControllerCameraRotateCommand command)
    {
        nowRotateGamepadValue_ = command.RotateValue;
    }
    private void Update()
    {
        if (!canMove_) return;
        rotateCameraFollowedObject();
        cmCameraController_.CMTVUpdater(maxHeadAngle_);
        stateMachine_.StageManagerUpdate();
    }

    void rotateCameraFollowedObject()
    {
        var sensitiveRotateValue = getSensitiveRotateValue();

        var rotationX = sensitiveRotateValue.x;
        var rotationY = sensitiveRotateValue.y;


        MainCamQuaternion_ = CameraFollowedObject.transform.rotation;
        MainCamQuaternion_ *= Quaternion.Euler(rotationX, rotationY, 0f);
        AimQuaternion = AimCameraFollowedObject_.transform.rotation;
        AimQuaternion *= Quaternion.Euler(rotationX, rotationY, 0f);
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
    public Vector3 ClampAimCameraRotateAngle(Vector3 target)
    {
        if (target.x > 180f)
        {
            target.x -= 360f;
        }
        target.x = Mathf.Clamp(target.x, -aimClampAngle_, aimClampAngle_);
        target.z = 0;
        return target;
    }

    void changeCamera(PlayerAimingButtonCommand command)
    {
        if (command.AimingButtonIsPressed)
        {
            movementVirtualCamera_.SetActive(false);
            aimVirtualCamera_.SetActive(true);
            stateMachine_.TransitionState("Aim");
        }
        else
        {
            backToMainGame();
        }
    }

    void launchCancelCameraRotate(PlayerLaunchGhostButtonCommand cmd)
    {
        nowRotateGamepadValue_ = Vector3.zero;
    }

    void backToMainGame()
    {
        movementVirtualCamera_.SetActive(true);
        aimVirtualCamera_.SetActive(false);
        stateMachine_.TransitionState("MainGame");
    }
}
