using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimMoverState : StateBase
{
    private float player_speed_;
    private float aimSpeed_ = 1.5f;
    public AimMoverState(StageManager m)
    {
        stateManager = m;
    }

    public override void OnEnter()
    {
        if (stateManager is ControllerMoverStateMachine controllerMSM)
        {
            player_speed_ = controllerMSM.PlayerControllerMover.MoveSpeed;
            controllerMSM.PlayerControllerMover.MoveSpeed = aimSpeed_;
        }
    }    

    public override void OnUpdate()
    {
        //if(stateManager is ControllerMoverStateMachine controllerMSM)
        //{
        //    controllerMSM.PlayerControllerMover.AimPointUpdate();
        //}
    }

    public override void OnExit()
    {
        if (stateManager is ControllerMoverStateMachine controllerMSM)
        {
            controllerMSM.PlayerControllerMover.MoveSpeed = player_speed_;
        }
    }
}
