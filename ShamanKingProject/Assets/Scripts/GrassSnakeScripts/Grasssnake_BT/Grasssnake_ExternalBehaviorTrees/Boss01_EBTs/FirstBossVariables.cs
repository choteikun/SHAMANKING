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
    [SerializeField, Tooltip("Boss換臉計時器")]
    private int faceChangeCounter_;
    public int MeleeAtkCounter { get { return meleeAtkCounter_; } set { meleeAtkCounter_ = value; } }
    [SerializeField, Tooltip("Bossr近戰計時器")]
    private int meleeAtkCounter_;
    public int RangeAtkCounter { get { return rangedAtkCounter_; } set { rangedAtkCounter_ = value; } }
    [SerializeField, Tooltip("Boss遠程計時器")]
    private int rangedAtkCounter_;

    public float FirstBossHp { get { return firstBossHp_; } set { firstBossHp_ = value; } }
    [SerializeField, Tooltip("Boss血量")]
    private float firstBossHp_;
    public float DistanceFromPlayer { get { return distanceFromPlayer_; } set { distanceFromPlayer_ = value; } }
    [Tooltip("與玩家的距離")]
    private float distanceFromPlayer_;
    public float FaceChangeProbability { get { return faceChangeProbability_; } set { faceChangeProbability_ = value; } }
    [SerializeField, Tooltip("Boss換臉機率")]
    private float faceChangeProbability_;

    #region -Boss Skill Probability-
    public float TripleScratchProbability { get { return tripleScratchProbability_; } set { tripleScratchProbability_ = value; } }
    [Tooltip("Boss三連爪擊機率")]
    private float tripleScratchProbability_;
    public float FireTackleProbability { get { return fireTackleProbability_; } set { fireTackleProbability_ = value; } }
    [Tooltip("Boss閃焰衝撞機率")]
    private float fireTackleProbability_;
    public float FlamethrowerProbability { get { return flamethrowerProbability_; } set { flamethrowerProbability_ = value; } }
    [Tooltip("Boss噴射火焰機率")]
    private float flamethrowerProbability_;
    public float FireTrackProbability { get { return fireTrackProbability_; } set { fireTrackProbability_ = value; } }
    [Tooltip("Boss閃焰衝撞機率")]
    private float fireTrackProbability_;

    #endregion


    public bool UpdatePosTrigger { get { return updatePosTrigger_; } set { updatePosTrigger_ = value; } }
    private bool updatePosTrigger_;

    public GameObject PlayerObj { get { return playerObj_; } set { playerObj_ = value; } }
    [SerializeField, Tooltip("PlayerObject")]
    private GameObject playerObj_;

    void Start()
    {
        playerObj_ = GameObject.FindWithTag("Player").gameObject;
        IntTypeStateOfFirstBoss = 2;
    }
    void Update()
    {
        DistanceFromPlayer = Vector3.Distance(transform.position, playerObj_.transform.position);
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
                FaceChangeProbability = 60;
                break;
            case 3:
                FaceChangeProbability = 80;
                break;
            case 4:
                FaceChangeProbability = 100;
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
                TripleScratchProbability = 20;
                FireTackleProbability = 20;
                break;
            case 3:
                TripleScratchProbability = 35;
                FireTackleProbability = 35;
                break;
            case 4:
                TripleScratchProbability = 50;
                FireTackleProbability = 50;
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
                FlamethrowerProbability = 20;
                FireTrackProbability = 20;
                break;
            case 3:
                FlamethrowerProbability = 35;
                FireTrackProbability = 35;
                break;
            case 4:
                FlamethrowerProbability = 50;
                FireTrackProbability = 50;
                break;
            default:
                break;
        }

    }
}
