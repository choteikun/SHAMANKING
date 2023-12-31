using Gamemanager;
using System;
using System.Diagnostics.Contracts;

public class MainGameEventPack : GameEventPack
{
    public IObservable<TestInputCommand> OnTestInput => getSubject<TestInputCommand>();

    /// <summary>
    /// 手把輸入的角色移動
    /// </summary>
    public IObservable<PlayerControllerMovementCommand> OnPlayerControllerMovement => getSubject<PlayerControllerMovementCommand>();

    /// <summary>
    /// 手把輸入的角色鏡頭移動
    /// </summary>
    public IObservable<PlayerControllerCameraRotateCommand> OnPlayerCameraRotate => getSubject<PlayerControllerCameraRotateCommand>();

    /// <summary>
    /// 按下與放開瞄準鏡的瞬間
    /// </summary>
    public IObservable<PlayerAimingButtonCommand> OnAimingButtonTrigger => getSubject<PlayerAimingButtonCommand>();
    public IObservable<PlayerChargingButtonCommand> OnPlayerChargingButtonTrigger => getSubject<PlayerChargingButtonCommand>();

    /// <summary>
    /// 手把輸入角色翻滾
    /// </summary>
    public IObservable<PlayerRollingButtonCommand> OnPlayerRoll => getSubject<PlayerRollingButtonCommand>();

    /// <summary>
    /// 手把輸入發射鍵
    /// </summary>
    public IObservable<PlayerLaunchGhostButtonCommand> OnPlayerLaunchGhost => getSubject<PlayerLaunchGhostButtonCommand>();

    /// <summary>
    /// 角色的發射到達終點，回傳角色是否撞到東西，與角色撞到的東西
    /// </summary>
    public IObservable<PlayerLaunchActionFinishCommand> OnPlayerLaunchActionFinish => getSubject<PlayerLaunchActionFinishCommand>();

    /// <summary>
    /// 手把輸入角色跳躍
    /// </summary>
    public IObservable<PlayerJumpButtonCommand> OnPlayerJump => getSubject<PlayerJumpButtonCommand>();

    /// <summary>
    /// 手把輸入角色輕攻擊鍵
    /// </summary>
    public IObservable<PlayerLightAttackButtonCommand> OnPlayerLightAttack => getSubject<PlayerLightAttackButtonCommand>();

    public IObservable<PlayerHeavyAttackButtonCommand> OnPlayerHeavyAttack => getSubject<PlayerHeavyAttackButtonCommand>();

    public IObservable<PlayerExecutionAttackCommand> OnPlayerExecutionAttack => getSubject<PlayerExecutionAttackCommand>();

    public IObservable<PlayerShootAttackCommand> OnPlayerShootAttack => getSubject<PlayerShootAttackCommand>();

    public IObservable<PlayerJumpAttackButtonCommand> OnPlayerJumpAttack => getSubject<PlayerJumpAttackButtonCommand>();

    /// <summary>
    /// 幽靈發出的動畫事件
    /// </summary>
    public IObservable<GhostAnimationEventsCommand> OnGhostAnimationEvents => getSubject<GhostAnimationEventsCommand>();

    /// <summary>
    /// 手把輸入角色取消附身
    /// </summary>
    public IObservable<PlayerCancelPossessCommand> OnPlayerCancelPossess => getSubject<PlayerCancelPossessCommand>();

    /// <summary>
    /// 少女發出的動畫事件
    /// </summary>
    public IObservable<PlayerAnimationEventsCommand> OnPlayerAnimationEvents => getSubject<PlayerAnimationEventsCommand>();

    public IObservable<AnimationCallRepeatShootAttackCommand> OnAnimationCallRepeatShootAttack => getSubject<AnimationCallRepeatShootAttackCommand>();

    /// <summary>
    /// 幽靈的發射事件完整結束時發出的信號
    /// </summary>
    public IObservable<GhostLaunchProcessFinishResponse> OnGhostLaunchProcessFinish => getSubject<GhostLaunchProcessFinishResponse>();

    /// <summary>
    /// 角色不可移動狀態結束指令
    /// </summary>
    public IObservable<PlayerMovementInterruptionFinishCommand> OnPlayerMovementInterruptionFinish => getSubject<PlayerMovementInterruptionFinishCommand>();

    /// <summary>
    /// 角色呼叫攻擊碰撞體指令
    /// </summary>
    public IObservable<PlayerAttackCallHitBoxCommand> OnPlayerAttackCallHitBox => getSubject<PlayerAttackCallHitBoxCommand>();

    public IObservable<PlayerGrabSuccessCommand> OnPlayerGrabSuccess => getSubject<PlayerGrabSuccessCommand>();

    public IObservable<PlayerGrabSuccessResponse> OnPlayerGrabSuccessForPlayer => getSubject<PlayerGrabSuccessResponse>();

    public IObservable<PlayerThrowAttackCallHitBoxCommand> OnPlayerThrowAttackCallHitBox => getSubject<PlayerThrowAttackCallHitBoxCommand>();

    /// <summary>
    /// 輔助瞄準系統進入可打物品
    /// </summary>
    public IObservable<SupportAimSystemGetHitableItemCommand> OnSupportAimSystemGetHitableItem => getSubject<SupportAimSystemGetHitableItemCommand>();

