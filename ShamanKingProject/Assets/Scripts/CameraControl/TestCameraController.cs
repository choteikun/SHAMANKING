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
        if (Input.GetKey(KeyCode.I))
        {
            lookedObject.transform.Rotate(new Vector3(1, 0, 0)*Time.deltaTime * 50f);
        }
        if (Input.GetKey(KeyCode.K))
        {
            lookedObject.transform.Rotate(new Vector3(-1, 0, 0) * Time.deltaTime*50f);
        }
        rotateAngle_ = lookedObject.transform.rotation.eulerAngles.x;
        if (rotateAngle_ == 0)
        {
            return;
        }
        thirdPersonFollow_.CameraDistance = 2.7f + rotateAngle_ * 3f / 75;
        composer_.m_ScreenY = 1 - rotateAngle_ * 0.5f / 75;
    }

}
