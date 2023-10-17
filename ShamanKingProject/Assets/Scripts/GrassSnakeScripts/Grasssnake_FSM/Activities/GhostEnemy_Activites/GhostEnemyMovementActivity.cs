using UnityEngine;

namespace AI.FSM.Activities
{
    [CreateAssetMenu(menuName = "AI/FSM/Activity/GhostEnemyMovementActivity")]
    public class GhostEnemyMovementActivity : Activity
    {
        public override void Enter(BaseStateMachine stateMachine)
        {
            stateMachine.transform.GetChild(0).GetComponent<GhostEnemyAnimator>().ResetAnimatorParametersInState_Movement();
        }

        public override void Execute(BaseStateMachine stateMachine)
        {
        }

        public override void Exit(BaseStateMachine stateMachine)
        {
        }

    }
}
