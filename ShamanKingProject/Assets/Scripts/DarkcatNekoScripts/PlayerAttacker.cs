using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Gamemanager;

public class PlayerAttacker : MonoBehaviour
{
    [SerializeField] GameObject playerLightAttackHitBox_;
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
            searchObjectInAttackRange();
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
    void searchObjectInAttackRange()
    {
        radius_ = 0.15f + 0.375f * GameManager.Instance.MainGameMediator.RealTimePlayerData.GhostNowGageBlockAmount;
        int colliderCount = Physics.OverlapSphereNonAlloc(playerHitBoxCenter_.transform.position, radius_, colliders_);
        for (int i = 0; i < colliderCount; i++)
        {
            if (colliders_[i].CompareTag("Enemy"))
            {
                var collidePoint = ghostCollider_.ClosestPointOnBounds(colliders_[i].transform.position);
                GameManager.Instance.MainGameEvent.Send(new PlayerAttackSuccessCommand() {CollidePoint = collidePoint, AttackTarget = colliders_[i].gameObject, AttackDamage = 20f });
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
    }
}
