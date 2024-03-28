using Gamemanager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAnimationEvents : MonoBehaviour
{
    //允許攻擊動畫切換
    public void Player_Attack_Allow()
    {
        //GameManager.Instance.MainGameEvent.Send(new PlayerAnimationEventsCommand() { AnimationEventName = "Player_Attack_Allow" });
        GameManager.Instance.MainGameEvent.Send(new SystemAttackAllowCommand() );
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

    public void PlayerThrowAttackReady()
    {
        GameManager.Instance.MainGameEvent.Send(new PlayerAnimationEventsCommand() { AnimationEventName = "PlayerThrowAttackReady" });
    }

    public void PlayerThrowAnimationStart()
    {
        GameManager.Instance.MainGameEvent.Send(new PlayerAnimationEventsCommand() { AnimationEventName = "PlayerThrowAnimationStart" });
    }
    public void Player_Throw_Attack_CallHitBox(int callOrCancel)
    {
        var callOrCancelBool = false; if (callOrCancel == 0) { callOrCancelBool = false; } else { callOrCancelBool = true; }
        GameManager.Instance.MainGameEvent.Send(new PlayerThrowAttackCallHitBoxCommand() { CallOrCancel = callOrCancelBool });
    }

    public void Player_Pull_Finish()
    {
        GameManager.Instance.MainGameEvent.Send(new PlayerAnimationEventsCommand() { AnimationEventName = "Player_Pull_Finish" });
        GameManager.Instance.MainGameEvent.Send(new GhostLaunchProcessFinishResponse());
    }
    public void PlayerRollMovementStart()
    {
        GameManager.Instance.MainGameEvent.Send(new StartRollMovementAnimationEvent());
    }

    public void PlayerAnimationMovementEvent(SO_AttackBlockBase attackBlock)
    {
        GameManager.Instance.MainGameEvent.Send(new AnimationMovementEventCommand() {Distance = attackBlock.Distance, Frame = attackBlock.Frame });
    }

    public void AnimationSpawnAttackColliderCommand(string spawnCommand)
    {
        var parts = spawnCommand.Split(",");

        var attackCollider = int.Parse( parts[0]);
        var effectId = int.Parse(parts[1]);

        GameManager.Instance.MainGameEvent.Send(new AnimationCallAttackEffectCommand() { ColliderId = attackCollider, SpawnEffectId = effectId,CommandSender = this.gameObject,AttackColliderType = AttackColliderType.Player });
    }
    public void AnimationSpawnShootAttackColliderCommand(string spawnCommand)
    {
        var parts = spawnCommand.Split(",");

        var attackCollider = int.Parse(parts[0]);
        var effectId = int.Parse(parts[1]);

        GameManager.Instance.MainGameEvent.Send(new AnimationCallRepeatShootAttackCommand() { ColliderId = attackCollider, SpawnEffectId = effectId, CommandSender = this.gameObject, AttackColliderType = AttackColliderType.Player });
    }
    public void AnimationTriggerInvincibleOn()
    {
        PlayerStatCalculator.PlayerInvincibleSwitch(true);
    }
    public void AnimationTriggerInvincibleOff()
    {
        PlayerStatCalculator.PlayerInvincibleSwitch(false);
    }

    public void AnimationCallSoundEffect(int id)
    {
        GameManager.Instance.MainGameEvent.Send(new GameCallSoundEffectGenerate() {SoundEffectID = id });
    }

    public void AnimationCallRandomWalkSoundEffect()
    {
        var random = Random.Range(28, 32);
        GameManager.Instance.MainGameEvent.Send(new GameCallSoundEffectGenerate() { SoundEffectID = random });
    }
}
