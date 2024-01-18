using Cysharp.Threading.Tasks;
using Gamemanager;
using UnityEngine;

public class EnemyAttackColliderBehavior : MonoBehaviour
{
    [SerializeField] EnemyHitPower thisAttackHitPower_;
    [SerializeField] LayerMask layerMask_;
    [SerializeField] int lastFrame_;

    [SerializeField] float minDamage_;
    [SerializeField] float maxDamage_;
    [SerializeField] bool unbreakble_ = false;
    [SerializeField] bool unDodgeable_ = false;
    [SerializeField] BossSpecialColliderType colliderType_ = BossSpecialColliderType.Normal;
    [SerializeField] GameObject specialAttackerPos_;
    async void Start()
    {
        if (unbreakble_) return;
        await UniTask.DelayFrame(lastFrame_);
        if (this.gameObject != null)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 检查collider是否在LayerMask中
        if ((layerMask_.value & (1 << other.gameObject.layer)) > 0)
        {
            if (GameManager.Instance.MainGameMediator.RealTimePlayerData.PlayerGuarding)
            {
                if (GameManager.Instance.MainGameMediator.RealTimePlayerData.PlayerGuardPerfectTimerFrame< GameManager.Instance.MainGameMediator.RealTimePlayerData.PlayerGuardPerfectTimerMaxFrame)
                {
                    //完美格檔
                    var commandP = new PlayerSuccessParryCommand();
                    var collidePointP = other.ClosestPoint(this.gameObject.transform.position);
                    commandP = new PlayerSuccessParryCommand() { CollidePoint = collidePointP, AttackDamage = getDamege(), ThisAttackHitPower = thisAttackHitPower_, AttackerPos = this.gameObject.transform.position };
                    GameManager.Instance.MainGameEvent.Send(commandP);
                    //回滿盾
                    Debug.LogWarning("Parry!");
                    return;
                }
                else
                {
                    GameManager.Instance.MainGameMediator.RealTimePlayerData.PlayerGuardPoint = Mathf.Clamp(GameManager.Instance.MainGameMediator.RealTimePlayerData.PlayerGuardPoint - getDamege(), 0, GameManager.Instance.MainGameMediator.RealTimePlayerData.PlayerMaxGuardPoint);
                    if (GameManager.Instance.MainGameMediator.RealTimePlayerData.PlayerGuardPoint != 0)
                    {
                        return;
                    }
                    else 
                    {
                        GameManager.Instance.MainGameEvent.Send(new SystemStopGuardingCommand());
                    }
                }
            }
            if (GameManager.Instance.MainGameMediator.RealTimePlayerData.PlayerInvincible && unDodgeable_ == false) return;
            Debug.LogError("Hit!!!");
            //var collidePoint = collidePoint_.ClosestPoint(other.transform.position);
            var collidePoint = other.ClosestPoint(this.gameObject.transform.position);
            var command = new EnemyAttackSuccessCommand();
            if (colliderType_ == BossSpecialColliderType.Normal)
            {
                command = new EnemyAttackSuccessCommand() { CollidePoint = collidePoint, AttackDamage = getDamege(), ThisAttackHitPower = thisAttackHitPower_, AttackerPos = this.gameObject.transform.position };
            }
            else if (colliderType_ == BossSpecialColliderType.FlameThrower)
            {
                command = new EnemyAttackSuccessCommand() { CollidePoint = collidePoint, AttackDamage = getDamege(), ThisAttackHitPower = thisAttackHitPower_, AttackerPos = specialAttackerPos_.transform.position };
            }
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

public enum EnemyHitPower
{
    Light,
    HardKnockBack,
    OneShot,
}

public enum BossSpecialColliderType
{
    Normal,
    FlameThrower,
}