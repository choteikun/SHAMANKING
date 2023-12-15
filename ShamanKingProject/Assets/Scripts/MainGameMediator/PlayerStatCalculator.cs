using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamemanager;
public static class PlayerStatCalculator 
{
    public static void PlayerAddOrMinusSpirit(float amount)
    {
        var realTimePlayerData = GameManager.Instance.MainGameMediator.RealTimePlayerData;
        realTimePlayerData.GhostSoulGageCurrentAmount = Mathf.Clamp(realTimePlayerData.GhostSoulGageCurrentAmount + amount, 0, realTimePlayerData.GhostSoulGageMaxAmount);
        GameManager.Instance.UIGameEvent.Send(new UISoulGageUpdateCommand());
        if (GameManager.Instance.MainGameMediator.RealTimePlayerData.GhostSoulGageCurrentAmount == GameManager.Instance.MainGameMediator.RealTimePlayerData.GhostSoulGageMaxAmount)
        {
            GameManager.Instance.MainGameEvent.Send(new SystemStopChargingCommand());
        }
    }
}
