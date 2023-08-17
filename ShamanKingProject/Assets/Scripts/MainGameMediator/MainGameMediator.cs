using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

public class MainGameMediator
{
    public CompositeDisposable Disposable = new CompositeDisposable();
    public void DisposeObserber()
    {
        Disposable.Dispose();
    }
}
