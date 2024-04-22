using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAnimatorFather : MonoBehaviour
{
    CharacterController cc_;
    [SerializeField]
    bool attackMoverEnabled_ = false;
    [SerializeField]
    bool executionAttackMoverEnabled_ = false;
    [SerializeField]
    bool heavyAttackMoverEnabled_ = false;
    [SerializeField]
    bool knockBackEnabled_ = false;
    private void Start()
    {
        cc_ = GetComponent<CharacterController>();
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnAnimationMovementEnable, cmd => { attackMoverEnabled_ = true; });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerBeAttackByEnemySuccess, cmd => { knockBackEnabled_ = true; });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnAnimationMovementDisable, cmd => { attackMoverEnabled_ = false; });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerRoll, cmd => { attackMoverEnabled_ = false; });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerMovementInterruptionFinish, cmd => { attackMoverEnabled_ = false; knockBackEnabled_ = false; executionAttackMoverEnabled_ = false; heavyAttackMoverEnabled_ = false; });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerExecutionAttack, cmd => { executionAttackMoverEnabled_ = true; });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerHeavyAttack, cmd => { heavyAttackMoverEnabled_ = true; });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerAttackRecheckAnimationMovement, cmd =>
        {
            if (cmd.InputType == AttackInputType.HeavyAttack)
            {
                attackMoverEnabled_ = false;
                heavyAttackMoverEnabled_ = true;
            }
        });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerAttackRecheckAnimationMovement, cmd =>
        {
            if (cmd.InputType == AttackInputType.ExecutionAttack)
            {
                attackMoverEnabled_ = false;
                executionAttackMoverEnabled_ = true;
            }
        });
    }
    public void OnUpdateRootMotion(Animator anim)
    {
        if (knockBackEnabled_)
        {
            cc_.Move(anim.transform.forward * anim.deltaPosition.magnitude * 0.5f * -1);
        }
        if (heavyAttackMoverEnabled_)
        {
            cc_.Move(anim.transform.forward * anim.deltaPosition.magnitude * 0.35f);
        }
        if (executionAttackMoverEnabled_)
        {
            cc_.Move(-anim.transform.forward * anim.deltaPosition.magnitude * 1.0f);
        }
        if (!attackMoverEnabled_) return;
        cc_.Move(anim.transform.forward * anim.deltaPosition.magnitude * 1.0f);
    }
}
