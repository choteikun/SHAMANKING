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
    private PlayerAnimState playerAnimState;

    private TestPlayerController playerController;
    private Animator animator;
    private InputValue inputValue;

    //�˴����s
    private bool inputDetected;
    //����ʵe�����t��
    private float player_horizontalAnimVel;
    //����ʵe�����t��
    private float player_verticalVel;


    [SerializeField, Tooltip("�L����H��Idle�ʵe�һݭn�᪺�ɶ�")]
    private float idleTimeOut;
    //Idle�ʵe�p�ɾ�(������H���ʵe)
    private float idleTimer;

    //Move���A���^��idle���p�ɾ�(�קK��V������ɨ���^��idle���A)
    private float _moveToIdleTimer;

    //�������a���ʫ��O�ҭp��Ϊ��ʵe�t��
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

        #region - ²���ʵe���A�޲z 
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

    #region - �ݾ��ʵe�B�z -
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

    #region - �ʵe�Ѽƭp�� -

    void GetTargetAnimSpeed(PlayerControllerMovementCommand playerControllerMovementCommand)
    {
        float targetSpeed = 1.0f;
        var clampedDirection = Mathf.Clamp(playerControllerMovementCommand.Direction.magnitude, 0, 1);
        player_targetAnimSpeed = clampedDirection * targetSpeed;
    }
    void SetHorizontalAnimVel()//�]�m�ʵe�����t�׵�LocomtionBlendTree
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
