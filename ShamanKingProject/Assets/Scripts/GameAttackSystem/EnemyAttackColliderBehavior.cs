using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamemanager;

public class EnemyAttackColliderBehavior : MonoBehaviour
{
    [SerializeField] LayerMask layerMask_;
    [SerializeField] int lastFrame_;

    [SerializeField] float minDamage_;
    [SerializeField] float maxDamage_;
    async void Start()
    {
        await UniTask.DelayFrame(lastFrame_);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        // 检查collider是否在LayerMask中
        if ((layerMask_.value & (1 << other.gameObject.layer)) > 0)
        {
            //var collidePoint = collidePoint_.ClosestPoint(other.transform.position);
            var collidePoint = other.ClosestPoint(this.gameObject.transform.position);           
            var command = new EnemyAttackSuccessCommand() { CollidePoint = collidePoint, AttackDamage = getDamege() };
            awaitSendAttackMessage(command);
            Debug.Log("HitTarget" + other.name);
        }
    }
    void awaitSendAttackMessage(EnemyAttackSuccessCommand cmd)
    {
        GameManager.Instance.MainGameEvent.Send(cmd);
    }
    float getDamege()
    {
        var damage = Random.Range(minDamage_, maxDamage_ + 1);
        damage = damage * GameManager.Instance.MainGameMediator.RealTimePlayerData.PlayerBasicAttackPercentage;
        return damage;
    }
}
