using Gamemanager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerBOOMBOOM : MonoBehaviour
{
    [SerializeField] float destroyDelayTimer_;
    Collider collider_;
    void Start()
    {
        collider_ = GetComponent<Collider>();
        Invoke("spawnBOOMBOOMCollider", destroyDelayTimer_);
    }
    void Update()
    {
        transform.position = GameManager.Instance.MainGameMediator.RealTimePlayerData.PlayerGameObject.transform.position;
    }
    void spawnBOOMBOOMCollider()
    {
        collider_.enabled = true;
        GameManager.Instance.GhostEnemyGameEvent.Send(new EliteGhostEnemyRangedAttackHitCommand());
        Destroy(gameObject, 2.0f);
    }
}
