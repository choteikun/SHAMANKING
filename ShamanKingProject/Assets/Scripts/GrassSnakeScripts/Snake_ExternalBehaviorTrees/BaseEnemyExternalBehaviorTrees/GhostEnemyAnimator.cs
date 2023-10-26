using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostEnemyAnimator : MonoBehaviour
{
    #region 提前Hash進行優化
    readonly int animID_Idle = Animator.StringToHash("Idle");

    readonly int animID_Walk = Animator.StringToHash("Walk");

    readonly int animID_Run = Animator.StringToHash("Run");

    readonly int animID_Warn = Animator.StringToHash("Warn");

    readonly int animID_EnemyState = Animator.StringToHash("EnemyState");
    #endregion

    Animator anim;
    EnemyBehaviorTreeSupport enemyBehaviorTreeSupport;

    void Awake()
    {
        anim = GetComponent<Animator>();
        enemyBehaviorTreeSupport = GetComponentInParent<EnemyBehaviorTreeSupport>();
    }
    public void ResetAnimatorParametersInState_Idle()
    {
        anim.SetInteger(animID_EnemyState, 1);
        ResetAllAnim();
    }
    public void ResetAnimatorParametersInState_Movement()
    {
        anim.SetInteger(animID_EnemyState, 2);
        ResetAllAnim();
    }
    public void ResetAnimatorParametersInState_Fight()
    {
        anim.SetInteger(animID_EnemyState, 3);
        ResetAllAnim();
    }
    public void ResetAllAnim()
    {
        anim.SetBool(animID_Walk, false);
        anim.SetBool(animID_Run, false);
        anim.SetBool(animID_Warn, false);
    }
}
