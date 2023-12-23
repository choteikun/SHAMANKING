using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalJumpAttackBehavior : MonoBehaviour
{
    [SerializeField] GameObject jumpEndPoint_;
    [SerializeField] float distance_;
    void Start()
    {
        GameManager.Instance.HellDogGameEvent.SetSubscribe(GameManager.Instance.HellDogGameEvent.OnBossCallJumpAttackLocate, cmd => { locatePlayerPosition(); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void locatePlayerPosition()
    {
        jumpEndPoint_.transform.position = GameManager.Instance.MainGameMediator.RealTimePlayerData.PlayerGameObject.transform.position;
        var vector = GameManager.Instance.MainGameMediator.RealTimePlayerData.PlayerGameObject.transform.position - this.transform.position;
        distance_ = vector.magnitude;
    }
}
