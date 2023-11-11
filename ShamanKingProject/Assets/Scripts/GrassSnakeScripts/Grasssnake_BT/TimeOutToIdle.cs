using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("Snake")]
[TaskDescription("如果要用此節點，請在Animator Controller Parameters裡添加一個TimeOutToIdle的Trigger參數")]
public class TimeOutToIdle : Action
{
    public Animator anim;

    [BehaviorDesigner.Runtime.Tasks.Tooltip("過渡到隨機Idle動畫所需要花的時間/秒")]
    public float IdleTimeOut;
    
    //Idle動畫計時器(跳轉至隨機動畫)
    protected float idleTimer;

    //提前Hash進行優化
    readonly int animID_TimeOutToIdle = Animator.StringToHash("TimeOutToIdle");

    public override void OnStart()
	{
        if (!anim)
        {
            anim = GetComponent<Animator>();
        }
    }

    public override TaskStatus OnUpdate()
    {
        idleTimer += Time.deltaTime;
        if (idleTimer >= IdleTimeOut)
        {
            idleTimer = 0f;
            anim.SetTrigger(animID_TimeOutToIdle);
        }
        else
        {
            anim.ResetTrigger(animID_TimeOutToIdle);
        }

        return base.OnUpdate();
    }
}