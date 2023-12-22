using Gamemanager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellDogSprintBehavior : MonoBehaviour
{

    [SerializeField] GameObject sprintAttackUseCollider_;

    private void Start()
    {
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.HellDogGameEvent.OnBossCallSprintColliderSwitch, cmd => { switchSprintCollider(cmd); });
    }

    void switchSprintCollider(BossCallSprintColliderSwitchCommand cmd)
    {
        sprintAttackUseCollider_.SetActive(cmd.OnOrOff);
    }
}
