using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 供狀態機與行為樹同步的WrathPhantomBossState
/// </summary>
public enum WrathPhantomState
{
    //思考狀態
    WrathPhantomState_THINKING,
    //一般進攻模式
    WrathPhantomState_OffensiveMode
}
/**************************************************
* Description
* 該腳本主要放置一些讓行為樹使用的變量(Variable Mappings require a C# property)
**************************************************/
public class WrathPhantomVariables : MonoBehaviour
{
    [Tooltip("Boss狀態")]
    public WrathPhantomState WrathPhantomState;

    [Tooltip("切換Boss狀態用的Int參數")]
    public int IntTypeStateOfWrathPhantom { get { return intTypeStateOfWrathPhantom_; } set { intTypeStateOfWrathPhantom_ = value; } }
    private int intTypeStateOfWrathPhantom_;

    public float DistanceFromPlayer { get { return distanceFromPlayer_; } set { distanceFromPlayer_ = value; } }
    [SerializeField, Tooltip("與玩家的距離")]
    private float distanceFromPlayer_;
    public float AngleFacingPlayer { get { return angleFacingPlayer_; } set { angleFacingPlayer_ = value; } }
    [SerializeField, Tooltip("當前面向玩家的角度")]
    private float angleFacingPlayer_;

    public int MeleeAtkCounter { get { return meleeAtkCounter_; } set { meleeAtkCounter_ = value; } }
    [SerializeField, Tooltip("Bossr近戰計數器")]
    private int meleeAtkCounter_;
    public int RangeAtkCounter { get { return rangedAtkCounter_; } set { rangedAtkCounter_ = value; } }
    [SerializeField, Tooltip("Boss遠程計數器")]
    private int rangedAtkCounter_;

    #region -Boss Skill Probability-
    public float MementoMoriProbability { get { return mementoMoriProbability_; } set { mementoMoriProbability_ = value; } }
    [Tooltip("終有一死技能機率")]
    private float mementoMoriProbability_;
    public float MementoVitaProbability { get { return mementoVitaProbability_; } set { mementoVitaProbability_ = value; } }
    [Tooltip("只此一生技能機率")]
    private float mementoVitaProbability_;
    public float MorsCertaProbability { get { return morsCertaProbability_; } set { morsCertaProbability_ = value; } }
    [Tooltip("生死有常技能機率")]
    private float morsCertaProbability_;
    #endregion

    #region -Boss Trigger-
    public bool PreludeTrigger { get { return preludeTrigger_; } set { preludeTrigger_ = value; } }
    [SerializeField, Tooltip("開場白行為觸發器")]
    private bool preludeTrigger_;
    public bool UpdatePosTrigger { get { return updatePosTrigger_; } set { updatePosTrigger_ = value; } }
    [SerializeField, Tooltip("更新玩家位置的觸發器")]
    private bool updatePosTrigger_;

    #endregion



    public GameObject PlayerObj { get { return playerObj_; } set { playerObj_ = value; } }
    [SerializeField, Tooltip("PlayerObject")]
    private GameObject playerObj_;
    public Rigidbody Rigidbody { get { return rb_; } set { rb_ = value; } }
    [SerializeField, Tooltip("BossRigidbody")]
    private Rigidbody rb_;
    public Collider WrathPhantomCollider { get { return wrathPhantomCollider_; } set { wrathPhantomCollider_ = value; } }
    [SerializeField, Tooltip("BossCollider")]
    private Collider wrathPhantomCollider_;

    [Tooltip("Root Motion的位移量 用於腳本運用Root Motion")]
    private Vector3 deltaPos_;

    private void Start()
    {
        PlayerObj = GameObject.FindGameObjectWithTag("Player");
        Rigidbody = GetComponent<Rigidbody>();
        if (!WrathPhantomCollider) { WrathPhantomCollider = GameObject.Find("WrathPhantomCollider").GetComponent<Collider>(); }
        PreludeTrigger = false;
        IntTypeStateOfWrathPhantom = 2;
    }
    private void Update()
    {
        getAngleFacingPlayer();
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
    public void OnUpdateRootMotion(Animator anim)
    {
        ////過濾掉不需要套用動畫位移的動畫
        //if (!anim.GetCurrentAnimatorStateInfo(0).IsName(""))
        //{
        //    // 更新deltaPos_為動畫機的Root Motion 之所以用累加是因為物理幀和動畫幀不一樣 在物理幀的最後會將deltaPos_清零
        //    deltaPos_ += anim.deltaPosition;
        //    transform.rotation = anim.rootRotation;
        //}
        deltaPos_ += anim.deltaPosition;
        transform.rotation = anim.rootRotation;
    }
}
