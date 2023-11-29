using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Gamemanager;
using System;
using System.Linq;

public class PlayerAttacker : MonoBehaviour
{
    [SerializeField] LayerMask enemyLayer_;
    [SerializeField] GameObject playerAttackRayStartPoint_;//AttackRayStartPoint
    [SerializeField] GameObject gizmosAnimationFollowObject_;
    [SerializeField] GameObject playerHitBoxCenter_;
    [SerializeField] Collider ghostCollider_;
    [SerializeField] float radius_;
    Collider[] colliders_ = new Collider[10];
    [SerializeField] bool attacking_ = false;
    [SerializeField] bool throwAttacking_ = false;
    [SerializeField] int attackHash_;

    public void Start()
    {
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerAttackCallHitBox, cmd => { activateHitBox(cmd.CallOrCancel); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerThrowAttackCallHitBox, cmd => { activateThrowHitBox(cmd.CallOrCancel); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerThrowAttackFinish, cmd => { activateThrowHitBox(false); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerLaunchActionFinish, cmd => { playerAimingHitGhost(cmd); });
    }
    private void Update()
    {
        if (attacking_)
        {
            var sphereArray = searchObjectInAttackRange();
            var rayList = searchObjectInAttackRangeByRayCast();
            sendAttackSuccessCommand(sphereArray, rayList);
        }
        if (throwAttacking_)
        {
            searchObjectInGrabRange();
        }
    }
    void activateHitBox(bool callOrCancel)
    {
        attacking_ = callOrCancel;
    }
    void activateThrowHitBox(bool callOrCancel)
    {
        throwAttacking_ = callOrCancel;
    }
    List<GameObject> searchObjectInAttackRangeByRayCast()
    {
        // 從核心點向攻擊點發射射線
        Vector3 direction = playerHitBoxCenter_.transform.position - playerAttackRayStartPoint_.transform.position;
        float distance = Vector3.Distance(playerAttackRayStartPoint_.transform.position, playerHitBoxCenter_.transform.position);

        // 檢測射線上的所有敵人
        RaycastHit[] hits = Physics.RaycastAll(playerAttackRayStartPoint_.transform.position, direction, distance, enemyLayer_);

        // 創建一個陣列來儲存所有碰撞的敵人
        List<GameObject> enemiesHit = new List<GameObject>();

        foreach (RaycastHit hit in hits)
        {
            // 如果碰撞的對象是敵人，則加入陣列
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                enemiesHit.Add(hit.collider.gameObject);
            }
        }
        return enemiesHit;      
    }
    GameObject[] searchObjectInAttackRange()
    {
        radius_ = 0.15f + 0.375f * GameManager.Instance.MainGameMediator.RealTimePlayerData.GhostNowGageBlockAmount;
        int colliderCount = Physics.OverlapSphereNonAlloc(playerHitBoxCenter_.transform.position, radius_, colliders_);       
        var gameobjectArray = new GameObject[colliderCount];
        for (int i = 0; i < colliderCount; i++)
        {
            gameobjectArray[i] = colliders_[i].transform.gameObject;
        }
        return gameobjectArray;
    }

    void sendAttackSuccessCommand(GameObject[] sphereArray, List<GameObject> RaycastList)
    {
        var uniqueItems = RaycastList.Union(sphereArray).ToList();
        // 現在 enemiesHit 陣列包含所有被射線擊中的敵人
        // 你可以根據需要處理這些敵人
        for (int i = 0; i < uniqueItems.Count; i++)
        {
            if (uniqueItems[i].CompareTag("Enemy"))
            {
                var collidePoint = ghostCollider_.ClosestPointOnBounds(uniqueItems[i].transform.position);
                GameManager.Instance.MainGameEvent.Send(new PlayerAttackSuccessCommand() { CollidePoint = collidePoint, AttackTarget = uniqueItems[i].gameObject, AttackDamage = 20f });
            }
        }
    }
    void searchObjectInGrabRange()
    {
        radius_ = 0.15f + 0.375f * GameManager.Instance.MainGameMediator.RealTimePlayerData.GhostNowGageBlockAmount;
        int colliderCount = Physics.OverlapSphereNonAlloc(playerHitBoxCenter_.transform.position, radius_, colliders_);
        for (int i = 0; i < colliderCount; i++)
        {
            if (colliders_[i].CompareTag("Enemy"))
            {
                var collidePoint = ghostCollider_.ClosestPointOnBounds(colliders_[i].transform.position);
                GameManager.Instance.MainGameEvent.Send(new PlayerGrabSuccessCommand() { CollidePoint = collidePoint, AttackTarget = colliders_[i].gameObject, AttackDamage = 20f });
                throwAttacking_ = false;
                return;
            }
        }
    }

    void playerAimingHitGhost(PlayerLaunchActionFinishCommand cmd)
    {
        if (!cmd.Hit) return;
        if (cmd.HitInfo.HitTag == HitObjecctTag.Enemy)
        {
            var command = new PlayerGrabSuccessCommand() { CollidePoint = cmd.HitInfo.onHitPoint_.transform.position, AttackTarget = cmd.HitObjecct, AttackDamage = 20f };
            Debug.Log(command.AttackTarget.name);
            GameManager.Instance.MainGameEvent.Send(command);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(playerHitBoxCenter_.transform.position, 0.15f);

        // 畫一條線從核心點到攻擊點
        Gizmos.DrawLine(playerAttackRayStartPoint_.transform.position, playerHitBoxCenter_.transform.position);

        Gizmos.color = Color.green;
        // 如果需要，也可以在這裡添加更多的 Gizmos 繪製代碼
        Gizmos.DrawLine(playerAttackRayStartPoint_.transform.position, gizmosAnimationFollowObject_.transform.position);
    }
}
