using Cysharp.Threading.Tasks;
using Gamemanager;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerAttackModel
{
    public GameObject characterControllerObj_;
    public int PassedFrameAfterAttack;
    public List<AttackBlockBase> CurrentAttackInputs = new List<AttackBlockBase>();
    bool isAttacking_;
    int currentInputCount_ = -1;
    bool comboDeclaim = false;
    private Animator playerAnimator_;
    private Animator uncleGhostAnimator_;
    bool isJumpAttacking_ = false;
    bool isThrowing_ = false;
    bool isDead_ = false;
    public void PlayerAttackModelInit()
    {
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerLightAttack, cmd => { whenGetAttackTrigger(AttackInputType.LightAttack); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerJumpAttack, cmd => { whenGetAttackTrigger(AttackInputType.JumpAttack); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerThrowAttack, cmd => { whenGetAttackTrigger(AttackInputType.Throw); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerHeavyAttack, cmd => { whenGetAttackTrigger(AttackInputType.HeavyAttack, cmd); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerExecutionAttack, cmd => { whenGetAttackTrigger(AttackInputType.ExecutionAttack); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerShootAttack, cmd => { whenGetAttackTrigger(AttackInputType.ShootAttack); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerRoll, cmd => { whenGetAttackTrigger(AttackInputType.Dodge); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerUltimateAttack, cmd => { whenGetAttackTrigger(AttackInputType.Ultimate); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnSystemCallPlayerGameover, cmd => { isDead_ = true; });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerLaunchGhost, cmd =>
        {
            isThrowing_ = true;
        });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnGhostLaunchProcessFinish, cmd =>
        {
            isThrowing_ = false;
        });
        //GameManager.Instance.MainGameEvent.Send(new GhostLaunchProcessFinishResponse());
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerJumpTouchGround, cmd =>
        {
            if (isJumpAttacking_)
            {
                isJumpAttacking_ = false;
                backToLanding();
            }
        });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerThrowAttackFinish, cmd =>
        {
            if (isThrowing_)
            {
                isThrowing_ = false;
                backToIdleFromThrowing();
            }
        });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerGrabSuccessForPlayer, cmd =>
        {
            if (isThrowing_)
            {
                isThrowing_ = false;
                backToPulling();
            }
        });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnEnemyAttackSuccess, cmd => { Debug.LogWarning("PlayerGetHit"); playerGetHit(cmd); });
        var haveAnimatorObject = characterControllerObj_.gameObject.transform.GetChild(0);
        playerAnimator_ = haveAnimatorObject.gameObject.GetComponent<Animator>();
        uncleGhostAnimator_ = characterControllerObj_.GetComponent<PlayerControllerView>().UncleGhostAnimator;
    }
    public PlayerAttackModel(GameObject view)
    {
        characterControllerObj_ = view;
    }
    // Update is called once per frame
    public void PlayerAttackModelUpdate()
    {
        if (isAttacking_)
        {
            PassedFrameAfterAttack++;
            checkNextInput();
        }
    }
    void whenGetAttackTrigger(AttackInputType inputType)
    {
        if (CurrentAttackInputs.Count > 0)
        {
            //檢查現在是否是輸入窗口       
            if (PassedFrameAfterAttack < CurrentAttackInputs[currentInputCount_].LeastNeedAttackFrame) return;
            //檢查現在的物件裡 他是否有下一段接技
            if (comboDeclaim) return;
            var nextAttack = GameManager.Instance.AttackBlockDatabase.Database[CurrentAttackInputs[currentInputCount_].SkillId].CheckNextAttack(inputType);
            if (nextAttack != null)
            {
                //如果有 則根據id加進操作欄
                CurrentAttackInputs.Add(new AttackBlockBase(GameManager.Instance.AttackBlockDatabase.Database[nextAttack.NextAttackId], GameManager.Instance.AttackBlockDatabase.Database[nextAttack.NextAttackId].SkillFrame));
                if (PassedFrameAfterAttack <= nextAttack.SkippedFrame)
                {
                    CurrentAttackInputs[CurrentAttackInputs.Count - 2].FrameShouldBeSkipped = nextAttack.SkippedFrame;
                    comboDeclaim = true;
                }
                else
                {
                    ChangeAction(nextAttack.NextAttackId);
                }
            }

            //如果沒有 忽略這次的操作
        }
        else
        {

            switch (inputType)
            {
                case AttackInputType.LightAttack:
                    addFirstLightAttack();
                    return;
                case AttackInputType.JumpAttack:
                    addFirstJumpAttack();
                    return;
                case AttackInputType.Throw:
                    addFirstThrowAttack();
                    return;
                case AttackInputType.HeavyAttack:
                    var cmd = new PlayerHeavyAttackButtonCommand() { Charged = false };
                    addFirstHeavyAttack(cmd);
                    return;
                case AttackInputType.Dodge:
                    addFirstDash();
                    return;
                case AttackInputType.ExecutionAttack:
                    addFirstExecutionAttack();
                    return;
                case AttackInputType.ShootAttack:
                    addFirstShootAttack();
                    return;
                case AttackInputType.Ultimate:
                    addFirstUltimateAttack();
                    return;
            }
        }
    }
    void whenGetAttackTrigger(AttackInputType inputType, GameEventMessageBase cmd)
    {
        if (CurrentAttackInputs.Count > 0)
        {
            //檢查現在是否是輸入窗口       
            if (PassedFrameAfterAttack < CurrentAttackInputs[currentInputCount_].LeastNeedAttackFrame) return;
            //檢查現在的物件裡 他是否有下一段接技
            if (comboDeclaim) return;
            var nextAttack = GameManager.Instance.AttackBlockDatabase.Database[CurrentAttackInputs[currentInputCount_].SkillId].CheckNextAttack(inputType);
            if (nextAttack != null)
            {
                //如果有 則根據id加進操作欄
                CurrentAttackInputs.Add(new AttackBlockBase(GameManager.Instance.AttackBlockDatabase.Database[nextAttack.NextAttackId], GameManager.Instance.AttackBlockDatabase.Database[nextAttack.NextAttackId].SkillFrame));
                if (PassedFrameAfterAttack <= nextAttack.SkippedFrame)
                {
                    CurrentAttackInputs[CurrentAttackInputs.Count - 2].FrameShouldBeSkipped = nextAttack.SkippedFrame;
                    comboDeclaim = true;
                }
                else
                {
                    ChangeAction(nextAttack.NextAttackId);
                }
            }

            //如果沒有 忽略這次的操作
        }
        else
        {

            switch (inputType)
            {
                case AttackInputType.LightAttack:
                    addFirstLightAttack();
                    return;
                case AttackInputType.JumpAttack:
                    addFirstJumpAttack();
                    return;
                case AttackInputType.Throw:
                    addFirstThrowAttack();
                    return;
                case AttackInputType.HeavyAttack:
                    if (cmd is PlayerHeavyAttackButtonCommand command)
                    {
                        addFirstHeavyAttack(command);
                    }
                    return;
                case AttackInputType.Dodge:
                    addFirstDash();
                    return;
                case AttackInputType.ExecutionAttack:
                    addFirstExecutionAttack();
                    return;
                case AttackInputType.ShootAttack:
                    addFirstShootAttack();
                    return;
            }
        }
    }

    void addFirstLightAttack()
    {
        CurrentAttackInputs.Add(new AttackBlockBase(GameManager.Instance.AttackBlockDatabase.Database[0], GameManager.Instance.AttackBlockDatabase.Database[0].SkillFrame));
        currentInputCount_++;
        if (!isAttacking_)
        {
            //animator_.Rebind();
            playerAnimator_.CrossFadeInFixedTime("AttackCombo1", 0.05f);
            uncleGhostAnimator_.CrossFadeInFixedTime("UncleGhost_AttackCombo_A", 0.25f);
            PassedFrameAfterAttack = 0;
            isAttacking_ = true;
        }
    }

    void addFirstJumpAttack()
    {
        CurrentAttackInputs.Add(new AttackBlockBase(GameManager.Instance.AttackBlockDatabase.Database[3], GameManager.Instance.AttackBlockDatabase.Database[3].SkillFrame));
        currentInputCount_++;
        if (!isAttacking_)
        {
            //animator_.Rebind();
            playerAnimator_.CrossFadeInFixedTime("JumpAttack1", 0.25f);
            PassedFrameAfterAttack = 0;
            isJumpAttacking_ = true;
            isAttacking_ = true;
        }
    }

    void addFirstThrowAttack()
    {
        CurrentAttackInputs.Add(new AttackBlockBase(GameManager.Instance.AttackBlockDatabase.Database[4], GameManager.Instance.AttackBlockDatabase.Database[4].SkillFrame));
        currentInputCount_++;
        if (!isAttacking_)
        {
            //animator_.Rebind();
            playerAnimator_.CrossFadeInFixedTime("ThrowAttack", 0.25f);
            PassedFrameAfterAttack = 0;
            isThrowing_ = true;
            isAttacking_ = true;
        }
    }
    void addFirstHeavyAttack(PlayerHeavyAttackButtonCommand cmd)
    {
        if (cmd.Charged)
        {
            CurrentAttackInputs.Add(new AttackBlockBase(GameManager.Instance.AttackBlockDatabase.Database[12], GameManager.Instance.AttackBlockDatabase.Database[12].SkillFrame));
            currentInputCount_++;
            if (!isAttacking_)
            {
                //animator_.Rebind();
                playerAnimator_.CrossFadeInFixedTime("HeavyAttack2", 0.25f);
                PassedFrameAfterAttack = 0;
                isAttacking_ = true;
            }
        }
        else
        {
            CurrentAttackInputs.Add(new AttackBlockBase(GameManager.Instance.AttackBlockDatabase.Database[21], GameManager.Instance.AttackBlockDatabase.Database[21].SkillFrame));
            currentInputCount_++;
            if (!isAttacking_)
            {
                //animator_.Rebind();
                playerAnimator_.CrossFadeInFixedTime("BasicHeavyAttack", 0.25f);
                PassedFrameAfterAttack = 0;
                isAttacking_ = true;
            }
        }

    }

    void addFirstExecutionAttack()
    {
        CurrentAttackInputs.Add(new AttackBlockBase(GameManager.Instance.AttackBlockDatabase.Database[6], GameManager.Instance.AttackBlockDatabase.Database[6].SkillFrame));
        currentInputCount_++;
        if (!isAttacking_)
        {
            //animator_.Rebind();
            playerAnimator_.CrossFadeInFixedTime("ExecutionAttack", 0.25f);
            PassedFrameAfterAttack = 0;
            isAttacking_ = true;
        }
    }

    void addFirstUltimateAttack()
    {
        if (GameManager.Instance.MainGameMediator.RealTimePlayerData.GhostNowGageBlockAmount < 2)
        {
            return;
        }
        else if (GameManager.Instance.MainGameMediator.RealTimePlayerData.GhostNowGageBlockAmount < 4 && GameManager.Instance.MainGameMediator.RealTimePlayerData.GhostNowGageBlockAmount >= 2)
        {
            PlayerStatCalculator.PlayerAddOrMinusSpirit(-200);
            CurrentAttackInputs.Add(new AttackBlockBase(GameManager.Instance.AttackBlockDatabase.Database[24], GameManager.Instance.AttackBlockDatabase.Database[24].SkillFrame));
            currentInputCount_++;
            if (!isAttacking_)
            {
                playerAnimator_.CrossFadeInFixedTime("PlayerUltimateState1", 0.25f);
                PassedFrameAfterAttack = 0;
                isAttacking_ = true;
            }
        }
        else if (GameManager.Instance.MainGameMediator.RealTimePlayerData.GhostNowGageBlockAmount == 4)
        {
            PlayerStatCalculator.PlayerAddOrMinusSpirit(-400);
            CurrentAttackInputs.Add(new AttackBlockBase(GameManager.Instance.AttackBlockDatabase.Database[25], GameManager.Instance.AttackBlockDatabase.Database[25].SkillFrame));
            currentInputCount_++;
            if (!isAttacking_)
            {
                playerAnimator_.CrossFadeInFixedTime("PlayerUltimateState2", 0.25f);
                PassedFrameAfterAttack = 0;
                isAttacking_ = true;
            }
        }

    }
    void addFirstShootAttack()
    {
        if (GameManager.Instance.MainGameMediator.RealTimePlayerData.GhostSoulGageCurrentAmount == GameManager.Instance.MainGameMediator.RealTimePlayerData.GhostSoulGageMaxAmount)
        {
            PlayerStatCalculator.PlayerAddOrMinusSpirit(GameManager.Instance.MainGameMediator.RealTimePlayerData.GhostSoulGageMaxAmount * -1);
            CurrentAttackInputs.Add(new AttackBlockBase(GameManager.Instance.AttackBlockDatabase.Database[22], GameManager.Instance.AttackBlockDatabase.Database[22].SkillFrame));
            currentInputCount_++;
            if (!isAttacking_)
            {
                //animator_.Rebind();
                playerAnimator_.CrossFadeInFixedTime("PowerShootAttack", 0.25f);
                PassedFrameAfterAttack = 0;
                isAttacking_ = true;
            }
        }
        else
        {
            CurrentAttackInputs.Add(new AttackBlockBase(GameManager.Instance.AttackBlockDatabase.Database[19], GameManager.Instance.AttackBlockDatabase.Database[19].SkillFrame));
            currentInputCount_++;
            if (!isAttacking_)
            {
                //animator_.Rebind();
                playerAnimator_.CrossFadeInFixedTime("ShootAttack", 0.25f);
                PassedFrameAfterAttack = 0;
                isAttacking_ = true;
            }
        }

    }
    void playerGetHit(EnemyAttackSuccessCommand cmd)
    {
        PlayerStatCalculator.PlayerAddOrMinusHealth(cmd.AttackDamage * -1);
        if (cmd.ThisAttackHitPower == EnemyHitPower.Light)
        {
            Debug.Log("受擊");
            GameManager.Instance.MainGameEvent.Send(new PlayerBeAttackByEnemySuccessResponse());
            isAttacking_ = false;
            CurrentAttackInputs = new List<AttackBlockBase> { };
            currentInputCount_ = -1;
            PassedFrameAfterAttack = 0;
            comboDeclaim = false;
            CurrentAttackInputs.Add(new AttackBlockBase(GameManager.Instance.AttackBlockDatabase.Database[18], GameManager.Instance.AttackBlockDatabase.Database[18].SkillFrame));
            currentInputCount_++;
            if (!isAttacking_)
            {
                playerAnimator_.CrossFadeInFixedTime(GameManager.Instance.AttackBlockDatabase.Database[18].SkillName, 0);
                PassedFrameAfterAttack = 0;
                isAttacking_ = true;
            }
        }
        else if (cmd.ThisAttackHitPower == EnemyHitPower.HardKnockBack)
        {
            Debug.Log("受擊");
            GameManager.Instance.MainGameEvent.Send(new PlayerBeAttackByEnemySuccessResponse());
            isAttacking_ = false;
            CurrentAttackInputs = new List<AttackBlockBase> { };
            currentInputCount_ = -1;
            PassedFrameAfterAttack = 0;
            comboDeclaim = false;
            CurrentAttackInputs.Add(new AttackBlockBase(GameManager.Instance.AttackBlockDatabase.Database[23], GameManager.Instance.AttackBlockDatabase.Database[23].SkillFrame));
            currentInputCount_++;
            if (!isAttacking_)
            {
                playerAnimator_.CrossFadeInFixedTime(GameManager.Instance.AttackBlockDatabase.Database[23].SkillName, 0);
                PassedFrameAfterAttack = 0;
                isAttacking_ = true;
            }
        }

    }
    void addFirstDash()
    {
        CurrentAttackInputs.Add(new AttackBlockBase(GameManager.Instance.AttackBlockDatabase.Database[8], GameManager.Instance.AttackBlockDatabase.Database[8].SkillFrame));
        currentInputCount_++;
        if (!isAttacking_)
        {
            //animator_.Rebind();
            playerAnimator_.CrossFadeInFixedTime("Girl_Dash", 0.25f);
            GameManager.Instance.MainGameEvent.Send(new GameCallSoundEffectGenerate() { SoundEffectID = 8 });
            PassedFrameAfterAttack = 0;
            isAttacking_ = true;
        }
    }
    void checkNextInput()
    {
        //確認是否有下一個動作
        //如果有 是否已經大於這個動作所需要的最低幀數
        //如果有 切換進下一個動作
        //如果沒有 繼續動作
        //如果沒有 回到idle
        if (CurrentAttackInputs.Count > currentInputCount_ + 1)
        {
            if (PassedFrameAfterAttack >= CurrentAttackInputs[currentInputCount_].FrameShouldBeSkipped)
            {
                ChangeAction(CurrentAttackInputs[currentInputCount_ + 1].SkillId);
            }
        }
        else
        {
            if (PassedFrameAfterAttack >= CurrentAttackInputs[currentInputCount_].FrameShouldBeSkipped)
            {
                backToIdle();
            }
        }
    }

    void ChangeAction(int actionID)
    {
        //呼叫動畫片段
        playerAnimator_.CrossFadeInFixedTime(GameManager.Instance.AttackBlockDatabase.Database[actionID].SkillName, 0.25f);
        Debug.Log("技能施放成功!");
        PassedFrameAfterAttack = 0;
        currentInputCount_++;
        comboDeclaim = false;
        switch (actionID)
        {
            case 1:
                uncleGhostAnimator_.CrossFadeInFixedTime("UncleGhost_AttackCombo_B", 0.25f);
                break;
            case 2:
                uncleGhostAnimator_.CrossFadeInFixedTime("UncleGhost_AttackCombo_C", 0.25f);
                break;
            case 13:
                uncleGhostAnimator_.CrossFadeInFixedTime("UncleGhost_AttackCombo_D", 0.25f);
                break;
            default:
                break;
        }
        if (GameManager.Instance.AttackBlockDatabase.Database[actionID].M_SkillType == AttackInputType.Throw)
        {
            isThrowing_ = true;
        }
        if (GameManager.Instance.AttackBlockDatabase.Database[actionID].M_SkillType == AttackInputType.Dodge)
        {
            GameManager.Instance.MainGameEvent.Send(new GameCallSoundEffectGenerate() { SoundEffectID = 8 });
        }
    }

    void backToIdle()
    {
        //呼叫動畫
        // animator_.Rebind();
        if (!isDead_)
        {
            GameManager.Instance.MainGameEvent.Send(new PlayerMovementInterruptionFinishCommand());
            playerAnimator_.CrossFadeInFixedTime("Player_Locomotion", 0.25f);
            uncleGhostAnimator_.CrossFadeInFixedTime("UncleGhost_Idle", 0.25f);
            Debug.Log("回歸");
            isAttacking_ = false;
            CurrentAttackInputs = new List<AttackBlockBase> { };
            currentInputCount_ = -1;
            PassedFrameAfterAttack = 0;
        }
    }
    async void backToIdleFromThrowing()
    {
        //呼叫動畫
        // animator_.Rebind();
        await UniTask.Delay(200);
        GameManager.Instance.MainGameEvent.Send(new PlayerMovementInterruptionFinishCommand());
        playerAnimator_.CrossFadeInFixedTime("Player_Locomotion", 0.25f);
        Debug.Log("回歸");
        isAttacking_ = false;
        CurrentAttackInputs = new List<AttackBlockBase> { };
        currentInputCount_ = -1;
        PassedFrameAfterAttack = 0;
    }
    void backToLanding()
    {
        //呼叫動畫
        // animator_.Rebind();
        playerAnimator_.CrossFadeInFixedTime("Girl_LandHarder", 0.25f);
        Debug.Log("回歸");
        isAttacking_ = false;
        CurrentAttackInputs = new List<AttackBlockBase> { };
        currentInputCount_ = -1;
        PassedFrameAfterAttack = 0;
    }
    void backToPulling()
    {
        //呼叫動畫
        // animator_.Rebind();
        playerAnimator_.CrossFadeInFixedTime("PullAnimation", 0.25f);
        Debug.Log("回歸");
        isAttacking_ = false;
        CurrentAttackInputs = new List<AttackBlockBase> { };
        currentInputCount_ = -1;
        PassedFrameAfterAttack = 0;
    }
}

public enum AttackInputType
{
    LightAttack,
    HeavyAttack,
    Dodge,
    JumpAttack,
    Throw,
    SpecialAttack,
    ChainAttack,
    GetHurt,
    ExecutionAttack,
    ShootAttack,
    SoulAttack,
    Ultimate,
}


