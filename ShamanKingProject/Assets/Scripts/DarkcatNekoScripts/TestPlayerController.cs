using BehaviorDesigner.Runtime.Tasks.Unity.UnityCharacterController;
using Gamemanager;
using Obi;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.Windows;

public enum PlayerState
{

}
[RequireComponent(typeof(CharacterController))]
public class TestPlayerController : MonoBehaviour
{
    [Header("Player")]
    [Tooltip("玩家移動速度")]
    public float MoveSpeed = 2.0f;

    [Tooltip("玩家衝刺速度")]
    public float SprintSpeed = 5.5f;

    [Tooltip("運動速度達到最大值之前的速度數值，數值越大達到最大值的時間越快")]
    public float SpeedChangeRate = 10.0f;

    [Tooltip("角色轉向面對運動方向的速度有多快")]
    [Range(0.0f, 0.3f)]
    public float TurnSmoothTime = 0.15f;

    [Tooltip("就算是粗糙的地面也能接受的偵測範圍")]
    public float GroundedOffset = -0.15f;

    [Tooltip("地板檢查的半徑。 應與CharacterControlle的半徑匹配")]
    public float GroundedRadius = 0.3f;

    [Tooltip("角色使用哪些Layer作為地面")]
    public LayerMask GroundLayers;

    [Header("Player Grounded")]
    [Tooltip("地板檢查，這不是CharacterController自帶的isGrounded，那東西是大便")]
    public bool Grounded = true;

    [SerializeField] GameObject playerOBj_;

    //--------------------------------------------------------------------------------------------------------------

    private const float speedOffset = 0.01f;

    private CharacterController player_CC;
    private GameObject _mainCamera;

    private bool player_SprintStatus;

    private float player_Speed;
    private float player_TargetRotation = 0.0f;

    private float turnSmoothVelocity;
    private float verticalVelocity;

    private Vector2 player_Dir;

    void Start()
    {
        // get a reference to our main camera
        if (_mainCamera == null)
        {
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }

        player_CC = GetComponent<CharacterController>();

        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerControllerMovement, GetPlayer_Direction);
        //GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerControllerMovement, GetPlayer_SprintStatus);
    }

    void Update()
    {
        GroundedCheck();
        Move();
    }
    void GroundedCheck()
    {
        //設置球的偵測位置
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
            transform.position.z);
        Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
            QueryTriggerInteraction.Ignore);

    }
    void Move()
    {

        //根據移動速度、衝刺速度以及是否按下衝刺設置目標速度
        float targetSpeed = player_SprintStatus ? SprintSpeed : MoveSpeed;

        //如果沒有輸入，則將目標速度設置為0
        if (player_Dir == Vector2.zero) targetSpeed = 0.0f;

        //玩家當前水平速度的引用
        float currentHorizontalSpeed = new Vector3(player_CC.velocity.x, 0.0f, player_CC.velocity.z).magnitude;

        
        float inputMagnitude = player_Dir.magnitude;

        //為了提供一個容錯範圍。當當前速度與目標速度之間的差值小於容錯範圍時，就不需要進行加速或減速操作，
        //因為這時候已經非常接近目標速度了，再進行微小的變化可能會導致速度上下抖動，產生不良的遊戲體驗。
        //因此，這個容錯範圍可以幫助確保角色在接近目標速度時保持穩定。
        if (currentHorizontalSpeed < targetSpeed - speedOffset ||
            currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            // 改善速度變化，計算速度為滑順的而不是線性結果
            player_Speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                Time.deltaTime * SpeedChangeRate);

            //去除3位小數點之後的數字
            player_Speed = Mathf.Round(player_Speed * 1000f) / 1000f;
        }
        else
        {
            player_Speed = targetSpeed;
        }

        // 歸一化輸入方向
        Vector3 inputDirection = new Vector3(player_Dir.x, 0.0f, player_Dir.y).normalized;

        //玩家移動中
        if (player_Dir != Vector2.zero)
        {
            player_TargetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
            //float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, player_TargetRotation, ref turnSmoothVelocity, TurnSmoothTime);

            //旋轉至相對於相機位置的輸入方向
            //transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);

        }

        Vector3 targetDirection = Quaternion.Euler(0.0f, player_TargetRotation, 0.0f) * Vector3.forward;

        player_CC.Move(targetDirection.normalized * (player_Speed * Time.deltaTime) + new Vector3(0.0f, verticalVelocity, 0.0f) * Time.deltaTime);
    }
    void JumpAndFall()
    {
        if (Grounded)
        {
            //角色在地面上的操作
        }
    }
    void GetPlayer_Direction(PlayerControllerMovementCommand playerControllerMovementCommand)
    {
        player_Dir = playerControllerMovementCommand.Direction;
    }
    void GetPlayer_SprintStatus(PlayerControllerMovementCommand playerControllerMovementCommand)
    {
        //player_SprintStatus = playerControllerMovementCommand.Sprint;
    }


    private void OnDrawGizmosSelected()
    {
        if (Grounded) 
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.red;
        }
        Gizmos.DrawSphere(
            new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
            GroundedRadius);
    }


}
