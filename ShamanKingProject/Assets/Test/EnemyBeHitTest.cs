using Gamemanager;
using System.Collections;
using UnityEngine;

public class EnemyBeHitTest : MonoBehaviour
{
    [SerializeField] float healthPoint_ = 10000;
    [SerializeField] float beHitTimer_ = 0.2f;
    [SerializeField] bool canGetHit_ = true;
    [SerializeField]
    GameObject onHitParticle_;
    private void Start()
    {
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerAttackSuccess, cmd => { checkDamage(cmd); });
    }

    void checkDamage(PlayerAttackSuccessCommand cmd)
    {
        if (cmd.AttackTarget == this.gameObject)
        {
            if (!canGetHit_) return;
            canGetHit_ = false;
            healthPoint_ -= cmd.AttackDamage;
            onHitParticle_.transform.position = cmd.CollidePoint;
            onHitParticle_.GetComponent<ParticleSystem>().Play();
            StartCoroutine("beAttackTimer");
        }
    }

    public IEnumerator beAttackTimer()
    {
        yield return new WaitForSeconds(beHitTimer_);
        canGetHit_ = true;
    }
    
}

