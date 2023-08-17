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

    readonly int h_TimeOutToIdle = Animator.StringToHash("TimeOutToIdle");

    readonly int h_AnimMoveSpeed = Animator.StringToHash("AnimMoveSpeed");

    readonly int h_InputDetected = Animator.StringToHash("InputDetected");
    readonly int h_Grounded = Animator.StringToHash("Grounded");
    readonly int h_Airborne = Animator.StringToHash("Airborne");//�Ť������A�U���Otrue�A�W�ɬOfalse

    readonly int h_Idle = Animator.StringToHash("Idle");
    //readonly int h_Jump = Animator.StringToHash("Jump");
    #endregion

    //��e���A
    private PlayerAnimState playerAnimState;

    private Animator animator;
    private InputValue inputValue;

    //�˴����s
    private bool inputDetected;
    //��������t��
    private float player_horizontalVel;
    //���⫫���t��
    private float player_verticalVel;


    [SerializeField, Tooltip("�L����H��Idle�ʵe�һݭn�᪺�ɶ�")]
    private float idleTimeOut;
    //Idle�ʵe�p�ɾ�(������H���ʵe)
    private float idleTimer;

    //Move���A���^��idle���p�ɾ�(�קK��V������ɨ���^��idle���A)
    private float _moveToIdleTimer;


    void Start()
    {
        animator = GetComponent<Animator>();
        GameManager.Instance.MainGameEvent.OnPlayerControllerMovement.Subscribe(getAnimMoveSpeed);
    }

    void Update()
    {
        


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

    void getAnimMoveSpeed(PlayerControllerMovementCommand playerControllerMovementCommand)
    {
        player_horizontalVel = playerControllerMovementCommand.Direction.magnitude;
        //if (playerControllerMovementCommand.Direction != Vector2.zero)
        //{
        //    player_horizontalVel = inputValue.Get<Vector2>().magnitude;
        //}

        animator.SetFloat(h_AnimMoveSpeed, player_horizontalVel);
    }

    #region - Animation Events -

    #endregion
}
