using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimMoverState : StateBase
{
    public AimMoverState(StageManager m)
    {
        stateManager = m;
    }

    public override void OnEnter()
    {
        
    }    

    public override void OnUpdate()
    {
        if(stateManager is ControllerMoverStateMachine controllerMSM)
        {
            controllerMSM.PlayerControllerMover.AimPointUpdate();
        }
    }

    public override void OnExit()
    {

    }
}
