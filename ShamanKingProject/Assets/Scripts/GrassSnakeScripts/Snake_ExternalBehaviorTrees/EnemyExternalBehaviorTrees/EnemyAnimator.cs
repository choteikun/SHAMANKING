using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    #region 提前Hash進行優化
    readonly int animID_Idle = Animator.StringToHash("Idle");

    readonly int animID_Walk = Animator.StringToHash("Walk");

    readonly int animID_Run = Animator.StringToHash("Run");

    readonly int animID_EnemyState = Animator.StringToHash("EnemyState");
    #endregion

    Animator anim;
    EnemyBehaviorTreeSupport enemyBehaviorTreeSupport;

    void Start()
    {
        anim = GetComponent<Animator>();
        enemyBehaviorTreeSupport = GetComponentInParent<EnemyBehaviorTreeSupport>();
    }

    void Update()
    {
        anim.SetInteger(animID_EnemyState, (int)enemyBehaviorTreeSupport.enemyBehaviorTreeState);
    }
}
