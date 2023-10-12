using UnityEngine;

namespace AI.FSM.Activities
{
    [CreateAssetMenu(menuName = "AI/FSM/Activity/FightActivity")]
    public class FightActivity : Activity
    {
        public override void Enter(BaseStateMachine stateMachine)
        {
            stateMachine.GetComponent<EnemyBehaviorTreeSupport>().switchExternalBehavior((int)EnemyState.ENEMY_FIGHT);
        }

        public override void Execute(BaseStateMachine stateMachine)
        {
        }

        public override void Exit(BaseStateMachine stateMachine)
        {
        }

    }
}
