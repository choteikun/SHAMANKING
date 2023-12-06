using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class GameManager : ToSingletonMonoBehavior<GameManager>
{
    public MainGameEventPack MainGameEvent { get; private set; } = new MainGameEventPack();
    public BT_EventPack BT_Event { get; private set; } = new BT_EventPack();
    public UIGameEventPack UIGameEvent { get; private set; } = new UIGameEventPack();

    public HellDogGameEventPack HellDogGameEvent { get; private set; } = new HellDogGameEventPack();
    [field:SerializeField] public MainGameMediator MainGameMediator { get; private set; }
    [SerializeField] public SO_AttackBlockDatabase AttackBlockDatabase;
 
    protected override void init()
    {
        hideCurser();
        QualitySettings.vSyncCount = 0; 
        Application.targetFrameRate = 120;
        base.init();
        MainGameMediator = new MainGameMediator();
        MainGameMediator.MainGameMediatorInit();
    }
    void hideCurser()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

}
