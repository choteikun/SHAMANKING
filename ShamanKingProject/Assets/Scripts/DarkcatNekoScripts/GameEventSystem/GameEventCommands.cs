using System.Collections.Generic;
using UnityEngine;

namespace Gamemanager
{
    public class TestInputCommand : GameEventMessageBase
    {
        public int CommandCount;
    }

    /// <summary>
    /// 玩家手把操作移動
    /// </summary>
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

    

}
