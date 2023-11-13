using UnityEngine;

namespace AI.FSM.Activities
{
    [CreateAssetMenu(menuName = "AI/FSM/Activity/FightActivity")]
    public class FightActivity_Base : Activity
    {
        public override void Enter(BaseStateMachine stateMachine)
        {
            Debug.Log("FIGHT_ACTIVITY");
            //依照GhostEnemyState將鬼魂敵人行為樹切換成戰鬥行為樹
            stateMachine.SwitchExternalBehavior((int)GhostEnemyState.GhostEnemy_FIGHT);

        }

        public override void Execute(BaseStateMachine stateMachine)
        {
        }

        public override void Exit(BaseStateMachine stateMachine)
        {
        }
    }
}
