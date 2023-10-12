using UnityEngine;

namespace AI.FSM
{
    [CreateAssetMenu(menuName = "AI/FSM/Transition")]
    public sealed class Transition : ScriptableObject
    {
        public Decision decision;
        public BaseState TrueState;
        public BaseState FalseState;

        public void Execute(BaseStateMachine stateMachine)
        {
            //...
        }
    }
}
