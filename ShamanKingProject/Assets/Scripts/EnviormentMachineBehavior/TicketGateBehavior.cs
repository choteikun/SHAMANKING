using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamemanager;
using Cysharp.Threading.Tasks;

public class TicketGateBehavior : EnviormentMachineBehaviorBase
{
    [SerializeField]
    GameObject gateCollider_;
    public override async void EnviormaneMachinePossessInteract()
    {
        Debug.Log("This is a ticket machine");
        await UniTask.Delay(200);
        gateCollider_.SetActive(false);
        GameManager.Instance.MainGameEvent.Send(new PlayerCancelPossessCommand());
    }
}
