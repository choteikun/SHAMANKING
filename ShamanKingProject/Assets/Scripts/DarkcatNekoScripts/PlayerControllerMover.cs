using System;
using UnityEngine;

public enum PlayerState
{

}
[Serializable]
[RequireComponent(typeof(CharacterController))]
public class PlayerControllerMover
{
    [Header("Player")]
    [Tooltip("玩家移動速度")]
    public float MoveSpeed = 5f;

    [Tooltip("玩家衝刺速度")]
    public float SprintSpeed = 5.5f;

    [Tooltip("角色轉向面對運動方向的速度有多快")]
    [Range(0.0f, 0.3f)]
    public float TurnSmoothTime = 0.1f;

    [Tooltip("就算是粗糙的地面也能接受的偵測範圍")]
    public float GroundedOffset = -0.15f;

    [Tooltip("地板檢查的半徑。 應與CharacterControlle的半徑匹配")]
    public float GroundedRadius = 0.3f;

    [Tooltip("角色使用哪些Layer作為地面")]
    public LayerMask GroundLayers;
    [Tooltip("射線偵測所使用的Layer")]
    public LayerMask aimColliderMask;

    //--------------------------------------------------------------------------------------------------------------
    private Player_Stats player_Stats_;

    private const float speedOffset = 0.01f;

    private CharacterController player_CC_;
    private Transform model_Transform_;

    private Transform aimDestination_Transform_;

    private GameObject mainCamera_;

    private GameObject characterControllerObj_;


    private bool player_SprintStatus_;


    private float player_TargetRotation_ = 0.0f;

    //private float player_Speed_;
    private float turnSmoothVelocity_;
    private float verticalVelocity_;

    private ControllerMoverStateMachine controllerMoverStateMachine_;


    public PlayerControllerMover(GameObject controller)
    {
        characterControllerObj_ = controller;
    }
    public void Awake()
    {
        controllerMoverStateMachine_ = new ControllerMoverStateMachine(this);
        controllerMoverStateMachine_.StageManagerInit();
    }
    public void Start(Player_Stats player_Stats)
    {
        player_Stats_ = player_Stats;
        if (mainCamera_ == null)
        {
            mainCamera_ = GameObject.FindGameObjectWithTag("MainCamera");
        }

        aimDestination_Transform_ = GameObject.Find("AimingCameraFollowTarget").transform;

        player_CC_ = characterControllerObj_.GetComponent<CharacterController>();

        model_Transform_ = characterControllerObj_.GetComponentInChildren<Animator>().transform;

    }

