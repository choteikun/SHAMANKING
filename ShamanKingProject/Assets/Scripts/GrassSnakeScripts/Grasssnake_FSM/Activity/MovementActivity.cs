using UnityEngine;

namespace AI.FSM.Activities
{
    [CreateAssetMenu(menuName = "AI/FSM/Activity/MovementActivity")]
    public class MovementActivity : Activity
    {
        public override void Enter(BaseStateMachine stateMachine)
        {
            stateMachine.GetComponent<EnemyBehaviorTreeSupport>().enemyState = EnemyState.ENEMY_MOVEMENT;
            stateMachine.GetComponent<EnemyBehaviorTreeSupport>().switchExternalBehavior((int)EnemyState.ENEMY_MOVEMENT);
        }

        public override void Execute(BaseStateMachine stateMachine)
        {
        }

        public override void Exit(BaseStateMachine stateMachine)
        {
        }

    }
}
