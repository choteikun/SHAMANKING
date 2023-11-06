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

    public void Player_Attack_CallHitBox(int callOrCancel)
    {
        var callOrCancelBool = false; if (callOrCancel == 0) { callOrCancelBool = false; } else { callOrCancelBool = true; }
        GameManager.Instance.MainGameEvent.Send(new PlayerAttackCallHitBoxCommand() { CallOrCancel = callOrCancelBool});
    }

    public void Player_Attack_End()
    {
        GameManager.Instance.MainGameEvent.Send(new PlayerAnimationEventsCommand() { AnimationEventName = "Player_Attack_End" });
    }
    public void SendPlayer_Attack(string attackName)
    {
        GameManager.Instance.MainGameEvent.Send(new PlayerAttackCommand() { AttackName = attackName });
    }

    public void Player_AimRecoil_End()
    {
        GameManager.Instance.MainGameEvent.Send(new PlayerAnimationEventsCommand() { AnimationEventName = "Player_AimRecoil_End" });
    }

    public void Player_JumpAttackStartFalling()
    {
        GameManager.Instance.MainGameEvent.Send(new PlayerAnimationEventsCommand() { AnimationEventName = "Player_JumpAttackStartFalling" });
    }

    public void PlayerJumpAttackStart()
    {
        GameManager.Instance.MainGameEvent.Send(new PlayerAnimationEventsCommand() { AnimationEventName = "PlayerJumpAttackStart" });
    }
}
