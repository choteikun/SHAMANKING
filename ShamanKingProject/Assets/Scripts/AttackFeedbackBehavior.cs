using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Gamemanager;
using DG.Tweening;

public class AttackFeedbackBehavior : MonoBehaviour
{
    [SerializeField] GameObject mainCM_;
    [SerializeField] GameObject parryCM_;
    [SerializeField]  CinemachineVirtualCamera vCamera_;
    [SerializeField] private CinemachineImpulseSource impulseSource_;
    [SerializeField] float force_;
    [SerializeField] int delayFrame_ = 10;

    [SerializeField] float heavyForce_;
    [SerializeField] int heavyDelayFrame_ = 10;

    [SerializeField] float parryForce_;
    [SerializeField] int parryDelayFrame_ = 20;
    void Start()
    {
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerAttackSuccess, cmd => { onGetPlayerAttackSuccessCommand(cmd); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerSuccessParry, cmd => parrySuccessTimeScale());
    }

    void onGetPlayerAttackSuccessCommand(PlayerAttackSuccessCommand cmd)
    {
        if (cmd.AttackFeedBackType == AttackFeedBackType.Heavy)
        {
            attackHeavyHitTimeScale();
        }
        else if(cmd.AttackFeedBackType == AttackFeedBackType.Light)
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
    async void parrySuccessTimeScale()
    {
        mainCM_.SetActive(false);
        parryCM_.SetActive(true);
        impulseSource_.GenerateImpulse(parryForce_);
        Time.timeScale = 0.01f;        
        await UniTask.DelayFrame(parryDelayFrame_);
        mainCM_.SetActive(true);
        parryCM_.SetActive(false);
        Time.timeScale = 1;
    }
    void SetCameraDistance(float distance)
    {
        vCamera_.GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraDistance = distance;
    }
}
