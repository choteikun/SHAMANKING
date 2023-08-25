using BehaviorDesigner.Runtime.Tasks.Unity.UnityCharacterController;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
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
    public float MoveSpeed = 3.5f;

    [Tooltip("玩家衝刺速度")]
    public float SprintSpeed = 5.5f;

    [Tooltip("運動速度達到最大值之前的速度數值，數值越大達到最大值的時間越快")]
    public float SpeedChangeRate = 10.0f;

    [Tooltip("角色轉向面對運動方向的速度有多快")]
    [Range(0.0f, 0.3f)]
    public float TurnSmoothTime = 0.1f;

    [Tooltip("就算是粗糙的地面也能接受的偵測範圍")]
    public float GroundedOffset = -0.15f;

    [Tooltip("地板檢查的半徑。 應與CharacterControlle的半徑匹配")]
    public float GroundedRadius = 0.3f;

    [Tooltip("角色使用哪些Layer作為地面")]
    public LayerMask GroundLayers;

    [Header("Player Grounded")]
    [Tooltip("地板檢查，這不是CharacterController自帶的isGrounded，那東西是大便")]
    public bool Grounded = true;

    //--------------------------------------------------------------------------------------------------------------

    private const float speedOffset = 0.01f;

    private CharacterController player_CC_;
    private Transform model_Transform_;
    private GameObject mainCamera_;

    private bool player_SprintStatus_;

    private float player_Speed_;
    private float player_TargetRotation_ = 0.0f;

    private float turnSmoothVelocity_;
    private float verticalVelocity_;

    private Vector2 player_Dir_;

    void Start()
    {
        if (mainCamera_ == null)
        {
            mainCamera_ = GameObject.FindGameObjectWithTag("MainCamera");
        }

        player_CC_ = GetComponent<CharacterController>();

        model_Transform_ = GetComponentInChildren<Animator>().transform;

        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerControllerMovement, getPlayer_Direction);
        //GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerControllerMovement, GetPlayer_SprintStatus);
    }

    void Update()
    {
        groundedCheck();
        move();
    }
    void groundedCheck()
    {
        //設置球的偵測位置
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
            transform.position.z);
        Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
            QueryTriggerInteraction.Ignore);

    }
    void move()
    {

        //根據移動速度、衝刺速度以及是否按下衝刺設置目標速度
        float targetSpeed = player_SprintStatus_ ? SprintSpeed : MoveSpeed;

        //如果沒有輸入，則將目標速度設置為0
        if (player_Dir_ == Vector2.zero) targetSpeed = 0.0f;

        //玩家當前水平速度的引用
        float currentHorizontalSpeed = new Vector3(player_CC_.velocity.x, 0.0f, player_CC_.velocity.z).magnitude;

        
        float inputMagnitude = player_Dir_.magnitude;

        //為了提供一個容錯範圍。當當前速度與目標速度之間的差值小於容錯範圍時，就不需要進行加速或減速操作，
        //因為這時候已經非常接近目標速度了，再進行微小的變化可能會導致速度上下抖動，產生不良的遊戲體驗。
        //因此，這個容錯範圍可以幫助確保角色在接近目標速度時保持穩定。
        if (currentHorizontalSpeed < targetSpeed - speedOffset ||
            currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            // 改善速度變化，計算速度為滑順的而不是線性結果
            player_Speed_ = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                Time.deltaTime * SpeedChangeRate);

            //去除3位小數點之後的數字
            player_Speed_ = Mathf.Round(player_Speed_ * 1000f) / 1000f;
        }
        else
        {
            player_Speed_ = targetSpeed;
        }

        //單位化，防止同時兩個方向移動，速度變快
        Vector3 inputDirection = new Vector3(player_Dir_.x, 0.0f, player_Dir_.y).normalized;

        //玩家移動中
        if (player_Dir_ != Vector2.zero)
        {
            //計算輸入端輸入後所需要的轉向角度，加上相機的角度實現相對相機的前方的移動
            player_TargetRotation_ = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + mainCamera_.transform.eulerAngles.y;
            //旋轉平滑用的插值運算
            float rotation = Mathf.SmoothDampAngle(model_Transform_.eulerAngles.y, player_TargetRotation_, ref turnSmoothVelocity_, TurnSmoothTime);

            //將模型旋轉至相對於相機位置的輸入方向
            model_Transform_.transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);

        }

        Vector3 targetDirection = Quaternion.Euler(0.0f, player_TargetRotation_, 0.0f) * Vector3.forward;

        player_CC_.Move(targetDirection.normalized * (player_Speed_ * Time.deltaTime) + new Vector3(0.0f, verticalVelocity_, 0.0f) * Time.deltaTime);
    }
    void jumpAndFall()
    {
        if (Grounded)
        {
            //角色在地面上的操作
        }
    }
    void getPlayer_Direction(PlayerControllerMovementCommand playerControllerMovementCommand)
    {
        player_Dir_ = playerControllerMovementCommand.Direction;
    }
    void getPlayer_SprintStatus(PlayerControllerMovementCommand playerControllerMovementCommand)
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
