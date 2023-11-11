using UnityEngine;

namespace AI.FSM.Activities
{
    [CreateAssetMenu(menuName = "AI/FSM/Activity/IdleActivity")]
    public class IdleActivity_Base : Activity
    { 
        public override void Enter(BaseStateMachine stateMachine)
        {
            //依照依照GhostEnemyState將鬼魂敵人行為樹切換成待機行為樹
            stateMachine.SwitchExternalBehavior((int)GhostEnemyState.GhostEnemy_IDLE);
        }

        public override void Execute(BaseStateMachine stateMachine)
        {
        }

        public override void Exit(BaseStateMachine stateMachine)
        {
        }

    }
}
