using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Gamemanager;
using BehaviorDesigner.Runtime;



public class GhostAnimator
{
    #region 提前Hash進行優化
    readonly int animID_Idle = Animator.StringToHash("Idle");

    readonly int animID_BeingCaught = Animator.StringToHash("BeingCaught");

    readonly int animID_ShootOut = Animator.StringToHash("ShootOut");

    readonly int animID_GhostBite = Animator.StringToHash("GhostBite");

    readonly int animID_Possess = Animator.StringToHash("Possess");

    readonly int animID_TimeOutToIdle = Animator.StringToHash("TimeOutToIdle");
    #endregion


    [Tooltip("過渡到隨機Idle動畫所需要花的時間/秒")]
    public float IdleTimeOut_ = 5;

    private Animator animator_;

    private ObservableStateMachineTrigger animOSM_Trigger_;

    private Ghost_Stats ghost_Stats_;

    private GameObject ghostControllerObj_;

    //Idle動畫計時器(跳轉至隨機動畫)
    private float idleTimer_;

    private bool ghost_beingCaught_;

    private bool ghost_possess_;

   




    public GhostAnimator(GameObject controller)
    {
        ghostControllerObj_ = controller;
    }

    public void Start(Ghost_Stats ghost_Stats)
    {
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerLaunchGhost, ghostShootButtonTrigger);

        ghost_Stats_ = ghost_Stats;
        animator_ = ghostControllerObj_.gameObject.transform.GetChild(0).GetComponent<Animator>();
        animOSM_Trigger_ = animator_.GetBehaviour<ObservableStateMachineTrigger>();

        ResetAllAnimations();
    }
    void ghostShootButtonTrigger(PlayerLaunchGhostButtonCommand command)
    {
        //如果在鬼魂行為模式中且不是擊發的狀態下
        if(ghost_Stats_.ghostCurrentState == GhostState.GHOST_MOVEMENT && !ghost_Stats_.Ghost_ShootOut_)
        {
            //如果是鬼魂被抓住的狀態下
            if (ghost_beingCaught_)
            {
                //擊發
                ghost_Stats_.Ghost_ShootOut_ = true;
                //播放擊發動畫
                animator_.SetBool(animID_ShootOut, ghost_Stats_.Ghost_ShootOut_);
            }
        }
    }
    
    void ResetAllAnimations()
    {
        ghost_beingCaught_ = false; ghost_Stats_.Ghost_ShootOut_ = false; ghost_possess_ = false;
    }
    public void Update()
    {
        switch (ghost_Stats_.ghostCurrentState)
        {
            case GhostState.GHOST_IDLE:
                //進入待機模式播放Idle
                animator_.SetBool(animID_Idle, true);
                //結束擊發狀態
                ghost_Stats_.Ghost_ShootOut_ = false;
                animator_.SetBool(animID_ShootOut, ghost_Stats_.Ghost_ShootOut_);
                break;
            case GhostState.GHOST_MOVEMENT:
                animator_.SetBool(animID_Idle, false);
                //不是在被擊發的狀態下
                if (!ghost_Stats_.Ghost_ShootOut_)
                {
                    //如果是沒有被抓住的狀態下按下瞄準
                    if (ghost_Stats_.Ghost_ReadyButton && !ghost_beingCaught_)
                    {
                        //抓住鬼魂
                        ghost_beingCaught_ = true;
                        animator_.SetBool(animID_BeingCaught, ghost_beingCaught_);
                    }
                    //鬆開瞄準
                    else if (!ghost_Stats_.Ghost_ReadyButton)
                    {
                        //放開鬼魂
                        ghost_beingCaught_ = false;
                        animator_.SetBool(animID_BeingCaught, ghost_beingCaught_);
                        //回到Idle模式
                        ghost_Stats_.ghostCurrentState = GhostState.GHOST_IDLE;
                    }  
                }
                //擊發的狀態下
                else
                {
                    //鬼魂是不能被抓住的
                    ghost_beingCaught_ = false;
                    animator_.SetBool(animID_BeingCaught, ghost_beingCaught_);
                }
                break;
            case GhostState.GHOST_REACT:
                //附身模式不會播放待機
                animator_.SetBool(animID_Idle, false);
                //結束擊發狀態
                ghost_Stats_.Ghost_ShootOut_ = false;
                animator_.SetBool(animID_ShootOut, ghost_Stats_.Ghost_ShootOut_);

                if (ghost_Stats_.Ghost_Possessable)
                {
                    //dissolve，拿取目標資料，transform接在目標上
                    ghost_Stats_.ghostCurrentState = GhostState.GHOST_IDLE;
                    Debug.Log("Possessed success!!");
                }
                if (ghost_Stats_.Ghost_Biteable)
                {
                    animator_.SetBool(animID_GhostBite, true);
                }
                //else if(!ghost_Stats_.Ghost_Biteable && animator_.GetCurrentAnimatorStateInfo(0).IsName("Ghost_Bite") && !animator_.IsInTransition(0))
                //{
                //    ghost_Stats_.ghostCurrentState = GhostState.GHOST_IDLE;
                //    Debug.Log("Bited success!!");
                //}
                else
                {
                    ghost_Stats_.ghostCurrentState = GhostState.GHOST_IDLE;
                    Debug.Log("Bited success!!");
                }





                break;

            default:
                break;
        }
        timeoutToIdle();
    }
    void timeoutToIdle()
    {
        if (ghost_Stats_.ghostCurrentState == GhostState.GHOST_IDLE)
        {
            idleTimer_ += Time.deltaTime;

            if (idleTimer_ >= IdleTimeOut_)
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
    }
}
