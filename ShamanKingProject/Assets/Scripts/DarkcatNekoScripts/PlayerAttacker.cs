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
    [SerializeField] int attackHash_;

    public void Start()
    {
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerAttackCallHitBox, cmd => { activateHitBox(cmd.CallOrCancel); });
    }
    private void Update()
    {
        if (!attacking_) return;
        searchObjectInAttackRange();
    }
    void activateHitBox(bool callOrCancel)
    {
        attacking_ = callOrCancel;
    }
    void searchObjectInAttackRange()
    {
        radius_ = 0.15f + 0.375f * GameManager.Instance.MainGameMediator.RealTimePlayerData.GhostNowEatAmount;
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(playerHitBoxCenter_.transform.position, 0.15f);
    }
}
