using PixelCrushers.DialogueSystem;
using UnityEngine;
using Gamemanager;

public class Scene2WaveManager : MonoBehaviour
{
    [SerializeField] int EliteGhostKilled_ = 0;
    [SerializeField] GameObject BossSceneTransfer_;
    [SerializeField] GameObject eliteGhostUI_;
    void Start()
    {
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnGhostIdentityCheck, cmd =>
        {
            if (cmd.GhostIdentityName == "GhostEnemy(Elite)")
            {
                checkWave4GhostKilled();
            }
        });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnSystemCallTutorial, cmd => { eliteGhostUI_.SetActive(true); });
    }

    void checkWave4GhostKilled()
    {
        EliteGhostKilled_++;
        if (EliteGhostKilled_ >= 1)
        {
            //waveWalls_[1].SetActive(false);
            eliteGhostUI_.SetActive(false);
            BossSceneTransfer_.SetActive(true);
            GameManager.Instance.MainGameEvent.Send(new SystemCallWaveClearCommand() { SceneName = "Scene2", WaveID = 4 });
            DialogueManager.StartConversation("chapter 1_4_3");
        }
    }
}
