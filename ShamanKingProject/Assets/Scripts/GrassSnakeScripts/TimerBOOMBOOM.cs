using Gamemanager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerBOOMBOOM : MonoBehaviour
{
    [SerializeField] float destroyDelayTimer_;
    EnemyAttackColliderBehavior enemyAttackColliderBehavior_;
    void Start()
    {
        enemyAttackColliderBehavior_ = GetComponent<EnemyAttackColliderBehavior>();
        Invoke("spawnBOOMBOOMCollider", destroyDelayTimer_);
    }
    void Update()
    {
        transform.position = GameManager.Instance.MainGameMediator.RealTimePlayerData.PlayerGameObject.transform.position;
    }
    void spawnBOOMBOOMCollider()
    {
        enemyAttackColliderBehavior_.enabled = true;
        GameManager.Instance.GhostEnemyGameEvent.Send(new EliteGhostEnemyRangedAttackHitCommand());
    }
}
