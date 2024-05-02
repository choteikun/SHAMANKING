using Gamemanager;
using UnityEngine;
using Datamanager;
using BehaviorDesigner.Runtime;
using AI.FSM;
using UnityEngine.VFX;

public class BossHellDogAnimationEvent : MonoBehaviour
{
    FirstBossVariables firstBossVariables;
    [SerializeField] GameObject bk_EffectPrefab_;
    [SerializeField] GameObject player_Obstacle_;
    [SerializeField] VisualEffect[] bossVfxs;
    void Awake()
    {
        firstBossVariables = GetComponentInParent<FirstBossVariables>();
        if(player_Obstacle_ == null)
        {
            player_Obstacle_ = GameObject.Find("PlayerObstacle").gameObject;
        }
        for (int i = 0; i <= 4; i++)
        {
            if (bossVfxs[i] == null)
            {
                bossVfxs[0] ??= GameObject.Find("BossFireVfx_Back").GetComponent<VisualEffect>();
                bossVfxs[1] ??= GameObject.Find("BossFireVfx_LFFoot").GetComponent<VisualEffect>();
                bossVfxs[2] ??= GameObject.Find("BossFireVfx_RFFoot").GetComponent<VisualEffect>();
                bossVfxs[3] ??= GameObject.Find("BossFireVfx_LBFoot").GetComponent<VisualEffect>();
                bossVfxs[4] ??= GameObject.Find("BossFireVfx_RBFoot").GetComponent<VisualEffect>();
            }
        }
    }
    public void BossCurAnimationEnd()
    {
        GameManager.Instance.MainGameEvent.Send(new BossCurAnimationEndCommand());
        firstBossVariables.isPillarTriggering = false;
    }
    public void PillarAviod()
    {
        firstBossVariables.isPillarTriggering = false;
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
        player_Obstacle_.SetActive(false);
        GameManager.Instance.HellDogGameEvent.Send(new BossCallSprintColliderSwitchCommand() { OnOrOff = true });
    }
    public void SystemCallSprintColliderOff()
    {
        player_Obstacle_.SetActive(true);
        GameManager.Instance.HellDogGameEvent.Send(new BossCallSprintColliderSwitchCommand() { OnOrOff = false });
    }
    public void AnimationCallSpawnPunishmentManager()
    {
        var punishmentManagerPrefab = GameContainer.Get<DataManager>().GetDataByID<GameEffectTemplete>(19).PrefabPath;
        var punishmentManagerObject = Instantiate(punishmentManagerPrefab, Vector3.zero, Quaternion.identity);
    }
    public void JumpAtkLocate()
    {
        player_Obstacle_.SetActive(false);
        GameManager.Instance.HellDogGameEvent.Send(new BossCallJumpAttackLocateCommand());
    }

    public void JumpAtkColliderSpawn()
    {
        var attackManagerPrefab = GameContainer.Get<DataManager>().GetDataByID<GameEffectTemplete>(20).PrefabPath;
        var pos = transform.position;
        pos.y = 12f;
        var attackManagerObject = Instantiate(attackManagerPrefab, pos, Quaternion.identity);
    }
    public void PlayerObstacleColliderOn()
    {
        player_Obstacle_.SetActive(true);
    }
    public void BossFlameThrowerTriggerOn()
    {
        GameManager.Instance.HellDogGameEvent.Send(new BossCallFlameThrowerSwitchCommand() { TurnedOn = true });
    }
    public void BossFlameThrowerTriggerOff()
    {        
        GameManager.Instance.HellDogGameEvent.Send(new BossCallFlameThrowerSwitchCommand() { TurnedOn = false });
    }

    public void HellDogCallCameraShake()
    {
        GameManager.Instance.HellDogGameEvent.Send(new BossCallCameraFeedBackCommand());
    }
    public void ExecuteCamFeedback()
    {
        GameManager.Instance.MainGameEvent.Send(new PlayerExecuteCamFeedBackCommand());
    }

    public void CallUISkillName(string command)
    {
        var parts = command.Split(",");

        var skillType = (BossSkillType)int.Parse(parts[0]);
        var skillName = parts[1];
        GameManager.Instance.UIGameEvent.Send(new BossCallUISkillNameCommand() { SkillType = skillType, Name = skillName });
    }
    public void BossCallSoundEffect(int soundEffectId)
    {
        GameManager.Instance.MainGameEvent.Send(new GameCallSoundEffectGenerate() { SoundEffectID = soundEffectId });
    }
    public void FirstBossEnable()
    {
        firstBossVariables.GetComponent<BaseStateMachine>().enabled = true;
        firstBossVariables.GetComponent<BehaviorTree>().enabled = true;
    }
    public void FirstBossDisable()
    {
        firstBossVariables.GetComponent<BaseStateMachine>().enabled = false;
        firstBossVariables.GetComponent<BehaviorTree>().enabled = false;
    }
    public void BK_EffectSpawn()
    {
        Instantiate(bk_EffectPrefab_, transform.position, transform.rotation);
    }
    public void FirstBossFireVfxStop()
    {
        for (int i = 0; i <= 4; i++)
        {
            bossVfxs[i].Stop();
        }
    }

    public void BossCallUltCamTransfer()
    {
        GameManager.Instance.HellDogGameEvent.Send(new BossCallUltCamTransfer() { trigger = true });
    }
    public void BossCallUltCamTransferBack()
    {
        GameManager.Instance.HellDogGameEvent.Send(new BossCallUltCamTransfer() { trigger = false });
    }
    public void FlameThrowerAnimationStart()
    {
        GameManager.Instance.HellDogGameEvent.Send(new BossCallFlameThrowerCommand());
    }
    public void DashAnimationStart()
    {
        GameManager.Instance.HellDogGameEvent.Send(new BossCallDashCommand());
    }
}
