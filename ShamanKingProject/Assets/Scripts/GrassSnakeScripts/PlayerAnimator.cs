using Gamemanager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UniRx;
using UniRx.Triggers;
using UnityEngine.Windows;
using UnityEditor;

public enum PlayerAnimState
{
    Attack,
    BeAttack,
    Dead
}
[Serializable]
public class PlayerAnimator 
{
    #region 提前Hash進行優化
    readonly int animID_AimMove = Animator.StringToHash("AimMove");
    readonly int animID_AimIdle = Animator.StringToHash("AimIdle");

    readonly int animID_AimMoveX = Animator.StringToHash("AimMoveX");
    readonly int animID_AimMoveY = Animator.StringToHash("AimMoveY");

    readonly int animID_TimeOutToIdle = Animator.StringToHash("TimeOutToIdle");

    readonly int animID_AnimMoveSpeed = Animator.StringToHash("AnimMoveSpeed");

    readonly int animID_InputDetected = Animator.StringToHash("InputDetected");
    readonly int animID_Grounded = Animator.StringToHash("Grounded");

    readonly int animID_Airborne = Animator.StringToHash("Airborne");//空中降落，下降是true，上升是false

    readonly int animID_Idle = Animator.StringToHash("Idle");

    readonly int animID_Attack_CanBeInterrupted = Animator.StringToHash("Attack_CanBeInterrupted");
    readonly int animID_AttackCombo1 = Animator.StringToHash("AttackCombo1");
    readonly int animID_AttackCombo2 = Animator.StringToHash("AttackCombo2");
    readonly int animID_AttackCombo3 = Animator.StringToHash("AttackCombo3");
    //readonly int h_Jump = Animator.StringToHash("Jump");
    #endregion

    //當前狀態
    private PlayerAnimState playerAnimState_;

    private Animator animator_;
    private ObservableStateMachineTrigger animOSM_Trigger_;

    //檢測按鈕
    private bool inputDetected_;
    //瞄準下的移動狀態
    private bool aimMove_;
    //角色動畫水平速度
    private float player_horizontalAnimVel_;
    private float player_horizontalAnimX;
    private float player_horizontalAnimY;
    //角色動畫垂直速度
    private float player_verticalVel_;


    [SerializeField, Tooltip("過渡到隨機Idle動畫所需要花的時間")]
    private float idleTimeOut_ = 5;
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
        timeoutToIdle();

        #region - 簡易動畫狀態管理 
        switch (playerAnimState_)
        {
            case PlayerAnimState.Attack:

                //處在動畫過渡條時重置指令為不允許攻擊
                if (animator_.IsInTransition(0))
                {
                    player_Stats_.Player_AttackCommandAllow = false;
                }
                //當指令為不允許攻擊
                if (player_Stats_.Player_AttackCommandAllow)
                {
                    //Animator Parameters 裡的攻擊動畫為不可以被打斷的狀態
                    animator_.SetBool(animID_Attack_CanBeInterrupted, false);
                }
                //animation events 發送指令為允許攻擊
                else
                {
                    //Animator Parameters 裡的攻擊動畫為可以被打斷的狀態
                    animator_.SetBool(animID_Attack_CanBeInterrupted, true);
                    if (UnityEngine.Input.GetMouseButtonDown(0))
                    {
                        //攻擊動畫
                    }
                }
                
                break;
            

            default:
                break;
        }
        #endregion
       
    }

    #region - 待機動畫處理 -
    void timeoutToIdle()
    {
        bool inputDetected = player_Stats_.Player_Dir != Vector2.zero || player_Stats_.Aiming;
        //如果沒有偵測到任何輸入產生的行為
        if (!inputDetected)
        {
            
            idleTimer_ += Time.deltaTime;

            if (idleTimer_ >= idleTimeOut_)
            {
                idleTimer_ = 0f;
                animator_.SetTrigger(animID_TimeOutToIdle);
            }
        }
        else
        {
            idleTimer_ = 0f;
            animator_.ResetTrigger(animID_TimeOutToIdle);
        }
        animator_.SetBool(animID_InputDetected, inputDetected);
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
        animator_.SetBool(animID_AimMove, aimMove_);

        if (player_Stats.Aiming)
        {
            aimMove_ = player_Stats_.Player_Dir != Vector2.zero ? true : false;
            if (aimMove_)
            {
                player_horizontalAnimX = Mathf.Lerp(player_horizontalAnimX, player_Stats_.Player_Dir.x * player_Stats_.Player_Speed, Time.deltaTime * player_Stats.SpeedChangeRate);
                player_horizontalAnimY = Mathf.Lerp(player_horizontalAnimY, player_Stats_.Player_Dir.y * player_Stats_.Player_Speed, Time.deltaTime * player_Stats.SpeedChangeRate);

                animator_.SetFloat(animID_AimMoveX, player_horizontalAnimX);
                animator_.SetFloat(animID_AimMoveY, player_horizontalAnimY);

                animator_.SetBool(animID_AimIdle, false);
            }
            else
            {
                animator_.SetBool(animID_AimIdle, true);
            }
        }
        else
        {
            aimMove_ = false;
            animator_.SetBool(animID_AimIdle, false);
        }  
    }
    #endregion


    
    #region - Animation Events -

    #endregion
}
