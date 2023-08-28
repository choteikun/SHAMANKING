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

    private void Awake()
    {
        playerAnimatorView_ = new PlayerAnimator(this.gameObject);
        playerControllerMover_ =  new PlayerControllerMover(this.gameObject);
    }
    private void Start()
    {
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerControllerMovement, getPlayer_Direction);
        //GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerControllerMovement, getPlayer_SprintStatus);
        playerAnimatorView_.Start(player_Stats_);
        playerControllerMover_.Start(player_Stats_);
    }
    void getPlayer_Direction(PlayerControllerMovementCommand playerControllerMovementCommand)
    {
        player_Stats_.Player_Dir = playerControllerMovementCommand.Direction;
        var clampedDirection = Mathf.Clamp(player_Stats_.Player_Dir.magnitude, 0, 1);
        player_Stats_.Player_InputMagnitude = clampedDirection;
    }

    private void Update()
    {
        playerAnimatorView_.Update();
        playerControllerMover_.Update();
    }
}


[Serializable]
public class Player_Stats
{
    [Tooltip("角色速度")]
    public float Player_Speed;

    [Tooltip("地板檢查，這不是CharacterController自帶的isGrounded，那東西是大便")]
    public bool Grounded = true;

    [Tooltip("運動速度達到最大值之前的速度數值，數值越大達到最大值的時間越快")]
    public float SpeedChangeRate = 10.0f;

    [Tooltip("角色輸入")]
    public float Player_InputMagnitude;

    public Vector2 Player_Dir;
}
