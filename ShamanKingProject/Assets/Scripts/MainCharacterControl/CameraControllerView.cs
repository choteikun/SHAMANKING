using Cinemachine;
using Gamemanager;
using UnityEngine;

public class CameraControllerView : MonoBehaviour
{
    [SerializeField]
    Vector2 nowRotateGamepadValue_ = new Vector2();
    [SerializeField]
    GameObject cameraFollowedObject_;
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
    CMCameraController cMCameraController_;
    Vector3 rotateValue_ => new Vector3(nowRotateGamepadValue_.y, nowRotateGamepadValue_.x, 0);
    private void Awake()
    {
        cMCameraController_.SetVirtualCamera();
    }
    private void Start()
    {
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerCameraRotate, changeRotateValue);
    }

    void changeRotateValue(PlayerControllerCameraRotateCommand command)
    {
        nowRotateGamepadValue_ = command.RotateValue;
    }
    private void Update()
    {
        rotateCameraFollowedObject();
        cMCameraController_.CMTVUpdater(maxHeadAngle_);
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
        var eulerAngles = finalAngle.eulerAngles;

        // �N���ਤ�׭���b 75 �� -90 ���d��
        if (eulerAngles.x > 180f)
        {
            eulerAngles.x -= 360f;
        }
        eulerAngles.x = Mathf.Clamp(eulerAngles.x, minHeadAngle_, maxHeadAngle_);
        eulerAngles.z = 0;
        // �N�کԨ��׳]�w�^���骺����
        cameraFollowedObject_.transform.eulerAngles = eulerAngles;        
    }
   
}
