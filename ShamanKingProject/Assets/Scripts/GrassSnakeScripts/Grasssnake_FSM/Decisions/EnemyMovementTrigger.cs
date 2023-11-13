using BehaviorDesigner.Runtime;
using Gamemanager;
using UnityEngine;

namespace AI.FSM.Decisions
{
    [CreateAssetMenu(menuName = "AI/FSM/Decisions/EnemyMovementTrigger")]
    //該功能是依照行為樹裡的SetEnemyState變量去改變外部的State，若要使用它，請先確保你的BehaviorTree裡的變量包含Int變量 : SetEnemyState
    public class EnemyMovementTrigger : Decision
    {
        int switchStatebyInt;
        public override void Enter(BaseStateMachine stateMachine)
        {
            GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.BT_Event.BT_SwitchStateMessage, getBT_Massage);
        }
        void getBT_Massage(BT_SwitchStateMessage bT_SwitchStateMessage)
        {
            switchStatebyInt = bT_SwitchStateMessage.StateIntType;
        }
        public override bool Decide(BaseStateMachine stateMachine)
        {
            //如果不是空的行為樹
            if(stateMachine.BehaviorTree != null)
            {
                if (switchStatebyInt == 2)
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
