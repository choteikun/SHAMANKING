using UnityEngine;

namespace AI.FSM
{
    public class BaseState : ScriptableObject
    {
        //當state進入時執行一次。
        public virtual void Enter(BaseStateMachine machine) { }
        //當state處於該狀態時，會不斷被呼叫。
        public virtual void Execute(BaseStateMachine machine) { }
        //當state離開狀態時運作。
        public virtual void Exit(BaseStateMachine machine) { }
    }
}
