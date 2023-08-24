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

    [Tooltip("��ĳ���׬�75��")]
    [Header("�̤j����")]
    [SerializeField]
    int maxHeadAngle_ = 75;

    [Tooltip("��ĳ���׬�-60��,�ФŤp��-89��")]
    [Header("�̤p����")]    
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
        // �ھ� rotateValue_ �� x �M y ���q�ӭp����ਤ��
        float rotationX = sensitiveRotateValue.x;
        float rotationY = sensitiveRotateValue.y;


        var finalAngle = cameraFollowedObject_.transform.rotation;
        // �ھڭp��X�����ਤ�רӱ��ફ��
       finalAngle *= Quaternion.Euler(rotationX, rotationY, 0f);

        // ���o���骺�کԨ���
        var mainCameraEulerAngles = finalAngle.eulerAngles;
        var aimCameraEulerAngles = finalAngle.eulerAngles;

        mainCameraEulerAngles = clampMainCameraRotateAngle(mainCameraEulerAngles);
        aimCameraEulerAngles = clampAimCameraRotateAngle(aimCameraEulerAngles);
        // �N�کԨ��׳]�w�^���骺����
        cameraFollowedObject_.transform.eulerAngles = mainCameraEulerAngles;        
        aimCameraFollowedObject_.transform.eulerAngles = aimCameraEulerAngles;
    }
    Vector3 clampMainCameraRotateAngle(Vector3 target)
    {
        // �N���ਤ�׭���b 75 �� -90 ���d��
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
