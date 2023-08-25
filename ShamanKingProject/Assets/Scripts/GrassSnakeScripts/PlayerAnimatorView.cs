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
    #region ���eHash�i���u��
    //readonly int h_HurtFromX = Animator.StringToHash("HurtFromX");
    //readonly int h_HurtFromY = Animator.StringToHash("HurtFromY");

    readonly int animID_TimeOutToIdle = Animator.StringToHash("TimeOutToIdle");

    readonly int animID_AnimMoveSpeed = Animator.StringToHash("AnimMoveSpeed");

    readonly int animID_InputDetected = Animator.StringToHash("InputDetected");
    readonly int animID_Grounded = Animator.StringToHash("Grounded");
    readonly int animID_Airborne = Animator.StringToHash("Airborne");//�Ť������A�U���Otrue�A�W�ɬOfalse

    readonly int animID_Idle = Animator.StringToHash("Idle");
    //readonly int h_Jump = Animator.StringToHash("Jump");
    #endregion

    //��e���A
    private PlayerAnimState playerAnimState_;

    private TestPlayerController playerController_;
    private Animator animator_;
    private InputValue inputValue_;

    //�˴����s
    private bool inputDetected_;
    //����ʵe�����t��
    private float player_horizontalAnimVel_;
    //����ʵe�����t��
    private float player_verticalVel_;


    [SerializeField, Tooltip("�L����H��Idle�ʵe�һݭn�᪺�ɶ�")]
    private float idleTimeOut_;
    //Idle�ʵe�p�ɾ�(������H���ʵe)
    private float idleTimer_;

    //Move���A���^��idle���p�ɾ�(�קK��V������ɨ���^��idle���A)
    private float moveToIdleTimer_;

    //�������a���ʫ��O�ҭp��Ϊ��ʵe�t��
    private float player_targetAnimSpeed_;

    void Start()
    {
        animator_ = GetComponent<Animator>();
        playerController_ = GetComponentInParent<TestPlayerController>();
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerControllerMovement, GetTargetAnimSpeed);
    }

    void Update()
    {

        SetHorizontalAnimVel();
        SetPlayer_animID_Grounded();

        //TimeoutToIdle();

        #region - ²���ʵe���A�޲z 
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

    #region - �ݾ��ʵe�B�z -
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

    #region - �ʵe�Ѽƭp�� -

    void GetTargetAnimSpeed(PlayerControllerMovementCommand playerControllerMovementCommand)
    {
        float targetSpeed = 1.0f;
        var clampedDirection = Mathf.Clamp(playerControllerMovementCommand.Direction.magnitude, 0, 1);
        player_targetAnimSpeed_ = clampedDirection * targetSpeed;
    }
    void SetHorizontalAnimVel()//�]�m�ʵe�����t�׵�LocomtionBlendTree
    {
        player_horizontalAnimVel_ = Mathf.Lerp(player_horizontalAnimVel_, player_targetAnimSpeed_, Time.deltaTime * 10.0f);
        if (player_horizontalAnimVel_ < 0.01f) player_horizontalAnimVel_ = 0f;

        animator_.SetFloat(animID_AnimMoveSpeed, player_horizontalAnimVel_);
    }
    void SetPlayer_animID_Grounded()
    {
        animator_.SetBool(animID_Grounded, playerController_.Grounded);
    }
    #endregion

    #region - Animation Events -

    #endregion
}
