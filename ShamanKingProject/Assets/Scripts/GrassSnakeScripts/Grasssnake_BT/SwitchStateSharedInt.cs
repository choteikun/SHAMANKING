using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using Gamemanager;
using UnityEngine;
using TooltipAttribute = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

[TaskCategory("Snake")]
[TaskDescription("藉由SharedInt切換外部行為樹.")]
public class SwitchStateSharedInt : Action
{
    [Tooltip("應停止的行為樹的遊戲物件。 如果為 null，則使用目前行為樹")]
    public SharedGameObject behaviorGameObject;
    [Tooltip("應停止的行為樹組")]
    public SharedInt group;
    [Tooltip("將 SharedInt 設定為的值")]
    public SharedInt targetValue;
    [RequiredField]
    [Tooltip("要設定的 SharedInt ")]
    public SharedInt targetVariable;
    [Tooltip("該行為樹只接收自己發出的訊息")]
    public SharedBool stateMessageChecker;

    private Behavior behavior;

    public override void OnStart()
    {
        var behaviorTrees = GetDefaultGameObject(behaviorGameObject.Value).GetComponents<Behavior>();
        if (behaviorTrees.Length == 1)
        {
            behavior = behaviorTrees[0];
        }
        else if (behaviorTrees.Length > 1)
        {
            for (int i = 0; i < behaviorTrees.Length; ++i)
            {
                if (behaviorTrees[i].Group == group.Value)
                {
                    behavior = behaviorTrees[i];
                    break;
                }
            }
            //如果找不到該群組，則使用第一個行為樹
            if (behavior == null)
            {
                behavior = behaviorTrees[0];
            }
        }
        targetVariable.Value = targetValue.Value;
        stateMessageChecker.Value = true;
        behavior.DisableBehavior();
        //發送訊息給外面FSM的決策系統//要再修改
        GameManager.Instance.BT_Event.Send(new BT_SwitchStateMessage()
        { 
            IntTypeStateOfGhostEnemy = targetValue.Value
        });
        Debug.Log("SwitchOnStart");
    }

    public override void OnReset()
    {
        targetValue = 0;
        targetVariable = 0;
    }
}
