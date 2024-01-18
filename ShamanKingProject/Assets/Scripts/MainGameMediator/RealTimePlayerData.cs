using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class RealTimePlayerData
{
    public int GhostNowGageBlockAmount => (int)GhostSoulGageCurrentAmount / 100;
    public float GhostSoulGageCurrentAmount = 0;
    public float GhostSoulGageMaxAmount = 100;
    public float PlayerMaxHealthPoint = 300;
    public float PlayerNowHealthPoint = 300;
    public float PlayerGuardPoint = 50;
    public float PlayerMaxGuardPoint = 50;
    public float PlayerGuardPerfectTimerFrame = 0;
    public float PlayerGuardPerfectTimerMaxFrame = 25;
    public float PlayerBasicAttackPercentage = 1;
    public bool PlayerInvincible = false;
    public bool PlayerGuarding = false;
    public GameObject PlayerGameObject;
}
