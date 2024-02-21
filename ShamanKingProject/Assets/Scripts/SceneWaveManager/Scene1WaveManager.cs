using PixelCrushers.DialogueSystem;
using UnityEngine;
using DG.Tweening;

public class Scene1WaveManager : MonoBehaviour
{
    [SerializeField] int wave1GhostKilled_ = 0;
    [SerializeField] int wave2GhostKilled_ = 0;
    [SerializeField] int wave3GhostKilled_ = 0;
    [SerializeField] GameObject[] waveWalls_;
    [SerializeField] GhostSpawner[] wave2Spawners_;
    [SerializeField] GameObject BossSceneTransfer_;
    [SerializeField] int wave2Count_;
    [SerializeField] GameObject wayPointPrefab_;
    [SerializeField] GameObject wayPointObject_;
    [SerializeField] GameObject wayPointEndPos_;
    void Start()
    {
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnSystemCallWaveStart, cmd =>
        {
            if (cmd.SceneName == "Scene1")
            {
                waveWalls_[cmd.WaveID].SetActive(true);
                switch (cmd.WaveID)
                {
                    case 0:
                        tutorial1Start();
                        return;
                    case 2:
                        wave2Start();
                        return;
                }
            }
        });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnGhostIdentityCheck, cmd =>
        {
            if (cmd.GhostIdentityName == "Wave1_GhostEnemy")
            {
                checkWave1GhostKilled();
            }
        });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnGhostIdentityCheck, cmd =>
        {
            if (cmd.GhostIdentityName == "Wave2_GhostEnemy")
            {
                checkWave2GhostKilled();
            }
        });

        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnSystemCallWaveClear, cmd => 
        {
            if (cmd.SceneName == "Scene1"&&cmd.WaveID == 0)
            {
                waveWalls_[0].SetActive(false);
                Destroy(wayPointObject_);
            }
        });
    }

    void Update()
    {

    }
    void wave2Start()
    {
        foreach (var spawners in wave2Spawners_)
        {
            spawners.SpawnGhost();
        }
    }
    void checkWave1GhostKilled()
    {
        wave1GhostKilled_++;
        if (wave1GhostKilled_ >= 1)
        {
            waveWalls_[1].SetActive(false);
            DialogueManager.StartConversation("chapter 1_1_2");
        }
    }

    void checkWave2GhostKilled()
    {
        wave2GhostKilled_++;
        if (wave2GhostKilled_ >= wave2Count_)
        {
            waveWalls_[2].SetActive(false);
            BossSceneTransfer_.SetActive(true);
            //DialogueManager.StartConversation("chapter1_2_2");
        }
    }
    void checkWave3GhostKilled()
    {

    }
    void tutorial1Start()
    {
        var wayPointStartPosition = GameManager.Instance.MainGameMediator.RealTimePlayerData.PlayerGameObject.transform.position;
        wayPointObject_ = Instantiate(wayPointPrefab_, wayPointStartPosition, Quaternion.identity);
        wayPointObject_.transform.DOMove(wayPointEndPos_.transform.position, 2.65f);
    }
}
