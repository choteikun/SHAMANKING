using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class TestAnimatorScript : MonoBehaviour
{

    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void OnAnimatorMove()
    {
        //SendMessageUpwards("OnUpdateRootMotion", anim.deltaPosition);
        SendMessageUpwards("OnUpdateRootMotion", new Vector3(anim.deltaPosition.x, 0, anim.deltaPosition.z));
    }
}
