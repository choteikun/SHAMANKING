using Gamemanager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class HellDogGameEventPack : GameEventPack
{
    public IObservable<SystemCallFireballLocateCommand> OnSystemCallFireballLocateCommand => getSubject<SystemCallFireballLocateCommand>();
    public IObservable<SystemCallFireballSpawnCommand> OnSystemCallFireballSpawn => getSubject<SystemCallFireballSpawnCommand>();
    public IObservable<SystemCallFireTrackBallSpawnCommand> OnSystemCallFireTrackBallSpawn => getSubject<SystemCallFireTrackBallSpawnCommand>();

    public IObservable<BossCallSprintColliderSwitchCommand> OnBossCallSprintColliderSwitch => getSubject<BossCallSprintColliderSwitchCommand>();
    public IObservable<BossCallJumpAttackLocateCommand> OnBossCallJumpAttackLocate => getSubject<BossCallJumpAttackLocateCommand>();

    public IObservable<BossCallFlameThrowerSwitchCommand> OnBossCallFlameThrowerSwitch => getSubject<BossCallFlameThrowerSwitchCommand>();
    public IObservable<BossCallCameraFeedBackCommand> OnBossCallCameraFeedBack => getSubject<BossCallCameraFeedBackCommand>();
    public IObservable<BossPunishmentAttackEndCommand> OnBossPunishmentAttackEnd => getSubject<BossPunishmentAttackEndCommand>();
    public IObservable<BossCallDeadCommand> OnBossCallDeadCommand => getSubject<BossCallDeadCommand>();
    public IObservable<BossCallUltCamTransfer> OnBossCallUltCamTransfer => getSubject<BossCallUltCamTransfer>();
    public IObservable<BossFireTrackBallTrigger> OnBossFireTrackBallTrigger => getSubject<BossFireTrackBallTrigger>();
}
