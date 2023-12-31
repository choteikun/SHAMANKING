using Cysharp.Threading.Tasks;
using Gamemanager;
using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class EnemyBeHitTest : MonoBehaviour
{
    [SerializeField] float executionDamage_;
    public float HealthPoint { get { return healthPoint_; } set { healthPoint_ = value; } }
   [SerializeField] float healthPoint_ = 100;
    [SerializeField] float maxHealthPoint_;
    public bool GainBlueShieldTrigger { get { return gainBlueShieldTrigger_; } set { gainBlueShieldTrigger_ = value; } }
    [SerializeField] bool gainBlueShieldTrigger_;
    public bool RevertBreakPointTrigger { get { return revertBreakPointTrigger_; } set { revertBreakPointTrigger_ = value; } }
    [SerializeField] bool revertBreakPointTrigger_;

    [SerializeField] public bool Break { get { return break_; } set { break_ = value; } } //是否已經在break狀態
    [SerializeField] bool break_ = false;
    public bool BeExecuted { get { return beExecuted_; } set { beExecuted_ = value; } } //是否已經在處決狀態
    [SerializeField] bool beExecuted_ = false;

    [SerializeField] bool canBeExecute_ = true;
    public float BlueShieldSettingParameter { get { return blueShieldSettingParameter_; } set { blueShieldSettingParameter_ = value; } }
    [SerializeField] float blueShieldSettingParameter_;

    [SerializeField] bool beenHurt_ = false; //是否在戰鬥狀態
    [SerializeField] int avoidDamageTicks_ = 0;//已經脫離戰鬥狀態幾幀
    [SerializeField] int maxNeedAvoidDamegeTicks_ = 250;//脫離戰鬥需要幾幀
    [SerializeField] public float BreakPoint = 0;//BK值已經累積幾點
    [SerializeField] public float MaxBreakPoint = 400;//滿BK值需要幾點

    [SerializeField] public bool BlueShieldOn { get { return blueShieldOn_; } set { blueShieldOn_ = value; } }//是否擁有藍盾 可以防止被斷招
    [SerializeField] bool blueShieldOn_ = false;
    [SerializeField] public float CurBlueShieldPoint { get { return curBlueShieldPoint_; } set { curBlueShieldPoint_ = value; } } //目前剩餘藍盾點數
    [SerializeField] float curBlueShieldPoint_;
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
    async void checkDamage(PlayerAttackSuccessCommand cmd)
    {
        if (cmd.AttackTarget == this.gameObject)
        {
            if (break_ && cmd.AttackInputType == AttackInputType.ExecutionAttack && canBeExecute_)
            {
                canBeExecute_ = false;
                //呼叫處決特效
                if (!BeExecuted)
                {
                    BeExecuted = true;
                }
                await UniTask.Delay(2000);
                BeExecuted = false;
                healthPoint_ -= executionDamage_;
                var percentage = healthPoint_ / maxHealthPoint_;
                var breakPercentage = BreakPoint / MaxBreakPoint;
                GameManager.Instance.MainGameEvent.Send(new PlayerAttackSuccessResponse(cmd, percentage, breakPercentage));
                RevertBreakPoint();
                canBeExecute_ = true;
            }
            else
            {
                canGetHit_ = false;
                healthPoint_ -= cmd.AttackDamage;
                var percentage = healthPoint_ / maxHealthPoint_;
                checkBreakPoint(cmd.AttackDamage);
                var breakPercentage = BreakPoint / MaxBreakPoint;
                checkBlueShieldDamage(cmd.AttackDamage);
                onHitParticle_.transform.position = cmd.CollidePoint;
                //onHitParticle_.GetComponent<ParticleSystem>().Play();
                onHitParticle_.GetComponent<VisualEffect>().Play();
                GameManager.Instance.MainGameEvent.Send(new PlayerAttackSuccessResponse(cmd, percentage,breakPercentage));
                StartCoroutine("beAttackTimer");
            }
        }
    }
    async void breakRevertTimer()
    {
        await UniTask.Delay(5000);
        if (break_ && canBeExecute_)
        {
            RevertBreakPoint();
        }
    }
    public void GainBlueShield(float maxBlueShield) //獲得藍盾 
    {
        BlueShieldOn = true;
        CurBlueShieldPoint = maxBlueShield;
        maxBlueShieldPoint_ = maxBlueShield;
    }

    public void RevertBreakPoint() //取消Break狀態 讓一個break循環可以再來一次
    {
        Break = false;
        BreakPoint = 0;
    }
    void checkBlueShieldDamage(float damage)//扣除藍盾
    {
        CurBlueShieldPoint = CurBlueShieldPoint - damage;
        if (CurBlueShieldPoint <= 0)
        {
            BlueShieldOn = false;
            CurBlueShieldPoint = 0;
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
            breakRevertTimer();
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
    private void Update()
    {
        if (GainBlueShieldTrigger)
        {
            GainBlueShield(BlueShieldSettingParameter);
            GainBlueShieldTrigger = false;
        }
        if (RevertBreakPointTrigger)
        {
            RevertBreakPoint();
            RevertBreakPointTrigger = false;
        }
    }
    private void FixedUpdate() //脫離戰鬥後減少bk值
    {
        if (beenHurt_)
        {
            avoidDamageTicks_++;
            if (avoidDamageTicks_ >= maxNeedAvoidDamegeTicks_)
            {
                beenHurt_ = false;
                avoidDamageTicks_ = 0;
            }
        }
        else
        {
            if (!Break)
            {
                BreakPoint = Mathf.Clamp(BreakPoint - 5 * Time.deltaTime, 0, MaxBreakPoint);
                var breakPercentage = BreakPoint / MaxBreakPoint;
                GameManager.Instance.UIGameEvent.Send(new UIUpdateBreakCommand() { AttackTarget = gameObject,BreakPercentage = breakPercentage });
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