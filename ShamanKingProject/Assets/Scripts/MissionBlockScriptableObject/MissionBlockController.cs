using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionBlockController : MonoBehaviour
{
    [SerializeField] NowGameplayType gameplayType_;
    [SerializeField] TextMeshProUGUI missionNameText_;
    [SerializeField] TextMeshProUGUI missionDescription_;
    [SerializeField] GameObject missionObject_;
    [SerializeField] GameObject missionStartPos_;
    [SerializeField] GameObject missionEndPos_;
    [SerializeField] Image missionInputPic_;
    [SerializeField] MissionBlockObject nowMissionBlock_;
    void Start()
    {       
        GameManager.Instance.UIGameEvent.SetSubscribe(GameManager.Instance.UIGameEvent.OnSystemCallMissionUIUpdate, cmd => { missionStart(cmd.MissionData); });
        GameManager.Instance.UIGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnSystemCallWaveClear, cmd => { missionEnd(cmd.WaveID); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnSystemCallInputTypeChange, cmd => { gameplayType_ = cmd.GameplayType; });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnGhostKilled, cmd => 
        {
            if (cmd.KilledName == "Wave2_GhostEnemy")
            {
                changeWave2MissionUIText(cmd.KilledAmount);
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (nowMissionBlock_ == null) return;
        switch (gameplayType_)
        {   
            case NowGameplayType.PlayStation:
                if (nowMissionBlock_.InputTutorialPicture_PS != null)
                {
                    missionInputPic_.sprite = nowMissionBlock_.InputTutorialPicture_PS;
                }
                break;
            case NowGameplayType.XBox:
                if (nowMissionBlock_.InputTutorialPicture_XB != null)
                {
                    missionInputPic_.sprite = nowMissionBlock_.InputTutorialPicture_XB;
                }
                break;
            case NowGameplayType.Keyboard:
                if (nowMissionBlock_.InputTutorialPicture != null)
                {
                    missionInputPic_.sprite = nowMissionBlock_.InputTutorialPicture;
                }
                break;
            default:
                break;
        }
        
    }
    void missionStart(MissionBlockObject missionOBJ)
    {
        missionObject_.transform.DOMove(missionEndPos_.transform.position, 0.6f);
        missionNameText_.text = missionOBJ.MissionName;
        missionDescription_.text = missionOBJ.MissionDescription;
        nowMissionBlock_ = missionOBJ;
        
    }

    void missionEnd(int waveData)
    {
        missionObject_.transform.DOMove(missionStartPos_.transform.position, 0.6f);
        PlayerStatCalculator.PlayerClearWaveWriteData(waveData);
    }
   
    void changeWave2MissionUIText(int amount)
    {
        missionDescription_.text = "À»±Ñ¦¨¸sªº±r«Þ¹C»î " + amount.ToString() + " / 2";
    }

}
