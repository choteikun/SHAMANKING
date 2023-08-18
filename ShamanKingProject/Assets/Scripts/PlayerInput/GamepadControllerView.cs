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
        var inputX = gamepadInput.x;
        var inputY = gamepadInput.y;
        //Debug.Log(gamepadInput);
        if (Mathf.Abs(inputX)<0.2f)
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
}
