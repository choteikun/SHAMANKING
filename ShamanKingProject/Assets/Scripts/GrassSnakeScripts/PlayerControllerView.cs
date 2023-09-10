using Gamemanager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        //GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerControllerMovement, getPlayer_SprintState);
        playerAnimatorView_.Start(player_Stats_);
        playerControllerMover_.Start(player_Stats_);
    }
    void Update()
    {
        playerAnimatorView_.Update();
        playerControllerMover_.Update();
    }

    void getPlayerDirection(PlayerControllerMovementCommand playerControllerMovementCommand)
    {
        player_Stats_.Player_Dir = playerControllerMovementCommand.Direction;
        var clampedDirection = Mathf.Clamp(player_Stats_.Player_Dir.magnitude, 0, 1);
        player_Stats_.Player_InputMagnitude = clampedDirection;
    }
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
}
