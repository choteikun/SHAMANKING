using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamemanager;

public class GhostEnemyAnimator : MonoBehaviour
{
    #region 提前Hash進行優化
    #endregion

    Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void EndOfHurtAnimation()
    {
        GameManager.Instance.MainGameEvent.Send(new PlayerAnimationEventsCommand() { AnimationEventName = "EndOfHurtAnimation" });
    }
}
