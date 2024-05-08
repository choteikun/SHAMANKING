using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.XInput;
using Gamemanager;

public class ControllerSwitcher : MonoBehaviour
{
    [SerializeField] NowGameplayType gameplayType_ = NowGameplayType.Keyboard;
    // Update is called once per frame
    void Update()
    {
        foreach (var gamepad in Gamepad.all)
        {
            foreach (var control in gamepad.allControls)
            {
                if (control.IsPressed())
                {
                    if (gamepad is XInputController)
                    {
                        if (gameplayType_!=NowGameplayType.XBox)
                        {
                            gameplayType_ = NowGameplayType.XBox;
                            GameManager.Instance.MainGameEvent.Send(new SystemCallInputTypeChangeCommand() { GameplayType = gameplayType_ });
                        }
                    }
                    if (gamepad is DualShockGamepad)
                    {
                        if (gameplayType_ != NowGameplayType.PlayStation)
                        {
                            gameplayType_ = NowGameplayType.PlayStation;
                            GameManager.Instance.MainGameEvent.Send(new SystemCallInputTypeChangeCommand() { GameplayType = gameplayType_ });
                        }
                    }
                }
            }
        }     
        if (Input.anyKeyDown && !AnyJoystickButtonPressed())
        {
            if (gameplayType_ != NowGameplayType.Keyboard)
            {
                gameplayType_ = NowGameplayType.Keyboard;
                GameManager.Instance.MainGameEvent.Send(new SystemCallInputTypeChangeCommand() { GameplayType = gameplayType_ });
            }
        }
    }
    bool AnyJoystickButtonPressed()
    {
        // 检查手柄按钮是否被按下
        for (int i = 0; i < 20; i++) // 假设手柄最多有20个按钮
        {
            if (Input.GetKey(KeyCode.JoystickButton0 + i))
            {
                return true;
            }
        }
        return false;
    }
}

public enum NowGameplayType
{
    PlayStation,
    XBox,
    Keyboard,
}

