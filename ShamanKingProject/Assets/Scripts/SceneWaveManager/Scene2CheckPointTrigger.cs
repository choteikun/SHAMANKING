using Gamemanager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene2CheckPointTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.MainGameMediator.RealTimePlayerData.PotionUsed();
            GameManager.Instance.MainGameEvent.Send(new SystemCallWaveClearCommand() { SceneName = "Scene2", WaveID = 3 });
        }
    }
}
