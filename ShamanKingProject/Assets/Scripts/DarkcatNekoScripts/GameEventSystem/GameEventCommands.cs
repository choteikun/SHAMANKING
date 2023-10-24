using System.Collections.Generic;
using TMPro;
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
    public class PlayerCancelPossessCommand:GameEventMessageBase
    {

    }
    public class PlayerLaunchActionFinishCommand: GameEventMessageBase

    {
        public bool Hit = false;
        public HitObjecctTag HitObjecctTag = HitObjecctTag.None;
        public GameObject HitObjecct;
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
    public class PlayerMovementInterruptionFinishCommand : GameEventMessageBase
    {

    }

    public class PlayerAttackCallHitBoxCommand:GameEventMessageBase 
    {
        public bool CallOrCancel;
    }

    public class SupportAimSystemGetHitableItemCommand:GameEventMessageBase
    {
        public GameObject HitObject;
        public HitableItemTest HitableItemInfo;
    }

    public class SupportAimSystemLeaveHitableItemCommand:GameEventMessageBase
    {
        public GameObject LeaveObject;
    }

    public class PlayerAttackCommand:GameEventMessageBase
    {
        public string AttackName;
    }

    public class PlayerBiteFinishResponse : GameEventMessageBase
    {
        public GameObject HitObject;
        public HitableItemTest HitInfo;
    }

    public class UISpiritUpdateCommand:GameEventMessageBase
    {

    }

    public class PlayerAttackSuccessCommand:GameEventMessageBase
    {
        public Vector3 CollidePoint;
        public GameObject AttackTarget;
        public float AttackDamage;
    }

    public class PlayerControllerPossessableInteractButtonCommand:GameEventMessageBase
    {

    }

    public class PlayerMoveStatusChangeCommand:GameEventMessageBase
    {
        public bool IsMoving;
    }

    public class PlayerJumpTouchGroundCommand:GameEventMessageBase
    {
        
    }
}
