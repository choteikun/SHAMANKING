using UnityEngine;

namespace AI.FSM.Activities
{
    [CreateAssetMenu(menuName = "AI/FSM/Activity/GhostEnemyFightActivity")]
    public class GhostEnemyFightActivity : Activity
    {
        public override void Enter(BaseStateMachine stateMachine)
        {
            stateMachine.transform.GetChild(0).GetComponent<GhostEnemyAnimator>().SetEnemyState(3);
        }

        public override void Execute(BaseStateMachine stateMachine)
        {
        }

        public override void Exit(BaseStateMachine stateMachine)
        {
        }

    }
}
