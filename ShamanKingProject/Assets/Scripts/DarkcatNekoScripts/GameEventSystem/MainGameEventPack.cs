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
}
