using Cysharp.Threading.Tasks;
using DG.Tweening;
using Gamemanager;
using System;
using System.Collections;
using System.Xml.Linq;
using System.Xml.Serialization;
using TMPro;
using UniRx;
using UnityEngine;
using Datamanager;


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

    [SerializeField]
    LayerMask groundLayer_;   


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
        //GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnAimingButtonTrigger, onAimButtonTrigger);
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerGuardingButtonTrigger, onPlayerGuardingButtonTrigger);
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnSystemGetTarget, cmd => { onTargetGetObject(); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnSystemResetTarget, cmd => { onTargetResetObject(); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerLightAttack, cmd => { cancelMoving(); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerThrowAttack, cmd => { cancelMoving(); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerHeavyAttack, cmd => { cancelMoving(); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerUltimateAttack, cmd => { cancelMoving(); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerShootAttack, cmd => { cancelMoving(); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerExecutionAttack, cmd => { cancelMoving(); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerBeAttackByEnemySuccess, cmd => { cancelMoving(); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnEnemyAttackSuccess, cmd => { playerModelTurn(cmd); });
        //GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerSuccessParry, cmd => { playerModelTurn(cmd); });
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
        GameManager.Instance.MainGameMediator.RealTimePlayerData.PlayerGameObject = this.gameObject;
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerGrabSuccessForPlayer, cmd => { DashToGrabTarget(cmd); });
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
    //void onAimButtonTrigger(PlayerAimingButtonCommand playerAimingButtonCommand)
    //{
    //    player_Stats_.Aiming = playerAimingButtonCommand.AimingButtonIsPressed;
    //    if (playerAimingButtonCommand.AimingButtonIsPressed)
    //    {
    //        player_Stats_.Targeting = false;
    //        playerControllerMover_.TransitionState("Aim");
    //    }
    //    else
    //    {
    //        if (player_Stats_.Targeting)
    //        {
    //            return;
    //        }
    //        aimingInterrupt();
    //        playerControllerMover_.TransitionState("MainGame");
    //    } 
    //}

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

    #region - Player取得蓄力指令 -
    void onPlayerGuardingButtonTrigger(PlayerGuardingButtonCommand playerGuardingButtonCommand)
    {
        player_Stats_.Guarding = playerGuardingButtonCommand.GuardingButtonIsPressed;
    }
    #endregion

    void playerModelTurn(EnemyAttackSuccessCommand cmd)
    {
        Debug.Log(cmd.AttackerPos);
        dashPointTest.transform.position = cmd.AttackerPos;
        // 計算物件A到座標B的方向向量
        Vector3 direction = cmd.AttackerPos - playerModel_.transform.position;
        direction.y = 0;
        // 將方向向量轉換為旋轉
        Quaternion rotation = Quaternion.LookRotation(direction);

        // 將物件A的Rotation設定為計算出的旋轉
        playerModel_.transform.rotation = rotation;
    }
    void playerModelTurn(PlayerSuccessParryCommand cmd)
    {
        Debug.Log(cmd.CollidePoint);
        dashPointTest.transform.position = cmd.AttackerPos;
        // 計算物件A到座標B的方向向量
        Vector3 direction = cmd.AttackerPos - playerModel_.transform.position;
        direction.y = 0;
        // 將方向向量轉換為旋轉
        Quaternion rotation = Quaternion.LookRotation(direction);

        // 將物件A的Rotation設定為計算出的旋轉
        playerModel_.transform.rotation = rotation;
    }
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
    void chargingInterrupt()
    {
        player_Stats_.Guarding = false;
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
       
        var final= Inputforward * player_Stats_.Player_DodgeDistance + stickInputIndicator_.transform.position;
        //playerModel_.transform.LookAt(final);
        if (Physics.Raycast(stickInputIndicator_.transform.position, Inputforward, out RaycastHit hit, player_Stats_.Player_DodgeDistance, groundLayer_))
        {
            // 擊中了Layer為Ground的碰撞器，hit變數中包含擊中點的信息
            final = hit.point;
            // 在這裡您可以使用hitPoint來進一步處理碰撞點
        }
        Debug.Log(GameManager.Instance.MainGameMediator.RealTimePlayerData.PlayerGameObject.name);
        dashPointTest.transform.position = final;
        //this.gameObject.transform.DOMove(final, player_Stats_.Player_DodgeSpeed).SetEase(Ease.InSine);
        StartCoroutine(MoveToPosition(final, player_Stats_.Player_DodgeSpeed));
        PlayerStatCalculator.PlayerInvincibleSwitch(true);       
        DOVirtual.Float(-0.5f, 1.5f, 0.1f, value => {
            Vector4 currentParams = test_[0].GetVector("_DissolveParams");
           // Debug.Log(currentParams);
            currentParams.z = value;
            currentParams.w = 0.5f;
            foreach (var item in test_)
            {
                item.SetVector("_DissolveParams", currentParams);
            }
        }).OnComplete(() => {  });
        await UniTask.Delay(((int)(player_Stats_.Player_DodgeSpeed*1000) - 100));       
        PlayerStatCalculator.PlayerInvincibleSwitch(false);
        spawnDashEffect();
        DOVirtual.Float(1.5f, -0.5f, 0.6f, value => {
            Vector4 currentParams = test_[0].GetVector("_DissolveParams");
           // Debug.Log(currentParams);
            currentParams.z = value;
            currentParams.w = 0.5f;
            foreach (var item in test_)
            {
                item.SetVector("_DissolveParams", currentParams);
            }
        }).SetEase(Ease.InSine).OnComplete(() => {  });
        
    }

    void spawnDashEffect()
    {
        var effect = GameContainer.Get<DataManager>().GetDataByID<GameEffectTemplete>(11).PrefabPath;
        Instantiate(effect, GameManager.Instance.MainGameMediator.RealTimePlayerData.PlayerGameObject.transform.position, Quaternion.identity);
    }
    public IEnumerator MoveToPosition(Vector3 target, float duration)
    {
        Vector3 startPosition = transform.position;
        float startTime = Time.time;

        while (Time.time < startTime + duration)
        {
            float t = (Time.time - startTime) / duration;
            t = Mathf.Sin(t * Mathf.PI * 0.5f); // 使用 Sine 函數作為緩動函數
            transform.position = Vector3.Lerp(startPosition, target, t);
            yield return null;
        }

        transform.position = target; // 確保最終位置正確
    }
    async void DashToGrabTarget(PlayerGrabSuccessResponse cmd)
    {
        var player = GameManager.Instance.MainGameMediator.RealTimePlayerData.PlayerGameObject;
        Debug.LogError(cmd.AttackTarget.name);
        var vector = cmd.AttackTarget.transform.position - player.transform.position;
        vector.y = 0;//水平化操作
        var direction = vector.normalized;
        var length = vector.magnitude;
        var destination = player.transform.position + direction * (length - 1.5f);

        await UniTask.Delay(150);
        StartCoroutine(MoveToPosition(destination,0.7f));
        //player.transform.DOMove(destination, 0.7f).OnComplete(() => {
        //    // 移动完成后的回调操作
        //    Debug.Log("Player has reached the enemy.");
        //    if (player_Stats_.Aiming) { GameManager.Instance.MainGameEvent.Send(new GhostLaunchProcessFinishResponse()); }
        //});
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
    [Tooltip("玩家瞬移距離")]
    public float Player_DodgeDistance;
    [Tooltip("玩家瞬移速度")]
    public float Player_DodgeSpeed;

    [Tooltip("玩家移動速度")]
    public float Player_Speed;

    [Tooltip("玩家蓄力時的移動速度")]
    public float Player_GuardingSpeed = 2.5f;

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
    [Tooltip("玩家格擋狀態")]
    public bool Guarding = false;

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
