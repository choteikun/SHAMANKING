using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Gamemanager;

public class AttackFeedbackBehavior : MonoBehaviour
{
    [SerializeField]  CinemachineVirtualCamera vCamera_;
    [SerializeField] private CinemachineImpulseSource impulseSource_;
    [SerializeField] float force_;
    [SerializeField] int delayFrame_ = 10;

    [SerializeField] float heavyForce_;
    [SerializeField] int heavyDelayFrame_ = 10;
    void Start()
    {
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerAttackSuccess, cmd => { onGetPlayerAttackSuccessCommand(cmd); });
    }

    void onGetPlayerAttackSuccessCommand(PlayerAttackSuccessCommand cmd)
    {
        if (cmd.AttackFeedBackType == AttackFeedBackType.Heavy)
        {
            attackHeavyHitTimeScale();
        }
        else
        {
            attackHitTimeScale();
        }
    }

    async void attackHitTimeScale()
    {
        impulseSource_.GenerateImpulse(force_);
        Time.timeScale = 0.15f;
        await UniTask.DelayFrame(delayFrame_);
        Time.timeScale = 1;
    }
    async void attackHeavyHitTimeScale()
    {
        impulseSource_.GenerateImpulse(heavyForce_);
        Time.timeScale = 0.15f;
        await UniTask.DelayFrame(heavyDelayFrame_);
        Time.timeScale = 1;
    }

}
