using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossAnimator : MonoBehaviour
{
    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    private void OnAnimatorMove()
    {
        SendMessageUpwards("OnUpdateRootMotion", anim);
    }
}
