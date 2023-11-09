using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMoverState : StateBase
{
    public TargetMoverState(StageManager m)
    {
        stateManager = m;
    }

    public override void OnEnter()
    {
       
    }

    public override void OnUpdate()
    {
        if (stateManager is ControllerMoverStateMachine controllerMSM)
        {           
            controllerMSM.PlayerControllerMover.TargetPointUpdate();
        }
    }

    public override void OnExit()
    {
        if (stateManager is ControllerMoverStateMachine controllerMSM)
        {
            
        }
    }
}
