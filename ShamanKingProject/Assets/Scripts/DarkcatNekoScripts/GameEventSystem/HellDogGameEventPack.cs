using Gamemanager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellDogGameEventPack : GameEventPack
{
    public IObservable<SystemCallFireballLocateCommand> OnSystemCallFireballLocateCommand => getSubject<SystemCallFireballLocateCommand>();
    public IObservable<SystemCallFireballSpawnCommand> OnSystemCallFireballSpawn => getSubject<SystemCallFireballSpawnCommand>();
}
