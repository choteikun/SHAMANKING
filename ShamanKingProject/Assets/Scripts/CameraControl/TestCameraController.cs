using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCameraController : MonoBehaviour
{
    [SerializeField]
    float rotateAngle_ = 0;

    [SerializeField] 
    CinemachineVirtualCamera virtualCamera_;

    [SerializeField]
    GameObject lookedObject;

    Cinemachine3rdPersonFollow thirdPersonFollow_;
    CinemachineComposer composer_;

    

    private void Start()
    {
        thirdPersonFollow_ = virtualCamera_.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
        composer_ = virtualCamera_.GetCinemachineComponent<CinemachineComposer>();
    }

    private void Update()
    {
        rotateAngle_ = lookedObject.transform.rotation.eulerAngles.x;
        if (rotateAngle_ == 0)
        {
            return;
        }
        if (rotateAngle_ >= 0 && rotateAngle_ <= 75)
        {
            thirdPersonFollow_.CameraDistance = 2.7f + rotateAngle_ * 3f / 75;
            composer_.m_ScreenY = 1 - rotateAngle_ * 0.5f / 75;

        }
        else if (rotateAngle_ >= 270 && rotateAngle_ <= 360)
        {
            thirdPersonFollow_.CameraDistance = 2.7f - (360 - rotateAngle_) * 2.7f / 90;
            //if (rotateAngle_ >= 270 && rotateAngle_ <= 360)
            //{
                thirdPersonFollow_.VerticalArmLength = 0.01f + (rotateAngle_ - 270) * 0.99f / 90;
                thirdPersonFollow_.ShoulderOffset.y =-0.1f+ (rotateAngle_ - 270) * 0.1f /90;
            //}
        }
    }

}
