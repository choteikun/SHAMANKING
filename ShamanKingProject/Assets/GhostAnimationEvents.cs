using Gamemanager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class GhostAnimationEvents : MonoBehaviour
{
    bool dissolveEventTrigger;
    GhostAnimationType animationType_;
    private void Start()
    {
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerLaunchActionFinish, ghostReactState);
    }
    void ghostReactState(PlayerLaunchActionFinishCommand command)
    {
        switch (command.HitObjecctTag)
        {
            //什麼都沒碰撞到
            case HitObjecctTag.None:
                dissolveEventTrigger = true;
                animationType_ = GhostAnimationType.DissolveWithRevert;
                break;
            //碰到可吸收的物體
            case HitObjecctTag.Biteable:
                dissolveEventTrigger = false;
                break;
            //碰到可附身的物體
            case HitObjecctTag.Possessable:
                dissolveEventTrigger = true;
                animationType_ = GhostAnimationType.Dissolve;
                break;
            //碰到敵人
            case HitObjecctTag.Enemy:
                dissolveEventTrigger = false;
                //暫時先關掉溶解特效
                break;
            default:
                dissolveEventTrigger = true;
                Debug.LogError("我不知道到底撞到了啥小");
                break;
        }
    }
    public void Ghost_Back_Start()
    {
        if (dissolveEventTrigger)
        {
            GameManager.Instance.MainGameEvent.Send(new GhostAnimationEventsCommand() { AnimationEventName = "GhostMat_Dissolve", AnimationType = animationType_ });
        }
    }
    public void Ghost_Back_End()
    {
        if (dissolveEventTrigger)
        {
            GameManager.Instance.MainGameEvent.Send(new GhostAnimationEventsCommand() { AnimationEventName = "GhostMat_Revert" ,AnimationType = animationType_});
        }
    }
    public void Ghost_Bite_Start()
    {

    }
    public void Ghost_Bite_End()
    {
        GameManager.Instance.MainGameEvent.Send(new GhostAnimationEventsCommand() { AnimationEventName = "Ghost_Bite_End" });
    }
}

public enum GhostAnimationType
{
    DissolveWithRevert,
    Dissolve,

}
