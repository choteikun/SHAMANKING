using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamemanager;

public class NearbyEnemyChecker : MonoBehaviour
{
    [SerializeField] float noticeZone = 10;
    [SerializeField] LayerMask targetLayers;
    void Start()
    {
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerPressExecutionButton, cmd => { canBreakResponse(); });
    }

    // Update is called once per frame
    void Update()
    {
        GameManager.Instance.UIGameEvent.Send(new SystemCallCanBreakUIUpdate() { CanBreak = checkNearbyEnemy() }); ;
    }

    void canBreakResponse()
    {
        if (checkNearbyEnemy())
        {
            GameManager.Instance.MainGameEvent.Send(new PlayerPressExecutionButtonResponse());
        }
    }
    bool checkNearbyEnemy()
    {
        var result = false;
        Collider[] nearbyTargets = Physics.OverlapSphere(transform.position, noticeZone, targetLayers);
        if (nearbyTargets.Length <= 0)
        {
            Debug.Log("Cant find"); return false;
        }
        foreach (var item in nearbyTargets)
        {
            if (item.GetComponent<EnemyBeHitTest>())
            {
                var enemy = item.GetComponent<EnemyBeHitTest>();
                if (enemy != null)
                {
                    if (enemy.getCanBreak())
                    {
                        result = true;
                    }
                }
            }
        }

        return result;
    }
}
