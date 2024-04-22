using Language.Lua;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AttackBlock")]
[System.Serializable]
public class SO_AttackBlockBase : ScriptableObject
{
    public string SkillName;
    public int SkillId;
    public int SkillFrame;
    public int LeastNeedAttackFrame;
    public AttackInputType M_SkillType;
    public List<NextComboSkillInfo> nextCombos = new List<NextComboSkillInfo>();
    public float Distance;
    public int Frame;
    public string EndingAnimationName;
    public bool HasEndingAnimation = false;
    public int EndingFrame;

    public NextComboSkillInfo CheckNextAttack(AttackInputType input)
    {
        for (int i = 0; i < nextCombos.Count; i++)
        {
            if (nextCombos[i].RequiredInputType == input)
            {
                return nextCombos[i];
            }
        }
        return null;
    }
}

[System.Serializable]
public class AttackBlockBase
{
    public string SkillName;
    public int SkillId;
    public int SkillFrame;
    public AttackInputType M_SkillType;
    public int LeastNeedAttackFrame;
    public int FrameShouldBeSkipped;
    public string EndingAnimationName;
    public bool HasEndingAnimation;
    public int EndingFrame;
    public AttackBlockBase(SO_AttackBlockBase sO_AttackBlock,int frameShouldBeSkipped)
    {
        LeastNeedAttackFrame = sO_AttackBlock.LeastNeedAttackFrame;
        SkillName =  sO_AttackBlock.SkillName;
        SkillId = sO_AttackBlock.SkillId;
        SkillFrame = sO_AttackBlock.SkillFrame;
        M_SkillType = sO_AttackBlock.M_SkillType;
        FrameShouldBeSkipped = frameShouldBeSkipped;
        EndingAnimationName = sO_AttackBlock.EndingAnimationName;
        HasEndingAnimation = sO_AttackBlock.HasEndingAnimation;
        EndingFrame = sO_AttackBlock.EndingFrame;
    }
}

[System.Serializable]
public class NextComboSkillInfo
{
    public AttackInputType RequiredInputType;
    public int SkippedFrame;
    public int NextAttackId;
    public bool CanCancelEndingAnimation = false;
}


