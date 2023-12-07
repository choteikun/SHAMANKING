using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class Scene1WaveManager : MonoBehaviour
{
    [SerializeField] int wave1GhostKilled_ = 0;
    [SerializeField] int wave2GhostKilled_ = 0;
    [SerializeField] int wave3GhostKilled_ = 0;
    [SerializeField] GameObject[] waveWalls_;
    void Start()
    {
        
    }
    
    void Update()
    {
        
    }
    void checkWave1GhostKilled()
    {
        wave1GhostKilled_++;
        if (wave1GhostKilled_>=1)
        {
            DialogueManager.StartConversation("chapter 1_1_2");
        }
    }

    void checkWave2GhostKilled()
    {
        wave2GhostKilled_++;
        if (wave1GhostKilled_ >= 4)
        {
            DialogueManager.StartConversation("chapter 1_2_2");
        }
    }
    void checkWave3GhostKilled() 
    {

    }
}
