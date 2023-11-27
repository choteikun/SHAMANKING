using UnityEngine;

namespace AI.FSM.Activities
{
    [CreateAssetMenu(menuName = "AI/FSM/Activity/Boss_ThinkingActivity")]
    public class Boss_ThinkingActivity : Activity
    {
        public override void Enter(BaseStateMachine stateMachine)
        {
            stateMachine.SwitchExternalBehavior((int)FirstBossState.FirstBossState_THINKING);
        }

        public override void Execute(BaseStateMachine stateMachine)
        {
        }

        public override void Exit(BaseStateMachine stateMachine)
        {
        }
    }
}
