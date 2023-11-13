using UnityEngine;

namespace AI.FSM.Activities
{
    [CreateAssetMenu(menuName = "AI/FSM/Activity/GhostEnemyMovementActivity")]
    public class GhostEnemyMovementActivity : Activity
    {
        public override void Enter(BaseStateMachine stateMachine)
        {
            Debug.Log("MOVEMENT_ACTIVITY");
            stateMachine.transform.GetChild(0).GetComponent<GhostEnemyAnimator>().SetEnemyState(2);
        }

        public override void Execute(BaseStateMachine stateMachine)
        {
        }

        public override void Exit(BaseStateMachine stateMachine)
        {
        }

    }
}
