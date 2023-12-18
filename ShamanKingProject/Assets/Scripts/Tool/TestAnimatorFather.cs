using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAnimatorFather : MonoBehaviour
{
    CharacterController cc;
    bool attackMoverEnabled = false;
    bool knockBackEnabled = false;
    private void Start()
    {
        cc = GetComponent<CharacterController>();
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnAnimationMovementEnable, cmd => { attackMoverEnabled = true; });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerBeAttackByEnemySuccess, cmd => { knockBackEnabled = true; });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnAnimationMovementDisable, cmd => { attackMoverEnabled = false; });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerRoll, cmd => { attackMoverEnabled = false; });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerMovementInterruptionFinish, cmd => { attackMoverEnabled = false; knockBackEnabled = false; });
        
    }
    public void OnUpdateRootMotion(Animator anim)
    {
        if (knockBackEnabled)
        {
            cc.Move(anim.transform.forward * anim.deltaPosition.magnitude * 0.5f * -1);
        }
        if (!attackMoverEnabled) return;
        cc.Move(anim.transform.forward * anim.deltaPosition.magnitude * 0.2f);
       
    }
}
