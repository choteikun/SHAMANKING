using Gamemanager;
using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGameEventPack : GameEventPack
{
    public IObservable<UISoulGageUpdateCommand> OnSoulGageUpdate => getSubject<UISoulGageUpdateCommand>();
    public IObservable<UIPlayerInvincibleUpdateCommand> OnPlayerInvincibleUpdate => getSubject<UIPlayerInvincibleUpdateCommand>();

    public IObservable<UICallPlayerHealthBarUIUpdateCommand> OnCallPlayerHealthBarUIUpdate => getSubject<UICallPlayerHealthBarUIUpdateCommand>();

    public IObservable<UIUpdateBreakCommand> OnUIUpdateBreak => getSubject<UIUpdateBreakCommand>();

    public IObservable<DebugUIHeavyAttackCharge> OnDebugUIHeavyAttackCharge => getSubject<DebugUIHeavyAttackCharge>();

    public IObservable<BossCallUISkillNameCommand> OnBossCallUISkillName => getSubject<BossCallUISkillNameCommand>();

    public IObservable<SystemCallMissionUIUpdateCommand> OnSystemCallMissionUIUpdate => getSubject<SystemCallMissionUIUpdateCommand>();
}
