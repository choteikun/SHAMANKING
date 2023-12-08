using Gamemanager;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.VFX;
using static Unity.Burst.Intrinsics.X86.Avx;

public class EnemyBeHitTest : MonoBehaviour
{

    public float HealthPoint { get { return healthPoint_; } set { healthPoint_ = value; } }
    [SerializeField] float healthPoint_ = 100;

    [SerializeField] public bool Break = false; //是否已經在break狀態

    [SerializeField] bool beenHurt_ = false; //是否在戰鬥狀態
    [SerializeField] int avoidDamageTicks_ = 0;//已經脫離戰鬥狀態幾貞
    [SerializeField] int maxNeedAvoidDamegeTicks_ = 250;//脫離戰鬥需要幾貞
    [SerializeField] public float BreakPoint = 0;//BK值已經累積幾點
    [SerializeField] public float MaxBreakPoint = 400;//滿BK值需要幾點

    [SerializeField] public bool BlueShieldOn = false;//是否擁有藍盾 可以防止被斷招
    [SerializeField] float blueShieldPoint_; //目前剩餘藍盾點數
    [SerializeField] float maxBlueShieldPoint_; //藍盾滿條的量


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
    public void GainBlueShield(int maxBlueShield) //獲得藍盾 
    {
        BlueShieldOn = true;
        blueShieldPoint_ = maxBlueShield;
        maxBlueShieldPoint_ = maxBlueShield;
    }

    public void RevertBreakPoint() //取消Break狀態 讓一個break循環可以再來一次
    {
        Break = false;
        BreakPoint = 0;
    }
    void checkBlueShieldDamage(float damage)//扣除藍盾
    {
        blueShieldPoint_ = blueShieldPoint_ - damage;
        if (blueShieldPoint_<=0)
        {
            BlueShieldOn = false;
            blueShieldPoint_ = 0;
        }
    }
    void checkBreakPoint(float damage)//增加bk值
    {
        if (Break) return;
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
    private void FixedUpdate() //脫離戰鬥後減少bk值
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

