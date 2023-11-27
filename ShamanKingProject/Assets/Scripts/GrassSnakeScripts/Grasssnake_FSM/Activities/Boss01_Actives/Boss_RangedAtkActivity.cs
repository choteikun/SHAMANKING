using UnityEngine;

namespace AI.FSM.Activities
{
    [CreateAssetMenu(menuName = "AI/FSM/Activity/Boss_RangedAtkActivity")]
    public class Boss_RangedAtkActivity : Activity
    {
        public override void Enter(BaseStateMachine stateMachine)
        {
            stateMachine.SwitchExternalBehavior((int)FirstBossState.FirstBossState_RANGEDATKMODE);
        }

        public override void Execute(BaseStateMachine stateMachine)
        {
        }

        public override void Exit(BaseStateMachine stateMachine)
        {
        }
    }
}
