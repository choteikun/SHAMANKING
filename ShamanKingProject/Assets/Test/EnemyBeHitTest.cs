using Gamemanager;
using System.Collections;
using UnityEngine;
using UnityEngine.VFX;
using static Unity.Burst.Intrinsics.X86.Avx;

public class EnemyBeHitTest : MonoBehaviour
{

    public float HealthPoint { get { return healthPoint_; } set { healthPoint_ = value; } }
    [SerializeField] float healthPoint_ = 100;

    [SerializeField] public bool Break = false;

    [SerializeField] bool beenHurt_ = false;
    [SerializeField] int avoidDamageTicks_ = 0;
    [SerializeField] int maxNeedAvoidDamegeTicks_ = 250;
    [SerializeField] public float BreakPoint = 0;
    [SerializeField] public float MaxBreakPoint = 400;

    [SerializeField] public bool BlueShieldOn = false;
    [SerializeField] float blueShieldPoint_;
    [SerializeField] float maxBlueShieldPoint_;


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
            canGetHit_ = false;
            healthPoint_ -= cmd.AttackDamage;
            checkBreakPoint(cmd.AttackDamage);
            checkBlueShieldDamage(cmd.AttackDamage);
            onHitParticle_.transform.position = cmd.CollidePoint;
            //onHitParticle_.GetComponent<ParticleSystem>().Play();
            onHitParticle_.GetComponent<VisualEffect>().Play();
            GameManager.Instance.MainGameEvent.Send(new PlayerAttackSuccessResponse(cmd));           
            StartCoroutine("beAttackTimer");
        }
    }
    public void GainBlueShield(int maxBlueShield)
    {
        BlueShieldOn = true;
        blueShieldPoint_ = maxBlueShield;
        maxBlueShieldPoint_ = maxBlueShield;
    }

    public void RevertBreakPoint()
    {
        Break = false;
        BreakPoint = 0;
    }
    void checkBlueShieldDamage(float damage)
    {
        blueShieldPoint_ = blueShieldPoint_ - damage;
        if (blueShieldPoint_<=0)
        {
            BlueShieldOn = false;
            blueShieldPoint_ = 0;
        }
    }
    void checkBreakPoint(float damage)
    {
        beenHurt_ = true;
        avoidDamageTicks_ = 0;
        BreakPoint += damage;
        if (BreakPoint >= MaxBreakPoint)
        {
            Break = true;
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
    private void FixedUpdate()
    {
        if (beenHurt_)
        {
            avoidDamageTicks_++;
            if (avoidDamageTicks_>= maxNeedAvoidDamegeTicks_)
            {
                beenHurt_ = false;
                avoidDamageTicks_ = 0;
            }
        }
        else
        {
            if (!Break)
            {
                BreakPoint = Mathf.Clamp(BreakPoint - 5 * Time.deltaTime,0,MaxBreakPoint) ;            
            }
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

