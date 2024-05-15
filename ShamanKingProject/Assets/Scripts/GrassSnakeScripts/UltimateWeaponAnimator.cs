using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltimateWeaponAnimator : MonoBehaviour
{
    Animator weaponAnim_;

    void Start()
    {
        weaponAnim_ = GetComponent<Animator>();
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerUltimatePrepareSuccess, cmd => { ultimateAttackStart(); });
    }
    public void ultimateAttackStart()
    {
        gameObject.SetActive(true);
        weaponAnim_.CrossFadeInFixedTime("PlayerUltimateAnimation", 0);
    }
   
}
