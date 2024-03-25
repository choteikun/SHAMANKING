using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamemanager;

public class FirstBossAnimator : MonoBehaviour
{
    Animator anim;
    FirstBossVariables firstBossVariables;

    bool firstBossDeadTrigger_;

    private void Awake()
    {
        firstBossDeadTrigger_ = false;
        anim = GetComponent<Animator>();
        firstBossVariables = GetComponentInParent<FirstBossVariables>();
    }
    private void Update()
    {
        if (firstBossVariables.PreludeTrigger)
        {
            anim.SetBool("Prelude", true);
        }
        else
        {
            anim.SetBool("Prelude", false);
        }
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Helldog_Dead") && !firstBossDeadTrigger_)
        {
            GameManager.Instance.HellDogGameEvent.Send(new BossCallDeadCommand());
            firstBossDeadTrigger_ = true;
        }
    }
    private void OnAnimatorMove()
    {
        SendMessageUpwards("OnUpdateRootMotion", anim);
    }
}
