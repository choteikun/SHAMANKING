using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamemanager;
using UniRx;
using System;

public class MainGameEventPack : GameEventPack
{
    public IObservable<TestInputCommand> OnTestInput => getSubject<TestInputCommand>();

    public IObservable<PlayerControllerMovementCommand> OnPlayerControllerMovement => getSubject<PlayerControllerMovementCommand>();

    public IObservable<PlayerControllerCameraRotateCommand> OnPlayerCameraRotate => getSubject<PlayerControllerCameraRotateCommand>();

    public IObservable<PlayerAimingButtonCommand> OnAimingButtonTrigger => getSubject<PlayerAimingButtonCommand>();

    public IObservable<PlayerRollingButtonCommand> OnPlayerRoll => getSubject<PlayerRollingButtonCommand>();

    public IObservable<PlayerLaunchGhostButtonCommand> OnPlayerLaunchGhost => getSubject<PlayerLaunchGhostButtonCommand>();

    public IObservable<PlayerLaunchActionFinishCommand> OnPlayerLaunchActionFinish => getSubject<PlayerLaunchActionFinishCommand>();

    public IObservable<PlayerJumpButtonCommand> OnPlayerJump => getSubject<PlayerJumpButtonCommand>();

    public IObservable<PlayerLightAttackButtonCommand> OnPlayerLightAttack => getSubject<PlayerLightAttackButtonCommand>();

    public IObservable<GhostAnimationEventsCommand> OnGhostAnimationEvents => getSubject<GhostAnimationEventsCommand>();

    public IObservable<PlayerAnimationEventsCommand> OnPlayerAnimationEvents => getSubject<PlayerAnimationEventsCommand>();

    public IObservable<GhostLaunchProcessFinishResponse> OnGhostLaunchProcessFinish => getSubject<GhostLaunchProcessFinishResponse>();
}
