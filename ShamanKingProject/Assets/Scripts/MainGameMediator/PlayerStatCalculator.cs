using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamemanager;
public static class PlayerStatCalculator 
{
    public static void PlayerAddOrMinusSpirit(int amount)
    {
        var realTimePlayerData = GameManager.Instance.MainGameMediator.RealTimePlayerData;
        realTimePlayerData.GhostNowEatAmount = Mathf.Clamp(realTimePlayerData.GhostNowEatAmount + amount, 0, realTimePlayerData.GhostEatAmountMax);
        GameManager.Instance.UIGameEvent.Send(new UISpiritUpdateCommand());
    }
}
