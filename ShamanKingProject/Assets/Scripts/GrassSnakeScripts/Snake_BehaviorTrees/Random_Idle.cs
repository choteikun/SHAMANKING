using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityTransform;

[TaskCategory("Snake")]
[TaskDescription("如果要用此節點，請在Animator Controller Parameters裡添加一個 RandomIdle 的Int參數" +
    "，以及確保進入IdleSM裡的第一個狀態機名稱為Idle無誤")]

public class Random_Idle : Action
{
    //提前Hash進行優化
    readonly int animID_RandomIdle = Animator.StringToHash("RandomIdle");

    public Animator anim;

    [BehaviorDesigner.Runtime.Tasks.Tooltip("設置Idle數量")]
    public int NumberOfIdleStates;

    [BehaviorDesigner.Runtime.Tasks.Tooltip("設置最小隨機跳轉時間，建議5秒以上看起來比較正常")]
    [Range(5, 10)]
    public float minNormTime;
    [BehaviorDesigner.Runtime.Tasks.Tooltip("設置最大隨機跳轉時間")]
    [Range(11, 15)]
    public float maxNormTime;

    //計時Idle播放多久
    protected float IdleTimer;
    //亂數計時器
    protected float randomTimer;


    public override void OnStart()
	{
        if (!anim)
        {
            anim = transform.GetChild(0).GetComponent<Animator>();
        }
        randomTimer = Random.Range(minNormTime, maxNormTime);//一個範圍內的隨機亂數計時器
    }

    public override TaskStatus OnUpdate()
    {
        //Debug.Log("randomIdleTime:" + randomIdleTime/60 + "     randomIdleTimer:" + randomIdleTimer);
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") && !anim.IsInTransition(0))//如果當前狀態是idle且不處於過渡條下
        {
            IdleTimer++;
            if (IdleTimer >= randomTimer * 60)
            {
                anim.SetInteger(animID_RandomIdle, Random.Range(1, NumberOfIdleStates));//設置隨機idle1,2,3,4等...
            }
            else
            {
                anim.SetInteger(animID_RandomIdle, -1);//參數設為-1
            }
        }
        else
        {
            IdleTimer = 0;
        }
        return base.OnUpdate();
    }
}