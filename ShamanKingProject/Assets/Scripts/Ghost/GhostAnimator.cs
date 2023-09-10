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

    bool moveOver;

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
    }


    public void Update()
    {
        switch (ghost_Stats_.ghostCurrentState)
        {
            case GhostState.GHOST_IDLE:

                break;
            case GhostState.GHOST_MOVEMENT:

                break;
            case GhostState.GHOST_POSSESSED:

                break;

            default:
                break;
        }
        ////如果大叔被抓住且為行動模式時
        //if(beingCaught)
        //{
        //    animator_.SetBool(animID_Idle, false);
        //    animator_.SetBool(animID_BeingCaught, true);
        //}
    }


    #region - Animation Events -
    public void GhostAnimationEventTest()
    {

    }
    #endregion

}
