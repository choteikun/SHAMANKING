using UnityEngine;

namespace AI.FSM.Activities
{
    [CreateAssetMenu(menuName = "AI/FSM/Activity/WrathPhantom_OffensiveActivity")]
    public class WrathPhantom_OffensiveActivity : Activity
    {
        public override void Enter(BaseStateMachine stateMachine)
        {
            stateMachine.SwitchExternalBehavior((int)WrathPhantomState.WrathPhantomState_OffensiveMode);
        }

        public override void Execute(BaseStateMachine stateMachine)
        {
        }

        public override void Exit(BaseStateMachine stateMachine)
        {
        }
    }
}
