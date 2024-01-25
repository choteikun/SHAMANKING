using Datamanager;
using Gamemanager;
using UnityEngine;

public class GhostEnemyAnimationEvent : MonoBehaviour
{
    [SerializeField] GameObject shadowBallPrefab;
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
        Instantiate(shadowBallPrefab, shadowBallSpawnPoint_.transform.position, shadowBallSpawnPoint_.transform.rotation);
    }
    public void FollowAttackSpawn()
    {
        GameManager.Instance.GhostEnemyGameEvent.Send(new GhostCallFollowAttackCommand());
    }
}
