using Gamemanager;
using System.Collections;
using UnityEngine;
using UnityEngine.VFX;
using static Unity.Burst.Intrinsics.X86.Avx;

public class EnemyBeHitTest : MonoBehaviour
{

    public float HealthPoint { get { return healthPoint_; } set { healthPoint_ = value; } }
    [SerializeField] float healthPoint_ = 100;

    [SerializeField] bool canGetHit_ = true;
    [SerializeField] bool canGetGrab_ = true;
    [SerializeField] float beHitTimer_ = 0.2f;
    [SerializeField] float beGrabTimer_ = 1f;

    [SerializeField]
    GameObject onHitParticle_;
    private void Start()
    {
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerAttackSuccess, cmd => { checkDamage(cmd); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerGrabSuccess, cmd => { checkGrab(cmd); });
    }
    void checkDamage(PlayerAttackSuccessCommand cmd)
    {
        if (cmd.AttackTarget == this.gameObject)
        {
            //if (!canGetHit_) return;
            canGetHit_ = false;
            healthPoint_ -= cmd.AttackDamage;
            onHitParticle_.transform.position = cmd.CollidePoint;
            //onHitParticle_.GetComponent<ParticleSystem>().Play();
            onHitParticle_.GetComponent<VisualEffect>().Play();
            GameManager.Instance.MainGameEvent.Send(new PlayerAttackSuccessResponse(cmd));
            StartCoroutine("beAttackTimer");
        }
    }
    void checkGrab(PlayerGrabSuccessCommand cmd)
    {
        if (cmd.AttackTarget == this.gameObject)
        {
            if (!canGetGrab_) return;
            canGetGrab_ = false;
            onHitParticle_.transform.position = cmd.CollidePoint;
            //onHitParticle_.GetComponent<ParticleSystem>().Play();
            onHitParticle_.GetComponent<VisualEffect>().Play();
            GameManager.Instance.MainGameEvent.Send(new PlayerGrabSuccessResponse(cmd));
            Debug.Log("GrabSuccess");
            StartCoroutine("beGrabTimer");
        }
    }
    public IEnumerator beAttackTimer()
    {
        yield return new WaitForSeconds(beHitTimer_);
        canGetHit_ = true;
    }
    public IEnumerator beGrabTimer()
    {
        yield return new WaitForSeconds(beGrabTimer_);
        canGetGrab_ = true;
    }
}

