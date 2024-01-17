using Gamemanager;
using System;

public class GhostEnemyGameEventPack : GameEventPack
{
    public IObservable<SystemCallShadowballSpawnCommand> OnSystemCallShadowballSpawn => getSubject<SystemCallShadowballSpawnCommand>();
}
