using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAimState : StateBase
{
    public CameraAimState(StageManager m)
    {
        stateManager = m;
    }

    public override void OnEnter()
    {

        rotateAimCameraFollowedObject();
    }

    public override void OnUpdate()
    {
        replaceAimCameraFollowedObject();
    }
    void replaceAimCameraFollowedObject()
    {

        if (stateManager is CameraControllerStateMachine cameraController)
        {
            var aimCameraEulerAngles = cameraController.CameraControllerView_.AimQuaternion.eulerAngles;

            aimCameraEulerAngles = cameraController.CameraControllerView_.ClampAimCameraRotateAngle(aimCameraEulerAngles);

            cameraController.CameraControllerView_.AimCameraFollowedObject_.transform.eulerAngles = aimCameraEulerAngles;

        }
    }
    void rotateAimCameraFollowedObject()
    {
        if (stateManager is CameraControllerStateMachine cameraController)
        {
            var aimCameraEulerAngles = cameraController.CameraControllerView_.MainCamQuaternion_.eulerAngles;

            aimCameraEulerAngles = cameraController.CameraControllerView_.ClampAimCameraRotateAngle(aimCameraEulerAngles);

            cameraController.CameraControllerView_.AimCameraFollowedObject_.transform.eulerAngles = aimCameraEulerAngles;

        }
    }
    public override void OnExit()
    {

    }
}
