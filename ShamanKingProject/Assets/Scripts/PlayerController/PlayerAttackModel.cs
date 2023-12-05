using Cysharp.Threading.Tasks;
using Gamemanager;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerAttackModel
{
    [SerializeField] GameObject inputIndicator_;
    public GameObject characterControllerObj_;
    public int PassedFrameAfterAttack;
    public List<AttackBlockBase> CurrentAttackInputs = new List<AttackBlockBase>();
    bool isAttacking_;
    int currentInputCount_ = -1;
    bool comboDeclaim = false;
    private Animator animator_;
    bool isJumpAttacking_ = false;
    bool isThrowing_ = false;
    public void PlayerAttackModelInit()
    {
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerLightAttack, cmd => { whenGetAttackTrigger(AttackInputType.LightAttack); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerJumpAttack, cmd => { whenGetAttackTrigger(AttackInputType.JumpAttack); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerThrowAttack, cmd => { whenGetAttackTrigger(AttackInputType.Throw); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerHeavyAttack, cmd => { whenGetAttackTrigger(AttackInputType.HeavyAttack); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerRoll, cmd => { whenGetAttackTrigger(AttackInputType.Dodge); });
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
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnEnemyAttackSuccess, cmd => { Debug.LogWarning("PlayerGetHit"); playerGetHit(); });
        var haveAnimatorObject = characterControllerObj_.gameObject.transform.GetChild(0);
        animator_ = haveAnimatorObject.gameObject.GetComponent<Animator>();
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
                    addFirstHeavyAttack();
                    return;
                case AttackInputType.Dodge:
                    addFirstDash();
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
            animator_.CrossFadeInFixedTime("AttackCombo1", 0.25f);
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
            animator_.CrossFadeInFixedTime("JumpAttack1", 0.25f);
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
            animator_.CrossFadeInFixedTime("ThrowAttack", 0.25f);
            PassedFrameAfterAttack = 0;
            isThrowing_ = true;
            isAttacking_ = true;
        }
    }
    void addFirstHeavyAttack()
    {
        CurrentAttackInputs.Add(new AttackBlockBase(GameManager.Instance.AttackBlockDatabase.Database[5], GameManager.Instance.AttackBlockDatabase.Database[5].SkillFrame));
        currentInputCount_++;
        if (!isAttacking_)
        {
            //animator_.Rebind();
            animator_.CrossFadeInFixedTime("HeavyAttack1", 0.25f);
            PassedFrameAfterAttack = 0;
            isThrowing_ = true;
            isAttacking_ = true;
        }
    }
    void playerGetHit()
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
            animator_.CrossFadeInFixedTime(GameManager.Instance.AttackBlockDatabase.Database[18].SkillName, 0);
            PassedFrameAfterAttack = 0;
            isAttacking_ = true;
        }

    }
    void addFirstDash()
    {
        CurrentAttackInputs.Add(new AttackBlockBase(GameManager.Instance.AttackBlockDatabase.Database[8], GameManager.Instance.AttackBlockDatabase.Database[8].SkillFrame));
        currentInputCount_++;
        if (!isAttacking_)
        {
            //animator_.Rebind();
            animator_.CrossFadeInFixedTime("Girl_Dash", 0.25f);
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
        animator_.CrossFadeInFixedTime(GameManager.Instance.AttackBlockDatabase.Database[actionID].SkillName, 0.25f);
        Debug.Log("技能施放成功!");
        PassedFrameAfterAttack = 0;
        currentInputCount_++;
        comboDeclaim = false;
        if (GameManager.Instance.AttackBlockDatabase.Database[actionID].M_SkillType == AttackInputType.Throw)
        {
            isThrowing_ = true;
        }
    }

    void backToIdle()
    {
        //呼叫動畫
        // animator_.Rebind();
        GameManager.Instance.MainGameEvent.Send(new PlayerMovementInterruptionFinishCommand());
        animator_.CrossFadeInFixedTime("Player_Locomotion", 0.25f);
        Debug.Log("回歸");
        isAttacking_ = false;
        CurrentAttackInputs = new List<AttackBlockBase> { };
        currentInputCount_ = -1;
        PassedFrameAfterAttack = 0;
    }
    async void backToIdleFromThrowing()
    {
        //呼叫動畫
        // animator_.Rebind();
        await UniTask.Delay(200);
        GameManager.Instance.MainGameEvent.Send(new PlayerMovementInterruptionFinishCommand());
        animator_.CrossFadeInFixedTime("Player_Locomotion", 0.25f);
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
        animator_.CrossFadeInFixedTime("Girl_LandHarder", 0.25f);
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
        animator_.CrossFadeInFixedTime("PullAnimation", 0.25f);
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
}


