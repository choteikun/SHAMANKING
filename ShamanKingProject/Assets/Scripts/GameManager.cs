using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : ToSingletonMonoBehavior<GameManager>
{
    public MainGameEventPack MainGameEvent { get; private set; } = new MainGameEventPack();
    public MainGameMediator MainGameMediator { get; private set; }

    protected override void init()
    {
        hideCurser();
        QualitySettings.vSyncCount = 0; 
        Application.targetFrameRate = 120;
        base.init();
        MainGameMediator = new MainGameMediator();
    }
    void hideCurser()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

}
