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

[Serializable]
public class PlayerAnimator 
{
    #region 提前Hash進行優化
    readonly int animID_AimMove = Animator.StringToHash("AimMove");
    readonly int animID_AimIdle = Animator.StringToHash("AimIdle");
    readonly int animID_AimRecoil = Animator.StringToHash("AimRecoil");

    readonly int animID_AimMoveX = Animator.StringToHash("AimMoveX");
    readonly int animID_AimMoveY = Animator.StringToHash("AimMoveY");

    readonly int animID_TimeOutToIdle = Animator.StringToHash("TimeOutToIdle");
    readonly int animID_InterruptedByLocomotion = Animator.StringToHash("InterruptedByLocomotion");

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
    public enum PlayerAnimState
    {
        Locomotion,
        Attack,
        BeAttack,
        Dead
    }
    
    public PlayerAnimState playerAnimState_;

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
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerAnimationEvents, playertAnimationEventsToDo);
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerLaunchGhost, playerAimRecoil);
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerLightAttack, playerLightAttack);

        player_Stats_ = player_Stats;
        var haveAnimatorObject = characterControllerObj_.gameObject.transform.GetChild(0);
        animator_ = haveAnimatorObject.gameObject.GetComponent<Animator>();
        animOSM_Trigger_ = animator_.GetBehaviour<ObservableStateMachineTrigger>();

        playerAnimState_ = PlayerAnimState.Locomotion;

        //範例
        //IObservable<ObservableStateMachineTrigger.OnStateInfo> idleStart = animOSM_Trigger_.OnStateEnterAsObservable().Where(x => x.StateInfo.IsName("Idle"));
        //IObservable<ObservableStateMachineTrigger.OnStateInfo> idleEnd = animOSM_Trigger_.OnStateExitAsObservable().Where(x => x.StateInfo.IsName("Idle"));

        //characterControllerObj_.UpdateAsObservable().SkipUntil(idleStart).TakeUntil(idleEnd).RepeatUntilDestroy(characterControllerObj_).Subscribe(x => { Debug.Log("Idle中"); }).AddTo(characterControllerObj_);
    }

    public void Update()
    {
        setHorizontalAnimVel();
        setPlayer_animID_Grounded();
        setPlayer_animID_Aiming();
        timeoutToIdle();

        #region - 簡易動畫狀態管理 
        switch (playerAnimState_)
        {
            case PlayerAnimState.Locomotion:

                break;

            case PlayerAnimState.Attack:
                //Debug.Log(player_Stats_.Player_AttackCommandAllow);
                //當指令為不允許攻擊
                if (!player_Stats_.Player_AttackCommandAllow)
                {
                    //Animator Parameters 裡的攻擊動畫為不可以被打斷的狀態
                }
                //animation events 發送指令為允許攻擊
                else
                {
                    //Debug.Log(animator_.GetCurrentAnimatorStateInfo(0).length * animator_.GetCurrentAnimatorStateInfo(0).normalizedTime);
                    //Animator Parameters 裡的攻擊動畫為可以被打斷的狀態
                } 
                break;
            default:
                break;
        }
        #endregion
       
    }
    void playerLightAttack(PlayerLightAttackButtonCommand command)
    {
        switch (playerAnimState_)
        {
            case PlayerAnimState.Locomotion:
                animator_.SetTrigger(animID_AttackCombo1);
                playerAnimState_ = PlayerAnimState.Attack;
                break;
            case PlayerAnimState.Attack:
                //當指令為不允許攻擊
                if (!player_Stats_.Player_AttackCommandAllow)
                {
                    //Animator Parameters 裡的攻擊動畫為不可以被打斷的狀態
                    //animID_Attack_CanBeInterrupted 觀察用
                    animator_.SetBool(animID_Attack_CanBeInterrupted, false);
                    //reset attack Comb
                    animator_.ResetTrigger(animID_AttackCombo1);
                    animator_.ResetTrigger(animID_AttackCombo2);
                    animator_.ResetTrigger(animID_AttackCombo3);
                    animator_.ResetTrigger(animID_InterruptedByLocomotion);
                }
                //animation events 發送指令為允許攻擊
                else
                {
                    //Animator Parameters 裡的攻擊動畫為可以被打斷的狀態
                    //animID_Attack_CanBeInterrupted 觀察用
                    animator_.SetBool(animID_Attack_CanBeInterrupted, true);
                    //如果現在的狀態機為AttackCombo1時可以切換到下一個攻擊動畫
                    if (animator_.GetCurrentAnimatorStateInfo(0).IsName("AttackCombo1"))
                    {
                        animator_.SetTrigger(animID_AttackCombo2);
                    }
                    //如果現在的狀態機為AttackCombo2時可以切換到下一個攻擊動畫
                    else if (animator_.GetCurrentAnimatorStateInfo(0).IsName("AttackCombo2"))
                    {
                        animator_.SetTrigger(animID_AttackCombo3);
                    }
                }
                break;
            case PlayerAnimState.BeAttack:
                break;
            case PlayerAnimState.Dead:
                break;
            default:
                break;
        }
    }
    #region - Player動畫事件管理 -
    void playertAnimationEventsToDo(PlayerAnimationEventsCommand command)
    {
        switch (command.AnimationEventName)
        {
            case "Player_Attack_Allow":
                player_Stats_.Player_AttackCommandAllow = true;
                
                break;
            case "Player_Attack_Prohibit":
                player_Stats_.Player_AttackCommandAllow = false;
                
                break;
            case "Player_Attack_End":
                //animator_.SetTrigger(animID_InterruptedByLocomotion);
                GameManager.Instance.MainGameEvent.Send(new PlayerMovementInterruptionFinishCommand());
                playerAnimState_ = PlayerAnimState.Locomotion;
                break;
            default:
                break;
        }
    }
    #endregion

    #region - Player擊發鬼魂 -
    void playerAimRecoil(PlayerLaunchGhostButtonCommand command)
    {
        player_Stats_.Aiming = false;
        animator_.SetBool(animID_AimRecoil, true);
    }
    #endregion

    void playerAttackButtonTrigger()
    {

    }
    #region - 待機動畫處理 -
    void timeoutToIdle()
    {
        bool inputDetected = player_Stats_.Player_Dir != Vector2.zero || player_Stats_.Aiming || playerAnimState_== PlayerAnimState.Attack;
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

    void setHorizontalAnimVel()//設置動畫水平速度給LocomtionBlendTree
    {
        player_horizontalAnimVel_ = Mathf.Lerp(player_horizontalAnimVel_, player_Stats_.Player_Speed, Time.deltaTime * player_Stats_.SpeedChangeRate);

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
    void setPlayer_animID_Grounded()
    {
        animator_.SetBool(animID_Grounded, player_Stats_.Grounded);
    }
    void setPlayer_animID_Aiming()
    {
        animator_.SetBool(animID_AimMove, aimMove_);

        if (player_Stats_.Aiming)
        {
            aimMove_ = player_Stats_.Player_Dir != Vector2.zero ? true : false;
            if (aimMove_)
            {
                player_horizontalAnimX = Mathf.Lerp(player_horizontalAnimX, player_Stats_.Player_Dir.x * player_Stats_.Player_Speed, Time.deltaTime * player_Stats_.SpeedChangeRate);
                player_horizontalAnimY = Mathf.Lerp(player_horizontalAnimY, player_Stats_.Player_Dir.y * player_Stats_.Player_Speed, Time.deltaTime * player_Stats_.SpeedChangeRate);

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
            animator_.SetBool(animID_AimRecoil, false);
            animator_.SetBool(animID_AimIdle, false);
        }  
    }
    #endregion
}
