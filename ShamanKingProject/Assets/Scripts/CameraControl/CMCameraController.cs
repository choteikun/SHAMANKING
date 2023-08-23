using Cinemachine;
using UnityEngine;

[System.Serializable]
public class CMCameraController
{
    [SerializeField]
    float rotateAngle_ = 0;

    [SerializeField]
    CinemachineVirtualCamera virtualCamera_;

    [SerializeField]
    GameObject lookedObject;


    [Header("��v����m�Ѽ�")]
    [Tooltip("��v����¦��m �w�]2.7")]
    [SerializeField]
    float basicCameraDistance_ = 2.7f;

    [Tooltip("��v���Y���W���Z�� �w�]��3")]
    [SerializeField]
    float upperCameraDistance_ = 3;

    Cinemachine3rdPersonFollow thirdPersonFollow_;
    CinemachineComposer composer_;

    /// <summary>
    /// do at awake
    /// </summary>
    public void SetVirtualCamera()
    {
        thirdPersonFollow_ = virtualCamera_.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
        composer_ = virtualCamera_.GetCinemachineComponent<CinemachineComposer>();

    }
    public void CMTVUpdater(float maxHeadAngle)
    {
        rotateAngle_ = lookedObject.transform.rotation.eulerAngles.x;
        if (rotateAngle_ == 0)
        {
            return;
        }
        if (rotateAngle_ >= 0 && rotateAngle_ <= maxHeadAngle)
        {
            thirdPersonFollow_.CameraDistance = basicCameraDistance_ + rotateAngle_ * upperCameraDistance_ / maxHeadAngle;
            composer_.m_ScreenY = 1 - rotateAngle_ * 0.5f / maxHeadAngle;

        }
        else if (rotateAngle_ >= 270 && rotateAngle_ <= 360)//�V�U���ʥd�b90�פ� ���i�W�L�ε���90
        {
            thirdPersonFollow_.CameraDistance = basicCameraDistance_ - (360 - rotateAngle_) * basicCameraDistance_ / 90;
            thirdPersonFollow_.VerticalArmLength = 0.01f + (rotateAngle_ - 270) * 0.99f / 90;
            thirdPersonFollow_.ShoulderOffset.y = -0.1f + (rotateAngle_ - 270) * 0.1f / 90;
        }
    }
    

}
