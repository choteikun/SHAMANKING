using Gamemanager;
using System;

public class GhostEnemyGameEventPack : GameEventPack
{
    public IObservable<SystemCallShadowballSpawnCommand> OnSystemCallShadowballSpawn => getSubject<SystemCallShadowballSpawnCommand>();

    public IObservable<GhostEnemyCallFollowAttackCommand> OnGhostEnemyCallFollowAttack => getSubject<GhostEnemyCallFollowAttackCommand>();

    public IObservable<EliteGhostEnemyRangedAttackCommand> OnEliteGhostEnemyRangedAttack => getSubject<EliteGhostEnemyRangedAttackCommand>();
}