    public void Update()
    {
        //groundedCheck();
        move();
        controllerMoverStateMachine_.StageManagerUpdate();
    }
    public void TransitionState(string state)
    {
        controllerMoverStateMachine_.TransitionState(state);
    }
    void groundedCheck(Player_Stats player_Stats)
    {
        //設置球的偵測位置
        Vector3 spherePosition = new Vector3(characterControllerObj_.transform.position.x, characterControllerObj_.transform.position.y - GroundedOffset,
            characterControllerObj_.transform.position.z);
        player_Stats.Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
            QueryTriggerInteraction.Ignore);

    }
    void move()
    {
        //根據移動速度、衝刺速度以及是否按下衝刺設置目標速度
        float targetSpeed = player_SprintStatus_ ? SprintSpeed : MoveSpeed;

        //如果沒有輸入，則將目標速度設置為0
        if (player_Stats_.Player_Dir == Vector2.zero) targetSpeed = 0.0f;

        //玩家當前水平速度的引用
        float currentHorizontalSpeed = new Vector3(player_CC_.velocity.x, 0.0f, player_CC_.velocity.z).magnitude;

        //為了提供一個容錯範圍。當當前速度與目標速度之間的差值小於容錯範圍時，就不需要進行加速或減速操作，
        //因為這時候已經非常接近目標速度了，再進行微小的變化可能會導致速度上下抖動，產生不良的遊戲體驗。
        //因此，這個容錯範圍可以幫助確保角色在接近目標速度時保持穩定。
        if (currentHorizontalSpeed < targetSpeed - speedOffset ||
            currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            // 改善速度變化，計算速度為滑順的而不是線性結果
            player_Stats_.Player_Speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed,
                Time.deltaTime * player_Stats_.SpeedChangeRate);

            //去除3位小數點之後的數字
            player_Stats_.Player_Speed = Mathf.Round(player_Stats_.Player_Speed * 1000f) / 1000f;
            //Debug.Log("進入運動插值計算player_Speed_ : " + player_Stats_.Player_Speed + " currentHorizontalSpeed : " + currentHorizontalSpeed + " inputMagnitude : " + player_Stats_.Player_Dir.magnitude);
        }
        else
        {
            //針對手把操作改善
            if (player_Stats_.Player_InputMagnitude >= 0.999)
            {
                player_Stats_.Player_Speed = targetSpeed;
                //Debug.Log("沒進入差值計算player_Speed_ : " + player_Stats_.Player_Speed + " currentHorizontalSpeed : " + currentHorizontalSpeed + " inputMagnitude : " + player_Stats_.Player_Dir.magnitude);
            }
            else
            {
                // 改善速度變化，計算速度為滑順的而不是線性結果
                player_Stats_.Player_Speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed,
                    Time.deltaTime * player_Stats_.SpeedChangeRate);
                //去除3位小數點之後的數字
                player_Stats_.Player_Speed = Mathf.Round(player_Stats_.Player_Speed * 1000f) / 1000f;
                //Debug.Log("持續運動插值計算player_Speed_ : " + player_Stats_.Player_Speed + " currentHorizontalSpeed : " + currentHorizontalSpeed + " inputMagnitude : " + player_Stats_.Player_Dir.magnitude);
            }
            //player_Speed_ = targetSpeed;
            //Debug.Log("沒進入差值計算player_Speed_ : " + player_Speed_ + " currentHorizontalSpeed : " + currentHorizontalSpeed + " inputMagnitude : " + inputMagnitude);
        }

        //單位化，防止同時兩個方向移動，速度變快
        Vector3 inputDirection = new Vector3(player_Stats_.Player_Dir.x, 0.0f, player_Stats_.Player_Dir.y).normalized;

        //玩家移動中
        if (player_Stats_.Player_Dir != Vector2.zero)
        {
            //計算輸入端輸入後所需要的轉向角度，加上相機的角度實現相對相機的前方的移動
            player_TargetRotation_ = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + mainCamera_.transform.eulerAngles.y;

            ////將模型旋轉至相對於相機位置的輸入方向
            //model_Transform_.transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            if (!player_Stats_.Aiming)
            {
                //旋轉平滑用的插值運算
                float rotation = Mathf.SmoothDampAngle(model_Transform_.eulerAngles.y, player_TargetRotation_, ref turnSmoothVelocity_, TurnSmoothTime);
                //將模型旋轉至相對於相機位置的輸入方向
                model_Transform_.transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }
        }

        Vector3 targetDirection = Quaternion.Euler(0.0f, player_TargetRotation_, 0.0f) * Vector3.forward;
        //if (!player_Stats_.Player_AttackCommandAllow) return;
        player_CC_.Move(targetDirection.normalized * (player_Stats_.Player_Speed * Time.deltaTime) + new Vector3(0.0f, verticalVelocity_, 0.0f) * Time.deltaTime);

    }

    public void AimPointUpdate()
    {
        //Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        //Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);

        //if (Physics.Raycast(ray, out RaycastHit raycastHit, 9999f, aimColliderMask))
        //{
        //    aimDestination_Transform_.position = raycastHit.point;
        //}

        //Vector3 worldAimTarget = aimDestination_Transform_.position;
        //worldAimTarget.y = model_Transform_.position.y;
        //Vector3 aimdirection = (worldAimTarget - model_Transform_.position).normalized;
        var result = new Vector3(0, aimDestination_Transform_.rotation.eulerAngles.y, 0);
        var q_result = Quaternion.Euler(result);
        Quaternion newRotation = Quaternion.Slerp(model_Transform_.rotation, q_result, 15f * Time.deltaTime);
        model_Transform_.rotation = newRotation;
    }
    void jumpAndFall()
    {
        if (player_Stats_.Grounded)
        {
            //角色在地面上的操作
        }
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
            new Vector3(characterControllerObj_.transform.position.x, characterControllerObj_.transform.position.y - GroundedOffset, characterControllerObj_.transform.position.z),
            GroundedRadius);
    }

   
}
