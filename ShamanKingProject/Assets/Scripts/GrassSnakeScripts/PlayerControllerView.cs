using Cysharp.Threading.Tasks;
using DG.Tweening;
using Gamemanager;
using System;
using System.Xml.Linq;
using System.Xml.Serialization;
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

    [SerializeField]
    GameObject stickInputIndicator_;

    [SerializeField]
    GameObject CamObject_;

    [SerializeField]
    float inputAngle_;

    [SerializeField]
    GameObject playerModel_;

    [SerializeField]
    GameObject dashPointTest;

    [SerializeField]
    Material[] test_;



    void Awake()
    {
        playerAnimatorView_ = new PlayerAnimator(this.gameObject);
        playerControllerMover_ = new PlayerControllerMover(this.gameObject);
        playerAttackModel_ = new PlayerAttackModel(this.gameObject);
        playerControllerMover_.Awake();
    }
    void Start()
    {
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnSystemAttackAllow, cmd =>
        {
            
                //playerModel_.transform.rotation = stickInputIndicator_.transform.rotation;
                playerModel_.transform.DORotateQuaternion(stickInputIndicator_.transform.rotation, 0.15f);
            
        });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerControllerMovement, getPlayerDirection);
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnAimingButtonTrigger, onAimButtonTrigger);
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnSystemGetTarget, cmd => { onTargetGetObject(); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnSystemResetTarget, cmd => { onTargetResetObject(); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerLightAttack, cmd => { cancelMoving(); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerThrowAttack, cmd => { cancelMoving(); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerHeavyAttack, cmd => { cancelMoving(); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerRoll,  cmd => { cancelMoving();});
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnStartRollMovementAnimation, cmd => { testRoll(); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerAnimationEvents, cmd =>
        {
            if (cmd.AnimationEventName == "PlayerJumpAttackStart")
            {
                cancelMoving();
            }
        });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerMovementInterruptionFinish, cmd => { player_Stats_.Player_CanMove = true; });

        GameManager.Instance.MainGameEvent.OnPlayerLaunchActionFinish.Where(cmd => cmd.Hit &&( cmd.HitObjecctTag == HitObjecctTag.Biteable|| cmd.HitObjecctTag == HitObjecctTag.Enemy)).Subscribe(cmd => aimingInterrupt());
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
            player_Stats_.Targeting = false;
            playerControllerMover_.TransitionState("Aim");
        }
        else
        {
            if (player_Stats_.Targeting)
            {
                return;
            }
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

    void stickInputIndicator()
    {
        Vector3 inputDirection = new Vector3(player_Stats_.Player_Dir.x, 0.0f, player_Stats_.Player_Dir.y).normalized;
        var player_TargetRotation_ = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;

        ////將模型旋轉至相對於相機位置的輸入方向
        //model_Transform_.transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);

            //旋轉平滑用的插值運算
            float turnSmoothVelocity_ = 0;
            float rotation = Mathf.SmoothDampAngle(stickInputIndicator_.transform.rotation.eulerAngles.y, player_TargetRotation_, ref turnSmoothVelocity_, 0.01f);
            //將模型旋轉至相對於相機位置的輸入方向
            stickInputIndicator_.transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        
    }
    void getInputAngle() 
    {
        // 獲取當前旋轉的 Y 軸角度
        float yRotation = CamObject_.transform.eulerAngles.y;

        // 創建一個新的旋轉，只在 Y 軸旋轉
        Quaternion newYOnlyRotation = Quaternion.Euler(0, yRotation, 0);

        // 獲取這個新旋轉的 forward 向量
        Vector3 Camforward = newYOnlyRotation * Vector3.forward;

        Vector3 inputDirection = new Vector3(player_Stats_.Player_Dir.x, 0.0f, player_Stats_.Player_Dir.y).normalized;
        var player_TargetRotation_ = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;

        // 使用 Vector3.Angle 計算兩個向量之間的角度
        Quaternion rotation = Quaternion.Euler(0, player_TargetRotation_, 0);
        Vector3 Inputforward = rotation * Vector3.forward;

        // 使用叉積來確定角度的方向（順時針或逆時針）
        float angle = Vector3.Angle( Inputforward, Camforward);
        Vector3 cross = Vector3.Cross(Inputforward, Camforward);
        if (cross.y < 0) // 如果 Y 分量是負的，則角度應該是大於 180 度的
        {
            angle = 360 - angle;
        }

        // 打印結果
        //Debug.Log("Forward 方向之間的角度: " + angle);
        inputAngle_ = angle;
    }

    void aimingInterrupt()
    {
        player_Stats_.Aiming = false;
        playerControllerMover_.TransitionState("MainGame");
    }
    void cancelMoving()
    {
        player_Stats_.Player_CanMove = false;
    }

    async void testRoll()
    {
        Vector3 inputDirection = new Vector3(player_Stats_.Player_Dir.x, 0.0f, player_Stats_.Player_Dir.y).normalized;
        var player_TargetRotation_ = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;

        // 使用 Vector3.Angle 計算兩個向量之間的角度
        Quaternion rotation = Quaternion.Euler(0, player_TargetRotation_, 0);
        Vector3 Inputforward = rotation * Vector3.forward;
        var final= Inputforward * 3f + stickInputIndicator_.transform.position;
        //playerModel_.transform.LookAt(final);

        
        this.transform.DOMove(final, 0.55f).SetEase(Ease.InSine);
        DOVirtual.Float(-0.5f, 1.5f, 0.2f, value => {
            Vector4 currentParams = test_[0].GetVector("_DissolveParams");
            Debug.Log(currentParams);
            currentParams.z = value;
            currentParams.w = 0.5f;
            foreach (var item in test_)
            {
                item.SetVector("_DissolveParams", currentParams);
            }
        });
        await UniTask.Delay(250);
        DOVirtual.Float(1.5f, -0.5f, 0.6f, value => {
            Vector4 currentParams = test_[0].GetVector("_DissolveParams");
            Debug.Log(currentParams);
            currentParams.z = value;
            currentParams.w = 0.5f;
            foreach (var item in test_)
            {
                item.SetVector("_DissolveParams", currentParams);
            }
        }).SetEase(Ease.InSine);
        dashPointTest.transform.position = final;
    }
    void Update()
    {
        playerAnimatorView_.Update();
        playerControllerMover_.Update();
        stickInputIndicator();
        getInputAngle();
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
    public float GroundedRadius = 0.5f;

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
