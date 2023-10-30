using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Gamemanager;
using PixelCrushers.DialogueSystem;

public class TutorialSystemController : MonoBehaviour
{
    [SerializeField] GameObject tutorialCanvas_;
    private void Start()
    {
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnSystemCallTutorial, cmd => { callTutorial(cmd); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerEndTutorial, cmd => { closeTutorial(cmd); });
    }

    void callTutorial(SystemCallTutorialCommand cmd)
    {
        Time.timeScale = 0;
        tutorialCanvas_.SetActive(true);
    }

    void closeTutorial(PlayerEndTutorialCommand cmd)
    {
        Time.timeScale = 1;
        tutorialCanvas_.SetActive(false);
        tutorialEndEvent(cmd.TutorialID);
    }

    void tutorialEndEvent(int tutorialID)
    {
        switch(tutorialID)
        {
            case 3:
                DialogueManager.StartConversation("AfterBattleTutorial");
                return;
        }
    }
}
