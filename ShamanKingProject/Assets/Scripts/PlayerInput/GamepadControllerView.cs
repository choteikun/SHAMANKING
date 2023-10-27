using Cysharp.Threading.Tasks;
using DG.Tweening;
using Gamemanager;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
//using UnityEngine.InputSystem.Editor;

public class GamepadControllerView : MonoBehaviour
{

    [SerializeField] PlayerInput input_;
    [SerializeField] float mouse_X_Horrzontal_sensitivity_ = 1.2f;

    [SerializeField] bool isAiming_;
    [SerializeField] bool isLaunching_;
    [SerializeField] bool isPosscessing_ = false;
    [SerializeField] bool isBiting_ = false;
    [SerializeField] bool isAttacking_ = false;
    [SerializeField] bool isJumping_ = false;

    [SerializeField] bool aimingDelay_ = true;
    Tweener aimingDelayer_;

    private async void Start()
    {
        input_.SwitchCurrentActionMap("MainGameplay");
        Debug.Log("start");
        await UniTask.Delay(500);
        

        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnGhostLaunchProcessFinish, cmd => { finishLaunch(); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerMovementInterruptionFinish, cmd => { isAttacking_ = false; Debug.Log("finishSend"); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerJumpTouchGround, cmd => { isJumping_ = false; });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnGameConversationEnd, cmd => { input_.SwitchCurrentActionMap("MainGameplay"); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnSystemCallTutorial, cmd => { input_.SwitchCurrentActionMap("PlayerTutorialInput"); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerEndTutorial, cmd => { input_.SwitchCurrentActionMap("MainGameplay"); });
        var onLaunchHitPosscessableItem = GameManager.Instance.MainGameEvent.OnPlayerLaunchActionFinish.Where(cmd => cmd.Hit && (cmd.HitObjecctTag == HitObjecctTag.Possessable || cmd.HitObjecctTag == HitObjecctTag.Biteable)).Subscribe(cmd => { playerHitObject(cmd); });
        GameManager.Instance.MainGameMediator.AddToDisposables(onLaunchHitPosscessableItem);
    }
    //private void Update()
    //{
    //    if (inputActionMap_.FindAction("PlayerControl").WasPressedThisFrame())
    //    {
    //        Debug.Log("1");
    //        if (isAttacking_) return;
    //        var controllerDirection = inputActionMap_.FindAction("PlayerControl").ReadValue<Vector2>();
    //        if (controllerDirection.magnitude > 0.35f)
    //        {
    //            GameManager.Instance.MainGameEvent.Send(new PlayerControllerMovementCommand() { IsSmallMove = true, Direction = controllerDirection });
    //        }
    //        else
    //        {
    //            GameManager.Instance.MainGameEvent.Send(new PlayerControllerMovementCommand() { IsSmallMove = false, Direction = controllerDirection });
    //        }
    //    }

    //}
    void changeInpuMap(string map)
    {
        input_.SwitchCurrentActionMap(map);
    }
    void OnPlayerControl(InputValue value)
    {
        var controllerDirection = value.Get<Vector2>();
        if (controllerDirection.magnitude > 0.35f)
        {
            GameManager.Instance.MainGameEvent.Send(new PlayerControllerMovementCommand() { IsSmallMove = true, Direction = controllerDirection });
        }
        else
        {
            GameManager.Instance.MainGameEvent.Send(new PlayerControllerMovementCommand() { IsSmallMove = false, Direction = controllerDirection });
        }
    }
    void OnPlayerRoll()
    {
        if (isAiming_) return;
        GameManager.Instance.MainGameEvent.Send(new PlayerRollingButtonCommand());
        Debug.Log("Roll!");
    }

    void OnCameraControl(InputValue value)
    {
        if (isLaunching_) return;
        var gamepadInput = value.Get<Vector2>();
        var inputX = gamepadInput.x;
        var inputY = -gamepadInput.y;
        if (Mathf.Abs(inputX) < 0.2f)
        {
            inputX = 0;
        }
        if (Mathf.Abs(inputY) < 0.35f)
        {
            inputY = 0;
        }
        var output = new Vector2(inputX, inputY);
        GameManager.Instance.MainGameEvent.Send(new PlayerControllerCameraRotateCommand() { RotateValue = output });
    }

    void OnMouseCameraControl(InputValue value)
    {
        if (isLaunching_) return;
        var mouseInput = value.Get<Vector2>();
        var inputX = Input.GetAxis("Mouse X") * mouse_X_Horrzontal_sensitivity_;
        var inputY = -Input.GetAxis("Mouse Y");
        //Debug.Log(inputX);
        inputX = Mathf.Clamp(inputX, -5, 5);
        inputY = Mathf.Clamp(inputY, -3, 3);
        var output = new Vector2(inputX, inputY);
        GameManager.Instance.MainGameEvent.Send(new PlayerControllerCameraRotateCommand() { RotateValue = output });
    }

    void OnPlayerAim(InputValue value)
    {
        if (value.isPressed == isAiming_ || (isLaunching_) || isPosscessing_ || isAttacking_) return;
        GameManager.Instance.MainGameEvent.Send(new PlayerAimingButtonCommand() { AimingButtonIsPressed = value.isPressed });
        isAiming_ = value.isPressed;
        //Debug.Log("Aim" + isAiming_.ToString());
        if (value.isPressed)
        {
            var delayTimer = 0f;
            aimingDelayer_ = DOTween.To(() => delayTimer, x => delayTimer = x, 1, 0.25f).OnComplete(
                () =>
                {
                    aimingDelay_ = false;
                }
                );
        }
        else
        {
            aimingDelayer_.Kill();
            aimingDelay_ = true;
        }

    }

    void OnPlayerJump()
    {
        if (isAiming_||isAttacking_) return;
        isJumping_ = true;
        GameManager.Instance.MainGameEvent.Send(new PlayerJumpButtonCommand() { });
        Debug.Log("Jump!");
    }


    void OnPlayerLaunch()
    {
        if (!isAiming_ || aimingDelay_ || isLaunching_ || isPosscessing_) return;
        GameManager.Instance.MainGameEvent.Send(new PlayerLaunchGhostButtonCommand() { });
        isLaunching_ = true;
        //GameManager.Instance.MainGameEvent.Send(new PlayerAimingButtonCommand() { AimingButtonIsPressed = false });
        //isAiming_ =false;
    }

    void OnPlayerLightAttack()
    {
        if (isAiming_||isJumping_) return;
        isAttacking_ = true;
        GameManager.Instance.MainGameEvent.Send(new PlayerLightAttackButtonCommand() { });
        Debug.Log("Attack!");
    }

    void OnPlayerPossessCancel()
    {
        if (!isPosscessing_ || isBiting_) return;
        GameManager.Instance.MainGameEvent.Send(new PlayerCancelPossessCommand());
    }

    void finishLaunch()
    {
        isLaunching_ = false;
        GameManager.Instance.MainGameEvent.Send(new PlayerAimingButtonCommand() { AimingButtonIsPressed = false });
        isAiming_ = false;
        aimingDelayer_.Kill();
        aimingDelay_ = true;
        isPosscessing_ = false;
        isBiting_ = false;
    }
    void playerHitObject(PlayerLaunchActionFinishCommand command)
    {
        isLaunching_ = false;
        isPosscessing_ = true;
        if (command.HitObjecctTag == HitObjecctTag.Biteable)
        {
            isBiting_ = true;
        }
    }

    void OnPlayerPossessInteract()
    {
        if (!isPosscessing_) return;
        GameManager.Instance.MainGameEvent.Send(new PlayerControllerPossessableInteractButtonCommand());
    }

    void OnNextPage()
    {
        GameManager.Instance.MainGameEvent.Send(new PlayerEndTutorialCommand());
    }
}

