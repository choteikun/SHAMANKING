using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamemanager;

public class UltimateEnemyTransfer : MonoBehaviour
{
    [SerializeField] int beUltCount = 0;
    [SerializeField] List<GameObject> beUltimateEnemy;
    [SerializeField] Vector3[] originPos_;
    [SerializeField] Vector3[] originRotation_;
    [SerializeField] Transform[] FinalDestination;
    void Start()
    {
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerAttackSuccess, cmd => { addToTransferList(cmd); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnCallUltimateTransferStart, cmd => { transfer(); });
    }

    
    void Update()
    {
        
    }
    void addToTransferList(PlayerAttackSuccessCommand cmd)
    {
        if (cmd.AttackInputType != AttackInputType.UltimatePrepare) return;
        if (beUltCount<9)
        {
            beUltCount++;
            beUltimateEnemy.Add(cmd.AttackTarget);
        }
    }
    void transfer()
    {
              
        for (int i = 0; i < beUltimateEnemy.Count; i++)
        {
            originPos_[i] = beUltimateEnemy[i].transform.position;
            originRotation_[i] = beUltimateEnemy[i].transform.rotation.eulerAngles;
            beUltimateEnemy[i].transform.position = FinalDestination[i].position;
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
