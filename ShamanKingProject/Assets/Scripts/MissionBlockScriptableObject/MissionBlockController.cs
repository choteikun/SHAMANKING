using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionBlockController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI missionNameText_;
    [SerializeField] TextMeshProUGUI missionDescription_;
    [SerializeField] GameObject missionObject_;
    [SerializeField] GameObject missionStartPos_;
    [SerializeField] GameObject missionEndPos_;
    [SerializeField] Image missionInputPic_;
    void Start()
    {       
        GameManager.Instance.UIGameEvent.SetSubscribe(GameManager.Instance.UIGameEvent.OnSystemCallMissionUIUpdate, cmd => { missionStart(cmd.MissionData); });
        GameManager.Instance.UIGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnSystemCallWaveClear, cmd => { missionEnd(cmd.WaveID); });
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

    }
    void missionStart(MissionBlockObject missionOBJ)
    {
        missionObject_.transform.DOMove(missionEndPos_.transform.position, 0.6f);
        missionNameText_.text = missionOBJ.MissionName;
        missionDescription_.text = missionOBJ.MissionDescription;
        if (missionOBJ.InputTutorialPicture!=null)
        {
            missionInputPic_.sprite = missionOBJ.InputTutorialPicture;
        }
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
