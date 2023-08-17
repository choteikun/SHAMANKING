using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : ToSingletonMonoBehavior<GameManager>
{
    public MainGameEventPack MainGameEvent { get; private set; } = new MainGameEventPack();
    public MainGameMediator MainGameMediator { get; private set; }

    protected override void init()
    {
        base.init();
        MainGameMediator = new MainGameMediator();
    }
}
