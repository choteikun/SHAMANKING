using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

public class MainGameMediator
{
    CompositeDisposable disposable_ = new CompositeDisposable();

    PlayerControllerModel playerControllerModel_ = new PlayerControllerModel();

    public void MainGameMediatorInit()
    {
        playerControllerModel_.PlayerControllerModelInit();
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
