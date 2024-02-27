using System;
using UnityEngine;

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

    [Tooltip("Boss跑步速度")]
    public float FirstBossRunForwardSpeed;
    [Tooltip("Boss跳撲速度")]
    public float FirstBossJumpForwardSpeed;

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

    public float DistanceFromPlayer { get { return distanceFromPlayer_; } set { distanceFromPlayer_ = value; } }
    [SerializeField, Tooltip("與玩家的距離")]
    private float distanceFromPlayer_;
    public float AngleFacingPlayer { get { return angleFacingPlayer_; } set { angleFacingPlayer_ = value; } }
    [SerializeField, Tooltip("當前面向玩家的角度")]
    private float angleFacingPlayer_;
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
    public bool PreludeTrigger { get { return preludeTrigger_; } set { preludeTrigger_ = value; } }
    [SerializeField, Tooltip("開場白行為觸發器")]
    private bool preludeTrigger_;
    public bool RootTrigger { get { return rootTrigger_; } set { rootTrigger_ = value; } }
    [SerializeField, Tooltip("禁錮觸發器")]
    private bool rootTrigger_;

    public GameObject PlayerObj { get { return playerObj_; } set { playerObj_ = value; } }
    [SerializeField, Tooltip("PlayerObject")]
    private GameObject playerObj_;
    public Rigidbody Rigidbody { get { return rb_; } set { rb_ = value; } }
    [SerializeField, Tooltip("BossRigidbody")]
    private Rigidbody rb_;
    public Collider FirstBossCollider { get { return firstBossCollider_; } set { firstBossCollider_ = value; } }
    [SerializeField, Tooltip("BossCollider")]
    private Collider firstBossCollider_;


    public Vector3 RunForwardVec { get { return runForwardVec_; } set { runForwardVec_ = value; } }
    [SerializeField, Tooltip("RunForwardVec")]
    private Vector3 runForwardVec_;
    public Vector3 JumpForwardVec { get { return jumpForwardVec_; } set { jumpForwardVec_ = value; } }
    [SerializeField, Tooltip("JumpForwardVec")]
    private Vector3 jumpForwardVec_;

    [SerializeField] private float jumpAtkDistance_;

    // Root Motion的位移量 用於腳本運用Root Motion
    private Vector3 deltaPos_;

    private bool isJumpAttacking_ = false;
    public bool isPillarTriggering;

    void Start()
    {
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnCallBossSceneCutScene, cmd => { preludeTrigger_ = true; });
        GameManager.Instance.HellDogGameEvent.SetSubscribe(GameManager.Instance.HellDogGameEvent.OnBossCallJumpAttackLocate, cmd => { locatePlayerPosition(); });

        PlayerObj = GameObject.FindGameObjectWithTag("Player");
        Rigidbody = GetComponent<Rigidbody>();
        if (!FirstBossCollider) { FirstBossCollider = GameObject.Find("FirstBossCollider").GetComponent<Collider>(); }
        PreludeTrigger = false;
        IntTypeStateOfFirstBoss = 2;
    }
    void FixedUpdate()
    {
        if (isJumpAttacking_)
        {
            rb_.position += deltaPos_ * jumpAtkDistance_ / 19.8f * Convert.ToInt32(!isPillarTriggering);
        }
        else
        {
            // 運用Root Motion 要放到修改rb.velocity以前進行
            rb_.position += deltaPos_ * Convert.ToInt32(!isPillarTriggering);

            getRunForwardVector();
        }

        // 清零目前物理幀累積的deltaPos_
        deltaPos_ = Vector3.zero;
    }
    void Update()
    {
        getAngleFacingPlayer();

        //if (UpdatePosTrigger)
        //{
        //    DistanceFromPlayer = Vector3.Distance(transform.position, playerObj_.transform.position);
        //}
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
                ExplosionJudgmentProbability = 0.2f;
                ExplosionJudgmentTrigger = true;
                break;
            case 30.0f:
                ExplosionJudgmentProbability = 0.5f;
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
    void getRunForwardVector()
    {
        RunForwardVec = transform.forward * DistanceFromPlayer * FirstBossRunForwardSpeed;
    }
    void getJumpForwardAtkVector()
    {
        //根據Boss當下對玩家距離的判斷決定位移的速度(這個速度會在跳撲動畫位移時加上去)
        JumpForwardVec = transform.forward * DistanceFromPlayer * FirstBossJumpForwardSpeed;
    }

    void getAngleFacingPlayer()
    {
        //Boss到玩家的方向向量
        Vector3 toPlayer = PlayerObj.transform.position - transform.position;
        //Boss的正前方向量
        Vector3 forward = transform.forward;
        //計算點積
        float dotProduct = Vector3.Dot(forward, toPlayer);
        //計算夾角弧度
        float angle = Mathf.Acos(dotProduct / (forward.magnitude * toPlayer.magnitude)) * Mathf.Rad2Deg;
        //判斷夾角正負
        if (Vector3.Cross(forward, toPlayer).y < 0)
        {
            AngleFacingPlayer = -angle;
        }
        else
        {
            AngleFacingPlayer = angle;
        }
    }
    void locatePlayerPosition()
    {
        var vector = GameManager.Instance.MainGameMediator.RealTimePlayerData.PlayerGameObject.transform.position - this.transform.position;
        jumpAtkDistance_ = vector.magnitude;
    }
    public void OnUpdateRootMotion(Animator anim)
    {
        ////過濾掉不需要套用動畫位移的動畫
        //if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Helldog_Run"))
        //{
        //    // 更新deltaPos_為動畫機的Root Motion 之所以用累加是因為物理幀和動畫幀不一樣 在物理幀的最後會將deltaPos_清零
        //    deltaPos_ += anim.deltaPosition;
        //    transform.rotation = anim.rootRotation;
        //}
        deltaPos_ += anim.deltaPosition;
        transform.rotation = anim.rootRotation;

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Helldog_NormalAtk_JumpAttack"))
        {
            isJumpAttacking_ = true;
        }
        else
        {
            isJumpAttacking_ = false;
        }
        //deltaRot_ = anim.deltaRotation;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pillar"))
        {
            Debug.Log("BLOCK!!!!!!!!!");
            isPillarTriggering = true;
        }
    }
}
