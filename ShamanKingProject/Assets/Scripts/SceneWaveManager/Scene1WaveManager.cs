using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using static Unity.Burst.Intrinsics.X86.Avx;
using Gamemanager;

public class Scene1WaveManager : MonoBehaviour
{
    [SerializeField] int wave1GhostKilled_ = 0;
    [SerializeField] int wave2GhostKilled_ = 0;
    [SerializeField] int wave3GhostKilled_ = 0;
    [SerializeField] GameObject[] waveWalls_;
    [SerializeField] GhostSpawner[] wave2Spawners_;
    void Start()
    {
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnSystemCallWaveStart, cmd =>
        {
        if (cmd.SceneName == "Scene1")
            {
                waveWalls_[cmd.WaveID-1].SetActive(true);
                switch(cmd.WaveID)
                {
                    case 2:
                        wave2Start();
                        return;
                }
            } });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnGhostIdentityCheck, cmd =>
        {
            if (cmd.GhostIdentityName == "Wave1_GhostEnemy")
            {
                checkWave1GhostKilled();
            } });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnGhostIdentityCheck, cmd =>
        {
            if (cmd.GhostIdentityName == "Wave2_GhostEnemy(Clone)")
            {
                checkWave2GhostKilled();
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
        if (wave1GhostKilled_>=1)
        {
            waveWalls_[0].SetActive(false);
            DialogueManager.StartConversation("chapter 1_1_2");
        }
    }

    void checkWave2GhostKilled()
    {
        wave2GhostKilled_++;
        if (wave2GhostKilled_ >= 4)
        {
            waveWalls_[1].SetActive(false);
            DialogueManager.StartConversation("chapter1_2_2");
        }
    }
    void checkWave3GhostKilled() 
    {

    }
}
