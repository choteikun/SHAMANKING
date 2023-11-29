using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackFeedbackBehavior : MonoBehaviour
{
    [SerializeField] int delayFrame_ = 10;
    void Start()
    {
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerAttackSuccess, cmd => { attackHitTimeScale(); });
    }

    async void attackHitTimeScale()
    {
        Time.timeScale = 0.15f;
        await UniTask.DelayFrame(delayFrame_);
        Time.timeScale = 1;
    }
}
