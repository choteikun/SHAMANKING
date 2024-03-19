using Gamemanager;
using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Scene1Reseter : MonoBehaviour
{
    [SerializeField] List<GameObject> wave0Use_;
    [SerializeField] List<GameObject> wave1Use_;
    [SerializeField] GameObject mainPlayerObject_;
    [SerializeField] GameObject ghostGameObject_;
    [SerializeField] CheckPointDatabase checkPointDatabase_;
    private void Start()
    {
      var waveCount =  GameManager.Instance.MainGameMediator.RealTimePlayerData.PlayerNowCheckPoint;
        switch(waveCount)
        {
            case -1:
                DialogueManager.StartConversation("¶}§½¤¶²Ð");
                return;
            case 0:
                GameManager.Instance.MainGameEvent.Send(new GameConversationEndCommand());
                skipToWave1();
                return;
            case 1:
                GameManager.Instance.MainGameEvent.Send(new GameConversationEndCommand());
                skipToWave2();
                return;
        }
    }
    void skipToWave1()
    {
        foreach (var item in wave0Use_)
        {
            item.SetActive(false);
        }
        skipToWavePosition(1);
    }
    void skipToWave2()
    {
        foreach (var item in wave0Use_)
        {
            item.SetActive(false);
        }
        foreach (var item in wave1Use_)
        {
            item.SetActive(false);
        }
        skipToWavePosition(2);
    }

    void skipToWavePosition(int waveID)
    {
        mainPlayerObject_.transform.position = checkPointDatabase_.Database[waveID].PlayerTransformPosition;
        mainPlayerObject_.transform.rotation = Quaternion.Euler(checkPointDatabase_.Database[waveID].PlayerTransformRotation);
        ghostGameObject_.transform.position = checkPointDatabase_.Database[waveID].GhostTransformPosition;
        ghostGameObject_.transform.rotation = Quaternion.Euler(checkPointDatabase_.Database[waveID].GhostTransformRotation);
    }
}
