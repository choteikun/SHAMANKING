using System;
using UnityEngine;

namespace AI.FSM
{
    [CreateAssetMenu(menuName = "AI/FSM/Transition")]
    public sealed class Transition : ScriptableObject
    {
        //決策
        public Decision decision;
        public BaseState TrueState;
        public BaseState FalseState;

        public void Execute(BaseStateMachine stateMachine)
        {
            //如果符合決策並且TrueState不是RemainInState的情況下，將現在的state切換成TrueState
            if (decision.Decide(stateMachine) && !(TrueState is RemainInState))
            {
                stateMachine.CurrentState.Exit(stateMachine);
                stateMachine.CurrentState = TrueState;
                stateMachine.CurrentState.Enter(stateMachine);
            }
            //如果FalseState不是RemainInState的情況下，將現在的state切換成FalseState
            else if (!(FalseState is RemainInState))
            {
                stateMachine.CurrentState.Exit(stateMachine);
                stateMachine.CurrentState = FalseState;
                stateMachine.CurrentState.Enter(stateMachine);
            }
        }

        
    }
}
