using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTargetState : StateBase
{
    public CameraTargetState(StageManager m)
    {
        stateManager = m;
    }

    public override void OnEnter()
    {
        rotateTargetCameraFollowedObject();
    }

    public override void OnUpdate()
    {
        rotateMainCameraFollowedObject();
    }
    void rotateMainCameraFollowedObject()
    {
        if (stateManager is CameraControllerStateMachine cameraController)
        {
            var mainCameraEulerAngles = cameraController.CameraControllerView_.MainCamQuaternion_.eulerAngles;


            mainCameraEulerAngles = cameraController.CameraControllerView_.ClampMainCameraRotateAngle(mainCameraEulerAngles);


            cameraController.CameraControllerView_.CameraFollowedObject.transform.eulerAngles = mainCameraEulerAngles;

        }
    }
    void rotateTargetCameraFollowedObject()
    {
        if (stateManager is CameraControllerStateMachine cameraController)
        {
            var targetFollowObject = cameraController.CameraControllerView_.TargetCameraFollowedObject_.transform.rotation.eulerAngles;


            var y = cameraController.CameraControllerView_.CameraFollowedObject.transform.eulerAngles.y;
            cameraController.CameraControllerView_.CameraFollowedObject.transform.eulerAngles = new Vector3(targetFollowObject.x, y, targetFollowObject.z);

        }
    }
    public override void OnExit()
    {

    }
}
