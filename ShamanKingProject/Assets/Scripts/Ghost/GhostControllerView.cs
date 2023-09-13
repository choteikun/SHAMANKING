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
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerLaunchFinish, ghostPossessedStateOver);

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
    void ghostPossessedStateOver(PlayerLaunchFinishCommand command)
    {
        Debug.Log("command.Hit : " + command.Hit);

        if (command.Hit)
        {
            Debug.Log("command.Hit : " + command.Hit);
            mat_Dissolve();
            ghost_Stats_.ghostCurrentState = GhostState.GHOST_POSSESSED;
            //這邊要阻止command.Hit回傳false
        }
        //在附身狀態時卻又什麼都沒碰撞到的時候
        else
        {
            Debug.Log("command.Hit : " + command.Hit);
            if (ghost_Stats_.ghostCurrentState == GhostState.GHOST_POSSESSED)
            {
                mat_Revert();
                ghost_Stats_.ghostCurrentState = GhostState.GHOST_IDLE;
            }
            else
            {
                
                //回到待機狀態
                mat_Dissolve();
                mat_Revert();
                ghost_Stats_.ghostCurrentState = GhostState.GHOST_IDLE;
            }
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (ghost_Stats_.Ghost_ShootOut_)
        {
            if (other.CompareTag("Possessable"))
            {
                GameManager.Instance.MainGameEvent.Send(new PlayerLaunchFinishCommand() { Hit = true });
            }
            else
            {
                GameManager.Instance.MainGameEvent.Send(new PlayerLaunchFinishCommand() { Hit = true });
            }
        }
    }
    void mat_Revert()
    {
        Observable.Timer(TimeSpan.FromSeconds(1.5f)).Subscribe(_ => { mat_ShaderValueFloatTo("_DissolveAmount", ghost_Stats_.GhostShader_DissolveAmount = 1, 0, 1f); }).AddTo(this);
        
        Observable.Timer(TimeSpan.FromSeconds(2.4f)).Subscribe(_ => { mat_ShaderValueFloatTo("_SmoothStepAmount", ghost_Stats_.GhostShader_SmoothStepAmount = 1, 0, 0.1f); }).AddTo(this);
    }
    void mat_Dissolve()
    {
        //溶解特效啟動1秒後結束
        mat_ShaderValueFloatTo("_DissolveAmount", ghost_Stats_.GhostShader_DissolveAmount = 0, 1, 0.5f);
        //0.1秒後才啟動邊緣光0.4秒後結束
        Observable.Timer(TimeSpan.FromSeconds(0.1f)).Subscribe(_ => { mat_ShaderValueFloatTo("_SmoothStepAmount", ghost_Stats_.GhostShader_SmoothStepAmount = 1, 1, 0.4f); }).AddTo(this);
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

    public bool Ghost_Unpossessed;
}
