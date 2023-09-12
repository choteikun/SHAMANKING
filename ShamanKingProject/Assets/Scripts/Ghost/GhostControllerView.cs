using Gamemanager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;

public enum GhostState
{
    GHOST_IDLE,
    GHOST_MOVEMENT,
    GHOST_POSSESSED,
}
public class GhostControllerView : MonoBehaviour
{
    [SerializeField]
    Ghost_Stats ghost_Stats_ = new Ghost_Stats();

    [SerializeField]
    GhostAnimator ghostAnimator_;

    [SerializeField]
    GhostController ghostController_;

    [SerializeField]
    private BehaviorTree behaviorTree;
    [SerializeField]
    private ExternalBehavior[] externalBehaviorTrees;

    

    void Awake()
    {
        ghost_Stats_.rb = GetComponent<Rigidbody>();
        ghostAnimator_ = new GhostAnimator(this.gameObject);
        ghostController_ = new GhostController(this.gameObject);
        ghostController_.Awake();
    }
    void Start()
    {
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnAimingButtonTrigger, ghostReadyButtonTrigger);

        behaviorTree = GetComponent<BehaviorTree>();

        switchExternalBehavior((int)GhostState.GHOST_IDLE);
        ghost_Stats_.ghostCurrentState = GhostState.GHOST_IDLE;

        ghostAnimator_.Start(ghost_Stats_);
        ghostController_.Start(ghost_Stats_);
    }

    void ghostReadyButtonTrigger(PlayerAimingButtonCommand command)
    {
        if (command.AimingButtonIsPressed && ghost_Stats_.ghostCurrentState == GhostState.GHOST_IDLE)
        {
            //behaviorTree.SendEvent("MOVEMENT SENDEVENT TEST");
            //behaviorTree.SendEvent<object>("MOVEMENT SENDEVENT TEST", (object)transform.position);
            ghost_Stats_.Ghost_ReadyButton = true;
            switchExternalBehavior((int)GhostState.GHOST_MOVEMENT - 1);
            ghost_Stats_.ghostCurrentState = GhostState.GHOST_MOVEMENT; 
        }
        if (!command.AimingButtonIsPressed)
        {
            ghost_Stats_.Ghost_ReadyButton = false;
        }
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Possessable"))
        {
            GameManager.Instance.MainGameEvent.Send(new PlayerLaunchFinishCommand() { Hit = true });
            ghost_Stats_.ghostCurrentState = GhostState.GHOST_POSSESSED;
        }
        else
        {
            GameManager.Instance.MainGameEvent.Send(new PlayerLaunchFinishCommand() { Hit = true });
            ghost_Stats_.ghostCurrentState = GhostState.GHOST_IDLE;
        }

    }

    void Update()
    {

        ghostAnimator_.Update();
        ghostController_.Update();

        ghost_Stats_.Ghost_Timer += Time.deltaTime;
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
}


[Serializable]
public class Ghost_Stats
{
    public Rigidbody rb;

    public GhostState ghostCurrentState;

    public float Player_Distance;
    public float Ghost_Timer;

    public bool Ghost_ReadyButton;

}
