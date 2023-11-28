using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamemanager;

public class GhostEnemyAnimator : MonoBehaviour
{
    #region 提前Hash進行優化
    readonly int animID_EnemyState = Animator.StringToHash("EnemyState");
    #endregion

    Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
        anim.SetInteger(animID_EnemyState, 0);
    }

    public void SetEnemyState(int state)
    {
        anim.SetInteger(animID_EnemyState, state);
    }
    public void EndOfHurtAnimation()
    {
        GameManager.Instance.MainGameEvent.Send(new PlayerAnimationEventsCommand() { AnimationEventName = "EndOfHurtAnimation"});
    }
    //private void OnAnimatorMove()
    //{
    //    SendMessageUpwards("OnUpdateRootMotion", anim);
    //}
}
