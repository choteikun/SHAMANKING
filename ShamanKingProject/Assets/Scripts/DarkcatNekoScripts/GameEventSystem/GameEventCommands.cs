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

    public class PlayerJumpButtonCommand:GameEventMessageBase
    {

    }

    public class PlayerLaunchGhostButtonCommand: GameEventMessageBase 
    {

    }
    public class PlayerLightAttackButtonCommand : GameEventMessageBase
    {

    }
    public class PlayerLaunchActionFinishCommand: GameEventMessageBase

    {
        public bool Hit = false;
        public HitObjecctTag HitObjecctTag = HitObjecctTag.None;
        public HitableItemTest HitInfo;
    }

    public enum HitObjecctTag
    {
        None,
        Biteable,
        Possessable,
        Enemy,
    }


    public class GhostAnimationEventsCommand : GameEventMessageBase
    {
        public string AnimationEventName;
        public GhostAnimationType AnimationType;
    }
    public class PlayerAnimationEventsCommand : GameEventMessageBase
    {
        public string AnimationEventName;
    }
    public class GhostLaunchProcessFinishResponse : GameEventMessageBase
    {

    }
}
