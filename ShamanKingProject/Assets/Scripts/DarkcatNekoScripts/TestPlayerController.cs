using BehaviorDesigner.Runtime.Tasks.Unity.UnityCharacterController;
using Gamemanager;
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
    public float MoveSpeed = 2.0f;

    [Tooltip("���a�Ĩ�t��")]
    public float SprintSpeed = 5.335f;

    [Tooltip("�B�ʳt�׹F��̤j�Ȥ��e���t�׼ƭȡA�ƭȶV�j�F��̤j�Ȫ��ɶ��V��")]
    public float SpeedChangeRate = 10.0f;

    [Tooltip("������V����B�ʤ�V���t�צ��h��")]
    [Range(0.0f, 0.3f)]
    public float TurnSmoothTime = 0.15f;

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

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {

        //�ھڲ��ʳt�סB�Ĩ�t�ץH�άO�_���U�Ĩ�]�m�ؼгt��
        float targetSpeed = player_SprintStatus ? SprintSpeed : MoveSpeed;

        //�p�G�S����J�A�h�N�ؼгt�׳]�m��0
        if (player_Dir == Vector2.zero) targetSpeed = 0.0f;

        //���a��e�����t�ת��ޥ�
        float currentHorizontalSpeed = new Vector3(player_CC.velocity.x, 0.0f, player_CC.velocity.z).magnitude;

        //���F���Ѥ@�Ӯe���d��C���e�t�׻P�ؼгt�פ������t�Ȥp��e���d��ɡA�N���ݭn�i��[�t�δ�t�ާ@�A
        //�]���o�ɭԤw�g�D�`����ؼгt�פF�A�A�i��L�p���ܤƥi��|�ɭP�t�פW�U�ݰʡA���ͤ��}���C������C
        //�]���A�o�Ӯe���d��i�H���U�T�O����b����ؼгt�׮ɫO��í�w�C
        float inputMagnitude = player_Dir.magnitude;

        //�[�t�δ�t��ؼгt��
        if (currentHorizontalSpeed < targetSpeed - speedOffset ||
            currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            // �ﵽ�t���ܤơA�p��t�׬��ƶ����Ӥ��O�u�ʵ��G
            player_Speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                Time.deltaTime * SpeedChangeRate);

            // round speed to 3 decimal places
            player_Speed = Mathf.Round(player_Speed * 1000f) / 1000f;
        }
        else
        {
            player_Speed = targetSpeed;
        }

        // �k�@�ƿ�J��V
        Vector3 inputDirection = new Vector3(player_Dir.x, 0.0f, player_Dir.y).normalized;

        //���a���ʤ�
        if (player_Dir != Vector2.zero)
        {
            player_TargetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
            //float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, player_TargetRotation, ref turnSmoothVelocity, TurnSmoothTime);

            //����ܬ۹��۾���m����J��V
            //transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);

        }

        Vector3 targetDirection = Quaternion.Euler(0.0f, player_TargetRotation, 0.0f) * Vector3.forward;

        player_CC.Move(targetDirection.normalized * (player_Speed * Time.deltaTime) + new Vector3(0.0f, verticalVelocity, 0.0f) * Time.deltaTime);
    }
    void GroundedCheck()
    {

    }
    void GetPlayer_Direction(PlayerControllerMovementCommand playerControllerMovementCommand)
    {
        player_Dir = playerControllerMovementCommand.Direction;
    }
    void GetPlayer_SprintStatus(PlayerControllerMovementCommand playerControllerMovementCommand)
    {
        //player_SprintStatus = playerControllerMovementCommand.Sprint;
    }
}
