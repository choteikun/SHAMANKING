using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAnimatorFather : MonoBehaviour
{
    CharacterController cc;
    bool attackMoverEnabled = false;
    private void Start()
    {
        cc = GetComponent<CharacterController>();
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnAnimationMovementEnable, cmd => { attackMoverEnabled = true; });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnAnimationMovementDisable, cmd => { attackMoverEnabled = false; });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerRoll, cmd => { attackMoverEnabled = false; });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerMovementInterruptionFinish, cmd => { attackMoverEnabled = false; });
    }
    public void OnUpdateRootMotion(Animator anim)
    {
        if (!attackMoverEnabled) return;
        cc.Move(anim.transform.forward * anim.deltaPosition.magnitude*0.35f);
    }
}
