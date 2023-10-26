using UnityEngine;

namespace AI.FSM
{
    public class BaseStateMachine : MonoBehaviour
    {
        //初始state
        [SerializeField] private BaseState _initialState;
        //現在的state
        //public BaseState CurrentState { get; set; }
        public BaseState CurrentState;

        private void Awake()
        {
            CurrentState = _initialState;
        }

        private void Start()
        {
            CurrentState.Enter(this);
        }

        private void Update()
        {
            CurrentState.Execute(this);
        }
    }
}
