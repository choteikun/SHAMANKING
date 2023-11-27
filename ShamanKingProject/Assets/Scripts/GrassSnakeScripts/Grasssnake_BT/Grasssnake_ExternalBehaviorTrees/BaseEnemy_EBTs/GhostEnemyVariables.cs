using Gamemanager;
using UnityEngine;
using UniRx;
using System;

/// <summary>
/// 供狀態機與行為樹同步的GhostEnemyState
/// </summary>
public enum GhostEnemyState
{
    //待機狀態
    GhostEnemy_IDLE,
    //移動狀態
    GhostEnemy_MOVEMENT,
    //攻擊狀態
    GhostEnemy_FIGHT,
}

/**************************************************
* Description
*   該腳本主要放置一些讓行為樹使用的變量(Variable Mappings require a C# property)
**************************************************/
public class GhostEnemyVariables : MonoBehaviour
{
    public GhostEnemyState ghostEnemyState;
    //public ReactiveProperty<GhostEnemyState> ghostEnemyState = new ReactiveProperty<GhostEnemyState>(GhostEnemyState.GhostEnemy_IDLE);
    //public ReactiveProperty<bool> StateMessageChecker;
    public bool WanderTrigger { get { return wanderTrigger_; } set { wanderTrigger_ = value; } }
    [SerializeField]
    private bool wanderTrigger_;
    public bool UpdatePosTrigger { get { return updatePosTrigger_; } set { updatePosTrigger_ = value; } }
    [SerializeField]
    private bool updatePosTrigger_;
    public bool StunTrigger { get { return stunTrigger_; } set { stunTrigger_ = value; } }
    [SerializeField]
    private bool stunTrigger_;
    //public bool StateMessageChecker { get { return stateMessageChecker; } set { stateMessageChecker = value; } }
    //[SerializeField]
    //private bool stateMessageChecker;
    public int IntTypeStateOfGhostEnemy { get { return intTypeStateOfGhostEnemy_; } set { intTypeStateOfGhostEnemy_ = value; } }
    [SerializeField]
    private int intTypeStateOfGhostEnemy_;

    void Start()
    {
        //ghostEnemyState = GhostEnemyState.GhostEnemy_IDLE;

        //GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.BT_Event.BT_SwitchStateMessage, getBT_Massage);

        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerGrabSuccessForPlayer, cmd =>
        {
            if (cmd.AttackTarget == this.gameObject)
            {
                stunTrigger_ = true;

            }
        });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerAnimationEvents, cmd =>
        {
            if (cmd.AnimationEventName == "Player_Pull_Finish")
            {
                stunTrigger_ = false;
            }
        });
    }
    void Update()
    {
        //Debug.LogWarning(IntTypeStateOfGhostEnemy);
        switch (IntTypeStateOfGhostEnemy)
        {
            case 1:
                ghostEnemyState = GhostEnemyState.GhostEnemy_IDLE;
                break;
            case 2:
                ghostEnemyState = GhostEnemyState.GhostEnemy_MOVEMENT;
                break;
            case 3:
                ghostEnemyState = GhostEnemyState.GhostEnemy_FIGHT;
                break;
            default:
                break;
        }
    }
    //void getBT_Massage(BT_SwitchStateMessage bT_SwitchStateMessage)
    //{
        
    //}
}
