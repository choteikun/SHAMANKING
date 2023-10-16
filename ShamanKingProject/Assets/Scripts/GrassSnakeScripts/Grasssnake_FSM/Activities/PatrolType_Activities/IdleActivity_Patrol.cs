using UnityEngine;

namespace AI.FSM.Activities
{
    [CreateAssetMenu(menuName = "AI/FSM/Activity/IdleActivity_Patrol")]
    public class IdleActivity_Patrol : Activity
    {
        public override void Enter(BaseStateMachine stateMachine)
        {
            //啟動自動生成隨機時間的殭屍移動觸發器
            stateMachine.GetComponent<PatrolTypeEnemySupport>().randomTimeToMoveValueTrigger = true;
        }

        public override void Execute(BaseStateMachine stateMachine)
        {
        }

        public override void Exit(BaseStateMachine stateMachine)
        {
        }

    }
}
