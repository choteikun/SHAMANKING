using DG.Tweening;
using TMPro;
using UnityEngine;

public class MissionBlockController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI missionNameText_;
    [SerializeField] TextMeshProUGUI missionDescription_;
    [SerializeField] GameObject missionObject_;
    [SerializeField] Vector3 missionStartPos_;
    [SerializeField] GameObject missionEndPos_;
    void Start()
    {
        missionStartPos_ = missionObject_.transform.position;
        GameManager.Instance.UIGameEvent.SetSubscribe(GameManager.Instance.UIGameEvent.OnSystemCallMissionUIUpdate, cmd => { missionStart(cmd.MissionData); });
        GameManager.Instance.UIGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnSystemCallWaveClear, cmd => { missionEnd(); });
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
    }

    void missionEnd()
    {
        missionObject_.transform.DOMove(missionStartPos_, 0.6f);
    }
   
    void changeWave2MissionUIText(int amount)
    {
        missionDescription_.text = "���Ѧ��s���r�޹C�� " + amount.ToString() + " / 3";
    }
}
