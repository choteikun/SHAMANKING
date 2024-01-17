using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamemanager;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;
using Cysharp.Threading.Tasks;

public static class PlayerStatCalculator 
{
    public static void PlayerAddOrMinusSpirit(float amount)
    {
        var realTimePlayerData = GameManager.Instance.MainGameMediator.RealTimePlayerData;
        realTimePlayerData.GhostSoulGageCurrentAmount = Mathf.Clamp(realTimePlayerData.GhostSoulGageCurrentAmount + amount, 0, realTimePlayerData.GhostSoulGageMaxAmount);
        GameManager.Instance.UIGameEvent.Send(new UISoulGageUpdateCommand());
        //if (GameManager.Instance.MainGameMediator.RealTimePlayerData.GhostSoulGageCurrentAmount == GameManager.Instance.MainGameMediator.RealTimePlayerData.GhostSoulGageMaxAmount)
        //{
        //    GameManager.Instance.MainGameEvent.Send(new SystemStopChargingCommand());
        //}
    }
    public static void PlayerInvincibleSwitch(bool trigger)
    {
        var realTimePlayerData = GameManager.Instance.MainGameMediator.RealTimePlayerData;
        realTimePlayerData.PlayerInvincible = trigger;
        GameManager.Instance.UIGameEvent.Send(new UIPlayerInvincibleUpdateCommand());
    }
   
    public static void PlayerAddOrMinusHealth(float amount)
    {
        var realTimePlayerData = GameManager.Instance.MainGameMediator.RealTimePlayerData;
        realTimePlayerData.PlayerNowHealthPoint = Mathf.Clamp(realTimePlayerData.PlayerNowHealthPoint + amount, 0, realTimePlayerData.PlayerMaxHealthPoint);
        GameManager.Instance.UIGameEvent.Send(new UICallPlayerHealthBarUIUpdateCommand());
    }

    public static void PlayerGuardingSwitch(bool trigger)
    {
        var realTimePlayerData = GameManager.Instance.MainGameMediator.RealTimePlayerData;
        realTimePlayerData.PlayerGuarding = trigger;
        if (trigger)
        {
            realTimePlayerData.PlayerGuardPoint = realTimePlayerData.PlayerMaxGuardPoint;
            StartCountingAsync();
        }
    }
    async static void StartCountingAsync()
    {
        await CountAsync();

        // 达到50时输出消息
        Debug.Log("Counter reached 50!");
    }

    async static UniTask CountAsync()
    {
        var realTimePlayerData = GameManager.Instance.MainGameMediator.RealTimePlayerData;
        realTimePlayerData.PlayerGuardPerfectTimerFrame = 0;
        while (realTimePlayerData.PlayerGuardPerfectTimerFrame < realTimePlayerData.PlayerGuardPerfectTimerMaxFrame)
        {
            // 模拟一帧的等待
            await UniTask.Yield();

            // 计数器递增
            realTimePlayerData.PlayerGuardPerfectTimerFrame++;
        }
    }
}
