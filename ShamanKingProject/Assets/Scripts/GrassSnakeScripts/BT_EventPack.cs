using Gamemanager;
using System;

public class BT_EventPack : GameEventPack
{
    /// <summary>
    /// 通知FSM切換State
    /// </summary>
    public IObservable<BT_SwitchStateMessage> BT_SwitchStateMessage => getSubject<BT_SwitchStateMessage>();
    
}
