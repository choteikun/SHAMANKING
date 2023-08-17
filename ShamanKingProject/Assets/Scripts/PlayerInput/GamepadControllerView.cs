using AmplifyShaderEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Gamemanager;

public class GamepadControllerView : MonoBehaviour
{
    void OnPlayerControl(InputValue value)
    {
        var controllerDirection = value.Get<Vector2>();
        Debug.Log(controllerDirection);
        if (controllerDirection.magnitude>0.35f)
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
        GameManager.Instance.MainGameEvent.Send(new PlayerControllerCameraRotateCommand() { RotateValue = gamepadInput });
    }
}
