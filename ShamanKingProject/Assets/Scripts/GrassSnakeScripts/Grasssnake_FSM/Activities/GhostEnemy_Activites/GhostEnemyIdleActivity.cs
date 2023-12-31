using UnityEngine;

namespace AI.FSM.Activities
{
    [CreateAssetMenu(menuName = "AI/FSM/Activity/GhostEnemyIdleActivity")]
    public class GhostEnemyIdleActivity : Activity
    {
        public override void Enter(BaseStateMachine stateMachine)
        {
            Debug.Log("IDLE_ACTIVITY");
            stateMachine.transform.GetChild(0).GetComponent<GhostEnemyAnimator>().SetEnemyState(1);
        }

        public override void Execute(BaseStateMachine stateMachine)
        {
        }

        public override void Exit(BaseStateMachine stateMachine)
        {
        }

    }
}
