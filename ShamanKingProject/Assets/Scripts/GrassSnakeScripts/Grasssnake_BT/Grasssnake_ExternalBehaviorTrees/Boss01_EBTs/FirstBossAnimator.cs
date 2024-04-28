using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamemanager;

public class FirstBossAnimator : MonoBehaviour
{
    Animator anim_;
    FirstBossVariables firstBossVariables_;

    bool firstBossDeadTrigger_;

    private void Awake()
    {
        firstBossDeadTrigger_ = false;
        anim_ = GetComponent<Animator>();
        firstBossVariables_ = GetComponentInParent<FirstBossVariables>();
    }
    private void Update()
    {
        if (firstBossVariables_.PreludeTrigger)
        {
            anim_.SetBool("Prelude", true);
        }
        else
        {
            anim_.SetBool("Prelude", false);
        }
        if (anim_.GetCurrentAnimatorStateInfo(0).IsName("Helldog_Dead") && !firstBossDeadTrigger_)
        {
            GameManager.Instance.HellDogGameEvent.Send(new BossCallDeadCommand());
            firstBossDeadTrigger_ = true;
        }
    }
    private void OnAnimatorMove()
    {
        SendMessageUpwards("OnUpdateRootMotion", anim_);
    }
}
