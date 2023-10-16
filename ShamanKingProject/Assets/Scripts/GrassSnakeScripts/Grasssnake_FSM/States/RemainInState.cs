using UnityEngine;

namespace AI.FSM
{
    [CreateAssetMenu(menuName = "AI/FSM/Remain In State", fileName = "RemainInState")]
    //當傳入事件不符合轉換條件時，我們希望 AI 堅持相同的狀態
    //仍然需要實現它，以便我們可以從中創建。這樣就可以將它設定在Transition裡的TrueState或FalseState。
    public sealed class RemainInState : BaseState
    {
    }
}
