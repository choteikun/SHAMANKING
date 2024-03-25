using Gamemanager;
using UniRx;
using UnityEngine;

public class PlayerControllerModel
{
    GameObject nowPossessingObject_;

    public void PlayerControllerModelInit()
    {
        GameManager.Instance.MainGameEvent.OnPlayerLaunchActionFinish.Where(cmd => cmd.HitObjecctTag == HitObjecctTag.Possessable).Subscribe(cmd => { nowPossessingObject_ = cmd.HitObjecct; });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerPlayerEnterOrLeaveEnviormentObject, cmd => { setInteractionObject(cmd); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerControllerPossessableInteract, cmd => { activateEnviormentInteraction(); });
    }

    void activateEnviormentInteraction()
    {
        if (nowPossessingObject_ == null) return;
        if (nowPossessingObject_.GetComponent<EnviormentMachineBehaviorBase>())
        {
            var behavior = nowPossessingObject_.GetComponent<EnviormentMachineBehaviorBase>();
            behavior.EnviormaneMachinePossessInteract();
        }
    }

    void setInteractionObject(PlayerEnterOrLeaveEnviormentObjectCommand cmd)
    {
        if (cmd.EnterOrLeave)
        {
            nowPossessingObject_ = cmd.NowEnterEnviormentObject;
        }
        else
        {
            nowPossessingObject_ = null;
        }
    }

}
