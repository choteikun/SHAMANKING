using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 供狀態機與行為樹同步的FirstBossState
/// </summary>
public enum FirstBossState
{
    //思考狀態
    FirstBossState_THINKING,
    //近戰模式
    FirstBossState_MELEEATKMODE,
    //遠程模式
    FirstBossState_RANGEDATKMODE
}
/**************************************************
* Description
*   該腳本主要放置一些讓行為樹使用的變量(Variable Mappings require a C# property)
**************************************************/
[RequireComponent(typeof(Rigidbody))]
public class FirstBossVariables : MonoBehaviour
{
    [Tooltip("Boss狀態")]
    public FirstBossState FirstBossState;
    public int IntTypeStateOfFirstBoss { get { return intTypeStateOfFirstBoss_; } set { intTypeStateOfFirstBoss_ = value; } }
    private int intTypeStateOfFirstBoss_;
    public int IntTypeOfBossFace { get { return intTypeOfBossFace_; } set { intTypeOfBossFace_ = value; } }
    [SerializeField, Tooltip("檢查現在Boss的面具用的Int參數")]
    private int intTypeOfBossFace_;
    public int FaceChangeCounter { get { return faceChangeCounter_; } set { faceChangeCounter_ = value; } }
    [SerializeField, Tooltip("Boss換臉計數器")]
    private int faceChangeCounter_;
    public int MeleeAtkCounter { get { return meleeAtkCounter_; } set { meleeAtkCounter_ = value; } }
    [SerializeField, Tooltip("Bossr近戰計數器")]
    private int meleeAtkCounter_;
    public int RangeAtkCounter { get { return rangedAtkCounter_; } set { rangedAtkCounter_ = value; } }
    [SerializeField, Tooltip("Boss遠程計數器")]
    private int rangedAtkCounter_;
    public float ExplosionJudgmentTimer { get { return explosionJudgmentTimer_; } set { explosionJudgmentTimer_ = value; } }
    [SerializeField, Tooltip("審判之炎爆計時器")]
    private float explosionJudgmentTimer_;

    public float FirstBossHp { get { return firstBossHp_; } set { firstBossHp_ = value; } }
    [SerializeField, Tooltip("Boss血量")]
    private float firstBossHp_;
    public float FistBossBreakDownPoint { get { return fistBossBreakUpPoint_; } set { fistBossBreakUpPoint_ = value; } }
    [SerializeField, Tooltip("Boss BreakDown Point")]
    private float fistBossBreakUpPoint_;
    public float DistanceFromPlayer { get { return distanceFromPlayer_; } set { distanceFromPlayer_ = value; } }
    [Tooltip("與玩家的距離")]
    private float distanceFromPlayer_;
    public float FaceChangeProbability { get { return faceChangeProbability_; } set { faceChangeProbability_ = value; } }
    [SerializeField, Tooltip("Boss換臉機率")]
    private float faceChangeProbability_;

    #region -Boss Skill Probability-
    public float ExplosionJudgmentProbability { get { return explosionJudgmentProbability_; } set { explosionJudgmentProbability_ = value; } }
    [Tooltip("審判之炎爆機率")]
    private float explosionJudgmentProbability_;
    public float TripleScratchProbability { get { return tripleScratchProbability_; } set { tripleScratchProbability_ = value; } }
    [Tooltip("Boss三連爪擊機率")]
    private float tripleScratchProbability_;
    public float FireTackleProbability { get { return fireTackleProbability_; } set { fireTackleProbability_ = value; } }
    [Tooltip("Boss閃焰衝鋒機率")]
    private float fireTackleProbability_;
    public float FlamethrowerProbability { get { return flamethrowerProbability_; } set { flamethrowerProbability_ = value; } }
    [Tooltip("Boss噴射火焰機率")]
    private float flamethrowerProbability_;
    public float FireTrackProbability { get { return fireTrackProbability_; } set { fireTrackProbability_ = value; } }
    [Tooltip("Boss追蹤業火機率")]
    private float fireTrackProbability_;

    #endregion

    public bool UpdatePosTrigger { get { return updatePosTrigger_; } set { updatePosTrigger_ = value; } }
    [SerializeField, Tooltip("更新玩家位置的觸發器")]
    private bool updatePosTrigger_;
    public bool ExplosionJudgmentTrigger { get { return explosionJudgmentTrigger_; } set { explosionJudgmentTrigger_ = value; } }
    [SerializeField, Tooltip("審判之炎爆觸發器")]
    private bool explosionJudgmentTrigger_;
    public bool RootTrigger { get { return rootTrigger_; } set { rootTrigger_ = value; } }
    [SerializeField, Tooltip("禁錮觸發器")]
    private bool rootTrigger_;

