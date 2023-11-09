using UnityEngine;

public class CameraMainState : StateBase
{
    public CameraMainState(StageManager m)
    {
        stateManager = m;
    }

    public override void OnEnter()
    {
       // replaceMainCameraFollowedObject();
        //Debug.Log("Back!");
    }

    public override void OnUpdate()
    {
        rotateMainCameraFollowedObject();
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
    void rotateMainCameraFollowedObject()
    {
        if (stateManager is CameraControllerStateMachine cameraController)
        {
            var mainCameraEulerAngles = cameraController.CameraControllerView_.MainCamQuaternion_.eulerAngles;


            mainCameraEulerAngles = cameraController.CameraControllerView_.ClampMainCameraRotateAngle(mainCameraEulerAngles);


            cameraController.CameraControllerView_.CameraFollowedObject.transform.eulerAngles = mainCameraEulerAngles;

        }
    }
    public override void OnExit()
    {

    }
}
