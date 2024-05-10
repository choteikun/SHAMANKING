using UnityEngine;

namespace AI.FSM.Decisions
{
    [CreateAssetMenu(menuName = "AI/FSM/Decisions/WrathPhantom_OffensiveDecision")]
    //�ӥ\��O�̷Ӧ欰��̪�SetEnemyState�ܶq�h���ܥ~����State�A�Y�n�ϥΥ��A�Х��T�O�A��BehaviorTree�̪��ܶq�]�tInt�ܶq : SetEnemyState
    public class WrathPhantom_OffensiveDecision : Decision
    {
        public override bool Decide(BaseStateMachine stateMachine)
        {
            //�p�G���O�Ū��欰��
            if (stateMachine.BehaviorTree != null)
            {
                return stateMachine.GetComponent<WrathPhantomVariables>().WrathPhantomState == WrathPhantomState.WrathPhantomState_OffensiveMode ? true : false;
            }
            else
            {
                return false;
            }
        }
    }
}