    public GameObject PlayerObj { get { return playerObj_; } set { playerObj_ = value; } }
    [SerializeField, Tooltip("PlayerObject")]
    private GameObject playerObj_;
    public Rigidbody Rigidbody { get { return rb_; } set { rb_ = value; } }
    [SerializeField, Tooltip("FirstBossRigidbody")]
    private Rigidbody rb_;

    // Root Motion的位移量 用於腳本運用Root Motion
    private Vector3 deltaPos_;

    void Start()
    {
        PlayerObj = GameObject.FindWithTag("Player").gameObject;
        Rigidbody = GetComponent<Rigidbody>();
        IntTypeStateOfFirstBoss = 2;
    }
    void FixedUpdate()
    {
        // 運用Root Motion 要放到修改rb.velocity以前進行
        rb_.position += deltaPos_;

        // 清零目前物理幀累積的deltaPos_
        deltaPos_ = Vector3.zero;
    }
    void Update()
    {
        DistanceFromPlayer = Vector3.Distance(transform.position, playerObj_.transform.position);
        ExplosionJudgmentTimer += !ExplosionJudgmentTrigger ? 1 : 0;
        switch (IntTypeStateOfFirstBoss)
        {
            case 1:
                FirstBossState = FirstBossState.FirstBossState_THINKING;
                break;
            case 2:
                FirstBossState = FirstBossState.FirstBossState_MELEEATKMODE;
                break;
            case 3:
                FirstBossState = FirstBossState.FirstBossState_RANGEDATKMODE;
                break;
            default:
                break;
        }
        switch (FaceChangeCounter)
        {
            case 0:
                FaceChangeProbability = 0;
                break;
            case 1:
                FaceChangeProbability = 0;
                break;
            case 2:
                FaceChangeProbability = 0.6f;
                break;
            case 3:
                FaceChangeProbability = 0.8f;
                break;
            case 4:
                FaceChangeProbability = 1.0f;
                break;
            default:
                break;
        }
        switch (MeleeAtkCounter)
        {
            case 0:
                TripleScratchProbability = 0;
                FireTackleProbability = 0;
                break;
            case 1:
                TripleScratchProbability = 0;
                FireTackleProbability = 0;
                break;
            case 2:
                TripleScratchProbability = 0.2f;
                FireTackleProbability = 0.2f;
                break;
            case 3:
                TripleScratchProbability = 0.35f;
                FireTackleProbability = 0.35f;
                break;
            case 4:
                TripleScratchProbability = 0.5f;
                FireTackleProbability = 0.5f;
                break;
            default:
                break;
        }
        switch (RangeAtkCounter)
        {
            case 0:
                FlamethrowerProbability = 0;
                FireTrackProbability = 0;
                break;
            case 1:
                FlamethrowerProbability = 0;
                FireTrackProbability = 0;
                break;
            case 2:
                FlamethrowerProbability = 0.2f;
                FireTrackProbability = 0.2f;
                break;
            case 3:
                FlamethrowerProbability = 0.35f;
                FireTrackProbability = 0.35f;
                break;
            case 4:
                FlamethrowerProbability = 0.5f;
                FireTrackProbability = 0.5f;
                break;
            default:
                break;
        }
        switch (ExplosionJudgmentTimer / 60)
        {
            case 0f:
                ExplosionJudgmentProbability = 0;
                ExplosionJudgmentTrigger = true;
                break;
            case 10.0f:
                ExplosionJudgmentProbability = 0;
                ExplosionJudgmentTrigger = true;
                break;
            case 20.0f:
                ExplosionJudgmentProbability = 0.4f;
                ExplosionJudgmentTrigger = true;
                break;
            case 30.0f:
                ExplosionJudgmentProbability = 0.75f;
                ExplosionJudgmentTrigger = true;
                break;
            case 40.0f:
                ExplosionJudgmentProbability = 1.0f;
                ExplosionJudgmentTrigger = true;
                ExplosionJudgmentTimer = 0;
                break;
            default:
                break;
        }
    }
    public void OnUpdateRootMotion(Animator anim)
    {
        // 更新deltaPos_為動畫機的Root Motion 之所以用累加是因為物理幀和動畫幀不一樣 在物理幀的最後會將deltaPos_清零
        deltaPos_ += anim.deltaPosition;
    }
}
