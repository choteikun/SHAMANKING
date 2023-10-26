using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class GameManager : ToSingletonMonoBehavior<GameManager>
{
    public MainGameEventPack MainGameEvent { get; private set; } = new MainGameEventPack();
    public UIGameEventPack UIGameEvent { get; private set; } = new UIGameEventPack();

    [field:SerializeField] public MainGameMediator MainGameMediator { get; private set; }

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
