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
    readonly int animID_Guarding = Animator.StringToHash("Guarding");
    readonly int animID_TargetingGuardMove = Animator.StringToHash("TargetingGuardMove");
    readonly int animID_TargetMove = Animator.StringToHash("TargetMove");
    readonly int animID_AimMove = Animator.StringToHash("AimMove");
    readonly int animID_PossessMove = Animator.StringToHash("PossessMove");
    readonly int animID_AimIdle = Animator.StringToHash("AimIdle");
    readonly int animID_PossessIdle = Animator.StringToHash("PossessIdle");
    readonly int animID_AimRecoil = Animator.StringToHash("AimRecoil");

    readonly int animID_TargetMoveX = Animator.StringToHash("TargetMoveX");
    readonly int animID_TargetMoveY = Animator.StringToHash("TargetMoveY");
    readonly int animID_AimMoveX = Animator.StringToHash("AimMoveX");
    readonly int animID_AimMoveY = Animator.StringToHash("AimMoveY");

    readonly int animID_TimeOutToIdle = Animator.StringToHash("TimeOutToIdle");
    //readonly int animID_InterruptedByLocomotion = Animator.StringToHash("InterruptedByLocomotion");

    readonly int animID_AnimMoveSpeed = Animator.StringToHash("AnimMoveSpeed");
    readonly int animID_AnimVerticalSpeed = Animator.StringToHash("AnimVerticalSpeed");

    readonly int animID_InputDetected = Animator.StringToHash("InputDetected");
    readonly int animID_Grounded = Animator.StringToHash("Grounded");

    readonly int animID_Airborne = Animator.StringToHash("Airborne");//空中降落，下降是true，上升是false

    readonly int animID_Idle = Animator.StringToHash("Idle");

    readonly int animID_Attack_CanBeInterrupted = Animator.StringToHash("Attack_CanBeInterrupted");//變化時參考用，沒有實際上的作用
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
    private bool aimRecoil;
    //附身狀態下的移動狀態
    private bool possessMove_;
    //角色動畫水平速度
    private float player_horizontalAnimVel_;
    private float player_horizontalAnimX;
    private float player_horizontalAnimY;
    //角色動畫垂直速度
    private float player_verticalAnimVel_;


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
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnSystemGetTarget, cmd => { onTargetGetObject(); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnSystemResetTarget, cmd => { onTargetResetObject(); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerAnimationEvents, playertAnimationEventsToDo);
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerLaunchGhost, playerLaunchGhostButtonTrigger);
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnEnemyAttackSuccess, cmd => { Debug.LogWarning("PlayerGetHurt"); playerGetHurt(); });


        GameManager.Instance.MainGameEvent.OnPlayerLaunchActionFinish.Where(cmd => cmd.Hit && cmd.HitObjecctTag == HitObjecctTag.Possessable).Subscribe(cmd => playerPossessMoveAnimSet());


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
    void onTargetGetObject()
    {
        animator_.SetBool(animID_TargetMove, true);
    }
    void onTargetResetObject()
    {
        animator_.SetBool(animID_TargetMove, false);
    }
    void playerGetHurt()
    {
        if (player_Stats_.Guarding)
        {
            animator_.CrossFadeInFixedTime("PlayerGuardingHurt", 0);
        }
        else { animator_.CrossFadeInFixedTime("PlayerHurt", 0); }
    }
    public void Update()
    {
        //設置動畫觸地狀態
        setPlayer_animID_Grounded();

        //設置動畫水平速度
        setHorizontalAnimVel();
        //設置動畫垂直速度
        setVerticalAnimVel();

        //設置玩家瞄準狀態
        //setPlayer_animID_Aiming();
        //設置玩家鎖敵狀態
        set_animID_TargetingMove();
        //設置玩家蓄力狀態
        setPlayer_animID_Charging();

        //回到idle的計時器
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
                    //animID_Attack_CanBeInterrupted 觀察用
                    animator_.SetBool(animID_Attack_CanBeInterrupted, false);
                }
                //animation events 發送指令為允許攻擊
                else
                {
                    animator_.SetBool(animID_Attack_CanBeInterrupted, true);
                    //Debug.Log(animator_.GetCurrentAnimatorStateInfo(0).length * animator_.GetCurrentAnimatorStateInfo(0).normalizedTime);
                    //Animator Parameters 裡的攻擊動畫為可以被打斷的狀態
                } 
                break;
            default:
                break;
        }
        #endregion
       
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
            case "Player_AimRecoil_End":
                aimRecoil = false;
                break;
            default:
                break;
        }
    }
    #endregion

    #region - Player擊發鬼魂時的動畫處理 -
    //擊發時播放放矢動畫
    void playerLaunchGhostButtonTrigger(PlayerLaunchGhostButtonCommand command)
    { 
        aimRecoil = true;
    }
    void playerPossessMoveAnimSet()
    {
        possessMove_ = true;
    }

    #endregion
    #region - 待機動畫處理 -
    void timeoutToIdle()
    {
        bool inputDetected = player_Stats_.Player_Dir != Vector2.zero || player_Stats_.Aiming || player_Stats_.Guarding || playerAnimState_== PlayerAnimState.Attack || !player_Stats_.Grounded;
        //如果沒有偵測到任何輸入產生的行為
        if (!inputDetected)
        {
            //開始計時
            idleTimer_ += Time.deltaTime;

            //計時超過五秒後
            if (idleTimer_ >= idleTimeOut_)
            {
                //計時器為-1
                idleTimer_ = -1f;
                animator_.SetTrigger(animID_TimeOutToIdle);
                //GameManager.Instance.MainGameEvent.Send(new PlayerMoveStatusChangeCommand() { IsMoving = false });
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
    void setVerticalAnimVel()
    {
        //Debug.Log("player_Stats_.verticalVelocity_" + player_Stats_.verticalVelocity_);

        //如果在離開地板且verticalVelocity_大於0的時set player_verticalAnimVel_參數
        if (!player_Stats_.Grounded && player_Stats_.verticalVelocity_ > 0)
        {
            player_verticalAnimVel_ = player_Stats_.verticalVelocity_;
        }

        //如果角色屬於掉落狀態時set player_verticalAnimVel_參數
        else if (player_Stats_.Falling)
        {
            player_verticalAnimVel_ = player_Stats_.verticalVelocity_;
        }
       
        //animator_.SetFloat(animID_AnimVerticalSpeed, player_verticalAnimVel_);
        if (!player_Stats_.Grounded)
        {
            animator_.SetFloat(animID_AnimVerticalSpeed, player_verticalAnimVel_);
        }
    }
    void setPlayer_animID_Grounded()
    {
        animator_.SetBool(animID_Grounded, player_Stats_.Grounded);
    }

    //設置瞄準下的運動參數
    void setPlayer_AimingMoveXY()
    {
        
        player_horizontalAnimX = Mathf.Lerp(player_horizontalAnimX, player_Stats_.Player_Dir.x * player_Stats_.Player_Speed, Time.deltaTime * player_Stats_.SpeedChangeRate);
        player_horizontalAnimY = Mathf.Lerp(player_horizontalAnimY, player_Stats_.Player_Dir.y * player_Stats_.Player_Speed, Time.deltaTime * player_Stats_.SpeedChangeRate);

        animator_.SetFloat(animID_AimMoveX, player_horizontalAnimX);
        animator_.SetFloat(animID_AimMoveY, player_horizontalAnimY);
    }
   
    //void setPlayer_animID_Aiming()
    //{
    //    if (player_Stats_.Aiming)
    //    {
    //        aimMove_ = player_Stats_.Player_Dir != Vector2.zero ? true : false;
    //        setPlayer_AimingMoveXY();
    //        //取消鎖敵動畫
    //        animator_.SetBool(animID_TargetMove, false);

    //        //接收到可附身移動且如果移動向量不為零則
    //        if (possessMove_ && player_Stats_.Player_Dir != Vector2.zero)
    //        {
    //            animator_.SetBool(animID_PossessMove, true);
    //            animator_.SetBool(animID_PossessIdle, false);
    //            //附身狀態不會播放擊發動畫
    //            aimRecoil = false;
    //            animator_.SetBool(animID_AimRecoil, false);
    //        }
    //        //接收到可附身移動且如果移動向量為零則
    //        else if (possessMove_ && player_Stats_.Player_Dir == Vector2.zero)
    //        {
    //            animator_.SetBool(animID_PossessIdle, true);
    //            animator_.SetBool(animID_PossessMove, false);
    //            //附身狀態不會播放擊發動畫
    //            aimRecoil = false;
    //            animator_.SetBool(animID_AimRecoil, false);
    //        }
    //        //不在附身移動狀態
    //        else
    //        {
    //            //如果可以播放擊發動畫
    //            if (aimRecoil)
    //            {
    //                //播放擊發動畫
    //                animator_.SetBool(animID_AimRecoil, true);
    //                //關閉瞄準下的Idle & Move動畫
    //                animator_.SetBool(animID_AimIdle, false);
    //                animator_.SetBool(animID_AimMove, false);
    //            }
    //            //擊發動畫為false
    //            else
    //            {
    //                //傳出至NoneAnim
    //                animator_.SetBool(animID_AimRecoil, false);

    //                //瞄準移動下且為非擊發鬼魂時
    //                if (aimMove_)
    //                {
    //                    animator_.SetBool(animID_AimMove, true);
    //                    animator_.SetBool(animID_AimIdle, false);

    //                }
    //                //瞄準待機時
    //                else
    //                {
    //                    animator_.SetBool(animID_AimMove, false);
    //                    animator_.SetBool(animID_AimIdle, true);
    //                }
    //            }
                
    //        }
            
    //    }
    //    else
    //    {
    //        resetPlayer_AimingAllActions();
    //    }  
    //}
    void set_animID_TargetingMove()
    {
        if (player_Stats_.Targeting) 
        {
            player_horizontalAnimX = Mathf.Lerp(player_horizontalAnimX, player_Stats_.Player_Dir.x * player_Stats_.Player_Speed, Time.deltaTime * player_Stats_.SpeedChangeRate);
            player_horizontalAnimY = Mathf.Lerp(player_horizontalAnimY, player_Stats_.Player_Dir.y * player_Stats_.Player_Speed, Time.deltaTime * player_Stats_.SpeedChangeRate);

            animator_.SetFloat(animID_TargetMoveX, player_horizontalAnimX);
            animator_.SetFloat(animID_TargetMoveY, player_horizontalAnimY);
        }
    }
    void setPlayer_animID_Charging()
    {
        if (player_Stats_.Guarding && player_Stats_.Targeting)
        {
            animator_.SetBool(animID_TargetingGuardMove, true);
        }
        else
        {
            animator_.SetBool(animID_TargetingGuardMove, false);
        }
        if (player_Stats_.Guarding)
        {
            animator_.SetBool(animID_Guarding, true);
        }
        else
        {
            animator_.SetBool(animID_Guarding, false);
        }
    }
    //重置所有瞄準的bool動畫
    void resetPlayer_AimingAllActions()
    {
        possessMove_ = false;
        aimMove_ = false;
        aimRecoil = false;

        animator_.SetBool(animID_AimIdle, false);
        animator_.SetBool(animID_AimMove, false);

        animator_.SetBool(animID_PossessIdle, false);
        animator_.SetBool(animID_PossessMove, false);

        animator_.SetBool(animID_AimRecoil, false);
    }
    #endregion
}
