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
}
