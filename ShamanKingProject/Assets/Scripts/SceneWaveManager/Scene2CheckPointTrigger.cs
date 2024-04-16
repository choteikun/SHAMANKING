using Cysharp.Threading.Tasks;
using Gamemanager;
using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene2CheckPointTrigger : EnviormentMachineBehaviorBase
{
    [SerializeField] Animator checkPointAnimator_;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.MainGameEvent.Send(new PlayerEnterOrLeaveEnviormentObjectCommand() { EnterOrLeave = true, NowEnterEnviormentObject = this.gameObject }) ;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            onExit();
        }
    }

    void onExit()
    {
        GameManager.Instance.MainGameEvent.Send(new PlayerEnterOrLeaveEnviormentObjectCommand() { EnterOrLeave = false, NowEnterEnviormentObject = this.gameObject });
    }
    public override void EnviormaneMachinePossessInteract()
    {
        playCheckPointAnimator();
        GameManager.Instance.MainGameMediator.RealTimePlayerData.Revive();
        GameManager.Instance.MainGameEvent.Send(new SystemCallWaveClearCommand() { SceneName = "Scene2", WaveID = 3 });
        onExit();
        //this.gameObject.SetActive(false);
    }

    void playCheckPointAnimator()
    {
        checkPointAnimator_.CrossFadeInFixedTime("Armature_009_touch_bake", 0.15f);
    }
}
