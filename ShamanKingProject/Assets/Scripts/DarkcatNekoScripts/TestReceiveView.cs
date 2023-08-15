using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamemanager;
using UniRx;
using System;

public class TestReceiveView : MonoBehaviour
{
    void Start()
    {
        GameManager.Instance.MainGameEvent.OnTestInput.Subscribe(debugTest);
    }

    void debugTest(TestInputCommand command)
    {
        Debug.Log(command.CommandCount);
    }
}
