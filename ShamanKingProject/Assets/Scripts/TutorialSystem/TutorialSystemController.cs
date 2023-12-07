using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Gamemanager;
using PixelCrushers.DialogueSystem;
using UnityEngine.UI;
using System.Linq;
using static Unity.Burst.Intrinsics.X86.Avx;

public class TutorialSystemController : MonoBehaviour
{
    [SerializeField] GameObject tutorialCanvas_;
    [SerializeField] Image tutorialPic_;
    [SerializeField] Sprite[] firstTutorial_;
    [SerializeField] Sprite[] secondTutorial_;
    [SerializeField] Sprite[] thirdTutorial_;
    [SerializeField] int tutorialLastPicCount_;
    [SerializeField] List<Sprite[]> tempTutorials = new List<Sprite[]>();
    [SerializeField] GameObject ticketGate_;
    private void Start()
    {
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnSystemCallTutorial, cmd => { callTutorial(cmd); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerTutorialNextPage, cmd => { tempNextPage(cmd); });
        tempTutorials.Add(firstTutorial_);
        tempTutorials.Add(secondTutorial_);
        tempTutorials.Add(thirdTutorial_);
    }

    void callTutorial(SystemCallTutorialCommand cmd)
    {
        Time.timeScale = 0;
        tutorialCanvas_.SetActive(true);
        tempTutorialSystem((int)cmd.TutorialID);
    }

    void tempTutorialSystem(int tutorialID)//壞掉的
    {
        tutorialPic_.sprite = tempTutorials[tutorialID][0];
        tutorialLastPicCount_ = tempTutorials[tutorialID].Count();
    }

    void tempNextPage(PlayerTutorialNextPageCommand cmd)
    {
        tutorialLastPicCount_ --;
        if (tutorialLastPicCount_ == 0)
        {
            closeTutorial((int)cmd.TutorialID);
        }
        else
        {
            tutorialPic_.sprite = tempTutorials[(int)cmd.TutorialID][tempTutorials[(int)cmd.TutorialID].Count()- tutorialLastPicCount_];
        }
    }

    void closeTutorial(int cmd)
    {
        Time.timeScale = 1;
        tutorialCanvas_.SetActive(false);
        GameManager.Instance.MainGameEvent.Send(new PlayerEndTutorialCommand() { TutorialID = cmd });
        tutorialEndEvent(cmd);
    }

    void tutorialEndEvent(int tutorialID)
    {
        switch(tutorialID)
        {
            case 1:
                ticketGate_.SetActive(true);
                return;
        }
    }
}
