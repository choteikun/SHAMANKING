using System.Collections.Generic;
using UnityEngine;

namespace Gamemanager
{
    public class TestInputCommand : GameEventMessageBase
    {
        public int CommandCount;
    }

    public class PlayerControllerMovementCommand : GameEventMessageBase
    {
        public bool IsSmallMove = false;
        public Vector2 Direction;
    }
    
    public class PlayerControllerCameraRotateCommand:GameEventMessageBase
    {
        public Vector2 RotateValue;
    }
    public enum CameraType
    {
        MainCam,
        AimCam,
    }


    public class PlayerAimingButtonCommand : GameEventMessageBase
    {
        public bool AimingButtonIsPressed = false;
    }

    public class PlayerRollingButtonCommand : GameEventMessageBase
    {

    }

    public class PlayerLaunchGhostButtonCommand: GameEventMessageBase 
    {

    }
    
    public class PlayerLaunchFinishCommand: GameEventMessageBase
    {
        public bool Hit = false;
    }

}
