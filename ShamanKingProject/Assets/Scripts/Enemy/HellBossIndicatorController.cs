using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellBossIndicatorController : MonoBehaviour
{
    [SerializeField] Animator dashAnimator_;
    [SerializeField] Animator flameThrower_;
    
    void Start()
    {
        GameManager.Instance.HellDogGameEvent.SetSubscribe(GameManager.Instance.HellDogGameEvent.OnBossCallFlameThrower, cmd => { flameThrower_.CrossFadeInFixedTime("FlameStart", 0.1f); });
        GameManager.Instance.HellDogGameEvent.SetSubscribe(GameManager.Instance.HellDogGameEvent.OnBossCallDash, cmd => { dashAnimator_.CrossFadeInFixedTime("StartDash", 0.1f); });
    }

   
}
