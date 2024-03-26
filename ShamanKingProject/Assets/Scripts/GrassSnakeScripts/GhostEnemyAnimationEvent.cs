using Datamanager;
using Gamemanager;
using UnityEngine;

public class GhostEnemyAnimationEvent : MonoBehaviour
{
    [SerializeField] GameObject bk_EffectPrefab_;
    [SerializeField] GameObject shadowBallPrefab_;
    [SerializeField] GameObject shadowBallSpawnPoint_;

    void Start()
    {
        GameManager.Instance.GhostEnemyGameEvent.SetSubscribe(GameManager.Instance.GhostEnemyGameEvent.OnSystemCallShadowballSpawn, cmd => { shadowballSpawn(); });
    }
    public void SystemCallShadowBallSpawn()
    {
        GameManager.Instance.GhostEnemyGameEvent.Send(new SystemCallShadowballSpawnCommand());
    }
    void shadowballSpawn()
    {
        Instantiate(shadowBallPrefab_, shadowBallSpawnPoint_.transform.position, shadowBallSpawnPoint_.transform.rotation);
    }
    public void FollowAttackSpawn()
    {
        GameManager.Instance.GhostEnemyGameEvent.Send(new GhostEnemyCallFollowAttackCommand());
    }
    public void BK_EffectSpawn()
    {
        Instantiate(bk_EffectPrefab_, transform.position, transform.rotation);
    }
}
