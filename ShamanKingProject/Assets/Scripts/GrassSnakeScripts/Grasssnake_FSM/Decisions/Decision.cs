using UnityEditor;
using UnityEngine;

namespace AI.FSM
{
    public abstract class Decision : ScriptableObject
    {
        //負責抓取和處理傳入事件
        //它必須向有限狀態機輸出一個訊號（布林值），無論它是否應該轉換到新狀態
        public abstract bool Decide(BaseStateMachine stateMachine);
    }
}
