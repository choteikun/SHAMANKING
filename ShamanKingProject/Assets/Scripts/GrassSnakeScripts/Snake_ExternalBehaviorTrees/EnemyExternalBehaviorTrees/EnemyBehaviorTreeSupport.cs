using BehaviorDesigner.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
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
    public EnemyState enemyState;

    [SerializeField]
    private BehaviorTree behaviorTree;
    [SerializeField]
    private ExternalBehavior[] externalBehaviorTrees;

    void Start()
    {
        enemyState = EnemyState.ENEMY_IDLE;
        switchExternalBehavior((int)EnemyState.ENEMY_IDLE);

    }
    void Update()
    {
        switch (enemyState)
        {
            case EnemyState.ENEMY_IDLE:

                break;
            case EnemyState.ENEMY_MOVEMENT:

                break;
            case EnemyState.ENEMY_FIGHT:

                break;

            default:
                break;
        }
    }
    #region - 切換外部行為樹 -
    public void switchExternalBehavior(int externalTrees)
    {
        if (externalBehaviorTrees[externalTrees] != null)
        {
            behaviorTree.DisableBehavior();
            behaviorTree.ExternalBehavior = externalBehaviorTrees[externalTrees];
            behaviorTree.EnableBehavior();
        }
    }
    #endregion
}
