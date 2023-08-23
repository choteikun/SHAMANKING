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

    [Tooltip("建議角度為75度")]
    [Header("最大角度")]
    [SerializeField]
    int maxHeadAngle_ = 75;

    [Tooltip("建議角度為-60度,請勿小於-89度")]
    [Header("最小角度")]    
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
        // 根據 rotateValue_ 的 x 和 y 分量來計算旋轉角度
        float rotationX = sensitiveRotateValue.x;
        float rotationY = sensitiveRotateValue.y;


        var finalAngle = cameraFollowedObject_.transform.rotation;
        // 根據計算出的旋轉角度來旋轉物體
       finalAngle *= Quaternion.Euler(rotationX, rotationY, 0f);

        // 取得物體的歐拉角度
        var eulerAngles = finalAngle.eulerAngles;

        // 將旋轉角度限制在 75 到 -90 的範圍內
        if (eulerAngles.x > 180f)
        {
            eulerAngles.x -= 360f;
        }
        eulerAngles.x = Mathf.Clamp(eulerAngles.x, minHeadAngle_, maxHeadAngle_);
        eulerAngles.z = 0;
        // 將歐拉角度設定回物體的旋轉
        cameraFollowedObject_.transform.eulerAngles = eulerAngles;        
    }
   
}
