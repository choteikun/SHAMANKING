using Cysharp.Threading.Tasks;
using Gamemanager;
using UnityEditor;
using UnityEngine;

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
        if (realTimePlayerData.PlayerNowHealthPoint<=0)
        {
            GameManager.Instance.MainGameEvent.Send(new SystemCallPlayerGameoverCommand());
        }
        else
        {
        }
    }
    public static void PlayerAddMaxHealth(float amount)
    {
        var realTimePlayerData = GameManager.Instance.MainGameMediator.RealTimePlayerData;
        realTimePlayerData.PlayerMaxHealthPoint = Mathf.Clamp(realTimePlayerData.PlayerMaxHealthPoint + amount, 0, 9999);
        realTimePlayerData.PlayerNowHealthPoint = realTimePlayerData.PlayerMaxHealthPoint;
        GameManager.Instance.UIGameEvent.Send(new UICallPlayerHealthBarUIUpdateCommand());
    }

    public static void PlayerGuardingSwitch(bool trigger)
    {
        var realTimePlayerData = GameManager.Instance.MainGameMediator.RealTimePlayerData;
        realTimePlayerData.PlayerGuarding = trigger;
        GameManager.Instance.MainGameEvent.Send(new CallGuardParticle() { trigger = trigger });
        if (trigger)
        {
            //realTimePlayerData.PlayerGuardPoint = realTimePlayerData.PlayerMaxGuardPoint;
            //GameManager.Instance.UIGameEvent.Send(new SystemCallDefenceUIUpdateCommand() { Percentage = 1 });
            StartCountingAsync();
        }
    }
    public static void PlayerAddOrMinusHealthGuardPoint(float amount)
    {
        GameManager.Instance.MainGameMediator.RealTimePlayerData.PlayerGuardPoint = Mathf.Clamp(GameManager.Instance.MainGameMediator.RealTimePlayerData.PlayerGuardPoint + amount, 0, GameManager.Instance.MainGameMediator.RealTimePlayerData.PlayerMaxGuardPoint);
        var percentage = GameManager.Instance.MainGameMediator.RealTimePlayerData.PlayerGuardPoint / GameManager.Instance.MainGameMediator.RealTimePlayerData.PlayerMaxGuardPoint;
        GameManager.Instance.UIGameEvent.Send(new SystemCallDefenceUIUpdateCommand() { Percentage = percentage });
        GameManager.Instance.MainGameEvent.Send(new GameCallSoundEffectGenerate() { SoundEffectID = 24 });
    }

    public static void PlayerClearWaveWriteData(int waveID)
    {
        GameManager.Instance.MainGameMediator.RealTimePlayerData.PlayerCheckPointData.PlayerClearedWave[waveID] = true;
    }
    public static void ChangePlayerInputType(NowGameplayType type)
    {
        var realTimePlayerData = GameManager.Instance.MainGameMediator.RealTimePlayerData;
        realTimePlayerData.NowGameplayType = type;
        GameManager.Instance.MainGameEvent.Send(new SystemCallInputTypeChangeCommand() { GameplayType = type });
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

    public static void ChangeGameVolume(int amount)
    {
        var realTimePlayerData = GameManager.Instance.MainGameMediator.RealTimePlayerData;
        realTimePlayerData.GameVolume = Mathf.Clamp(realTimePlayerData.GameVolume + amount, 0, 10);
        AudioListener.volume = realTimePlayerData.GameVolume * 0.1f;
        GameManager.Instance.UIGameEvent.Send(new VolumeUIUpdateCommand());

    }
}
