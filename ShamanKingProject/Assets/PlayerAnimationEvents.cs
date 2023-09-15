using Gamemanager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    //允許攻擊動畫切換
    public void Player_Attack_Allow()
    {
        GameManager.Instance.MainGameEvent.Send(new PlayerAnimationEventsCommand() { AnimationEventName = "Player_Attack_Allow" });
    }
    //禁止攻擊動畫切換
    public void Player_Attack_Prohibit()
    {
        GameManager.Instance.MainGameEvent.Send(new PlayerAnimationEventsCommand() { AnimationEventName = "Player_Attack_Prohibit" });
    }
}
