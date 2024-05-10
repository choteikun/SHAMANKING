using UnityEngine;

namespace AI.FSM.Activities
{
    [CreateAssetMenu(menuName = "AI/FSM/Activity/WrathPhantom_ThinkingActivity")]
    public class WrathPhantom_ThinkingActivity : Activity
    {
        public override void Enter(BaseStateMachine stateMachine)
        {
            stateMachine.SwitchExternalBehavior((int)WrathPhantomState.WrathPhantomState_THINKING);
        }

        public override void Execute(BaseStateMachine stateMachine)
        {
        }

        public override void Exit(BaseStateMachine stateMachine)
        {
        }
    }
}

