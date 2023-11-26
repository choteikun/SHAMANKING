using UnityEngine;
using Gamemanager;
using UnityEditorInternal;

namespace AI.FSM.Decisions
{
    [CreateAssetMenu(menuName = "AI/FSM/Decisions/EnemyIdleTrigger")]
    //該功能是依照行為樹裡的SetEnemyState變量去改變外部的State，若要使用它，請先確保你的BehaviorTree裡的變量包含Int變量 : SetEnemyState
    public class EnemyIdleTrigger : Decision
    {
        //int switchStatebyInt;
        GhostEnemyVariables ghostEnemyVariables;
        public override void Enter(BaseStateMachine stateMachine)
        {
            //ghostEnemyVariables = stateMachine.GetComponent<GhostEnemyVariables>();
            //switchStatebyInt = 0;
            //stateMachine.GetComponent<GhostEnemyVariables>().ghostEnemyState.Subscribe(_ => { switchStatebyInt = (int)stateMachine.GetComponent<GhostEnemyVariables>().ghostEnemyState.Value;Debug.Log("switchStatebyInt" + switchStatebyInt); }).Dispose();
            //GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.BT_Event.BT_SwitchStateMessage, getBT_Massage);
        }
        //void getBT_Massage(BT_SwitchStateMessage  bT_SwitchStateMessage)
        //{
        //    switchStatebyInt = bT_SwitchStateMessage.StateIntType;
        //    Debug.Log("switchStatebyInt" + switchStatebyInt);
        //}
        public override bool Decide(BaseStateMachine stateMachine)
        {
            //如果不是空的行為樹
            if (stateMachine.BehaviorTree != null)
            {
                //獲取行為樹裡的變量
                //var btState = (SharedInt)stateMachine.BehaviorTree.GetVariable("SetEnemyState");
                //依變量判斷True / False
                //if (btState.Value == 1)
                //{
                //    return true;
                //}
                //else
                //{
                //    return false;
                //}
                //if(switchStatebyInt == 1)
                //{
                //    return true;
                //}
                //else
                //{
                //    return false;
                //}
                if (stateMachine.GetComponent<GhostEnemyVariables>().ghostEnemyState == GhostEnemyState.GhostEnemy_IDLE)
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
