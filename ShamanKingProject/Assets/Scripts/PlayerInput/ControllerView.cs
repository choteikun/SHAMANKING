using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerView : MonoBehaviour
{
    void OnPlayerControl(InputValue value)
    {
        Debug.Log(value.Get<Vector2>());
    }
    void OnPlayerRoll()
    {
        Debug.Log("Roll");
    }
}
