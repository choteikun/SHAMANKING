using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAnimatorFather : MonoBehaviour
{
    CharacterController cc;
    private void Start()
    {
        cc = GetComponent<CharacterController>();
    }
    public void OnUpdateRootMotion(object _deltaPos)
    {
        cc.Move((Vector3)_deltaPos);
    }
}
