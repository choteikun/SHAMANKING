using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamemanager;

public class TutorialTrigger : MonoBehaviour
{
    [SerializeField] GameObject tutorialTrigger_;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.MainGameEvent.Send(new SystemCallWaveClearCommand() { SceneName = "Scene1", WaveID = 0 });
            tutorialTrigger_.SetActive(false);
        }
    }
}
