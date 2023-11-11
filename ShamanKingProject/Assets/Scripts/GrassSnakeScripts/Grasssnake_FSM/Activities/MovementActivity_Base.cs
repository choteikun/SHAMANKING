using UnityEngine;

namespace AI.FSM.Activities
{
    [CreateAssetMenu(menuName = "AI/FSM/Activity/MovementActivity")]
    public class MovementActivity_Base : Activity
    {
        public override void Enter(BaseStateMachine stateMachine)
        {
            //依照依照GhostEnemyState將鬼魂敵人行為樹切換成移動行為樹
            stateMachine.SwitchExternalBehavior((int)GhostEnemyState.GhostEnemy_MOVEMENT);
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
