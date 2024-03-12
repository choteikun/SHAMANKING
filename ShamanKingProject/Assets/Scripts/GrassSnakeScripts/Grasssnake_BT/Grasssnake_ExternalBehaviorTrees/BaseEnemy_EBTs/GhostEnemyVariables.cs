using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

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
    private NavMeshAgent navMeshAgent_;

    //public ReactiveProperty<GhostEnemyState> ghostEnemyState = new ReactiveProperty<GhostEnemyState>(GhostEnemyState.GhostEnemy_IDLE);
    //public ReactiveProperty<bool> StateMessageChecker;
    public bool WanderTrigger { get { return wanderTrigger_; } set { wanderTrigger_ = value; } }
    [SerializeField]
    private bool wanderTrigger_;
    public bool UpdatePosTrigger { get { return updatePosTrigger_; } set { updatePosTrigger_ = value; } }
    [SerializeField]
    private bool updatePosTrigger_;
    public bool HasTakenDamage { get { return hasTakenDamage_; } set { hasTakenDamage_ = value; } }
    [SerializeField] bool hasTakenDamage_;
    public bool StunTrigger { get { return stunTrigger_; } set { stunTrigger_ = value; } }
    [SerializeField]
    private bool stunTrigger_;
    public bool RootTrigger { get { return rootTrigger_; } set { rootTrigger_ = value; } }
    [SerializeField]
    private bool rootTrigger_;
    public bool EliteRangedAtkTrigger { get { return eliteRangedAtkTrigger_; } set { eliteRangedAtkTrigger_ = value; } }
    [SerializeField]
    private bool eliteRangedAtkTrigger_;

    //public bool StateMessageChecker { get { return stateMessageChecker; } set { stateMessageChecker = value; } }
    //[SerializeField]
    //private bool stateMessageChecker;
    public int IntTypeStateOfGhostEnemy { get { return intTypeStateOfGhostEnemy_; } set { intTypeStateOfGhostEnemy_ = value; } }
    [SerializeField]
    private int intTypeStateOfGhostEnemy_;
    public int EliteAtkCounter { get { return eliteAtkCounter_; } set { eliteAtkCounter_ = value; } }
    [SerializeField, Tooltip("Elite GhostEnemy 特動計數器")]
    private int eliteAtkCounter_;
    public float DistanceFromPlayer { get { return distanceFromPlayer_; } set { distanceFromPlayer_ = value; } }
    [SerializeField, Tooltip("與玩家之間的距離")]
    private float distanceFromPlayer_;
    //public float Aggression { get { return aggression_; } set { aggression_ = value; } }
    //[SerializeField, Tooltip("攻擊慾望")]
    //private float aggression_;

    public GameObject PlayerObj { get { return playerObj_; } set { playerObj_ = value; } }
    [SerializeField]
    private GameObject playerObj_;
    public Collider GhostEnemyCollider { get { return ghostEnemyCollider_; } set { ghostEnemyCollider_ = value; } }
    [SerializeField]
    private Collider ghostEnemyCollider_;

    void Start()
    {
        PlayerObj = GameObject.FindWithTag("Player").gameObject;
        EliteAtkCounter = 3;
        //ghostEnemyState = GhostEnemyState.GhostEnemy_IDLE;

        //GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.BT_Event.BT_SwitchStateMessage, getBT_Massage);
        GameManager.Instance.GhostEnemyGameEvent.SetSubscribe(GameManager.Instance.GhostEnemyGameEvent.OnEliteGhostEnemyRangedAttack, cmd => 
        {
            //如果龍捲風結束且菁英怪遠程攻擊觸發器已啟用
            if (EliteRangedAtkTrigger)
            {
                //關閉菁英怪遠程攻擊觸發器(才可釋放下一次的遠程攻擊)
                EliteRangedAtkTrigger = false;
            }
        });

        navMeshAgent_ = GetComponent<NavMeshAgent>();
        ghostEnemyCollider_ = GetComponent<Collider>();
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerGrabSuccessForPlayer, cmd =>
        {
            if (cmd.AttackTarget == this.gameObject)
            {
                StunTrigger = true;
            }
        });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerAnimationEvents, cmd =>
        {
            if (cmd.AnimationEventName == "Player_Pull_Finish")
            {
                StunTrigger = false;
            }
        });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerAttackSuccess, async cmd =>
        {
            if (cmd.AttackTarget == this.gameObject)
            {
                hasTakenDamage_ = true;
                await UniTask.DelayFrame(1);
                hasTakenDamage_ = false;
            }
        });
        //GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerAttackSuccess, async cmd =>
        //{
        //    if (cmd.AttackTarget == this.gameObject && cmd.AttackInputType != AttackInputType.ChainAttack)
        //    {
        //        hasTakenDamage_ = true;
        //        await UniTask.DelayFrame(1);
        //        hasTakenDamage_ = false;
        //    }
        //});
        //GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerAttackSuccess, async cmd =>
        //{
        //    if (cmd.AttackTarget == this.gameObject && cmd.AttackInputType == AttackInputType.ChainAttack)
        //    {
        //        RootTrigger = true;
        //        await UniTask.Delay(5000);
        //        RootTrigger = false;
        //        GameManager.Instance.MainGameEvent.Send(new PlayerRootSuccessCommand());
        //    }
        //});

    }
    void Update()
    {
        DistanceFromPlayer = Vector3.Distance(transform.position, playerObj_.transform.position);
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
    public void OnUpdateRootMotion(Animator anim)
    {
        navMeshAgent_.Move(anim.deltaPosition);
    }
    //void getBT_Massage(BT_SwitchStateMessage bT_SwitchStateMessage)
    //{

    //}
}
