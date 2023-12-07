using Datamanager;
using UnityEngine;

public class HellDogFireballBehavior : MonoBehaviour
{
    [SerializeField] GameObject fireBallEndPoint_;
    [SerializeField] GameObject fireBallSpawnPoint_;
    void Start()
    {
        GameManager.Instance.HellDogGameEvent.SetSubscribe(GameManager.Instance.HellDogGameEvent.OnSystemCallFireballLocateCommand, cmd => { fireBallLocate(); });
        GameManager.Instance.HellDogGameEvent.SetSubscribe(GameManager.Instance.HellDogGameEvent.OnSystemCallFireballSpawn, cmd => { fireBallSpawn(); });
    }

    void fireBallLocate()
    {
        fireBallEndPoint_.transform.position = GameManager.Instance.MainGameMediator.RealTimePlayerData.PlayerGameObject.transform.position;
    }

    void fireBallSpawn()
    {
        var fireballPrefab = GameContainer.Get<DataManager>().GetDataByID<GameEffectTemplete>(9).PrefabPath;
        var fireballObject = Instantiate(fireballPrefab, fireBallSpawnPoint_.transform.position, fireBallSpawnPoint_.transform.rotation);
        fireballObject.transform.LookAt(fireBallEndPoint_.transform);
    }
}
