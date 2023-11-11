using UnityEngine;
using BehaviorDesigner.Runtime;

namespace AI.FSM
{
    public class BaseStateMachine : MonoBehaviour
    {
        [SerializeField, Tooltip("初始state")]
        private BaseState initialState;

        [Tooltip("行為樹")]
        public BehaviorTree BehaviorTree;
        [SerializeField, Tooltip("外部行為樹")]
        private ExternalBehavior[] externalBehaviorTrees;

        //現在的state
        public BaseState CurrentState { get; set; }


        private void Awake()
        {
            CurrentState = initialState;
        }

        private void Start()
        {
            CurrentState.Enter(this);
        }

        private void Update()
        {
            CurrentState.Execute(this);
        }

        #region - 切換外部行為樹 -
        public void SwitchExternalBehavior(int externalTrees)
        {
            if (externalBehaviorTrees[externalTrees] != null)
            {
                BehaviorTree.ExternalBehavior = externalBehaviorTrees[externalTrees];
                BehaviorTree.EnableBehavior();
            }
        }
        public void DisableBehaviorTree()
        {
            BehaviorTree.DisableBehavior();
        }
        #endregion
    }


}
