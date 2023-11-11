using UnityEngine;

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
    public bool WanderTrigger { get { return wanderTrigger; } set { wanderTrigger = value; } }
    [SerializeField]
    private bool wanderTrigger;
    public bool UpdatePosTrigger { get { return updatePosTrigger; } set { updatePosTrigger = value; } }
    [SerializeField]
    private bool updatePosTrigger;
    public bool StunTrigger { get { return stunTrigger; } set { stunTrigger = value; } }
    [SerializeField]
    private bool stunTrigger;

    void Start()
    {
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerGrabSuccessForPlayer, cmd =>
        {
            if (cmd.AttackTarget == this.gameObject)
            {
                stunTrigger = true;

            }
        });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerAnimationEvents, cmd =>
        {
            if (cmd.AnimationEventName == "Player_Pull_Finish")
            {
                stunTrigger = false;
            }
        });
    }

}
