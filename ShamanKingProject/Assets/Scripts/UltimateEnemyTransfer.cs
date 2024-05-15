using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamemanager;
using UnityEngine.AI;

public class UltimateEnemyTransfer : MonoBehaviour
{
    [SerializeField] int beUltCount = 0;
    [SerializeField] List<GameObject> beUltimateEnemy;
    [SerializeField] Vector3[] originPos_;
    [SerializeField] Vector3[] originRotation_;
    [SerializeField] Transform[] FinalDestination;
    void Start()
    {
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerAttackSuccess, cmd => { Debug.LogError("add"); addToTransferList(cmd); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnCallUltimateTransferStart, cmd => { Debug.LogError("Transfer"); transfer(); });
    }

    
    void Update()
    {
        
    }
    void addToTransferList(PlayerAttackSuccessCommand cmd)
    {
        if (cmd.AttackInputType != AttackInputType.UltimatePrepare) return;
        if (beUltCount<9)
        {
            beUltimateEnemy.Add(cmd.AttackTarget);
            originPos_[beUltCount] = beUltimateEnemy[beUltCount].transform.position;
            originRotation_[beUltCount] = beUltimateEnemy[beUltCount].transform.rotation.eulerAngles;
            beUltCount++;
        }
    }
    void transfer()
    {
              
        for (int i = 0; i < beUltimateEnemy.Count; i++)
        {
            Debug.Log(FinalDestination[i].position);
            beUltimateEnemy[i].GetComponent<NavMeshAgent>().Warp(FinalDestination[i].position);
            //beUltimateEnemy[i].transform.position =;
            beUltimateEnemy[i].transform.rotation = FinalDestination[i].rotation;
        }
    }
    void refresh()
    {
        beUltimateEnemy = new List<GameObject>();
        originPos_ = new Vector3[9];
        originRotation_ = new Vector3[9];
        beUltCount = 0;
    }
}
