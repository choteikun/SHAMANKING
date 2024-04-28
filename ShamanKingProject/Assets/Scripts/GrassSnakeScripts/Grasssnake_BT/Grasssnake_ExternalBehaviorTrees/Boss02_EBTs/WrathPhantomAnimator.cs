using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrathPhantomAnimator : MonoBehaviour
{
    Animator anim_;
    WrathPhantomVariables wrathPhantomVariables_;

    bool wrathPhantomDeadTrigger_;

    private void Start()
    {
        wrathPhantomDeadTrigger_ = false;
        anim_ = GetComponent<Animator>();
        wrathPhantomVariables_ = GetComponentInParent<WrathPhantomVariables>();
    }

    private void Update()
    {
        
    }
    private void OnAnimatorMove()
    {
        SendMessageUpwards("OnUpdateRootMotion", anim_);
    }
}
