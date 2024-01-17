using Datamanager;
using Gamemanager;
using UnityEngine;

public class GhostEnemyAnimationEvent : MonoBehaviour
{
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
        //var shadowballPrefab = GameContainer.Get<DataManager>().GetDataByID<GameEffectTemplete>(9).PrefabPath;
        //var fireballObject = Instantiate(shadowballPrefab, shadowBallSpawnPoint_.transform.position, shadowBallSpawnPoint_.transform.rotation);
        //fireballObject.transform.LookAt(shadowBallSpawnPoint_.transform);
    }
}
