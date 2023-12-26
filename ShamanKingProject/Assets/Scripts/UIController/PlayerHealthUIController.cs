using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using AmplifyImpostors;

public class PlayerHealthUIController : MonoBehaviour
{
    [SerializeField] GameObject redHealthBar_;
    [SerializeField] GameObject grayHealthBar_;
    [SerializeField] GameObject barEndPoint_;
    [SerializeField] Vector3 startPos_;
    [SerializeField] float barMaxRange_ = 116.2f;

    Tweener redHealthBarTweener_;
    Tweener grayHealthBarTweener_;
     
    void Start()
    {
        startPos_ = grayHealthBar_.transform.position;
        barMaxRange_ = (barEndPoint_.transform.position - startPos_).magnitude;
        GameManager.Instance.UIGameEvent.SetSubscribe(GameManager.Instance.UIGameEvent.OnCallPlayerHealthBarUIUpdate, cmd => { playerHealthChangeAnimation(); });
    }

    void playerHealthChangeAnimation()
    {
        if (redHealthBarTweener_!= null)
        {
            redHealthBarTweener_.Kill();
        }
        if(grayHealthBarTweener_!= null)
        {
            grayHealthBarTweener_.Kill();
        }
        var healthPercentage = GameManager.Instance.MainGameMediator.RealTimePlayerData.PlayerNowHealthPoint / GameManager.Instance.MainGameMediator.RealTimePlayerData.PlayerMaxHealthPoint;
        var moveDistance = barMaxRange_*(1f-healthPercentage);
        var finalPos = startPos_ - new Vector3(moveDistance, 0f, 0f);
        grayHealthBarTweener_ = grayHealthBar_.transform.DOMove(finalPos, 0.4f);
        redHealthBarTweener_ = redHealthBar_.transform.DOMove(finalPos, 0.1f);
    }
}
