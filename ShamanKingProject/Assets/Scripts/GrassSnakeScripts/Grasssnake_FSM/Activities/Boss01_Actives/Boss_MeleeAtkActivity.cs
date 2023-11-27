using UnityEngine;

namespace AI.FSM.Activities
{
    [CreateAssetMenu(menuName = "AI/FSM/Activity/Boss_MeleeAtkActivity")]
    public class Boss_MeleeAtkActivity : Activity
    {
        public override void Enter(BaseStateMachine stateMachine)
        {
            stateMachine.SwitchExternalBehavior((int)FirstBossState.FirstBossState_MELEEATKMODE);
        }

        public override void Execute(BaseStateMachine stateMachine)
        {
        }

        public override void Exit(BaseStateMachine stateMachine)
        {
        }
    }
}
