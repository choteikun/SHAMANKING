using UnityEngine;

namespace AI.FSM.Decisions
{
    [CreateAssetMenu(menuName = "AI/FSM/Decisions/EnemyMovementTrigger")]
    //該功能是依照行為樹裡的SetEnemyState變量去改變外部的State，若要使用它，請先確保你的BehaviorTree裡的變量包含Int變量 : SetEnemyState
    public class EnemyMovementTrigger : Decision
    {
        GhostEnemyVariables ghostEnemyVariables;
        public override void Enter(BaseStateMachine stateMachine)
        {
            ghostEnemyVariables = stateMachine.GetComponent<GhostEnemyVariables>();
        }
        public override bool Decide(BaseStateMachine stateMachine)
        {
            //如果不是空的行為樹
            if (stateMachine.BehaviorTree != null)
            {
                if (ghostEnemyVariables.ghostEnemyState == GhostEnemyState.GhostEnemy_MOVEMENT)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
