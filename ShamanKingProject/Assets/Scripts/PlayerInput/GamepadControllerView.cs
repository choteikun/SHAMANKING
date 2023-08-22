using Cysharp.Threading.Tasks;
using Gamemanager;
using UnityEngine;
using UnityEngine.InputSystem;

public class GamepadControllerView : MonoBehaviour
{

    [SerializeField] PlayerInput input_;
    private async void Start()
    {
        Debug.Log("start");
        //如果沒有鎖的話 會有暴衝的現象
        await UniTask.Delay(500);
        input_.SwitchCurrentActionMap("MainGameplay");
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
            GameManager.Instance.MainGameEvent.Send(new PlayerControllerMovementCommand() { IsSmallMove = false, Direction = controllerDirection }); ;
        }
    }
    void OnPlayerRoll()
    {
        Debug.Log("Roll");
    }

    void OnCameraControl(InputValue value)
    {
        var gamepadInput = value.Get<Vector2>();
        var inputX = gamepadInput.x;
        var inputY = -gamepadInput.y;
        //Debug.Log(gamepadInput);
        if (Mathf.Abs(inputX) < 0.2f)
        {
            inputX = 0;
        }
        if (Mathf.Abs(inputY) < 0.35f)
        {
            inputY = 0;
        }
        var output = new Vector2(inputX, inputY);
        //Debug.Log(gamepadInput);
        GameManager.Instance.MainGameEvent.Send(new PlayerControllerCameraRotateCommand() { RotateValue = output });
    }

    void OnMouseCameraControl(InputValue value)
    {
        var mouseInput = value.Get<Vector2>();
        var inputX = Input.GetAxis("Mouse X");
        var inputY = -Input.GetAxis("Mouse Y");
        //Debug.Log(mouseInput);
        inputX = Mathf.Clamp(inputX, -3, 3);
        inputY = Mathf.Clamp(inputY, -3, 3);
        //if (Mathf.Abs(inputX) < 0.25f)
        //{
        //    inputX = 0;
        //}
        //if (Mathf.Abs(inputY) < 0.25f)
        //{
        //    inputY = 0;
        //}
        var output = new Vector2(inputX, inputY);
        //Debug.Log(gamepadInput);
        GameManager.Instance.MainGameEvent.Send(new PlayerControllerCameraRotateCommand() { RotateValue = output });
    }
    private void Update()
    {

    }
}
