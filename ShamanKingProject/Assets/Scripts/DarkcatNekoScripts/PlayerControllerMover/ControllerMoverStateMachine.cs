using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerMoverStateMachine : StageManager
{
    public PlayerControllerMover PlayerControllerMover { get; private set; }
    protected override void changeAndNewState(string stateName, StageData stageData)
    {
        switch (stateName)
        {
            case "MainGame":
               CurrentState = new MainGameMoverState(this);
                return;
            case "Aim":
                CurrentState = new AimMoverState(this);
                return;
            case "Target":
                CurrentState = new TargetMoverState(this);
                return;
        }

    }
    public override void StageManagerInit()
    {
        TransitionState("MainGame");
    }
    public ControllerMoverStateMachine(PlayerControllerMover cameraControllerView)
    {
        PlayerControllerMover = cameraControllerView;
    }
}
