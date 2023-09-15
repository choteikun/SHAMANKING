using Gamemanager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class GhostAnimationEvents : MonoBehaviour
{
    bool dissolveEventTrigger;
    private void Start()
    {
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerLaunchFinish, ghostReactState);
    }
    void ghostReactState(PlayerLaunchFinishCommand command)
    {
        switch (command.HitObjecctTag)
        {
            //什麼都沒碰撞到
            case HitObjecctTag.None:
                dissolveEventTrigger = true;
                break;
            //碰到可吸收的物體
            case HitObjecctTag.Biteable:
                dissolveEventTrigger = false;
                break;
            //碰到可附身的物體
            case HitObjecctTag.Possessable:
                dissolveEventTrigger = true;
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
            GameManager.Instance.MainGameEvent.Send(new GhostAnimationEventsCommand() { AnimationEventName = "GhostMat_Dissolve" });
        }
    }
    public void Ghost_Back_End()
    {
        if (dissolveEventTrigger)
        {
            GameManager.Instance.MainGameEvent.Send(new GhostAnimationEventsCommand() { AnimationEventName = "GhostMat_Revert" });
        }
    }

}
