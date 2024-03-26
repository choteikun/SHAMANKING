using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUIController : MonoBehaviour
{
    [SerializeField] GameObject redHealthBar_;
    [SerializeField] GameObject grayHealthBar_;
    [SerializeField] GameObject barEndPoint_;
    [SerializeField] Vector3 startPos_;
    [SerializeField] float barMaxRange_ = 116.2f;
    [SerializeField] TextMeshProUGUI healthNumber_;
    [SerializeField] Image defenceBarImage_;
    [SerializeField] TextMeshProUGUI potionRemainUI_;
    [SerializeField] GameObject useUI_;
    [SerializeField] GameObject breakUI_;

    Tweener redHealthBarTweener_;
    Tweener grayHealthBarTweener_;

    void Start()
    {
        startPos_ = grayHealthBar_.transform.position;
        barMaxRange_ = (barEndPoint_.transform.position - startPos_).magnitude;
        GameManager.Instance.UIGameEvent.SetSubscribe(GameManager.Instance.UIGameEvent.OnCallPlayerHealthBarUIUpdate, cmd => { 
            playerHealthChangeAnimation(); playerPotionRemainUIUpdate(); });
        GameManager.Instance.UIGameEvent.SetSubscribe(GameManager.Instance.UIGameEvent.OnSystemCallDefenceUIUpdate, cmd => { playerDefenceBarUpdate(cmd.Percentage); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerPlayerEnterOrLeaveEnviormentObject, cmd => { playerEnterOrLeaveEnviormentMachine(cmd.EnterOrLeave); });
        GameManager.Instance.UIGameEvent.SetSubscribe(GameManager.Instance.UIGameEvent.OnSystemCallCanBreakUIUpdate, cmd => { breakUI_.SetActive(cmd.CanBreak); });
        playerHealthChangeAnimation();
    }

    void playerHealthChangeAnimation()
    {
        if (redHealthBarTweener_ != null)
        {
            redHealthBarTweener_.Kill();
        }
        if (grayHealthBarTweener_ != null)
        {
            grayHealthBarTweener_.Kill();
        }
        var healthPercentage = GameManager.Instance.MainGameMediator.RealTimePlayerData.PlayerNowHealthPoint / GameManager.Instance.MainGameMediator.RealTimePlayerData.PlayerMaxHealthPoint;
        var moveDistance = barMaxRange_ * (1f - healthPercentage);
        var finalPos = startPos_ - new Vector3(moveDistance, 0f, 0f);
        healthNumber_.text = Mathf.RoundToInt(GameManager.Instance.MainGameMediator.RealTimePlayerData.PlayerNowHealthPoint).ToString() + "/" + GameManager.Instance.MainGameMediator.RealTimePlayerData.PlayerMaxHealthPoint.ToString();
        grayHealthBarTweener_ = grayHealthBar_.transform.DOMove(finalPos, 0.4f);
        redHealthBarTweener_ = redHealthBar_.transform.DOMove(finalPos, 0.1f);
    }

    void playerDefenceBarUpdate(float percentage)
    {
        defenceBarImage_.DOFillAmount(percentage,0.35f);
    }

    void playerPotionRemainUIUpdate()
    {
        var remain = GameManager.Instance.MainGameMediator.RealTimePlayerData.PlayerPotionCount;
        potionRemainUI_.text = "X" + remain.ToString();
    }

    void playerEnterOrLeaveEnviormentMachine(bool enterOrLeave)
    {
        useUI_.SetActive(enterOrLeave);
    }
}
