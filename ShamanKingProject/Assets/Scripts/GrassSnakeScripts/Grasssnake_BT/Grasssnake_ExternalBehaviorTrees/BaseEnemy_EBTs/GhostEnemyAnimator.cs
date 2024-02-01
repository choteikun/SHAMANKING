using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamemanager;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityTransform;

public class GhostEnemyAnimator : MonoBehaviour
{
    #region 提前Hash進行優化
    readonly int animID_EnemyState = Animator.StringToHash("EnemyState");
    #endregion

   [SerializeField] string ghostIdentityName_; 
    Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
        anim.SetInteger(animID_EnemyState, 0);
    }
    void Start()
    {
        //ghostIdentityName_ = this.gameObject.transform.parent.name;
    }
    public void SetEnemyState(int state)
    {
        anim.SetInteger(animID_EnemyState, state);
    }
    public void EndOfHurtAnimation()
    {
        GameManager.Instance.MainGameEvent.Send(new PlayerAnimationEventsCommand() { AnimationEventName = "EndOfHurtAnimation"});
    }
    public void GhostEnemyIdentityCheck()
    {
        GameManager.Instance.MainGameEvent.Send(new GhostIdentityCheckCommand() { GhostIdentityName = ghostIdentityName_ });
    }
    private void OnAnimatorMove()
    {
        SendMessageUpwards("OnUpdateRootMotion", anim);
    }
    public void AnimationSpawnAttackColliderCommand(string spawnCommand)
    {
        var parts = spawnCommand.Split(",");

        var attackCollider = int.Parse(parts[0]);
        var effectId = int.Parse(parts[1]);

        GameManager.Instance.MainGameEvent.Send(new AnimationCallAttackEffectCommand() { ColliderId = attackCollider, SpawnEffectId = effectId, CommandSender = this.gameObject, AttackColliderType = AttackColliderType.Monster });
    }

    public void GhostCallSpawnSoundEffect(int soundEffectID)
    {
        GameManager.Instance.MainGameEvent.Send(new GameCallSoundEffectGenerate() {SoundEffectID =  soundEffectID});
    }
}
