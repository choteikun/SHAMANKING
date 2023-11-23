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
    public class PlayerHeavyAttackButtonCommand : GameEventMessageBase
    {

    }


    public class PlayerJumpAttackButtonCommand : GameEventMessageBase
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

    public class PlayerThrowAttackFinishCommand:GameEventMessageBase
    {

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

    public class PlayerThrowAttackCallHitBoxCommand : GameEventMessageBase
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
    public class PlayerAttackSuccessResponse:GameEventMessageBase
    {
        public Vector3 CollidePoint;
        public GameObject AttackTarget;
        public float AttackDamage;
        public PlayerAttackSuccessResponse(PlayerAttackSuccessCommand cmd)
        {
            CollidePoint = cmd.CollidePoint;
            AttackTarget = cmd.AttackTarget;
            AttackDamage = cmd.AttackDamage;
        }
    }

    public class PlayerGrabSuccessCommand : GameEventMessageBase
    {
        public Vector3 CollidePoint;
        public GameObject AttackTarget;
        public float AttackDamage;
    }
    public class PlayerGrabSuccessResponse:GameEventMessageBase
    {
        public Vector3 CollidePoint;
        public GameObject AttackTarget;
        public float AttackDamage;

        public PlayerGrabSuccessResponse(PlayerGrabSuccessCommand cmd)
        {
            CollidePoint = cmd.CollidePoint;
            AttackTarget = cmd.AttackTarget; 
            AttackDamage = cmd.AttackDamage;
        }
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

    public class GameConversationEndCommand : GameEventMessageBase
    {

    }

    public class SystemCallTutorialCommand:GameEventMessageBase
    {
        public float TutorialID;
    }

    public class PlayerTutorialNextPageCommand:GameEventMessageBase
    {
        public float TutorialID;
    }

    public class PlayerEndTutorialCommand:GameEventMessageBase
    {
        public int TutorialID;
    }

    public class PlayerThrowAttackCommand : GameEventMessageBase
    {

    }

    public class PlayerTargetButtonTriggerCommand:GameEventMessageBase
    {

    }
    public class SystemGetTarget:GameEventMessageBase
    {
        public GameObject Target;
    }
    public class SystemResetTarget : GameEventMessageBase
    {

    }
    public class StartRollMovementAnimationEvent : GameEventMessageBase
    {

    }

    public class SystemAttackAllowCommand: GameEventMessageBase
    {

    }
    public class AnimationMovementEnableCommand:GameEventMessageBase 
    {

    }
    public class AnimationMovementDisableCommand : GameEventMessageBase
    {

    }

    #region 行為樹給FSM的通知
    public class BT_SwitchStateMessage : GameEventMessageBase
    {
        public int IntTypeStateOfGhostEnemy;
    }
    #endregion

}
