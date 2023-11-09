using Gamemanager;
using System;
using UniRx;
using UnityEngine;

public class PlayerControllerView : MonoBehaviour
{

    [SerializeField]
    Player_Stats player_Stats_ = new Player_Stats();

    [SerializeField]
    PlayerAnimator playerAnimatorView_;

    [SerializeField]
    PlayerControllerMover playerControllerMover_;

    [SerializeField]
    PlayerAttackModel playerAttackModel_;

    void Awake()
    {
        playerAnimatorView_ = new PlayerAnimator(this.gameObject);
        playerControllerMover_ = new PlayerControllerMover(this.gameObject);
        playerAttackModel_ = new PlayerAttackModel(this.gameObject);
        playerControllerMover_.Awake();
    }
    void Start()
    {
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerControllerMovement, getPlayerDirection);
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnAimingButtonTrigger, onAimButtonTrigger);
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnSystemGetTarget, cmd => { onTargetGetObject(); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnSystemResetTarget, cmd => { onTargetResetObject(); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerLightAttack, cmd => { cancelMoving(); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerThrowAttack, cmd => { cancelMoving(); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerAnimationEvents, cmd =>
        {
            if (cmd.AnimationEventName == "PlayerJumpAttackStart")
            {
                cancelMoving();
            }
        });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerMovementInterruptionFinish, cmd => { player_Stats_.Player_CanMove = true; });

        GameManager.Instance.MainGameEvent.OnPlayerLaunchActionFinish.Where(cmd => cmd.Hit && cmd.HitObjecctTag == HitObjecctTag.Biteable).Subscribe(cmd => aimingInterrupt());
        //GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerControllerMovement, getPlayer_SprintState);
        playerAnimatorView_.Start(player_Stats_);
        playerControllerMover_.Start(player_Stats_);
        playerAttackModel_.PlayerAttackModelInit();
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
    void onAimButtonTrigger(PlayerAimingButtonCommand playerAimingButtonCommand)
    {
        player_Stats_.Aiming = playerAimingButtonCommand.AimingButtonIsPressed;
        if (playerAimingButtonCommand.AimingButtonIsPressed)
        {
            playerControllerMover_.TransitionState("Aim");
        }
        else
        {
            aimingInterrupt();
            playerControllerMover_.TransitionState("MainGame");
        } 
    }

    void onTargetGetObject()
    {
        player_Stats_.Targeting = true;
        playerControllerMover_.TransitionState("Target");
    }
    void onTargetResetObject()
    {
        player_Stats_.Targeting = false;
        playerControllerMover_.TransitionState("MainGame");
    }
    #endregion

    void aimingInterrupt()
    {
        player_Stats_.Aiming = false;
        playerControllerMover_.TransitionState("MainGame");
    }
    void cancelMoving()
    {
        player_Stats_.Player_CanMove = false;
    }
    void Update()
    {
        playerAnimatorView_.Update();
        playerControllerMover_.Update();
    }

    private void FixedUpdate()
    {
        playerAttackModel_.PlayerAttackModelUpdate();
        
    }
    private void OnDrawGizmosSelected()
    {
        if (player_Stats_.Grounded)
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.red;
        }
        Gizmos.DrawSphere(
            new Vector3(transform.position.x, transform.position.y - player_Stats_.GroundedOffset, transform.position.z),
            player_Stats_.GroundedRadius);
    }
}


[Serializable]
public class Player_Stats
{
    [Tooltip("玩家移動速度")]
    public float Player_Speed;

    [Tooltip("玩家衝刺速度")]
    public float Player_DashSpeed = 20;

    [Tooltip("玩家衝刺時間")]
    public float Player_DashTime = 0.25f;

    [Tooltip("地板檢查，這不是CharacterController自帶的isGrounded，那東西是大便")]
    public bool Grounded = true;

    [Tooltip("就算是粗糙的地面也能接受的偵測範圍")]
    public float GroundedOffset = -0.15f;

    [Tooltip("地板檢查的半徑。 應與CharacterControlle的半徑匹配")]
    public float GroundedRadius = 0.3f;

    [Tooltip("角色的垂直速度")]
    public float verticalVelocity_;


    [Tooltip("角色使用哪些Layer作為地面")]
    public LayerMask GroundLayers;
    [Tooltip("射線偵測所使用的Layer")]
    public LayerMask aimColliderMask;


    [Tooltip("玩家瞄準狀態")]
    public bool Aiming = false;

    [Tooltip("玩家落下狀態")]
    public bool Falling;

    [Tooltip("運動速度達到最大值之前的速度數值，數值越大達到最大值的時間越快")]
    public float SpeedChangeRate = 10.0f;

    [Tooltip("角色輸入")]
    public float Player_InputMagnitude;

    [Tooltip("玩家鎖定狀態")]
    public bool Targeting = false;


    public Vector2 Player_Dir;

    public bool Player_AttackCommandAllow;

    public bool Player_CanMove = true;

    public bool Player_IsMoving = false;

    public int PlayerJumpCount = 2;
}
