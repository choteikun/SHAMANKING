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
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerLightAttack, cmd => { attackMoverEnabled = true; });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerThrowAttack, cmd => { attackMoverEnabled = true; });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerHeavyAttack, cmd => { attackMoverEnabled = true; });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerRoll, cmd => { attackMoverEnabled = false; });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerMovementInterruptionFinish, cmd => { attackMoverEnabled = false; });
    }
    public void OnUpdateRootMotion(object _deltaPos)
    {
        if (!attackMoverEnabled) return;
       cc.Move((Vector3)_deltaPos);
    }
}
