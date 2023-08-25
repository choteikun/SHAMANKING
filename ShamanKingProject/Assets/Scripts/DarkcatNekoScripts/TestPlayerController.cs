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
    [Tooltip("���a���ʳt��")]
    public float MoveSpeed = 3.5f;

    [Tooltip("���a�Ĩ�t��")]
    public float SprintSpeed = 5.5f;

    [Tooltip("�B�ʳt�׹F��̤j�Ȥ��e���t�׼ƭȡA�ƭȶV�j�F��̤j�Ȫ��ɶ��V��")]
    public float SpeedChangeRate = 10.0f;

    [Tooltip("������V����B�ʤ�V���t�צ��h��")]
    [Range(0.0f, 0.3f)]
    public float TurnSmoothTime = 0.1f;

    [Tooltip("�N��O���W���a���]�౵���������d��")]
    public float GroundedOffset = -0.15f;

    [Tooltip("�a�O�ˬd���b�|�C ���PCharacterControlle���b�|�ǰt")]
    public float GroundedRadius = 0.3f;

    [Tooltip("����ϥέ���Layer�@���a��")]
    public LayerMask GroundLayers;

    [Header("Player Grounded")]
    [Tooltip("�a�O�ˬd�A�o���OCharacterController�۱a��isGrounded�A���F��O�j�K")]
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
        //�]�m�y��������m
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
            transform.position.z);
        Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
            QueryTriggerInteraction.Ignore);

    }
    void move()
    {

        //�ھڲ��ʳt�סB�Ĩ�t�ץH�άO�_���U�Ĩ�]�m�ؼгt��
        float targetSpeed = player_SprintStatus_ ? SprintSpeed : MoveSpeed;

        //�p�G�S����J�A�h�N�ؼгt�׳]�m��0
        if (player_Dir_ == Vector2.zero) targetSpeed = 0.0f;

        //���a��e�����t�ת��ޥ�
        float currentHorizontalSpeed = new Vector3(player_CC_.velocity.x, 0.0f, player_CC_.velocity.z).magnitude;

        
        float inputMagnitude = player_Dir_.magnitude;

        //���F���Ѥ@�Ӯe���d��C���e�t�׻P�ؼгt�פ������t�Ȥp��e���d��ɡA�N���ݭn�i��[�t�δ�t�ާ@�A
        //�]���o�ɭԤw�g�D�`����ؼгt�פF�A�A�i��L�p���ܤƥi��|�ɭP�t�פW�U�ݰʡA���ͤ��}���C������C
        //�]���A�o�Ӯe���d��i�H���U�T�O����b����ؼгt�׮ɫO��í�w�C
        if (currentHorizontalSpeed < targetSpeed - speedOffset ||
            currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            // �ﵽ�t���ܤơA�p��t�׬��ƶ����Ӥ��O�u�ʵ��G
            player_Speed_ = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                Time.deltaTime * SpeedChangeRate);

            //�h��3��p���I���᪺�Ʀr
            player_Speed_ = Mathf.Round(player_Speed_ * 1000f) / 1000f;
        }
        else
        {
            player_Speed_ = targetSpeed;
        }

        //���ơA����P�ɨ�Ӥ�V���ʡA�t���ܧ�
        Vector3 inputDirection = new Vector3(player_Dir_.x, 0.0f, player_Dir_.y).normalized;

        //���a���ʤ�
        if (player_Dir_ != Vector2.zero)
        {
            //�p���J�ݿ�J��һݭn����V���סA�[�W�۾������׹�{�۹�۾����e�誺����
            player_TargetRotation_ = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + mainCamera_.transform.eulerAngles.y;
            //���७�ƥΪ����ȹB��
            float rotation = Mathf.SmoothDampAngle(model_Transform_.eulerAngles.y, player_TargetRotation_, ref turnSmoothVelocity_, TurnSmoothTime);

            //�N�ҫ�����ܬ۹��۾���m����J��V
            model_Transform_.transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);

        }

        Vector3 targetDirection = Quaternion.Euler(0.0f, player_TargetRotation_, 0.0f) * Vector3.forward;

        player_CC_.Move(targetDirection.normalized * (player_Speed_ * Time.deltaTime) + new Vector3(0.0f, verticalVelocity_, 0.0f) * Time.deltaTime);
    }
    void jumpAndFall()
    {
        if (Grounded)
        {
            //����b�a���W���ާ@
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
