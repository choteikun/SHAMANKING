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
}
