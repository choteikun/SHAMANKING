public class CameraMainState : StateBase
{
    public CameraMainState(StageManager m)
    {
        stateManager = m;
    }

    public override void OnEnter()
    {
        replaceMainCameraFollowedObject();
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

            aimCameraEulerAngles = cameraController.CameraControllerView_.ClampAimCameraRotateAngle(aimCameraEulerAngles);

            cameraController.CameraControllerView_.CameraFollowedObject.transform.eulerAngles = aimCameraEulerAngles;

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
