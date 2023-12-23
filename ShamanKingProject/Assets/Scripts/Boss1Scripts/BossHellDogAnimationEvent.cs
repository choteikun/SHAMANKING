using Gamemanager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Datamanager;

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
    public void SystemCallSprintColliderOn()
    {
        GameManager.Instance.HellDogGameEvent.Send(new BossCallSprintColliderSwitchCommand() { OnOrOff = true });
    }
    public void SystemCallSprintColliderOff()
    {
        GameManager.Instance.HellDogGameEvent.Send(new BossCallSprintColliderSwitchCommand() { OnOrOff = false });
    }
    public void AnimationCallSpawnPunishmentManager()
    {
        var punishmentManagerPrefab = GameContainer.Get<DataManager>().GetDataByID<GameEffectTemplete>(19).PrefabPath;
        var punishmentManagerObject = Instantiate(punishmentManagerPrefab, Vector3.zero, Quaternion.identity);
    }
    public void JumpAtkLocate()
    {
        GameManager.Instance.HellDogGameEvent.Send(new BossCallJumpAttackLocateCommand());
    }
}
