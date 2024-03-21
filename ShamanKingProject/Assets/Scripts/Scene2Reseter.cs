using Gamemanager;
using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene2Reseter : MonoBehaviour
{
    [SerializeField] List<GameObject> wave3Use_;
    [SerializeField] GameObject mainPlayerObject_;
    [SerializeField] GameObject ghostGameObject_;
    [SerializeField] CheckPointDatabase checkPointDatabase_;
    private void Start()
    {
        var waveCount = GameManager.Instance.MainGameMediator.RealTimePlayerData.PlayerNowCheckPoint;
        switch (waveCount)
        {
            case 3:
                GameManager.Instance.MainGameEvent.Send(new GameConversationEndCommand());
                skipToWave4();
                return;           
        }
    }

    void skipToWave4()
    {
        foreach (var item in wave3Use_)
        {
            item.SetActive(false);
        }
        skipToWavePosition(3);
    }
    void skipToWavePosition(int waveID)
    {
        mainPlayerObject_.transform.position = checkPointDatabase_.Database[waveID].PlayerTransformPosition;
        mainPlayerObject_.transform.rotation = Quaternion.Euler(checkPointDatabase_.Database[waveID].PlayerTransformRotation);
        ghostGameObject_.transform.position = checkPointDatabase_.Database[waveID].GhostTransformPosition;
        ghostGameObject_.transform.rotation = Quaternion.Euler(checkPointDatabase_.Database[waveID].GhostTransformRotation);
    }
}
