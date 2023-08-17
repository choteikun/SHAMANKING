using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamemanager;
using UnityEditor.Rendering;
using UnityEngine.Android;

public class CameraControllerView : MonoBehaviour
{
    [SerializeField]
    Vector2 nowRotateGamepadValue_ = new Vector2();
    [SerializeField]
    GameObject cameraFollowedObject_;
    [SerializeField]
    float rotateSpeed_ = 20f;

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
        var rotateAngle = rotateValue_ * Time.deltaTime * rotateSpeed_ + cameraFollowedObject_.transform.rotation.eulerAngles;
        Debug.Log(rotateAngle);
        if (rotateAngle.x >= 75 && rotateAngle.x < 90)
        {
            rotateAngle.x = 75;
        }
        var rotateQuaternion = Quaternion.Euler(rotateAngle);
        cameraFollowedObject_.transform.rotation = rotateQuaternion;
    }
}
