using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharger : MonoBehaviour
{
    [SerializeField] bool isCharging_=false;
    [SerializeField] float chargingSpeed_;
    void Start()
    {
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerChargingButtonTrigger, cmd => { isCharging_ = cmd.ChargingButtonIsPressed; });
    }

    private void FixedUpdate()
    {
        if (isCharging_) 
        {
            PlayerStatCalculator.PlayerAddOrMinusSpirit(chargingSpeed_ * Time.fixedDeltaTime);
        }
    }
}
