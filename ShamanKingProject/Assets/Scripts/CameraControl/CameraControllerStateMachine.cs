using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerStateMachine : StageManager
{
    public CameraControllerView CameraControllerView_ { get; private set; }
    public string CameraState { get; private set; } = "init";
    protected override void changeAndNewState(string stateName, StageData stageData)
    {
           
        CameraState = stateName;
        switch (stateName)
        {
            case "MainGame":
                CurrentState = new CameraMainState(this);             
                return;
            case "Aim":
                CurrentState = new CameraAimState(this);
                return;
            case "Target":
                CurrentState = new CameraTargetState(this);
                return;

        }

    }
    public override void TransitionState(string stateName, StageData stageData)
    {
        //Debug.Log(CameraState + "::::::" + stateName);
        if (CameraState == stateName) return;//欠債 暫時不會讓他跳兩次攝影機
        if (CurrentState != null)
        {
            CurrentState.OnExit();
        }
        changeAndNewState(stateName, stageData);

        CurrentState.OnEnter();
    }
    public override void TransitionState(string stateName)
    {
        var stagedata = new StageData();
        //Debug.Log(CameraState + "::::::" + stateName);
        if (CameraState == stateName) return;//欠債 暫時不會讓他跳兩次攝影機
        if (CurrentState != null)
        {
            CurrentState.OnExit();
        }

        changeAndNewState(stateName, stagedata);


        CurrentState.OnEnter();



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
