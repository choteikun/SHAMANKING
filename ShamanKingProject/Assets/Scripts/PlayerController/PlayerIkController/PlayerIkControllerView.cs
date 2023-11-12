using DG.Tweening;
using Gamemanager;
using UniRx;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerIkControllerView : MonoBehaviour
{
    public MultiAimConstraint multiAimConstraint;
    public MultiAimConstraint multiAimConstraintForPossessable;
    [SerializeField]
    float ToAimAnimationDamping = 0.75f;
    [SerializeField]
    float BackAnimationDamping = 0.25f;

    Tweener aimTweener_;
    private void Start()
    {
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnAimingButtonTrigger, aimButtonTrigger);
        GameManager.Instance.MainGameEvent.OnPlayerLaunchActionFinish.Where(cmd => cmd.Hit && (cmd.HitObjecctTag == HitObjecctTag.Biteable|| cmd.HitObjecctTag == HitObjecctTag.Enemy)).Subscribe(cmd => activateAimRig(0));
        GameManager.Instance.MainGameEvent.OnPlayerLaunchActionFinish.Where(cmd => cmd.Hit && cmd.HitObjecctTag == HitObjecctTag.Possessable).Subscribe(cmd => activePossessableIK());
    }
    void aimButtonTrigger(PlayerAimingButtonCommand command)
    {
        if (command.AimingButtonIsPressed)
        {
            activateAimRig(1);
        }
        else
        {
            activateAimRig(0);
            multiAimConstraintForPossessable.weight = 0;
        }
    }
    void activateAimRig(float value)
    {
        if (multiAimConstraint == null)
        {
            Debug.LogError("MultiAimConstraint component not assigned!");
            return;
        }

        var originalWeight = multiAimConstraint.weight;
        if (value == 1)
        {
            aimTweener_ = DOTween.To(() => originalWeight, newWeight => { multiAimConstraint.weight = newWeight; }, value, ToAimAnimationDamping);

        }
        else
        {
            aimTweener_.Kill();
            aimTweener_ = DOTween.To(() => originalWeight, newWeight => { multiAimConstraint.weight = newWeight; }, value, BackAnimationDamping);
        }
    }   

    void activePossessableIK()
    {
        activateAimRig(0);
        multiAimConstraintForPossessable.weight = 1;
    }
}
