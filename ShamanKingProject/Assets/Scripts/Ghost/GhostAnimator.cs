using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Gamemanager;
using BehaviorDesigner.Runtime;
using UnityEditor;


public enum GhostState
{
    GHOST_IDLE,
    GHOST_MOVEMENT
}
public class GhostAnimator : MonoBehaviour
{
    #region 提前Hash進行優化
    readonly int animID_Idle = Animator.StringToHash("Idle");

    readonly int animID_Aiming = Animator.StringToHash("Aiming");

    readonly int animID_ShootOut = Animator.StringToHash("ShootOut");

    readonly int animID_Possess = Animator.StringToHash("Possess");
    #endregion

    public GhostState ghostCurrentState;
    [SerializeField]
    private BehaviorTree behaviorTree;
    [SerializeField]
    private ExternalBehavior[] externalBehaviorTrees;

    


    void Start()
    {
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnAimingButtonTrigger, aimButtonTrigger);
        switchExternalBehavior((int)GhostState.GHOST_IDLE);
        ghostCurrentState = GhostState.GHOST_IDLE;
    }


    void Update()
    {
        
    }

    void aimButtonTrigger(PlayerAimingButtonCommand command)
    {
        if (command.AimingButtonIsPressed)
        {
            switchExternalBehavior((int)GhostState.GHOST_MOVEMENT);
            ghostCurrentState = GhostState.GHOST_MOVEMENT;
        }
        //else
        //{
        //    switchExternalBehavior((int)GhostState.GHOST_IDLE);
        //    ghostCurrentState = GhostState.GHOST_IDLE;
        //}
    }
    void ghost_MOVEMENT()
    {
        
    }
    void ghost_Aiming()
    {
        
    }

    //切換外部行為樹
    void switchExternalBehavior(int externalTrees)
    {
        if (externalBehaviorTrees[externalTrees] != null)
        {
            behaviorTree.DisableBehavior();
            behaviorTree.ExternalBehavior = externalBehaviorTrees[externalTrees];
            behaviorTree.EnableBehavior();
        }
    }
    void setGhost_animID_Aiming()
    {

    }


    #region - Animation Events -
    public void GhostAnimationEventTest()
    {

    }
    #endregion

}
