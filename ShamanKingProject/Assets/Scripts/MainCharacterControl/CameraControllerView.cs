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

    Vector3 rotateValue_ => new Vector3(nowRotateGamepadValue_.y, nowRotateGamepadValue_.x, 0);
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
    }

    void rotateCameraFollowedObject()
    {
        var sensitiveRotateValue = new Vector3(rotateValue_.x * Time.deltaTime * rotateSpeed_Y_, rotateValue_.y * Time.deltaTime * rotateSpeed_X_, 0);
        var rotateAngle = sensitiveRotateValue + cameraFollowedObject_.transform.rotation.eulerAngles;
        if (rotateAngle.x >= 75 && rotateAngle.x < 90)
        {
            rotateAngle.x = 75;
        }
        var rotateQuaternion = Quaternion.Euler(rotateAngle);
        cameraFollowedObject_.transform.rotation = rotateQuaternion;
    }
}
