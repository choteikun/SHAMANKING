using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

[System.Serializable]
public class MainGameMediator
{
    CompositeDisposable disposable_ = new CompositeDisposable();

    PlayerControllerModel playerControllerModel_ = new PlayerControllerModel();

    PlayerDataModel playerDataModel_ = new PlayerDataModel();
    [field: SerializeField] public ReaTimePlayerData RealTimePlayerData { get; private set; } = new ReaTimePlayerData();

    public void MainGameMediatorInit()
    {
        playerControllerModel_.PlayerControllerModelInit();
        playerDataModel_.PlayerDataModelInit();
    }
    
    public void DisposeObserber()
    {
        disposable_.Dispose();
    }
    public void AddToDisposables(IDisposable disposable)
    {
        disposable_.Add(disposable);
    }
}
