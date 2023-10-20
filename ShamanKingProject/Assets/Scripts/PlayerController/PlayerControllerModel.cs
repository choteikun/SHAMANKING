using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Gamemanager;
using UnityEditor.Rendering;

public class PlayerControllerModel
{
    GameObject nowPossessingObject_;

  public void PlayerControllerModelInit()
    {
        GameManager.Instance.MainGameEvent.OnPlayerLaunchActionFinish.Where(cmd => cmd.HitObjecctTag == HitObjecctTag.Possessable).Subscribe(cmd => { nowPossessingObject_ = cmd.HitObjecct; });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerControllerPossessableInteract, cmd => { activateEnviormentInteraction(); });
    }

    void activateEnviormentInteraction()
    {
        if (nowPossessingObject_.GetComponent<EnviormentMachineBehaviorBase>())
        {
            var behavior = nowPossessingObject_.GetComponent<EnviormentMachineBehaviorBase>();
            behavior.EnviormaneMachinePossessInteract();
        }
    }

}
