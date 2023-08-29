using Gamemanager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UniRx;
using UniRx.Triggers;

public enum PlayerAnimState
{
    Idle,
    BeAttack,
    Dead
}
[Serializable]
public class PlayerAnimator 
{
    #region 提前Hash進行優化
    readonly int animID_AimMoveX = Animator.StringToHash("AimMoveX");
    readonly int animID_AimMoveY = Animator.StringToHash("AimMoveY");

    readonly int animID_TimeOutToIdle = Animator.StringToHash("TimeOutToIdle");

    readonly int animID_AnimMoveSpeed = Animator.StringToHash("AnimMoveSpeed");

    readonly int animID_InputDetected = Animator.StringToHash("InputDetected");
    readonly int animID_Grounded = Animator.StringToHash("Grounded");
    readonly int animID_Aiming = Animator.StringToHash("Aiming");
    readonly int animID_Airborne = Animator.StringToHash("Airborne");//空中降落，下降是true，上升是false

    readonly int animID_Idle = Animator.StringToHash("Idle");
    //readonly int h_Jump = Animator.StringToHash("Jump");
    #endregion

    //當前狀態
    private PlayerAnimState playerAnimState_;

    private Animator animator_;
    private ObservableStateMachineTrigger animOSM_Trigger_;

    //檢測按鈕
    private bool inputDetected_;
    //角色動畫水平速度
    private float player_horizontalAnimVel_;
    //角色動畫垂直速度
    private float player_verticalVel_;


    [SerializeField, Tooltip("過渡到隨機Idle動畫所需要花的時間")]
    private float idleTimeOut_;
    //Idle動畫計時器(跳轉至隨機動畫)
    private float idleTimer_;

    //Move狀態中回到idle的計時器(避免方向鍵切換時角色回到idle狀態)
    private float moveToIdleTimer_;

    private Player_Stats player_Stats_;

    private GameObject characterControllerObj_;



    public PlayerAnimator(GameObject controller)
    {
        characterControllerObj_ = controller;
    }
    public void Start(Player_Stats player_Stats)
    {
        player_Stats_ = player_Stats;
        var haveAnimatorObject = characterControllerObj_.gameObject.transform.GetChild(0);
        animator_ = haveAnimatorObject.gameObject.GetComponent<Animator>();
        animOSM_Trigger_ = animator_.GetBehaviour<ObservableStateMachineTrigger>();

        //範例
        //IObservable<ObservableStateMachineTrigger.OnStateInfo> idleStart = animOSM_Trigger_.OnStateEnterAsObservable().Where(x => x.StateInfo.IsName("Idle"));
        //IObservable<ObservableStateMachineTrigger.OnStateInfo> idleEnd = animOSM_Trigger_.OnStateExitAsObservable().Where(x => x.StateInfo.IsName("Idle"));

        //characterControllerObj_.UpdateAsObservable().SkipUntil(idleStart).TakeUntil(idleEnd).RepeatUntilDestroy(characterControllerObj_).Subscribe(x => { Debug.Log("Idle中"); }).AddTo(characterControllerObj_);
    }

    public void Update()
    {
        setHorizontalAnimVel(player_Stats_);
        setPlayer_animID_Grounded(player_Stats_);
        setPlayer_animID_Aiming(player_Stats_);
        //TimeoutToIdle();

        #region - 簡易動畫狀態管理 
        switch (playerAnimState_)
        {
            case PlayerAnimState.Idle:
                //animator.SetBool(h_Idle, true);
                //_moveToIdleTimer = 0;
                break;


            default:
                break;
        }
        #endregion

    }

    #region - 待機動畫處理 -
    void TimeoutToIdle()
    {
        if (playerAnimState_ == PlayerAnimState.Idle)
        {
            idleTimer_ += Time.deltaTime;

            if (idleTimer_ >= idleTimeOut_)
            {
                idleTimer_ = 0f;
                //animator.SetTrigger(h_TimeOutToIdle);
            }
        }
        else
        {
            idleTimer_ = 0f;
            //animator.ResetTrigger(h_TimeOutToIdle);
        }
        //animator.SetBool(h_InputDetected, inputDetected);
    }
    #endregion

    #region - 動畫參數計算 -

    void setHorizontalAnimVel(Player_Stats player_Stats)//設置動畫水平速度給LocomtionBlendTree
    {
        player_horizontalAnimVel_ = Mathf.Lerp(player_horizontalAnimVel_, player_Stats_.Player_Speed, Time.deltaTime * player_Stats.SpeedChangeRate);

        //if (player_Stats_.Player_Speed <= 1.0f)
        //{
        //    player_horizontalAnimVel_ = Mathf.Lerp(player_horizontalAnimVel_, player_Stats_.Player_Speed * 2.0f, Time.deltaTime * player_Stats.SpeedChangeRate);
        //}
        //else
        //{
        //    player_horizontalAnimVel_ = player_Stats_.Player_Speed;
        //}
        if (player_horizontalAnimVel_ < 0.01f) player_horizontalAnimVel_ = 0f;
        animator_.SetFloat(animID_AnimMoveSpeed, player_horizontalAnimVel_);
    }
    void setPlayer_animID_Grounded(Player_Stats player_Stats)
    {
        animator_.SetBool(animID_Grounded, player_Stats.Grounded);
    }
    void setPlayer_animID_Aiming(Player_Stats player_Stats)
    {
        animator_.SetBool(animID_Aiming, player_Stats.Aiming);
        animator_.SetFloat(animID_AimMoveX, player_Stats_.Player_Dir.x);
        animator_.SetFloat(animID_AimMoveY, player_Stats_.Player_Dir.y);
    }
    #endregion


    
    #region - Animation Events -

    #endregion
}
