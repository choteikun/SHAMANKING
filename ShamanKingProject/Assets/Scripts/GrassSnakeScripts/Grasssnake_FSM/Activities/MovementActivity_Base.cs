using UnityEngine;

namespace AI.FSM.Activities
{
    [CreateAssetMenu(menuName = "AI/FSM/Activity/MovementActivity")]
    public class MovementActivity_Base : Activity
    {
        public override void Enter(BaseStateMachine stateMachine)
        {
            //只有在MoveState裡才會進行Movement活動，進入Movement活動時將EnemyBehaviorTreeState改為EnemyBehaviorTreeState.ENEMY_MOVEMENT
            stateMachine.GetComponent<EnemyBehaviorTreeSupport>().enemyBehaviorTreeState = EnemyBehaviorTreeState.ENEMY_MOVEMENT;
            //依照EnemyBehaviorTreeState將敵人行為樹切換成移動行為樹
            stateMachine.GetComponent<EnemyBehaviorTreeSupport>().switchExternalBehavior((int)EnemyBehaviorTreeState.ENEMY_MOVEMENT);
            //在這一行的範例還可以獲取Enemy身上的相關組件，但由於Animator可能會到很複雜，所以採用別種方式進行管理
            //stateMachine.GetComponent<Animator>().SetBool("Walk", True);
        }

        public override void Execute(BaseStateMachine stateMachine)
        {
        }

        public override void Exit(BaseStateMachine stateMachine)
        {
        }

    }
}
