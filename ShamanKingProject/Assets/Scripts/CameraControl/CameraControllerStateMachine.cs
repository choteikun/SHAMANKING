using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerStateMachine : StageManager
{
    public CameraControllerView CameraControllerView_ { get; private set; }
    protected override void changeAndNewState(string stateName, StageData stageData)
    {
        switch (stateName)
        {
            case "MainGame":
                CurrentState = new CameraMainState(this);
                return;
               
        }

    }
    public override void StageManagerInit()
    {
        TransitionState("MainGame");
    }
    
    public CameraControllerStateMachine(CameraControllerView cameraControllerView)
    {
        CameraControllerView_ = cameraControllerView;
    }
}
