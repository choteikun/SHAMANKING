using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamemanager;
using UniRx;
using System;

public class MainGameEventPack : GameEventPack
{
    public IObservable<TestInputCommand> OnTestInput => getSubject<TestInputCommand>();
}
