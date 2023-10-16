using BehaviorDesigner.Runtime;
using System.ComponentModel;
using UnityEngine;

namespace AI.FSM.Decisions
{
    [CreateAssetMenu(menuName = "AI/FSM/Decisions/EnemyMovementTrigger")]
    //該功能是依照行為樹裡的SetEnemyState變量去改變外部的State，若要使用它，請先確保你的BehaviorTree裡的變量包含Int變量 : SetEnemyState
    public class EnemyMovementTrigger : Decision
    {
        public override bool Decide(BaseStateMachine stateMachine)
        {
            var enemyFightTrigger = (SharedInt)stateMachine.GetComponent<BehaviorTree>().GetVariable("SetEnemyState");
            if(enemyFightTrigger.Value == 2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
