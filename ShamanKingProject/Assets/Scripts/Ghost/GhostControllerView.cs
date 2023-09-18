using Gamemanager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using DG.Tweening;
using UniRx;
using Language.Lua;
using Cysharp.Threading.Tasks;

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

    void Awake()
    {       
        ghostAnimator_ = new GhostAnimator(this.gameObject);
        ghostController_ = new GhostController(this.gameObject);
        ghostController_.Awake();
    }
    void Start()
    {
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnAimingButtonTrigger, ghostReadyButtonTrigger);
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerLaunchFinish, ghostReactState);
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnGhostAnimationEvents, ghostAnimationEventsToDo);
        GameManager.Instance.MainGameEvent.OnGhostAnimationEvents.Where(cmd => cmd.AnimationEventName == "GhostMat_Dissolve").Subscribe(ghostAnimationEventsToDo).AddTo(this);
        GameManager.Instance.MainGameEvent.OnGhostAnimationEvents.Where(cmd => cmd.AnimationEventName == "GhostMat_Revert").Subscribe(ghostAnimationEventsToDo).AddTo(this);

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

    #region - 鬼魂獲取瞄準指令 -
    void ghostReadyButtonTrigger(PlayerAimingButtonCommand command)
    {
        if (command.AimingButtonIsPressed && ghost_Stats_.ghostCurrentState == GhostState.GHOST_IDLE)
        {
            //behaviorTree.SendEvent("MOVEMENT SENDEVENT TEST");
            //behaviorTree.SendEvent<object>("MOVEMENT SENDEVENT TEST", (object)transform.position);
            ghost_Stats_.Ghost_ReadyButton = true;
            switchExternalBehavior((int)GhostState.GHOST_MOVEMENT - 1);
            ghost_Stats_.ghostCurrentState = GhostState.GHOST_MOVEMENT; 
        }
        if (!command.AimingButtonIsPressed)
        {
            ghost_Stats_.Ghost_ReadyButton = false;
        }
    }
    #endregion

    #region - 鬼魂動畫事件管理 -
    void ghostAnimationEventsToDo(GhostAnimationEventsCommand command)
    {
        switch (command.AnimationEventName)
        {
            case "GhostMat_Dissolve":
                mat_Dissolve();
                break;
            case "GhostMat_Revert":
                mat_Revert();
                break;

            default:
                break;
        }
    }
    #endregion

    #region - 鬼魂碰撞處理 -
    private void OnTriggerEnter(Collider other)
    {
        if (ghost_Stats_.Ghost_ShootOut_)
        {
            //想用switch case提高可讀性，不過就效能來說先這樣寫就好

            if (other.CompareTag("Untagged")) 
            {
                GameManager.Instance.MainGameEvent.Send(new PlayerLaunchFinishCommand() { HitObjecctTag = HitObjecctTag.None });
            }
            if (other.CompareTag("Biteable")) 
            {
                GameManager.Instance.MainGameEvent.Send(new PlayerLaunchFinishCommand() { Hit = true, HitObjecctTag = HitObjecctTag.Biteable });
            }
            if (other.CompareTag("Possessable")) 
            {
                GameManager.Instance.MainGameEvent.Send(new PlayerLaunchFinishCommand() { Hit = true, HitObjecctTag = HitObjecctTag.Possessable });
            }
            if (other.CompareTag("Enemy")) 
            {
                GameManager.Instance.MainGameEvent.Send(new PlayerLaunchFinishCommand() { Hit = true, HitObjecctTag = HitObjecctTag.Enemy });
            }
        }
    }
    void ghostReactState(PlayerLaunchFinishCommand command)
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

        mat_ShaderValueFloatTo("_DissolveAmount", ghost_Stats_.GhostShader_DissolveAmount = 1, 0, 0.5f);

        Observable.Timer(TimeSpan.FromSeconds(0.4f)).Subscribe(_ => { mat_ShaderValueFloatTo("_SmoothStepAmount", ghost_Stats_.GhostShader_SmoothStepAmount = 1, 0, 0.1f); }).AddTo(this);
    }
    async void mat_Dissolve()
    {
        //溶解特效啟動1秒後結束
        mat_ShaderValueFloatTo("_DissolveAmount", ghost_Stats_.GhostShader_DissolveAmount = 0, 1, 0.5f);
        //0.1秒後才啟動邊緣光0.4秒後結束
        Observable.Timer(TimeSpan.FromSeconds(0.1f)).Subscribe(_ => { mat_ShaderValueFloatTo("_SmoothStepAmount", ghost_Stats_.GhostShader_SmoothStepAmount = 1, 1, 0.4f); }).AddTo(this);
        await UniTask.Delay(300);
        GameManager.Instance.MainGameEvent.Send(new GhostDisolveFinishResponse());
        ////1秒後回復原狀特效啟動1秒後結束
        //Observable.Timer(TimeSpan.FromSeconds(1f)).Subscribe(_ => { mat_ShaderValueFloatTo("_DissolveAmount", ghost_Stats_.GhostShader_DissolveAmount, 0, 1); }).AddTo(this);
        ////1.9秒後邊緣光關閉0.1秒後結束
        //Observable.Timer(TimeSpan.FromSeconds(1.9f)).Subscribe(_ => { mat_ShaderValueFloatTo("_SmoothStepAmount", ghost_Stats_.GhostShader_SmoothStepAmount = 0, 0, 0.1f); }).AddTo(this);
    }
    /// <summary>
    /// 使用Dotween快速實現Shader過渡(Float)
    /// </summary>
    /// <param name="ShaderValueName">獲取Shader Inspector你需要的參數名字</param>
    /// <param name="curValue">當下Shader參數的Float值</param>
    /// <param name="endValue">過渡到最後的值</param>
    /// <param name="lerpTime">過渡時間</param>
    void mat_ShaderValueFloatTo(string ShaderValueName, float curValue, float endValue, float lerpTime)
    {
        DOTween.To(() => curValue, x => curValue = x, endValue, lerpTime)
            .OnUpdate(() =>
            {
                // 在動畫更新時，可以使用 currentValue 來獲取當前的 float 值
                //Debug.Log(ghost_Stats_.Ghost_SkinnedMeshRenderer.material.name + curValue);
                ghost_Stats_.Ghost_SkinnedMeshRenderer.material.SetFloat(ShaderValueName, curValue);
                chainMat_.SetFloat(ShaderValueName, curValue); //硬加的
            })
            .OnComplete(() =>
            {
                // 在動畫完成時執行任何需要的操作
               // Debug.Log(ShaderValueName + "Complete!");
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
