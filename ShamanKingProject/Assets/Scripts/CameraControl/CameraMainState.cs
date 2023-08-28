public class CameraMainState : StateBase
{
    public CameraMainState(StageManager m)
    {
        stateManager = m;
    }

    public override void OnEnter()
    {

    }

    public override void OnUpdate()
    {
        rotateMainCameraFollowedObject();
    }
    void rotateMainCameraFollowedObject()
    {
        if (stateManager is CameraControllerStateMachine cameraController)
        {
            var mainCameraEulerAngles = cameraController.CameraControllerView_.FinalQuaternion_.eulerAngles;


            mainCameraEulerAngles = cameraController.CameraControllerView_.ClampMainCameraRotateAngle(mainCameraEulerAngles);


            cameraController.CameraControllerView_.CameraFollowedObject.transform.eulerAngles = mainCameraEulerAngles;

        }
    }
    public override void OnExit()
    {

    }
}
