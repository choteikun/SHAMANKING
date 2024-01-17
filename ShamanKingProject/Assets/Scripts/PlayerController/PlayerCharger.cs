using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharger : MonoBehaviour
{
    [SerializeField] bool isCharging_=false;
    [SerializeField] float chargingSpeed_;
    void Start()
    {
        //GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerGuardingButtonTrigger, cmd => { isCharging_ = cmd.GuardingButtonIsPressed; });
    }

    private void FixedUpdate()
    {
        if (isCharging_) 
        {
            PlayerStatCalculator.PlayerAddOrMinusSpirit(chargingSpeed_ * Time.fixedDeltaTime);
        }
    }
}
