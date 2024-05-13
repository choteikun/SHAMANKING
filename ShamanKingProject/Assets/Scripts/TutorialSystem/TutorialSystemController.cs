using Gamemanager;
using PixelCrushers.DialogueSystem;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialSystemController : MonoBehaviour
{
    [SerializeField] NowGameplayType gameplayType;
    [SerializeField] GameObject tutorialCanvas_;
    [SerializeField] Image tutorialPic_;
    [SerializeField] Sprite[] firstTutorial_;
    [SerializeField] Sprite[] secondTutorial_;
    [SerializeField] Sprite[] thirdTutorial_;
    [SerializeField] Sprite[] fourthTutorial_;
    [SerializeField] Sprite[] firstTutorial_PS;
    [SerializeField] Sprite[] secondTutorial_PS;
    [SerializeField] Sprite[] thirdTutorial_PS;
    [SerializeField] Sprite[] fourthTutorial_PS;
    [SerializeField] Sprite[] firstTutorial_XB;
    [SerializeField] Sprite[] secondTutorial_XB;
    [SerializeField] Sprite[] thirdTutorial_XB;
    [SerializeField] Sprite[] fourthTutorial_XB;
    [SerializeField] int tutorialLastPicCount_;
    [SerializeField] List<Sprite[]> tempTutorials = new List<Sprite[]>();
    [SerializeField] List<Sprite[]> tempTutorials_PS = new List<Sprite[]>();
    [SerializeField] List<Sprite[]> tempTutorials_XB = new List<Sprite[]>();
    [SerializeField] GameObject ticketGate_;
    [SerializeField] int nowTurtorial_;
    [SerializeField] int nowPage_;
    private void Start()
    {
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnSystemCallTutorial, cmd => { callTutorial(cmd); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerTutorialNextPage, cmd => { tempNextPage(cmd); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnSystemCallInputTypeChange, cmd => { gameplayType = cmd.GameplayType; });
        tempTutorials.Add(firstTutorial_);
        tempTutorials.Add(secondTutorial_);
        tempTutorials.Add(thirdTutorial_);
        tempTutorials.Add(fourthTutorial_);
        tempTutorials_PS.Add(firstTutorial_PS);
        tempTutorials_PS.Add(secondTutorial_PS);
        tempTutorials_PS.Add(thirdTutorial_PS);
        tempTutorials_PS.Add(fourthTutorial_PS);
        tempTutorials_XB.Add(firstTutorial_XB);
        tempTutorials_XB.Add(secondTutorial_XB);
        tempTutorials_XB.Add(thirdTutorial_XB);
        tempTutorials_XB.Add(fourthTutorial_XB);
    }

    void callTutorial(SystemCallTutorialCommand cmd)
    {
        Time.timeScale = 0;
        tutorialCanvas_.SetActive(true);
        tempTutorialSystem((int)cmd.TutorialID);
    }

    void tempTutorialSystem(int tutorialID)
    {
        nowTurtorial_ = tutorialID;
        nowPage_ = 0;

        tutorialLastPicCount_ = tempTutorials[tutorialID].Count();
    }

    void tempNextPage(PlayerTutorialNextPageCommand cmd)
    {
        Debug.LogError("NextPage");
        tutorialLastPicCount_--;
        if (tutorialLastPicCount_ == 0)
        {
            closeTutorial((int)cmd.TutorialID);
        }
        else
        {
            nowPage_ = tempTutorials[(int)cmd.TutorialID].Count() - tutorialLastPicCount_;
            //tutorialPic_.sprite = tempTutorials[(int)cmd.TutorialID][tempTutorials[(int)cmd.TutorialID].Count() - tutorialLastPicCount_];
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
        switch (tutorialID)
        {
            case 0:
                if (SceneManager.GetActiveScene().buildIndex == 5)
                {
                    GameManager.Instance.MainGameEvent.Send(new SystemCallWaveStartCommand() { SceneName = "Scene1", WaveID = 0 });
                    GameManager.Instance.UIGameEvent.Send(new SystemCallMissionUIUpdateCommand() { MissionData = GameManager.Instance.MissionBlockDatabase.Database[0] });
                }
                if (SceneManager.GetActiveScene().buildIndex == 3)
                {
                    GameManager.Instance.UIGameEvent.Send(new SystemCallMissionUIUpdateCommand() { MissionData = GameManager.Instance.MissionBlockDatabase.Database[4] });
                }
                return;
            case 1:
                GameManager.Instance.UIGameEvent.Send(new SystemCallMissionUIUpdateCommand() { MissionData = GameManager.Instance.MissionBlockDatabase.Database[2] });
                return;
            case 2:
                GameManager.Instance.UIGameEvent.Send(new SystemCallMissionUIUpdateCommand() { MissionData = GameManager.Instance.MissionBlockDatabase.Database[1] });
                return;
            case 3:
                DialogueManager.StartConversation("chapter 1_4_2");
                GameManager.Instance.UIGameEvent.Send(new SystemCallMissionUIUpdateCommand() { MissionData = GameManager.Instance.MissionBlockDatabase.Database[3] });
                return;
        }
    }
    private void Update()
    {
        switch (gameplayType)
        {
            case NowGameplayType.PlayStation:
                tutorialPic_.sprite = tempTutorials_PS[nowTurtorial_][nowPage_];
                break;
            case NowGameplayType.XBox:
                tutorialPic_.sprite = tempTutorials_XB[nowTurtorial_][nowPage_];
                break;
            case NowGameplayType.Keyboard:
                tutorialPic_.sprite = tempTutorials[nowTurtorial_][nowPage_];
                break;
            default:
                break;
        }
    }
}
