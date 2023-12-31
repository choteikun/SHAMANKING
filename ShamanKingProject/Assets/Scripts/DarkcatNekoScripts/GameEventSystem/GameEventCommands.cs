using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

    public class PlayerControllerCameraRotateCommand : GameEventMessageBase
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
    public class PlayerChargingButtonCommand : GameEventMessageBase
    {
        public bool ChargingButtonIsPressed = false;
    }
    public class SystemStopChargingCommand: GameEventMessageBase
    {

    }
    public class PlayerRollingButtonCommand : GameEventMessageBase
    {

    }

    public class PlayerJumpButtonCommand : GameEventMessageBase
    {

    }

    public class PlayerLaunchGhostButtonCommand : GameEventMessageBase
    {

    }
    public class PlayerLightAttackButtonCommand : GameEventMessageBase
    {

    }
    public class PlayerHeavyAttackButtonCommand : GameEventMessageBase
    {

    }
    public class PlayerExecutionAttackCommand:GameEventMessageBase
    {

    }
    public class PlayerShootAttackCommand:GameEventMessageBase
    {

    }
    public class PlayerJumpAttackButtonCommand : GameEventMessageBase
    {

    }

    public class PlayerCancelPossessCommand : GameEventMessageBase
    {

    }
    public class PlayerLaunchActionFinishCommand : GameEventMessageBase

    {
        public bool Hit = false;
        public HitObjecctTag HitObjecctTag = HitObjecctTag.None;
        public GameObject HitObjecct;
        public HitableItemTest HitInfo;
    }

    public class PlayerThrowAttackFinishCommand : GameEventMessageBase
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
    public class GhostIdentityCheckCommand : GameEventMessageBase
    {
        public string GhostIdentityName;
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

    public class PlayerAttackCallHitBoxCommand : GameEventMessageBase
    {
        public bool CallOrCancel;
    }

    public class PlayerThrowAttackCallHitBoxCommand : GameEventMessageBase
    {
        public bool CallOrCancel;
    }
    public class SupportAimSystemGetHitableItemCommand : GameEventMessageBase
    {
        public GameObject HitObject;
        public HitableItemTest HitableItemInfo;
    }

    public class SupportAimSystemLeaveHitableItemCommand : GameEventMessageBase
    {
        public GameObject LeaveObject;
    }

    public class PlayerAttackCommand : GameEventMessageBase
    {
        public string AttackName;
    }

    public class PlayerBiteFinishResponse : GameEventMessageBase
    {
        public GameObject HitObject;
        public HitableItemTest HitInfo;
    }

    public class UISoulGageUpdateCommand : GameEventMessageBase
    {

    }
    public class UIPlayerInvincibleUpdateCommand : GameEventMessageBase
    {

    }

    public class PlayerAttackSuccessCommand : GameEventMessageBase
    {
        public Vector3 CollidePoint;
        public GameObject AttackTarget;
        public AttackInputType AttackInputType;
        public float AttackDamage;
        public int AddSoulGage;
        public AttackFeedBackType AttackFeedBackType;
    }
    public enum AttackFeedBackType
    {
        Light,
        Heavy,
        None,
    }

    public class EnemyAttackSuccessCommand : GameEventMessageBase
    {
        public Vector3 CollidePoint;
        public float AttackDamage;
        public EnemyHitPower ThisAttackHitPower;
        public Vector3 AttackerPos;
    }
    public class PlayerAttackSuccessResponse : GameEventMessageBase
    {
        public Vector3 CollidePoint;
        public GameObject AttackTarget;
        public float AttackDamage;
        public int AttackAddSoul;
        public float EnemyHealthPercentage;
        public float EnemyBreakPercentage;
        public PlayerAttackSuccessResponse(PlayerAttackSuccessCommand cmd, float enemyHealthPercentage,float enemyBreakPercentage)
        {
            CollidePoint = cmd.CollidePoint;
            AttackTarget = cmd.AttackTarget;
            AttackDamage = cmd.AttackDamage;
            AttackAddSoul = cmd.AddSoulGage;
            EnemyHealthPercentage = enemyHealthPercentage;
            EnemyBreakPercentage = enemyBreakPercentage;
        }
    }

    public class PlayerGrabSuccessCommand : GameEventMessageBase
    {
        public Vector3 CollidePoint;
        public GameObject AttackTarget;
        public float AttackDamage;
    }
    public class PlayerGrabSuccessResponse : GameEventMessageBase
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
    public class PlayerRootSuccessCommand : GameEventMessageBase 
    {

    }
    public class PlayerControllerPossessableInteractButtonCommand : GameEventMessageBase
    {

    }

    public class PlayerMoveStatusChangeCommand : GameEventMessageBase
    {
        public bool IsMoving;
    }

    public class PlayerJumpTouchGroundCommand : GameEventMessageBase
    {

    }
    public class GameStandingConversationStartCommand:GameEventMessageBase
    {

    }
    public class GameStandingConversationEndCommand:GameEventMessageBase
    {

    }
    public class GameConversationEndCommand : GameEventMessageBase
    {

    }

    public class SystemCallTutorialCommand : GameEventMessageBase
    {
        public float TutorialID;
    }

    public class PlayerTutorialNextPageCommand : GameEventMessageBase
    {
        public float TutorialID;
    }

    public class PlayerEndTutorialCommand : GameEventMessageBase
    {
        public int TutorialID;
    }

    public class PlayerThrowAttackCommand : GameEventMessageBase
    {

    }

    public class PlayerTargetButtonTriggerCommand : GameEventMessageBase
    {

    }
    public class SystemGetTarget : GameEventMessageBase
    {
        public GameObject Target;
    }
    public class SystemResetTarget : GameEventMessageBase
    {

    }
    public class StartRollMovementAnimationEvent : GameEventMessageBase
    {

    }

    public class SystemAttackAllowCommand : GameEventMessageBase
    {

    }
    public class AnimationMovementEnableCommand : GameEventMessageBase
    {

    }
    public class AnimationMovementDisableCommand : GameEventMessageBase
    {

    }
    public class AnimationMovementEventCommand : GameEventMessageBase
    {
        public float Distance;
        public int Frame;
    }

    public class AnimationCallAttackEffectCommand : GameEventMessageBase
    {
        public int ColliderId;
        public int SpawnEffectId;
        public GameObject CommandSender;
        public AttackColliderType AttackColliderType;
    }
    public class AnimationCallRepeatShootAttackCommand:GameEventMessageBase
    {
        public int ColliderId;
        public int SpawnEffectId;
        public GameObject CommandSender;
        public AttackColliderType AttackColliderType;
    }
    public enum AttackColliderType
    {
        Player,
        Monster,
    }

    public class PlayerBeAttackByEnemySuccessResponse : GameEventMessageBase
    {

    }

    public class SystemCallFireballLocateCommand : GameEventMessageBase
    {

    }

    public class SystemCallFireballSpawnCommand : GameEventMessageBase
    {

    }
    public class SystemCallFireTrackBallSpawnCommand : GameEventMessageBase
    {

    }
    public class SystemCallFirstSceneCameraTransferCommand:GameEventMessageBase
    {
        public float CameraId;
    }
    public class SystemCallCameraTransferBackCommand:GameEventMessageBase
    {

    }
    public class SystemCallWaveStartCommand:GameEventMessageBase
    {
        public string SceneName;
        public int WaveID;
    }
    public class BossCurAnimationEndCommand : GameEventMessageBase 
    {

    }
    public class BossCallSprintColliderSwitchCommand:GameEventMessageBase
    {
        public bool OnOrOff;
    }

    public class BossCallJumpAttackLocateCommand:GameEventMessageBase
    {

    }

    public class BossCallFlameThrowerSwitchCommand:GameEventMessageBase
    {
        public bool TurnedOn;
    }
    public class UICallPlayerHealthBarUIUpdateCommand:GameEventMessageBase
    {

    }
    public class UIUpdateBreakCommand:GameEventMessageBase
    {
        public GameObject AttackTarget;
        public float BreakPercentage;
    }


    #region 行為樹給FSM的通知
    public class BT_SwitchStateMessage : GameEventMessageBase
    {
        public int IntTypeStateOfGhostEnemy;
    }
    #endregion

}
