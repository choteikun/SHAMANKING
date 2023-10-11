using Gamemanager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGameEventPack : GameEventPack
{
    public IObservable<UISpiritUpdateCommand> OnSpiritUpdate => getSubject<UISpiritUpdateCommand>();
}
