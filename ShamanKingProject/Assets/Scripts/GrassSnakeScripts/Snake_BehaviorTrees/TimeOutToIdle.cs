using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("Snake")]
public class TimeOutToIdle : Action
{
    public Animator anim;

    [BehaviorDesigner.Runtime.Tasks.Tooltip("過渡到隨機Idle動畫所需要花的時間")]
    public float IdleTimeOut;
    
    //Idle動畫計時器(跳轉至隨機動畫)
    protected float idleTimer;

    //提前Hash進行優化
    readonly int h_TimeOutToIdle = Animator.StringToHash("TimeOutToIdle");

    public override void OnStart()
	{
        anim = GetComponent<Animator>();
    }

    public override TaskStatus OnUpdate()
    {
        idleTimer += Time.deltaTime;
        if (idleTimer >= IdleTimeOut)
        {
            idleTimer = 0f;
            anim.SetTrigger(h_TimeOutToIdle);
        }
        else
        {
            anim.ResetTrigger(h_TimeOutToIdle);
        }

        return base.OnUpdate();
    }
}