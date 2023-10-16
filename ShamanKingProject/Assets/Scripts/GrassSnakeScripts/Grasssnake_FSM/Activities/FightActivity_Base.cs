using UnityEngine;

namespace AI.FSM.Activities
{
    [CreateAssetMenu(menuName = "AI/FSM/Activity/FightActivity")]
    public class FightActivity_Base : Activity
    {
        public override void Enter(BaseStateMachine stateMachine)
        {
            stateMachine.GetComponent<EnemyBehaviorTreeSupport>().enemyBehaviorTreeState = EnemyBehaviorTreeState.ENEMY_FIGHT;
            stateMachine.GetComponent<EnemyBehaviorTreeSupport>().switchExternalBehavior((int)EnemyBehaviorTreeState.ENEMY_FIGHT);
        }

        public override void Execute(BaseStateMachine stateMachine)
        {
        }

        public override void Exit(BaseStateMachine stateMachine)
        {
        }

    }
}
