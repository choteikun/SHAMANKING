using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Gamemanager;

public class TutorialSystemController : MonoBehaviour
{
    [SerializeField] GameObject tutorialCanvas_;
    private void Start()
    {
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnSystemCallTutorial, cmd => { callTutorial(cmd); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerEndTutorial, cmd => { closeTutorial(); });
    }

    void callTutorial(SystemCallTutorialCommand cmd)
    {
        Time.timeScale = 0;
        tutorialCanvas_.SetActive(true);
    }

    void closeTutorial()
    {
        Time.timeScale = 1;
        tutorialCanvas_.SetActive(false);
    }
}
