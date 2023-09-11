using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UniRx.Triggers;
using Gamemanager;
using BehaviorDesigner.Runtime;



public class GhostAnimator
{
    #region 提前Hash進行優化
    readonly int animID_Idle = Animator.StringToHash("Idle");

    readonly int animID_BeingCaught = Animator.StringToHash("BeingCaught");

    readonly int animID_ShootOut = Animator.StringToHash("ShootOut");

    readonly int animID_Possess = Animator.StringToHash("Possess");
    #endregion



    private Animator animator_;
    private ObservableStateMachineTrigger animOSM_Trigger_;

    bool beingCaught;
    bool shootOut;
    bool possess;

    private Ghost_Stats ghost_Stats_;
    private GameObject ghostControllerObj_;




    public GhostAnimator(GameObject controller)
    {
        ghostControllerObj_ = controller;
    }

    public void Start(Ghost_Stats ghost_Stats)
    {
        ghost_Stats_ = ghost_Stats;
        animator_ = ghostControllerObj_.gameObject.transform.GetChild(0).GetComponent<Animator>();
        animOSM_Trigger_ = animator_.GetBehaviour<ObservableStateMachineTrigger>();

        ResetAllAnimations();
    }

    void ResetAllAnimations()
    {
        beingCaught = false; shootOut = false; possess = false;
    }
    public void Update()
    {
        switch (ghost_Stats_.ghostCurrentState)
        {
            case GhostState.GHOST_IDLE:
                animator_.SetBool(animID_Idle, true);
                shootOut = false;
                animator_.SetBool(animID_ShootOut, shootOut);
                break;
            case GhostState.GHOST_MOVEMENT:
                animator_.SetBool(animID_Idle, false);

                if (!shootOut)//不是在被擊發的狀態下
                {
                    if (Input.GetMouseButtonDown(1) && !beingCaught)//如果是沒有被抓住的狀態下
                    {
                        beingCaught = true;
                        animator_.SetBool(animID_BeingCaught, beingCaught);
                    }
                    else if (Input.GetMouseButtonUp(1))//取消抓住
                    {
                        beingCaught = false;
                        animator_.SetBool(animID_BeingCaught, beingCaught);
                        ghost_Stats_.ghostCurrentState = GhostState.GHOST_IDLE;
                    }
                    if (Input.GetMouseButtonDown(0) && beingCaught)//擊發出去
                    {
                        shootOut = true;
                        animator_.SetBool(animID_ShootOut, shootOut);
                    }   
                }
                else
                {
                    beingCaught = false;
                    animator_.SetBool(animID_BeingCaught, beingCaught);
                }
                break;
            case GhostState.GHOST_POSSESSED:
                animator_.SetBool(animID_Idle, false);
                shootOut = false;
                animator_.SetBool(animID_ShootOut, shootOut);
                
                break;

            default:
                break;
        }
    }


    #region - Animation Events -
    public void GhostAnimationEventTest()
    {

    }
    #endregion

}
