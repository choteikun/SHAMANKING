using Cysharp.Threading.Tasks;
using Gamemanager;
using UnityEngine;

public class AttackColliderBehavior : MonoBehaviour
{
    [SerializeField] LayerMask layerMask_;
    [SerializeField] int startDelayFrame_;
    [SerializeField] int lastFrame_;

    [SerializeField] float minDamage_;
    [SerializeField] float maxDamage_;

    [SerializeField] AttackInputType attackInputType_;
    [SerializeField] AttackFeedBackType feedBackType_;

    [SerializeField] int attackAddSoul_;

    [SerializeField] int hitEnemyCount_;

    [SerializeField] bool canTrigger_ = false;
    private async void Start()
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
            int soulAdded = attackAddSoul_;
            if (hitEnemyCount_ > 0)
            {
                soulAdded = soulAdded / 4;
            }
            hitEnemyCount_++;
            var command = new PlayerAttackSuccessCommand() { CollidePoint = collidePoint, AttackTarget = other.gameObject, AttackDamage = getDamege(), AttackFeedBackType = feedBackType_, AttackInputType = attackInputType_, AddSoulGage = soulAdded };
            awaitSendAttackMessage(command);
        }
    }
    async void awaitSendAttackMessage(PlayerAttackSuccessCommand cmd)
    {
        await UniTask.DelayFrame(startDelayFrame_);
        GameManager.Instance.MainGameEvent.Send(cmd);
    }
    float getDamege()
    {
        var damage = Random.Range(minDamage_, maxDamage_ + 1);
        damage = damage * GameManager.Instance.MainGameMediator.RealTimePlayerData.PlayerBasicAttackPercentage;
        return damage;
    }
}
