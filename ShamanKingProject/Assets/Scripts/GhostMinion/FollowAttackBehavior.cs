using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FollowAttackBehavior : MonoBehaviour
{
    float smoothSpeed_ = 0f;
    void Start()
    {
        DOTween.To(() => smoothSpeed_, x => smoothSpeed_ = x, 15, 15f);
    }

    private void LateUpdate()
    {
        follower();
    }

    void follower()
    {
        var target = GameManager.Instance.MainGameMediator.RealTimePlayerData.PlayerGameObject;
        if (target != null)
        {

        transform.position = Vector3.Lerp(transform.position, target.transform.position, smoothSpeed_ * Time.deltaTime);
        }
    }
         
}
