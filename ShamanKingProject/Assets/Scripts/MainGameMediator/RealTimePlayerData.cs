using UnityEngine;
using Gamemanager;
[System.Serializable]
public class RealTimePlayerData
{
    public int GhostNowGageBlockAmount => (int)GhostSoulGageCurrentAmount / 100;
    public float GhostSoulGageCurrentAmount = 0;
    public float GhostSoulGageMaxAmount = 400;
    public float PlayerMaxHealthPoint = 100;
    public float PlayerNowHealthPoint = 100;
    public float PlayerGuardPoint = 30;
    public float PlayerMaxGuardPoint = 30;
    public int PlayerGuardingResetTimer = 0;
    public float PlayerGuardPerfectTimerFrame = 0;
    public float PlayerGuardPerfectTimerMaxFrame = 25;
    public float PlayerBasicAttackPercentage = 1;
    public bool PlayerInvincible = false;
    public bool PlayerGuarding = false;
    public int PlayerPotionCount = 3;
    public GameObject PlayerGameObject;
    public PlayerCheckPointData PlayerCheckPointData = new PlayerCheckPointData();
    public int PlayerNowCheckPoint = -1;
    public int GameVolume = 10;
    public bool[] SpecialConversionTrigger = new bool[2];

    public void SpecialConversationCheck(int num)
    {
        if (SpecialConversionTrigger[num]==false)
        {
            SpecialConversionTrigger[num] = true;
            GameManager.Instance.MainGameEvent.Send(new SystemCallSpecialConversationCommand() { TriggerNum = num });
        }
    }
    public void Refresh()
    {
        PlayerPotionCount = 3;
        GhostSoulGageCurrentAmount = 0;
        PlayerNowHealthPoint = PlayerMaxHealthPoint;
        PlayerGuardPoint = PlayerMaxGuardPoint;
        PlayerGuardingResetTimer = 0;
        PlayerGuardPerfectTimerFrame = 0;
        PlayerInvincible = false;
        PlayerGuarding = false;
        PlayerCheckPointData = new PlayerCheckPointData();
        PlayerNowCheckPoint = -1;
    }

    public int GetNowHighestWaveCount()
    {
        int count = 0;
        for (int i = 0; i < PlayerCheckPointData.PlayerClearedWave.Length; i++)
        {
            if (PlayerCheckPointData.PlayerClearedWave[i])
            {
                count = i;
            }
        }
        return count;
    }

    public void Revive()
    {
        PlayerPotionCount = 3;
        GhostSoulGageCurrentAmount = 0;
        PlayerNowHealthPoint = PlayerMaxHealthPoint;
        PlayerInvincible = false;
        PlayerGuarding = false;
        GameManager.Instance.UIGameEvent.Send(new UICallPlayerHealthBarUIUpdateCommand());
    }

    public void PotionUsed()
    {       
        if (PlayerPotionCount > 0)
        {
            PlayerPotionCount--;
            PlayerNowHealthPoint = PlayerMaxHealthPoint;
            GameManager.Instance.UIGameEvent.Send(new UICallPlayerHealthBarUIUpdateCommand());
        }
    }


}
