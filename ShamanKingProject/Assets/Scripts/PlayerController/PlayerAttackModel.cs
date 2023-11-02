using PixelCrushers.DialogueSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerAttackModel
{
    public int PassedFrameAfterAttack;
    public List<AttackBlockBase> CurrentAttackInputs = new List<AttackBlockBase>();
    bool isAttacking_;
    int currentInputCount_ = -1;
    bool comboDeclaim = false;
    public void PlayerAttackModelInit()
    {
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerLightAttack, cmd => { whenGetAttackTrigger(AttackInputType.LightAttack); });
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
        if (CurrentAttackInputs.Count>0)
        {
            //檢查現在是否是輸入窗口       
            if (PassedFrameAfterAttack < CurrentAttackInputs[currentInputCount_].LeastNeedAttackFrame) return;
            //檢查現在的物件裡 他是否有下一段接技
            if (comboDeclaim) return;
            var nextAttack = GameManager.Instance.AttackBlockDatabase.Database[CurrentAttackInputs[currentInputCount_].SkillId].CheckNextAttack(inputType);
            if (nextAttack!= null)
            {
                //如果有 則根據id加進操作欄
                CurrentAttackInputs.Add(new AttackBlockBase(GameManager.Instance.AttackBlockDatabase.Database[nextAttack.NextAttackId], GameManager.Instance.AttackBlockDatabase.Database[nextAttack.NextAttackId].SkillFrame));
                if (PassedFrameAfterAttack<=nextAttack.SkippedFrame)
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
            CurrentAttackInputs.Add(new AttackBlockBase(GameManager.Instance.AttackBlockDatabase.Database[0], GameManager.Instance.AttackBlockDatabase.Database[0].SkillFrame));
            currentInputCount_++;
            if (!isAttacking_)
            {
                isAttacking_ = true;
                PassedFrameAfterAttack = 0;
            }
        }
    }

    void checkNextInput()
    {
        //確認是否有下一個動作
        //如果有 是否已經大於這個動作所需要的最低幀數
        //如果有 切換進下一個動作
        //如果沒有 繼續動作
        //如果沒有 回到idle
        if (CurrentAttackInputs.Count>currentInputCount_+1)
        {
            if (PassedFrameAfterAttack>= CurrentAttackInputs[currentInputCount_].FrameShouldBeSkipped)
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
        PassedFrameAfterAttack = 0;
        currentInputCount_++;
        comboDeclaim = false;
    }

    void backToIdle()
    {
        //呼叫動畫
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
}

