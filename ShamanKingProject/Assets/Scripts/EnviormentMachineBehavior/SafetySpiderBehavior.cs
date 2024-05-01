using Gamemanager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafetySpiderBehavior : EnviormentMachineBehaviorBase
{
    [SerializeField] Animator spiderAnimator_;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.MainGameEvent.Send(new PlayerEnterOrLeaveEnviormentObjectCommand() { EnterOrLeave = true, NowEnterEnviormentObject = this.gameObject });
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
        playSpidertAnimator();      
        onExit();
        this.gameObject.SetActive(false);
    }
    void playSpidertAnimator()
    {
        spiderAnimator_.CrossFadeInFixedTime("SpiderRun", 0.15f);
    }
}