    /// <summary>
    /// 輔助瞄準系統出可打物品
    /// </summary>
    public IObservable<SupportAimSystemLeaveHitableItemCommand> OnSupportAimSystemLeaveHitableItem => getSubject<SupportAimSystemLeaveHitableItemCommand>();


    /// <summary>
    /// 玩家確認打出甚麼攻擊
    /// </summary>
    public IObservable<PlayerAttackCommand> OnPlayerAttack => getSubject<PlayerAttackCommand>();

    /// <summary>
    /// 玩家結束咬的事件 要與物件與角色數值做結算
    /// </summary>
    public IObservable<PlayerBiteFinishResponse> OnPlayerBiteFinish => getSubject<PlayerBiteFinishResponse>();

    public IObservable<PlayerAttackSuccessCommand> OnPlayerAttackSuccess => getSubject<PlayerAttackSuccessCommand>();
    public IObservable<PlayerAttackSuccessResponse> OnPlayerAttackSuccessForData => getSubject<PlayerAttackSuccessResponse>();

    public IObservable<PlayerControllerPossessableInteractButtonCommand> OnPlayerControllerPossessableInteract => getSubject<PlayerControllerPossessableInteractButtonCommand>();

    public IObservable<PlayerMoveStatusChangeCommand> OnPlayerMoveStatusChange => getSubject<PlayerMoveStatusChangeCommand>();

    public IObservable<PlayerJumpTouchGroundCommand> OnPlayerJumpTouchGround => getSubject<PlayerJumpTouchGroundCommand>();

    public IObservable<GameConversationEndCommand> OnGameConversationEnd => getSubject<GameConversationEndCommand>();

    public IObservable<SystemCallTutorialCommand> OnSystemCallTutorial => getSubject<SystemCallTutorialCommand>();

    public IObservable<PlayerEndTutorialCommand> OnPlayerEndTutorial => getSubject<PlayerEndTutorialCommand>();

    public IObservable<PlayerTutorialNextPageCommand> OnPlayerTutorialNextPage => getSubject<PlayerTutorialNextPageCommand>();

    public IObservable<PlayerThrowAttackCommand> OnPlayerThrowAttack => getSubject<PlayerThrowAttackCommand>();

    public IObservable<PlayerThrowAttackFinishCommand> OnPlayerThrowAttackFinish => getSubject<PlayerThrowAttackFinishCommand>();

    public IObservable<SystemAttackAllowCommand> OnSystemAttackAllow => getSubject<SystemAttackAllowCommand>();

    public IObservable<AnimationMovementEnableCommand> OnAnimationMovementEnable => getSubject<AnimationMovementEnableCommand>();
    public IObservable<AnimationMovementDisableCommand> OnAnimationMovementDisable => getSubject<AnimationMovementDisableCommand>();

    public IObservable<AnimationMovementEventCommand> OnAnimationMovementEvent => getSubject<AnimationMovementEventCommand>();
    public IObservable<PlayerTargetButtonTriggerCommand> OnPlayerTargetButtonTrigger => getSubject<PlayerTargetButtonTriggerCommand>();
    public IObservable<SystemGetTarget> OnSystemGetTarget => getSubject<SystemGetTarget>();
    public IObservable<SystemResetTarget> OnSystemResetTarget => getSubject<SystemResetTarget>();

    public IObservable<StartRollMovementAnimationEvent> OnStartRollMovementAnimation => getSubject<StartRollMovementAnimationEvent>();

    public IObservable<AnimationCallAttackEffectCommand> OnAnimationCallAttackEffect => getSubject<AnimationCallAttackEffectCommand>();

    public IObservable<EnemyAttackSuccessCommand> OnEnemyAttackSuccess => getSubject<EnemyAttackSuccessCommand>();

    public IObservable<PlayerBeAttackByEnemySuccessResponse> OnPlayerBeAttackByEnemySuccess => getSubject<PlayerBeAttackByEnemySuccessResponse>();

    public IObservable<PlayerRootSuccessCommand> OnPlayerRootSuccess => getSubject<PlayerRootSuccessCommand>();

    public IObservable<GameStandingConversationStartCommand> OnGameStandingConversationStart => getSubject<GameStandingConversationStartCommand>();
    public IObservable<GameStandingConversationEndCommand> OnGameStandingConversationEnd => getSubject<GameStandingConversationEndCommand>();

    public IObservable<SystemCallFirstSceneCameraTransferCommand> OnSystemCallFirstSceneCameraTransfer => getSubject<SystemCallFirstSceneCameraTransferCommand>();

    public IObservable<SystemCallCameraTransferBackCommand> OnSystemCallCameraTransferBack => getSubject<SystemCallCameraTransferBackCommand>();
    public IObservable<GhostIdentityCheckCommand> OnGhostIdentityCheck => getSubject<GhostIdentityCheckCommand>();
    public IObservable<SystemCallWaveStartCommand> OnSystemCallWaveStart => getSubject<SystemCallWaveStartCommand>();
    public IObservable<BossCurAnimationEndCommand> OnBossCurAnimationEnd => getSubject<BossCurAnimationEndCommand>();
    public IObservable<SystemStopChargingCommand> OnSystemStopCharging => getSubject<SystemStopChargingCommand>();
    public IObservable<BossCallSprintColliderSwitchCommand> OnBossCallSprintColliderSwitch => getSubject<BossCallSprintColliderSwitchCommand>();
}
