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
        //GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnAnimationMovementEnable, cmd => { attackMoverEnabled = true; });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnAnimationMovementDisable, cmd => { attackMoverEnabled = false; });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerRoll, cmd => { attackMoverEnabled = false; });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerMovementInterruptionFinish, cmd => { attackMoverEnabled = false; });
    }
    public void OnUpdateRootMotion(object _deltaPos)
    {
        if (!attackMoverEnabled) return;
       cc.Move((Vector3)_deltaPos);
    }
}
