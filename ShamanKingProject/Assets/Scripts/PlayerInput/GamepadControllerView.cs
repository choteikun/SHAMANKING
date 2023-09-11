using Cysharp.Threading.Tasks;
using Gamemanager;
using Language.Lua;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public class GamepadControllerView : MonoBehaviour
{

    [SerializeField] PlayerInput input_;
    [SerializeField] float mouse_X_Horrzontal_sensitivity_ = 1.2f;

    [SerializeField] bool isAiming_;
    [SerializeField] bool isLaunching_;
    private async void Start()
    {
        Debug.Log("start");
        //�p�G�S���ꪺ�� �|���ɽĪ��{�H
        await UniTask.Delay(500);
        input_.SwitchCurrentActionMap("MainGameplay");
    }
    void changeInpuMap(string map)
    {
        input_.SwitchCurrentActionMap(map);
    }
    void OnPlayerControl(InputValue value)
    {
        if (isLaunching_) return;
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
        GameManager.Instance.MainGameEvent.Send(new PlayerRollingButtonCommand());
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
        if (value.isPressed == isAiming_ || (isLaunching_)) return;
        GameManager.Instance.MainGameEvent.Send(new PlayerAimingButtonCommand() { AimingButtonIsPressed = value.isPressed });
        isAiming_ = value.isPressed;
        Debug.Log("Aim" + isAiming_.ToString());
    }    

    

    void OnPlayerLaunch()
    {
        if (!isAiming_) return;
        GameManager.Instance.MainGameEvent.Send(new PlayerLaunchGhostButtonCommand() { });
        isLaunching_ = true;
        //GameManager.Instance.MainGameEvent.Send(new PlayerAimingButtonCommand() { AimingButtonIsPressed = false });
        //isAiming_ =false;
    }
}
