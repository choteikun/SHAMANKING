using Gamemanager;
using System;

public class GhostEnemyGameEventPack : GameEventPack
{
    public IObservable<SystemCallShadowballSpawnCommand> OnSystemCallShadowballSpawn => getSubject<SystemCallShadowballSpawnCommand>();

     public IObservable<GhostCallFollowAttackCommand> OnGhostCallFollowAttack => getSubject<GhostCallFollowAttackCommand>();
}
