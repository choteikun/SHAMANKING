using Cysharp.Threading.Tasks;
using DG.Tweening;
using Gamemanager;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
//using UnityEngine.InputSystem.Editor;

public class GamepadControllerView : MonoBehaviour
{
    [SerializeField] bool heavyAttackCharger_ = false;
    [SerializeField] bool lightAttackCharger_ = false;
    [SerializeField] bool heavyButtonCharging_;
    [SerializeField] bool lightButtonCharging_;
    [SerializeField] PlayerInput input_;
    [SerializeField] float mouse_X_Horrzontal_sensitivity_ = 1.2f;

    [SerializeField] bool isAiming_;
    [SerializeField] bool isGuarding_;
    [SerializeField] bool isLaunching_;
    [SerializeField] bool isPosscessing_ = false;
    [SerializeField] bool isBiting_ = false;
    [SerializeField] bool isAttacking_ = false;
    [SerializeField] bool isJumping_ = false;
    [SerializeField] bool canRevive_ = false;

    [SerializeField] bool aimingDelay_ = true;
    Tweener aimingDelayer_;
    [SerializeField] bool buttonEastPressed_ = false;

    [Header("Dubug Use")]
    [SerializeField] bool isDebuging_;

    int nowTutorial_ = 0;

    private async void Awake()
    {
        //var data = GameContainer.Get<DataManager>();//建置的時候記得刪掉
        //await data.InitDataMananger();
        //if (isDebuging_) input_.SwitchCurrentActionMap("MainGameplay");
        //if (GameManager.Instance.MainGameMediator.RealTimePlayerData.PlayerNowCheckPoint!=-1) input_.SwitchCurrentActionMap("MainGameplay");
        Debug.Log("start");
        //await UniTask.Delay(500);
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerPressExecutionButtonResponse, cmd => { ExecutionAttackChecked(); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnCutSceneOverStartControl, cmd => { input_.SwitchCurrentActionMap("MainGameplay"); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnSystemStopGuarding, cmd => { if (isGuarding_) isAiming_ = false; isGuarding_ = false; GameManager.Instance.MainGameEvent.Send(new PlayerGuardingButtonCommand() { GuardingButtonIsPressed = isGuarding_ }); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnGhostLaunchProcessFinish, cmd => { finishLaunch(); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnEnemyAttackSuccess, cmd => { isAttacking_ = true; });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerMovementInterruptionFinish, cmd => { isAttacking_ = false; Debug.Log("finishSend"); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerJumpTouchGround, cmd => { isJumping_ = false; });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnGameStandingConversationStart, cmd => { input_.SwitchCurrentActionMap("Freeze"); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnGameConversationEnd, cmd => { input_.SwitchCurrentActionMap("MainGameplay"); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnGameStandingConversationEnd, cmd => { input_.SwitchCurrentActionMap("MainGameplay"); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnSystemCallTutorial, cmd => { input_.SwitchCurrentActionMap("PlayerTutorialInput"); nowTutorial_ = (int)cmd.TutorialID; });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnSystemCallPlayerGameover, cmd => { input_.SwitchCurrentActionMap("PlayerGameOverUIInput"); });
        GameManager.Instance.HellDogGameEvent.SetSubscribe(GameManager.Instance.HellDogGameEvent.OnBossCallDeadCommand, cm => { reviveDelayer(); input_.SwitchCurrentActionMap("PlayerGameOverUIInput"); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerEndTutorial, cmd => { input_.SwitchCurrentActionMap("MainGameplay"); });
        var onLaunchHitPosscessableItem = GameManager.Instance.MainGameEvent.OnPlayerLaunchActionFinish.Where(cmd => cmd.Hit && (cmd.HitObjecctTag == HitObjecctTag.Possessable || cmd.HitObjecctTag == HitObjecctTag.Biteable)).Subscribe(cmd => { playerHitObject(cmd); });
        GameManager.Instance.MainGameMediator.AddToDisposables(onLaunchHitPosscessableItem);
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnSystemCallPlayerGameover, cmd => { reviveDelayer(); });
    }
    //private void Update()
    //{
    //    if (inputActionMap_.FindAction("PlayerControl").WasPressedThisFrame())
    //    {
    //        Debug.Log("1");
    //        if (isAttacking_) return;
    //        var controllerDirection = inputActionMap_.FindAction("PlayerControl").ReadValue<Vector2>();
    //        if (controllerDirection.magnitude > 0.35f)
    //        {
    //            GameManager.Instance.MainGameEvent.Send(new PlayerControllerMovementCommand() { IsSmallMove = true, Direction = controllerDirection });
    //        }
    //        else
    //        {
    //            GameManager.Instance.MainGameEvent.Send(new PlayerControllerMovementCommand() { IsSmallMove = false, Direction = controllerDirection });
    //        }
    //    }

    //}
    void changeInpuMap(string map)
    {
        input_.SwitchCurrentActionMap(map);
    }
    void OnPlayerControl(InputValue value)
    {
        var controllerDirection = value.Get<Vector2>();
        if (controllerDirection.magnitude > 0.35f)
        {
            GameManager.Instance.MainGameEvent.Send(new PlayerControllerMovementCommand() { IsSmallMove = true, Direction = controllerDirection });
        }
        else
        {
            GameManager.Instance.MainGameEvent.Send(new PlayerControllerMovementCommand() { IsSmallMove = false, Direction = controllerDirection });
        }
    }
    void OnPlayerRoll()
    {
        //if (isAiming_) return;
        if (heavyButtonCharging_)
        {
            playerCancelHeavyCharge();
        }
        if (lightButtonCharging_)
        {
            playerCancelLightCharge();
        }
        GameManager.Instance.MainGameEvent.Send(new PlayerRollingButtonCommand());
        Debug.Log("Roll!");
    }

    void OnCameraControl(InputValue value)
    {
        if (isLaunching_) return;
        var gamepadInput = value.Get<Vector2>();
        var inputX = gamepadInput.x;
        var inputY = -gamepadInput.y;
        if (Mathf.Abs(inputX) < 0.2f)
        {
            inputX = 0;
        }
        if (Mathf.Abs(inputY) < 0.35f)
        {
            inputY = 0;
        }
        var output = new Vector2(inputX, inputY);
        GameManager.Instance.MainGameEvent.Send(new PlayerControllerCameraRotateCommand() { RotateValue = output });
    }

    void OnMouseCameraControl(InputValue value)
    {
        if (isLaunching_) return;
        var mouseInput = value.Get<Vector2>();
        var inputX = Input.GetAxis("Mouse X") * mouse_X_Horrzontal_sensitivity_;
        var inputY = -Input.GetAxis("Mouse Y");
        //Debug.Log(inputX);
        inputX = Mathf.Clamp(inputX, -5, 5);
        inputY = Mathf.Clamp(inputY, -3, 3);
        var output = new Vector2(inputX, inputY);
        GameManager.Instance.MainGameEvent.Send(new PlayerControllerCameraRotateCommand() { RotateValue = output });
    }

    //void OnPlayerAim(InputValue value)
    //{
    //    if (value.isPressed == isAiming_ || (isLaunching_) || isPosscessing_ || isAttacking_) return;
    //    GameManager.Instance.MainGameEvent.Send(new PlayerAimingButtonCommand() { AimingButtonIsPressed = value.isPressed });
    //    isAiming_ = value.isPressed;
    //    //Debug.Log("Aim" + isAiming_.ToString());
    //    if (value.isPressed)
    //    {
    //        var delayTimer = 0f;
    //        aimingDelayer_ = DOTween.To(() => delayTimer, x => delayTimer = x, 1, 0.25f).OnComplete(
    //            () =>
    //            {
    //                aimingDelay_ = false;
    //            }
    //            );
    //    }
    //    else
    //    {
    //        aimingDelayer_.Kill();
    //        aimingDelay_ = true;
    //    }

    //}
    //void OnPlayerCharge(InputValue value)
    //{
    //    if (isJumping_ || (isLaunching_) || isPosscessing_ || isAttacking_) return;
    //    if (GameManager.Instance.MainGameMediator.RealTimePlayerData.GhostSoulGageCurrentAmount == GameManager.Instance.MainGameMediator.RealTimePlayerData.GhostSoulGageMaxAmount) return;
    //    isCharging_ = !isCharging_; 
    //    isAiming_ = !isAiming_;
    //    GameManager.Instance.MainGameEvent.Send(new PlayerChargingButtonCommand() { ChargingButtonIsPressed = isCharging_ });
    //}
    void OnPlayerJump()
    {
        if (isAiming_ || isAttacking_) return;
        isJumping_ = true;
        GameManager.Instance.MainGameEvent.Send(new PlayerJumpButtonCommand() { });
        Debug.Log("Jump!");
    }


    //void OnPlayerLaunch()
    //{
    //    if (!isAiming_ || aimingDelay_ || isLaunching_ || isPosscessing_) return;
    //    GameManager.Instance.MainGameEvent.Send(new PlayerLaunchGhostButtonCommand() { });
    //    isLaunching_ = true;
    //    //GameManager.Instance.MainGameEvent.Send(new PlayerAimingButtonCommand() { AimingButtonIsPressed = false });
    //    //isAiming_ =false;
    //}

    async void OnPlayerLightAttack()
    {
        if (isAiming_) return;
        if (isGuarding_)
        {
            GameManager.Instance.MainGameEvent.Send(new SystemStopGuardingCommand());
        }
        await UniTask.DelayFrame(1);
        if (buttonEastPressed_) return;
        if (heavyButtonCharging_)
        {
            playerCancelHeavyCharge();
        }
        if (isJumping_)
        {
            //isAttacking_ = true;
            //GameManager.Instance.MainGameEvent.Send(new PlayerJumpAttackButtonCommand() { });
        }
        else
        {
            isAttacking_ = true;
            GameManager.Instance.MainGameEvent.Send(new PlayerLightAttackButtonCommand() { Charged = lightAttackCharger_ });
            lightAttackCharger_ = false;
        }
        Debug.Log("Attack!");
    }
    void OnPlayerHeavyAttackCharge()
    {
        if (heavyButtonCharging_ == false) return;
        heavyAttackCharger_ = true;
        GameManager.Instance.UIGameEvent.Send(new DebugUIHeavyAttackCharge());
        GameManager.Instance.MainGameEvent.Send(new GameCallSoundEffectGenerate() { SoundEffectID = 4 });
        GameManager.Instance.MainGameEvent.Send(new PlayerHeavyAttackChargeFinishCommand());
        Debug.Log("PlayerHeavyAttackCharge!");
    }
    void OnPlayerTutorialHeavyAttackCharge()
    {
        if (heavyButtonCharging_ == false) return;
        heavyAttackCharger_ = true;
        GameManager.Instance.UIGameEvent.Send(new DebugUIHeavyAttackCharge());
        GameManager.Instance.MainGameEvent.Send(new GameCallSoundEffectGenerate() { SoundEffectID = 4 });
        GameManager.Instance.MainGameEvent.Send(new PlayerHeavyAttackChargeFinishCommand());
        Debug.Log("PlayerHeavyAttackCharge!");
    }
    void OnPlayerLightAttackCharge()
    {
        if (lightButtonCharging_ == false) return;
        lightAttackCharger_ = true;
    }
    async void OnPlayerHeavyAttack()
    {
        if (isGuarding_)
        {
            GameManager.Instance.MainGameEvent.Send(new SystemStopGuardingCommand());
        }
        if (isAiming_ || isJumping_) return;
        await UniTask.DelayFrame(1);
        if (lightButtonCharging_)
        {
            playerCancelLightCharge();
        }
        isAttacking_ = true;
        GameManager.Instance.MainGameEvent.Send(new PlayerHeavyAttackButtonCommand() { Charged = heavyAttackCharger_ });
        Debug.Log("HeavyAttack!" + heavyAttackCharger_);
        heavyAttackCharger_ = false;
    }
    async void OnPlayerTutorialHeavyAttack()
    {
        if (isGuarding_)
        {
            GameManager.Instance.MainGameEvent.Send(new SystemStopGuardingCommand());
        }
        if (isAiming_ || isJumping_) return;
        await UniTask.DelayFrame(1);
        if (lightButtonCharging_)
        {
            playerCancelLightCharge();
        }
        if (heavyAttackCharger_)
        {
            isAttacking_ = true;
            GameManager.Instance.MainGameEvent.Send(new PlayerHeavyAttackButtonCommand() { Charged = heavyAttackCharger_ });
            Debug.Log("HeavyAttack!" + heavyAttackCharger_);
            input_.SwitchCurrentActionMap("MainGameplay");
            heavyAttackCharger_ = false;
        }
    }
    async void OnPlayerUltimate()
    {
        if (isGuarding_)
        {
            GameManager.Instance.MainGameEvent.Send(new SystemStopGuardingCommand());
        }
        if (GameManager.Instance.MainGameMediator.RealTimePlayerData.GhostNowGageBlockAmount < 2)
        {
            return;
        }
        if (isAiming_ || isJumping_) return;
        await UniTask.DelayFrame(1);
        isAttacking_ = true;
        GameManager.Instance.MainGameEvent.Send(new PlayerUltimateAttackCommand() { });
        Debug.Log("Attack!");
    }
    void OnPlayerExecutionAttack()
    {
        GameManager.Instance.MainGameEvent.Send(new PlayerPressExecutionButtonCommand());
    }

    async void ExecutionAttackChecked()
    {
        if (isGuarding_)
        {
            GameManager.Instance.MainGameEvent.Send(new SystemStopGuardingCommand());
        }
        if (isAiming_ || isJumping_) return;
        await UniTask.DelayFrame(1);
        isAttacking_ = true;
        GameManager.Instance.MainGameEvent.Send(new PlayerExecutionAttackCommand() { });
        Debug.Log("Attack!");
    }

    void OnPlayerPossessCancel()
    {
        if (!isPosscessing_ || isBiting_) return;
        GameManager.Instance.MainGameEvent.Send(new PlayerCancelPossessCommand());
    }

    void finishLaunch()
    {
        isLaunching_ = false;
        GameManager.Instance.MainGameEvent.Send(new PlayerAimingButtonCommand() { AimingButtonIsPressed = false });
        isAiming_ = false;
        aimingDelayer_.Kill();
        aimingDelay_ = true;
        isPosscessing_ = false;
        isBiting_ = false;
    }
    void playerHitObject(PlayerLaunchActionFinishCommand command)
    {
        isLaunching_ = false;
        isPosscessing_ = true;
        if (command.HitObjecctTag == HitObjecctTag.Biteable)
        {
            isBiting_ = true;
        }
    }

    void OnPlayerInteract(InputValue value)
    {
        buttonEastPressed_ = value.isPressed;
        //if (!isPosscessing_) return;
        if (value.isPressed)
        {
            GameManager.Instance.MainGameEvent.Send(new PlayerControllerInteractButtonCommand());
        }
    }

    void OnNextPage()
    {
        GameManager.Instance.MainGameEvent.Send(new PlayerTutorialNextPageCommand() { TutorialID = nowTutorial_ });
    }

    void OnPlayerSkip()
    {
        Debug.Log("Hold!!");
        GameManager.Instance.MainGameEvent.Send(new PlayerSkipConversationCommand());
    }

    void OnThrowAttack()
    {
        if (isGuarding_)
        {
            GameManager.Instance.MainGameEvent.Send(new SystemStopGuardingCommand());
        }
        if (isAiming_) return;
        if (isJumping_)
        {
            return;
        }
        Debug.Log("OnThrowAttack");
        isAttacking_ = true;
        GameManager.Instance.MainGameEvent.Send(new PlayerThrowAttackCommand());
    }

    void OnTargetModeSwitch()
    {
        if (isAiming_) return;
        GameManager.Instance.MainGameEvent.Send(new PlayerTargetButtonTriggerCommand());
    }

    //async void OnPlayerShootAttack()
    //{
    //    if (isAiming_ || isJumping_) return;
    //    await UniTask.DelayFrame(1);
    //    isAttacking_ = true;
    //    GameManager.Instance.MainGameEvent.Send(new PlayerShootAttackCommand() { });
    //    Debug.Log("Attack!");
    //}


    void OnPlayerGuard(InputValue value)
    {
        if (isJumping_) return;
        if (heavyButtonCharging_)
        {
            playerCancelHeavyCharge();
        }
        if (lightButtonCharging_)
        {
            playerCancelLightCharge();
        }
        isGuarding_ = value.isPressed;
        //isAiming_ = isGuarding_;
        GameManager.Instance.MainGameEvent.Send(new PlayerGuardingButtonCommand() { GuardingButtonIsPressed = isGuarding_ });
        Debug.Log(isGuarding_ + "Guarding");
    }

    void OnPlayerRestartLevel()
    {
        if (!canRevive_) return;
        var waveCount = GameManager.Instance.MainGameMediator.RealTimePlayerData.GetNowHighestWaveCount();
        GameManager.Instance.MainGameMediator.RealTimePlayerData.PlayerNowCheckPoint = waveCount;
        GameManager.Instance.MainGameMediator.RealTimePlayerData.Revive();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void OnPlayerToMainMenu()
    {
        if (!canRevive_) return;
        Cursor.visible = true;
        SceneManager.LoadScene("GameMainMenu");
    }

    void OnPlayerPotion()
    {
        GameManager.Instance.MainGameMediator.RealTimePlayerData.PotionUsed();
    }

    async void reviveDelayer()
    {
        await UniTask.Delay(5000);
        canRevive_ = true;
    }

    void OnPlayerOpenControlUI()
    {
        Time.timeScale = 0;
        GameManager.Instance.UIGameEvent.Send(new PlayerSwitchControlUICommand() { Switch = true });
        input_.SwitchCurrentActionMap("PlayerPauseUIControl");
    }

    void OnPlayerCloseUI()
    {
        Time.timeScale = 1;
        GameManager.Instance.UIGameEvent.Send(new PlayerSwitchControlUICommand() { Switch = false });
        input_.SwitchCurrentActionMap("MainGameplay");
    }

    void OnPlayerLowerVolume()
    {
        PlayerStatCalculator.ChangeGameVolume(-1);
    }
    void OnPlayerUpperVolume()
    {
        PlayerStatCalculator.ChangeGameVolume(1);
    }

    void OnPlayerChargeButton(InputValue value)
    {
        var switcher = value.isPressed;
        if (switcher)
        {
            heavyButtonCharging_ = true;
        }
        GameManager.Instance.MainGameEvent.Send(new PlayerChargeSwitchCommand { Switch = switcher });
    }
    void OnPlayerTutorialChargeButton(InputValue value)
    {
        var switcher = value.isPressed;
        if (switcher)
        {
            heavyButtonCharging_ = true;
        }
        GameManager.Instance.MainGameEvent.Send(new PlayerChargeSwitchCommand { Switch = switcher });
    }
    void OnPlayerLightAttackChargeButton(InputValue value)
    {
        var switcher = value.isPressed;
        if (switcher)
        {
            lightButtonCharging_ = true;
        }
    }
    void playerCancelHeavyCharge()
    {
        heavyAttackCharger_ = false;
        heavyButtonCharging_ = false;
        GameManager.Instance.MainGameEvent.Send(new PlayerChargeSwitchCommand { Switch = false });
    }
    void playerCancelLightCharge()
    {
        lightAttackCharger_ = false;
        lightButtonCharging_ = false;
    }
}

