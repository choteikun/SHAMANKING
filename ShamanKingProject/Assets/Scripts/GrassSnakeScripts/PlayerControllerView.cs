using Gamemanager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PlayerControllerView : MonoBehaviour
{
    [SerializeField]
    Player_Stats player_Stats_ = new Player_Stats();

    [SerializeField]
    PlayerAnimator playerAnimatorView_;

    [SerializeField]
    PlayerControllerMover playerControllerMover_;

    void Awake()
    {
        playerAnimatorView_ = new PlayerAnimator(this.gameObject);
        playerControllerMover_ =  new PlayerControllerMover(this.gameObject);
        playerControllerMover_.Awake();
    }
    void Start()
    {
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerControllerMovement, getPlayerDirection);
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnAimingButtonTrigger, getPlayerAimingState);
        //GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerLaunchGhost, launchCancelMoving);

        GameManager.Instance.MainGameEvent.OnPlayerAnimationEvents.Where(cmd => cmd.AnimationEventName == "Player_Attack_Allow").Subscribe(playertAnimationEventsToDo).AddTo(this);
        GameManager.Instance.MainGameEvent.OnPlayerAnimationEvents.Where(cmd => cmd.AnimationEventName == "Player_Attack_Prohibit").Subscribe(playertAnimationEventsToDo).AddTo(this);
        //GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerControllerMovement, getPlayer_SprintState);
        playerAnimatorView_.Start(player_Stats_);
        playerControllerMover_.Start(player_Stats_);
    }
    
    #region - Player取得方向指令 -
    void getPlayerDirection(PlayerControllerMovementCommand playerControllerMovementCommand)
    {
        player_Stats_.Player_Dir = playerControllerMovementCommand.Direction;
        var clampedDirection = Mathf.Clamp(player_Stats_.Player_Dir.magnitude, 0, 1);
        player_Stats_.Player_InputMagnitude = clampedDirection;
    }
    #endregion

    #region - Player取得瞄準指令 -
    void getPlayerAimingState(PlayerAimingButtonCommand playerAimingButtonCommand)
    {
        player_Stats_.Aiming = playerAimingButtonCommand.AimingButtonIsPressed;
        if (playerAimingButtonCommand.AimingButtonIsPressed)
        {
            playerControllerMover_.TransitionState("Aim");
        }
        else
        {
            playerControllerMover_.TransitionState("MainGame");
        }
    }
    #endregion

    void launchCancelMoving(PlayerLaunchGhostButtonCommand cmd)
    {
        getPlayerDirection(new PlayerControllerMovementCommand {Direction = Vector3.zero });
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

            default:
                break;
        }
    }
    #endregion

    void Update()
    {
        playerAnimatorView_.Update();
        playerControllerMover_.Update();
    }
}


[Serializable]
public class Player_Stats
{
    [Tooltip("玩家移動速度")]
    public float Player_Speed;

    [Tooltip("地板檢查，這不是CharacterController自帶的isGrounded，那東西是大便")]
    public bool Grounded = true;

    [Tooltip("玩家瞄準狀態")]
    public bool Aiming = false;

    [Tooltip("運動速度達到最大值之前的速度數值，數值越大達到最大值的時間越快")]
    public float SpeedChangeRate = 10.0f;

    [Tooltip("角色輸入")]
    public float Player_InputMagnitude;

    public Vector2 Player_Dir;

    public bool Player_AttackCommandAllow;
}
