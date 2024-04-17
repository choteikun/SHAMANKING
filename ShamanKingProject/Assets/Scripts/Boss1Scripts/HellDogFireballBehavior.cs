using Datamanager;
using UnityEngine;
using Gamemanager;

public class HellDogFireballBehavior : MonoBehaviour
{
    [SerializeField] GameObject fireBallEndPoint_;
    [SerializeField] GameObject fireBallSpawnPoint_;
    [SerializeField] GameObject fireTrackBallPrefab;
    [SerializeField] int triggeredFireBall_ = 0;
    void Start()
    {
        GameManager.Instance.HellDogGameEvent.SetSubscribe(GameManager.Instance.HellDogGameEvent.OnSystemCallFireballLocateCommand, cmd => { fireBallLocate(); });
        GameManager.Instance.HellDogGameEvent.SetSubscribe(GameManager.Instance.HellDogGameEvent.OnSystemCallFireballSpawn, cmd => { fireBallSpawn(); });
        GameManager.Instance.HellDogGameEvent.SetSubscribe(GameManager.Instance.HellDogGameEvent.OnSystemCallFireTrackBallSpawn, cmd => { trackFireBall(); });
        GameManager.Instance.HellDogGameEvent.SetSubscribe(GameManager.Instance.HellDogGameEvent.OnBossFireTrackBallTrigger, cmd => { fireTrackCamTransfer(); });
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
    void trackFireBall()
    {
        Instantiate(fireTrackBallPrefab, fireBallSpawnPoint_.transform.position, fireBallSpawnPoint_.transform.rotation);
    }
    void fireTrackCamTransfer()
    {
        triggeredFireBall_++;
        if (triggeredFireBall_ == 3)
        {
            triggeredFireBall_ = 0;
            GameManager.Instance.HellDogGameEvent.Send(new BossCallUltCamTransfer() { trigger = false });
        }
    }
}
