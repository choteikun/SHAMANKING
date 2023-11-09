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
    void replaceMainCameraFollowedObject()
    {

        if (stateManager is CameraControllerStateMachine cameraController)
        {
            var aimCameraEulerAngles = cameraController.CameraControllerView_.AimQuaternion.eulerAngles;
            if (aimCameraEulerAngles.x > 180f)
            {
                aimCameraEulerAngles.x -= 360f;
            }
            var result = new UnityEngine.Vector3(Mathf.Clamp(aimCameraEulerAngles.x, -20f, 20f), aimCameraEulerAngles.y, aimCameraEulerAngles.z);
            //aimCameraEulerAngles = cameraController.CameraControllerView_.ClampAimCameraRotateAngle(result);
            cameraController.CameraControllerView_.CameraFollowedObject.transform.eulerAngles = result;

        }
    }
    public override void OnExit()
    {
        replaceMainCameraFollowedObject();
    }
}
