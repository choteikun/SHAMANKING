using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamemanager;

public class TestInputView : MonoBehaviour
{   
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            GameManager.Instance.MainGameEvent.Send(new TestInputCommand() { CommandCount = 0 });
        }
    }
}
