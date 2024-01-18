using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class RealTimePlayerData
{
    public int GhostNowGageBlockAmount => (int)GhostSoulGageCurrentAmount / 100;
    public float GhostSoulGageCurrentAmount = 0;
    public float GhostSoulGageMaxAmount = 100;
    public float PlayerMaxHealthPoint = 100;
    public float PlayerNowHealthPoint = 100;
    public float PlayerGuardPoint = 30;
    public float PlayerMaxGuardPoint = 30;
    public float PlayerGuardPerfectTimerFrame = 0;
    public float PlayerGuardPerfectTimerMaxFrame = 25;
    public float PlayerBasicAttackPercentage = 1;
    public bool PlayerInvincible = false;
    public bool PlayerGuarding = false;
    public GameObject PlayerGameObject;
}
