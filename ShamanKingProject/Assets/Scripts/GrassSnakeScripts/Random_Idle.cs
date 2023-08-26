using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Random_Idle : StateMachineBehaviour
{
    [Tooltip("設置Idle數量")]
    public int NumberOfIdleStates;

    [Tooltip("設置最小隨機跳轉時間，建議5秒以上看起來比較正常")]
    [Range(5, 10)]
    public float minNormTime;
    [Tooltip("設置最大隨機跳轉時間")]
    [Range(11, 15)]
    public float maxNormTime;

    //計時Idle播放多久
    protected float IdleTimer;
    //亂數計時器
    protected float randomTimer;

    readonly int m_HashRandomIdle = Animator.StringToHash("RandomIdle");

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //一個範圍內的隨機亂數計時器
        randomTimer = Random.Range(minNormTime, maxNormTime);//一個範圍內的隨機亂數計時器
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Debug.Log("randomIdleTime:" + randomIdleTime/60 + "     randomIdleTimer:" + randomIdleTimer);
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") && !animator.IsInTransition(0))//如果當前狀態是idle且不處於過渡條下
        {

            IdleTimer++;
            if (IdleTimer >= randomTimer * 60)
            {
                animator.SetInteger(m_HashRandomIdle, Random.Range(0, NumberOfIdleStates));//設置隨機idle1,2,3,4等...
            }
            else
            {
                animator.SetInteger(m_HashRandomIdle, -1);//參數設為-1
            }
        }
        else
        {
            IdleTimer = 0;
        }

        ////如果Idle狀態機超出隨機決定的歸一化時間並且尚未轉換，則設置隨機Idle
        //if (stateInfo.normalizedTime > m_RandomNormTime && !animator.IsInTransition(0))
        //{
        //    animator.SetInteger(m_HashRandomIdle, Random.Range(0, numberOfStates));
        //}
    }
}