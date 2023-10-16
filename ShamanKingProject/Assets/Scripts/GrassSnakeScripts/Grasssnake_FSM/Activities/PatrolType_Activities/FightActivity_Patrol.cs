using UnityEngine;

namespace AI.FSM.Activities
{
    [CreateAssetMenu(menuName = "AI/FSM/Activity/FightActivity_Patrol")]
    public class FightActivity_Patrol : Activity
    {

        public override void Enter(BaseStateMachine stateMachine)
        {
            //啟動自動生成隨機時間的殭屍發呆觸發器
            stateMachine.GetComponent<PatrolTypeEnemySupport>().randomTimeToDazeValueTrigger = true;
        }

        public override void Execute(BaseStateMachine stateMachine)
        {
        }

        public override void Exit(BaseStateMachine stateMachine)
        {
        }

    }
}
