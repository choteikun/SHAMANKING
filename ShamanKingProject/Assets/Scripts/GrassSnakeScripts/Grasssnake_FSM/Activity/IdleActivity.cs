using UnityEngine;

namespace AI.FSM.Activities
{
    [CreateAssetMenu(menuName = "AI/FSM/Activity/IdleActivity")]
    public class IdleActivity : Activity
    {
        public override void Enter(BaseStateMachine stateMachine)
        {
            stateMachine.GetComponent<EnemyBehaviorTreeSupport>().enemyBehaviorTreeState = EnemyBehaviorTreeState.ENEMY_IDLE;
            stateMachine.GetComponent<EnemyBehaviorTreeSupport>().switchExternalBehavior((int)EnemyBehaviorTreeState.ENEMY_IDLE);
        }

        public override void Execute(BaseStateMachine stateMachine)
        {
        }

        public override void Exit(BaseStateMachine stateMachine)
        {
        }

    }
}
