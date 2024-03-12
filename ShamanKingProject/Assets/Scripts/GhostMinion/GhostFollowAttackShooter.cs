using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostFollowAttackShooter : MonoBehaviour
{
    [SerializeField] GameObject followAttackPrefab_;
    void Start()
    {
        GameManager.Instance.GhostEnemyGameEvent.SetSubscribe(GameManager.Instance.GhostEnemyGameEvent.OnGhostEnemyCallFollowAttack, cmd => { genPrefab(); });
    }

   void genPrefab()
    {
        var Object = Instantiate(followAttackPrefab_,this.gameObject.transform.position,Quaternion.identity);
    }
}
