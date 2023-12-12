using Gamemanager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHellDogAnimationEvent : MonoBehaviour
{
    public void BossCurAnimationEnd()
    {
        GameManager.Instance.MainGameEvent.Send(new BossCurAnimationEndCommand());
    }
    public void SystemCallFireballLocate()
    {
        GameManager.Instance.HellDogGameEvent.Send(new SystemCallFireballLocateCommand());
    }
    public void AnimationSpawnAttackColliderCommand(string spawnCommand)
    {
        var parts = spawnCommand.Split(",");

        var attackCollider = int.Parse(parts[0]);
        var effectId = int.Parse(parts[1]);

        GameManager.Instance.MainGameEvent.Send(new AnimationCallAttackEffectCommand() { ColliderId = attackCollider, SpawnEffectId = effectId, CommandSender = this.gameObject, AttackColliderType = AttackColliderType.Monster });
    }
    public void SystemCallFireBallSpawn()
    {
        GameManager.Instance.HellDogGameEvent.Send(new SystemCallFireballSpawnCommand());
    }
    public void SystemCallFireTrackBallSpawn()
    {
        GameManager.Instance.HellDogGameEvent.Send(new SystemCallFireTrackBallSpawnCommand());
    }
}
