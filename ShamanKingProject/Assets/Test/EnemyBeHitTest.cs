using Gamemanager;
using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class EnemyBeHitTest : MonoBehaviour
{

    public float HealthPoint { get { return healthPoint_; } set { healthPoint_ = value; } }
    [SerializeField] float healthPoint_ = 100;
    public bool HasTakenDamage { get { return hasTakenDamage_; } set { hasTakenDamage_ = value; } }
    [SerializeField] bool hasTakenDamage_;

    [SerializeField] bool canGetHit_ = true;
    [SerializeField] float beHitTimer_ = 0.2f;
    
    [SerializeField]
    GameObject onHitParticle_;
    private void Start()
    {
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerAttackSuccess, cmd => { checkDamage(cmd); });
    }
    private void Update()
    {
        hasTakenDamage_ = !canGetHit_;
    }
    void checkDamage(PlayerAttackSuccessCommand cmd)
    {
        if (cmd.AttackTarget == this.gameObject)
        {
            if (!canGetHit_) return;
            canGetHit_ = false;
            healthPoint_ -= cmd.AttackDamage;
            onHitParticle_.transform.position = cmd.CollidePoint;
            //onHitParticle_.GetComponent<ParticleSystem>().Play();
            onHitParticle_.GetComponent<VisualEffect>().Play();
            StartCoroutine("beAttackTimer");
        }
    }

    public IEnumerator beAttackTimer()
    {
        yield return new WaitForSeconds(beHitTimer_);
        canGetHit_ = true;
    }
    
}

