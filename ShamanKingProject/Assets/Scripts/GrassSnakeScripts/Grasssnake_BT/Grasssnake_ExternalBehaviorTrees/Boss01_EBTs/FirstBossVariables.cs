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
    public FirstBossState FirstBossState;
    public int IntTypeStateOfFirstBoss { get { return intTypeStateOfFirstBoss_; } set { intTypeStateOfFirstBoss_ = value; } }
    [SerializeField]
    private int intTypeStateOfFirstBoss_;
    public bool UpdatePosTrigger { get { return updatePosTrigger_; } set { updatePosTrigger_ = value; } }
    [SerializeField]
    private bool updatePosTrigger_;


    void Start()
    {

    }
    void Update()
    {
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
    }
}
