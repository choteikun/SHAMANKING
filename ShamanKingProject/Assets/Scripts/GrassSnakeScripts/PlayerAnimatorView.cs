using Gamemanager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UniRx;

public enum PlayerAnimState
{
    Idle,
}

[RequireComponent(typeof(Animator))]
public class PlayerAnimatorView : MonoBehaviour
{
    #region 提前Hash進行優化
    //readonly int h_HurtFromX = Animator.StringToHash("HurtFromX");
    //readonly int h_HurtFromY = Animator.StringToHash("HurtFromY");

    readonly int animID_TimeOutToIdle = Animator.StringToHash("TimeOutToIdle");

    readonly int animID_AnimMoveSpeed = Animator.StringToHash("AnimMoveSpeed");

    readonly int animID_InputDetected = Animator.StringToHash("InputDetected");
    readonly int animID_Grounded = Animator.StringToHash("Grounded");
    readonly int animID_Airborne = Animator.StringToHash("Airborne");//空中降落，下降是true，上升是false

    readonly int animID_Idle = Animator.StringToHash("Idle");
    //readonly int h_Jump = Animator.StringToHash("Jump");
    #endregion

    //當前狀態
    private PlayerAnimState playerAnimState;

    private TestPlayerController playerController;
    private Animator animator;
    private InputValue inputValue;

    //檢測按鈕
    private bool inputDetected;
    //角色動畫水平速度
    private float player_horizontalAnimVel;
    //角色動畫垂直速度
    private float player_verticalVel;


    [SerializeField, Tooltip("過渡到隨機Idle動畫所需要花的時間")]
    private float idleTimeOut;
    //Idle動畫計時器(跳轉至隨機動畫)
    private float idleTimer;

    //Move狀態中回到idle的計時器(避免方向鍵切換時角色回到idle狀態)
    private float _moveToIdleTimer;

    //接收玩家移動指令所計算用的動畫速度
    private float player_targetAnimSpeed;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponentInParent<TestPlayerController>();
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerControllerMovement, GetTargetAnimSpeed);
    }

    void Update()
    {

        SetHorizontalAnimVel();
        SetPlayer_animID_Grounded();

        //TimeoutToIdle();

        #region - 簡易動畫狀態管理 
        switch (playerAnimState)
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
        if (playerAnimState == PlayerAnimState.Idle)
        {
            idleTimer += Time.deltaTime;

            if (idleTimer >= idleTimeOut)
            {
                idleTimer = 0f;
                //animator.SetTrigger(h_TimeOutToIdle);
            }
        }
        else
        {
            idleTimer = 0f;
            //animator.ResetTrigger(h_TimeOutToIdle);
        }
        //animator.SetBool(h_InputDetected, inputDetected);
    }
    #endregion

    #region - 動畫參數計算 -

    void GetTargetAnimSpeed(PlayerControllerMovementCommand playerControllerMovementCommand)
    {
        float targetSpeed = 1.0f;
        var clampedDirection = Mathf.Clamp(playerControllerMovementCommand.Direction.magnitude, 0, 1);
        player_targetAnimSpeed = clampedDirection * targetSpeed;
    }
    void SetHorizontalAnimVel()//設置動畫水平速度給LocomtionBlendTree
    {
        player_horizontalAnimVel = Mathf.Lerp(player_horizontalAnimVel, player_targetAnimSpeed, Time.deltaTime * 10.0f);
        if (player_horizontalAnimVel < 0.01f) player_horizontalAnimVel = 0f;

        animator.SetFloat(animID_AnimMoveSpeed, player_horizontalAnimVel);
    }
    void SetPlayer_animID_Grounded()
    {
        animator.SetBool(animID_Grounded, playerController.Grounded);
    }
    #endregion

    #region - Animation Events -

    #endregion
}
