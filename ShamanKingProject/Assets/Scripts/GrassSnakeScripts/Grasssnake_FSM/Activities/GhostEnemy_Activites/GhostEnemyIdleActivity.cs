using UnityEngine;

namespace AI.FSM.Activities
{
    [CreateAssetMenu(menuName = "AI/FSM/Activity/GhostEnemyIdleActivity")]
    public class GhostEnemyIdleActivity : Activity
    {
        public override void Enter(BaseStateMachine stateMachine)
        {
            stateMachine.transform.GetChild(0).GetComponent<GhostEnemyAnimator>().ResetAnimatorParametersInState_Idle();
        }

        public override void Execute(BaseStateMachine stateMachine)
        {
        }

        public override void Exit(BaseStateMachine stateMachine)
        {
        }

    }
}
