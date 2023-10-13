using System.Collections.Generic;
using UnityEngine;

namespace AI.FSM
{
    
    [CreateAssetMenu(menuName = "AI/FSM/State")]
    //擴展BaseState的同時並使用密封方法sealed class防止非有意的派生。
    public sealed class State : BaseState
    {
        //讓敵人按照他所處的狀態進行多種不同的活動。
        public List<Activity> Activities = new List<Activity>();
        //轉換的條件與每個state相關聯，根據預先定義的條件過渡到多種state，如果捕獲的事件不滿足轉換的要求，敵人將保持相同的state
        public List<Transition> Transitions = new List<Transition>();

        public override void Enter(BaseStateMachine machine)
        {
            foreach (var activity in Activities)
                activity.Enter(machine);
        }

        public override void Execute(BaseStateMachine machine)
        {
            foreach (var activity in Activities)
                activity.Execute(machine);

            foreach (var transition in Transitions)
                transition.Execute(machine);
        }

        public override void Exit(BaseStateMachine machine)
        {
            foreach (var activity in Activities)
                activity.Exit(machine);
        }
    }
}
