using Gamemanager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using DG.Tweening;
using UniRx;
using Language.Lua;

public enum GhostState
{
    GHOST_IDLE,
    GHOST_MOVEMENT,
    GHOST_POSSESSED,
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

    

    void Awake()
    {
        
        
        ghostAnimator_ = new GhostAnimator(this.gameObject);
        ghostController_ = new GhostController(this.gameObject);
        ghostController_.Awake();
    }
    void Start()
    {
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnAimingButtonTrigger, ghostReadyButtonTrigger);
        ghost_Stats_.rb = GetComponent<Rigidbody>();
        if (!ghost_Stats_.Ghost_SkinnedMeshRenderer)
        {
            ghost_Stats_.Ghost_SkinnedMeshRenderer = GameObject.Find("Ghost_Mesh").GetComponent<SkinnedMeshRenderer>();
        }
        ghost_Stats_.GhostShader_DissolveAmount = 0;

        behaviorTree = GetComponent<BehaviorTree>();

        switchExternalBehavior((int)GhostState.GHOST_IDLE);
        ghost_Stats_.ghostCurrentState = GhostState.GHOST_IDLE;

        ghostAnimator_.Start(ghost_Stats_);
        ghostController_.Start(ghost_Stats_);
    }

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
    

    private void OnTriggerEnter(Collider other)
    {
        if (ghost_Stats_.Ghost_ShootOut_)
        {
            if (other.CompareTag("Possessable"))
            {
                GameManager.Instance.MainGameEvent.Send(new PlayerLaunchFinishCommand() { Hit = true });
                ghost_Stats_.ghostCurrentState = GhostState.GHOST_POSSESSED;
                mat_Dissolve();
            }
            else
            {
                GameManager.Instance.MainGameEvent.Send(new PlayerLaunchFinishCommand() { Hit = true });
                ghost_Stats_.ghostCurrentState = GhostState.GHOST_IDLE;
                mat_Dissolve();
            }
        }
    }
    void mat_Dissolve()
    {
        mat_ShaderValueFloatTo("_DissolveAmount", ghost_Stats_.GhostShader_DissolveAmount, 1, 1);
        Observable.Timer(TimeSpan.FromSeconds(0.1f)).Subscribe(_ => { mat_ShaderValueFloatTo("_SmoothStepAmount", ghost_Stats_.GhostShader_SmoothStepAmount = 1, 1, 0.9f); }).AddTo(this);
        Observable.Timer(TimeSpan.FromSeconds(1f)).Subscribe(_ => { mat_ShaderValueFloatTo("_DissolveAmount", ghost_Stats_.GhostShader_DissolveAmount, 0, 1); }).AddTo(this);
        Observable.Timer(TimeSpan.FromSeconds(1.9f)).Subscribe(_ => { mat_ShaderValueFloatTo("_SmoothStepAmount", ghost_Stats_.GhostShader_SmoothStepAmount = 0, 0, 0.1f); }).AddTo(this);
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
                Debug.Log(ghost_Stats_.Ghost_SkinnedMeshRenderer.material.name + curValue);
                ghost_Stats_.Ghost_SkinnedMeshRenderer.material.SetFloat(ShaderValueName, curValue);
            })
            .OnComplete(() =>
            {
                // 在動畫完成時執行任何需要的操作
                Debug.Log(ShaderValueName + "Complete!");
            });
    }

    void Update()
    {

        ghostAnimator_.Update();
        ghostController_.Update();

        ghost_Stats_.Ghost_Timer += Time.deltaTime;
    }


    //切換外部行為樹
    void switchExternalBehavior(int externalTrees)
    {
        if (externalBehaviorTrees[externalTrees] != null)
        {
            behaviorTree.DisableBehavior();
            behaviorTree.ExternalBehavior = externalBehaviorTrees[externalTrees];
            behaviorTree.EnableBehavior();
        }
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
    public bool Ghost_Interrupted;
}
