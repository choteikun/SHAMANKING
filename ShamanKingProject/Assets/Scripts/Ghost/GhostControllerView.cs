using BehaviorDesigner.Runtime;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Gamemanager;
using System;
using UniRx;
using UnityEngine;

public enum GhostState
{
    //待機狀態
    GHOST_IDLE,
    //移動狀態
    GHOST_MOVEMENT,
    //化學反應狀態
    GHOST_REACT,
}
public class GhostControllerView : MonoBehaviour
{

    //public float GhostShader_DissolveAmount_forTree { get { return GhostShader_DissolveAmount; } set { GhostShader_DissolveAmount = value; } }

    [SerializeField]
    Ghost_Stats ghost_Stats_ = new Ghost_Stats();

    [SerializeField]
    GhostAnimator ghostAnimator_;

    [SerializeField]
    GhostController ghostController_;

    [SerializeField]
    private BehaviorTree behaviorTree;
    [SerializeField]
    private ExternalBehavior[] externalBehaviorTrees;

    [SerializeField]
    Material chainMat_;

    Tweener chainRevertTweener_;
    bool chainActivating_ = true;

    void Awake()
    {
        ghostAnimator_ = new GhostAnimator(this.gameObject);
        ghostController_ = new GhostController(this.gameObject);
        ghostController_.Awake();
    }
    void Start()
    {
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerLaunchActionFinish, ghostReactState);
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnGhostAnimationEvents, ghostAnimationEventsToDo);
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerThrowAttackFinish, cmd => { mat_DissolveWithRevertThrowAttack(); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerCancelPossess, cmd => { mat_Revert(); });//有時候會在Ghost_Back start前就呼叫(尚未解決)
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerCancelPossess, cmd => { GameManager.Instance.MainGameEvent.Send(new GhostLaunchProcessFinishResponse()); });
        // GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerMoveStatusChange, cmd => { playerMoveStatusToChainBehavior(cmd); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnAimingButtonTrigger, cmd => { playerAimReactivateChain(cmd); });

        behaviorTree = GetComponent<BehaviorTree>();
        ghost_Stats_.rb = GetComponent<Rigidbody>();

        if (!ghost_Stats_.Ghost_SkinnedMeshRenderer)
        {
            ghost_Stats_.Ghost_SkinnedMeshRenderer = GameObject.Find("Ghost_Mesh").GetComponent<SkinnedMeshRenderer>();
        }

        ghostAnimator_.Start(ghost_Stats_);
        ghostController_.Start(ghost_Stats_);

        ghost_Reset();
    }

    #region - 鬼魂參數重設 -
    void ghost_Reset()
    {
        ghost_Stats_.GhostShader_DissolveAmount = 0;

        switchExternalBehavior((int)GhostState.GHOST_IDLE);
        ghost_Stats_.ghostCurrentState = GhostState.GHOST_IDLE;
    }
    #endregion

    #region - 切換外部行為樹 -
    void switchExternalBehavior(int externalTrees)
    {
        if (externalBehaviorTrees[externalTrees] != null)
        {
            behaviorTree.DisableBehavior();
            behaviorTree.ExternalBehavior = externalBehaviorTrees[externalTrees];
            behaviorTree.EnableBehavior();
        }
    }
    #endregion



    #region - 鬼魂動畫事件管理 -
    void ghostAnimationEventsToDo(GhostAnimationEventsCommand command)
    {
        switch (command.AnimationEventName)
        {
            case "GhostMat_Dissolve":
                if (command.AnimationType == GhostAnimationType.DissolveWithRevert)
                {
                    mat_DissolveWithRevert();
                }
                else
                {
                    mat_Dissolve();
                }
                break;
            case "GhostMat_Revert":
                if (command.AnimationType == GhostAnimationType.DissolveWithRevert)
                {
                    mat_Revert();
                }
                break;
            case "Ghost_Bite_End":
                ghost_Stats_.Ghost_Biteable = false;
                ghost_Stats_.ghostCurrentState = GhostState.GHOST_IDLE;
                GameManager.Instance.MainGameEvent.Send(new GhostLaunchProcessFinishResponse());
                break;

            default:
                break;
        }
    }

    void playerMoveStatusToChainBehavior(PlayerMoveStatusChangeCommand cmd)
    {
        if (cmd.IsMoving)
        {
            if (chainRevertTweener_ != null) chainRevertTweener_.Kill();
            if (chainActivating_ == false) return;
            chain_Dissolve();
        }
        else
        {
            float tween = 0;
            chainRevertTweener_ = DOTween.To(() => tween, x => tween = x, 1, 2)
            .OnUpdate(() =>
            {

            })
            .OnComplete(() =>
            {
                chain_Revert();
            });
        }
    }

    void playerAimReactivateChain(PlayerAimingButtonCommand cmd)
    {
        if (cmd.AimingButtonIsPressed)
        {
            if (!chainActivating_)
            {
                chain_Revert();
            }
        }
    }
    #endregion

    #region - 鬼魂碰撞處理 -
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (ghost_Stats_.Ghost_ShootOut_)
    //    {
    //        //想用switch case提高可讀性，不過就效能來說先這樣寫就好

    //        if (other.CompareTag("Untagged"))
    //        {
    //            GameManager.Instance.MainGameEvent.Send(new PlayerLaunchActionFinishCommand() { HitObjecctTag = HitObjecctTag.None });
    //        }
    //        if (other.CompareTag("Biteable"))
    //        {
    //            var hitInfo = other.GetComponent<HitableItemTest>();
    //            GameManager.Instance.MainGameEvent.Send(new PlayerLaunchActionFinishCommand() { Hit = true, HitObjecctTag = HitObjecctTag.Biteable, HitInfo = hitInfo });
    //        }
    //        if (other.CompareTag("Possessable"))
    //        {
    //            var hitInfo = other.GetComponent<HitableItemTest>();
    //            GameManager.Instance.MainGameEvent.Send(new PlayerLaunchActionFinishCommand() { Hit = true, HitObjecctTag = HitObjecctTag.Possessable, HitInfo = hitInfo });
    //        }
    //        if (other.CompareTag("Enemy"))
    //        {
    //            var hitInfo = other.GetComponent<HitableItemTest>();
    //            GameManager.Instance.MainGameEvent.Send(new PlayerLaunchActionFinishCommand() { Hit = true, HitObjecctTag = HitObjecctTag.Enemy, HitInfo = hitInfo });
    //        }
    //    }
    //}
    void ghostReactState(PlayerLaunchActionFinishCommand command)
    {
        switch (command.HitObjecctTag)
        {
            //什麼都沒碰撞到
            case HitObjecctTag.None:
                ghost_Stats_.ghostCurrentState = GhostState.GHOST_IDLE;
                break;
            //碰到可吸收的物體
            case HitObjecctTag.Biteable:
                ghost_Stats_.ghostCurrentState = GhostState.GHOST_REACT;
                ghost_Stats_.Ghost_Biteable = true;
                break;
            //碰到可附身的物體
            case HitObjecctTag.Possessable:
                ghost_Stats_.ghostCurrentState = GhostState.GHOST_REACT;
                ghost_Stats_.Ghost_Possessable = true;
                break;
            //碰到敵人
            case HitObjecctTag.Enemy:
                ghost_Stats_.ghostCurrentState = GhostState.GHOST_REACT;
                //未來還要添加個對Enemy的判斷
                break;
            default:
                ghost_Stats_.ghostCurrentState = GhostState.GHOST_IDLE;
                Debug.LogError("我不知道到底撞到了啥小");
                break;
        }
    }
    #endregion

    #region - 材質球特效處理 -
    void mat_Revert()
    {
        //Observable.Timer(TimeSpan.FromSeconds(1.5f)).Subscribe(_ => { mat_ShaderValueFloatTo("_DissolveAmount", ghost_Stats_.GhostShader_DissolveAmount = 1, 0, 0.5f); }).AddTo(this);

        //Observable.Timer(TimeSpan.FromSeconds(1.9f)).Subscribe(_ => { mat_ShaderValueFloatTo("_SmoothStepAmount", ghost_Stats_.GhostShader_SmoothStepAmount = 1, 0, 0.1f); }).AddTo(this);

        mat_ShaderValueFloatTo("_DissolveAmount", ghost_Stats_.GhostShader_DissolveAmount = 1, 0, 0.5f, chainMat_);
        mat_ShaderValueFloatTo("_DissolveAmount", ghost_Stats_.GhostShader_DissolveAmount = 1, 0, 0.5f, ghost_Stats_.Ghost_SkinnedMeshRenderer.material);

        Observable.Timer(TimeSpan.FromSeconds(0.4f)).Subscribe(_ => { mat_ShaderValueFloatTo("_SmoothStepAmount", ghost_Stats_.GhostShader_SmoothStepAmount = 1, 0, 0.1f, chainMat_); }).AddTo(this);
        Observable.Timer(TimeSpan.FromSeconds(0.4f)).Subscribe(_ => { mat_ShaderValueFloatTo("_SmoothStepAmount", ghost_Stats_.GhostShader_SmoothStepAmount = 1, 0, 0.1f, ghost_Stats_.Ghost_SkinnedMeshRenderer.material); }).AddTo(this);
        Debug.Log("mat_Revert");
    }
    async void mat_DissolveWithRevert()
    {
        //溶解特效啟動1秒後結束
        //mat_ShaderValueFloatTo("_DissolveAmount", ghost_Stats_.GhostShader_DissolveAmount = 0, 1, 0.5f, chainMat_);
        //mat_ShaderValueFloatTo("_DissolveAmount", ghost_Stats_.GhostShader_DissolveAmount = 0, 1, 0.5f, ghost_Stats_.Ghost_SkinnedMeshRenderer.material);
        ////0.1秒後才啟動邊緣光0.4秒後結束
        //Observable.Timer(TimeSpan.FromSeconds(0.1f)).Subscribe(_ => { mat_ShaderValueFloatTo("_SmoothStepAmount", ghost_Stats_.GhostShader_SmoothStepAmount = 1, 1, 0.4f, chainMat_); }).AddTo(this);
        //Observable.Timer(TimeSpan.FromSeconds(0.1f)).Subscribe(_ => { mat_ShaderValueFloatTo("_SmoothStepAmount", ghost_Stats_.GhostShader_SmoothStepAmount = 1, 1, 0.4f, ghost_Stats_.Ghost_SkinnedMeshRenderer.material); }).AddTo(this);
        //await UniTask.Delay(300);
        DOVirtual.Float(-0.5f, 1.5f, 0.2f, value =>
        {
            Vector4 currentParams = chainMat_.GetVector("_DissolveParams");
            Debug.Log(currentParams);
            currentParams.z = value;
            currentParams.w = 0.5f;

            chainMat_.SetVector("_DissolveParams", currentParams);
        });
        await UniTask.Delay(250);
        DOVirtual.Float(1.5f, -1f, 0.6f, value =>
        {
            Vector4 currentParams = chainMat_.GetVector("_DissolveParams");
            Debug.Log(currentParams);
            currentParams.z = value;
            currentParams.w = 0.5f;

            chainMat_.SetVector("_DissolveParams", currentParams);
        }).SetEase(Ease.InSine);
        GameManager.Instance.MainGameEvent.Send(new GhostLaunchProcessFinishResponse());
        Debug.Log("mat_DissolveWithRevert");
        ////1秒後回復原狀特效啟動1秒後結束
        //Observable.Timer(TimeSpan.FromSeconds(1f)).Subscribe(_ => { mat_ShaderValueFloatTo("_DissolveAmount", ghost_Stats_.GhostShader_DissolveAmount, 0, 1); }).AddTo(this);
        ////1.9秒後邊緣光關閉0.1秒後結束
        //Observable.Timer(TimeSpan.FromSeconds(1.9f)).Subscribe(_ => { mat_ShaderValueFloatTo("_SmoothStepAmount", ghost_Stats_.GhostShader_SmoothStepAmount = 0, 0, 0.1f); }).AddTo(this);
    }
    async void mat_DissolveWithRevertThrowAttack()
    {
        //溶解特效啟動1秒後結束
        //mat_ShaderValueFloatTo("_DissolveAmount", ghost_Stats_.GhostShader_DissolveAmount = 0, 1, 0.5f, chainMat_);
        //mat_ShaderValueFloatTo("_DissolveAmount", ghost_Stats_.GhostShader_DissolveAmount = 0, 1, 0.5f, ghost_Stats_.Ghost_SkinnedMeshRenderer.material);
        ////0.1秒後才啟動邊緣光0.4秒後結束
        //Observable.Timer(TimeSpan.FromSeconds(0.1f)).Subscribe(_ => { mat_ShaderValueFloatTo("_SmoothStepAmount", ghost_Stats_.GhostShader_SmoothStepAmount = 1, 1, 0.4f, chainMat_); }).AddTo(this);
        //Observable.Timer(TimeSpan.FromSeconds(0.1f)).Subscribe(_ => { mat_ShaderValueFloatTo("_SmoothStepAmount", ghost_Stats_.GhostShader_SmoothStepAmount = 1, 1, 0.4f, ghost_Stats_.Ghost_SkinnedMeshRenderer.material); }).AddTo(this);
        //await UniTask.Delay(300);
        DOVirtual.Float(-0.5f, 1.5f, 0.2f, value =>
        {
            Vector4 currentParams = chainMat_.GetVector("_DissolveParams");
            Debug.Log(currentParams);
            currentParams.z = value;
            currentParams.w = 0.5f;

            chainMat_.SetVector("_DissolveParams", currentParams);
        });
        await UniTask.Delay(250);
        DOVirtual.Float(1.5f, -1f, 0.6f, value =>
        {
            Vector4 currentParams = chainMat_.GetVector("_DissolveParams");
            Debug.Log(currentParams);
            currentParams.z = value;
            currentParams.w = 0.5f;

            chainMat_.SetVector("_DissolveParams", currentParams);
        }).SetEase(Ease.InSine);
        Debug.Log("mat_DissolveWithRevert");
        ////1秒後回復原狀特效啟動1秒後結束
        //Observable.Timer(TimeSpan.FromSeconds(1f)).Subscribe(_ => { mat_ShaderValueFloatTo("_DissolveAmount", ghost_Stats_.GhostShader_DissolveAmount, 0, 1); }).AddTo(this);
        ////1.9秒後邊緣光關閉0.1秒後結束
        //Observable.Timer(TimeSpan.FromSeconds(1.9f)).Subscribe(_ => { mat_ShaderValueFloatTo("_SmoothStepAmount", ghost_Stats_.GhostShader_SmoothStepAmount = 0, 0, 0.1f); }).AddTo(this);
    }
    void mat_Dissolve()
    {
        //溶解特效啟動1秒後結束
        mat_ShaderValueFloatTo("_DissolveAmount", ghost_Stats_.GhostShader_DissolveAmount = 0, 1, 0.5f, ghost_Stats_.Ghost_SkinnedMeshRenderer.material);
        //0.1秒後才啟動邊緣光0.4秒後結束
        Observable.Timer(TimeSpan.FromSeconds(0.1f)).Subscribe(_ => { mat_ShaderValueFloatTo("_SmoothStepAmount", ghost_Stats_.GhostShader_SmoothStepAmount = 1, 1, 0.4f, ghost_Stats_.Ghost_SkinnedMeshRenderer.material); }).AddTo(this);
        Debug.Log("mat_Dissolve");
    }
    void chain_Dissolve()
    {
        //溶解特效啟動1秒後結束
        mat_ShaderValueFloatTo("_DissolveAmount", ghost_Stats_.GhostShader_DissolveAmount = 0, 1, 0.5f, chainMat_);
        //0.1秒後才啟動邊緣光0.4秒後結束
        Observable.Timer(TimeSpan.FromSeconds(0.1f)).Subscribe(_ => { mat_ShaderValueFloatTo("_SmoothStepAmount", ghost_Stats_.GhostShader_SmoothStepAmount = 1, 1, 0.4f, chainMat_); }).AddTo(this);
        Debug.Log("chain_Dissolve");
        chainActivating_ = false;
    }
    void chain_Revert()
    {
        mat_ShaderValueFloatTo("_DissolveAmount", ghost_Stats_.GhostShader_DissolveAmount = 1, 0, 0.5f, chainMat_);

        Observable.Timer(TimeSpan.FromSeconds(0.4f)).Subscribe(_ => { mat_ShaderValueFloatTo("_SmoothStepAmount", ghost_Stats_.GhostShader_SmoothStepAmount = 1, 0, 0.1f, chainMat_); }).AddTo(this);
        Debug.Log("chain_Revert");
        chainActivating_ = true;
    }
    /// <summary>
    /// 使用Dotween快速實現Shader過渡(Float)
    /// </summary>
    /// <param name="ShaderValueName">獲取Shader Inspector你需要的參數名字</param>
    /// <param name="curValue">當下Shader參數的Float值</param>
    /// <param name="endValue">過渡到最後的值</param>
    /// <param name="lerpTime">過渡時間</param>
    void mat_ShaderValueFloatTo(string ShaderValueName, float curValue, float endValue, float lerpTime, Material material)
    {
        DOTween.To(() => curValue, x => curValue = x, endValue, lerpTime)
            .OnUpdate(() =>
            {
                // 在動畫更新時，可以使用 currentValue 來獲取當前的 float 值
                //Debug.Log(ghost_Stats_.Ghost_SkinnedMeshRenderer.material.name + curValue);
                //ghost_Stats_.Ghost_SkinnedMeshRenderer.material.SetFloat(ShaderValueName, curValue);
                material.SetFloat(ShaderValueName, curValue); //硬加的
            })
            .OnComplete(() =>
            {
                // 在動畫完成時執行任何需要的操作
                //Debug.Log(ShaderValueName + "Complete!");
            });
    }
    #endregion
    void Update()
    {

        ghostAnimator_.Update();
        ghostController_.Update();

        ghost_Stats_.Ghost_Timer += Time.deltaTime;
    }
}

[Serializable]
public class Ghost_Stats
{

    public GhostState ghostCurrentState;

    public Rigidbody rb;

    public SkinnedMeshRenderer Ghost_SkinnedMeshRenderer;

    public float Player_Distance;

    public float GhostShader_DissolveAmount;
    public float GhostShader_SmoothStepAmount;

    public float Ghost_Timer;

    public bool Ghost_ReadyButton;
    public bool Ghost_ShootOut_;

    public bool Ghost_Possessable;
    public bool Ghost_Biteable;
}
