using BehaviorDesigner.Runtime;
using UnityEngine;

public enum EnemyBehaviorTreeState
{
    //待機狀態
    ENEMY_IDLE,
    //移動狀態
    ENEMY_MOVEMENT,
    //攻擊狀態
    ENEMY_FIGHT,
}
public class EnemyBehaviorTreeSupport : MonoBehaviour
{
    public EnemyBehaviorTreeState enemyBehaviorTreeState;

    [SerializeField]
    private BehaviorTree behaviorTree;
    [SerializeField]
    private ExternalBehavior[] externalBehaviorTrees;

    void Start()
    {
        enemyBehaviorTreeState = EnemyBehaviorTreeState.ENEMY_IDLE;
        SwitchExternalBehavior((int)EnemyBehaviorTreeState.ENEMY_IDLE);

    }
    void Update()
    {
        switch (enemyBehaviorTreeState)
        {
            case EnemyBehaviorTreeState.ENEMY_IDLE:

                break;
            case EnemyBehaviorTreeState.ENEMY_MOVEMENT:

                break;
            case EnemyBehaviorTreeState.ENEMY_FIGHT:

                break;

            default:
                break;
        }
    }
    #region - 切換外部行為樹 -
    public void SwitchExternalBehavior(int externalTrees)
    {
        if (externalBehaviorTrees[externalTrees] != null)
        {
            behaviorTree.ExternalBehavior = externalBehaviorTrees[externalTrees];
            behaviorTree.EnableBehavior();
        }
    }
    public void DisableBehaviorTree()
    {
        behaviorTree.DisableBehavior();
    }
    #endregion
}
