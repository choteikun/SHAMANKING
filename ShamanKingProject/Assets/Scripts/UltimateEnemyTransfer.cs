using Cysharp.Threading.Tasks;
using Gamemanager;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UltimateEnemyTransfer : MonoBehaviour
{
    [SerializeField] int beUltCount = 0;
    [SerializeField] List<GameObject> beUltimateEnemy;
    [SerializeField] Vector3[] originPos_;
    [SerializeField] Vector3[] originRotation_;
    [SerializeField] Transform[] FinalDestination;
    [SerializeField] GameObject bossOBJ;
    [SerializeField] bool nonAgent = false;
    public Vector3 UltUsePos { get { return originPos_[0]; } set { originPos_[0] = value; } }
    public Quaternion UltUseRot { get { return Quaternion.Euler(originRotation_[0]); } set { originRotation_[0] = value.eulerAngles; } }
    void Start()
    {
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerAttackSuccess, cmd => { Debug.LogError("add"); addToTransferList(cmd); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnCallUltimateTransferStart, cmd => { Debug.LogError("Transfer"); transfer(); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerUltimateAttackFinish, async cmd => { refresher(); });
    }


    void Update()
    {

    }
    async void refresher()
    {
        backToPos();
        if (nonAgent)
        {
            await UniTask.Delay(2500);
            refresh();
            return;
        }
        await UniTask.Delay(500);
        refresh();
    }
    void addToTransferList(PlayerAttackSuccessCommand cmd)
    {
        if (cmd.AttackInputType != AttackInputType.UltimatePrepare) return;
        if (nonAgent)
        {
            beUltimateEnemy.Add(bossOBJ);
            originPos_[0] = beUltimateEnemy[0].transform.position;
            originRotation_[0] = beUltimateEnemy[0].transform.rotation.eulerAngles;
            return;
        }
        if (beUltCount < 9)
        {
            beUltimateEnemy.Add(cmd.AttackTarget);
            originPos_[beUltCount] = beUltimateEnemy[beUltCount].transform.position;
            originRotation_[beUltCount] = beUltimateEnemy[beUltCount].transform.rotation.eulerAngles;
            beUltCount++;
        }
    }
    void transfer()
    {
        if (nonAgent)
        {
            //bossBehavior_.DisableBehavior();
            //bossOBJ.transform.position = FinalDestination[0].position;
            //bossOBJ.transform.rotation = FinalDestination[0].rotation;
            return;
        }
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

        PlayerStatCalculator.PlayerInvincibleSwitch(false);
        beUltimateEnemy = new List<GameObject>();
        originPos_ = new Vector3[9];
        originRotation_ = new Vector3[9];
        beUltCount = 0;
    }
    void backToPos()
    {
        if (nonAgent)
        {
            //bossOBJ.transform.position = originPos_[0];
            //bossOBJ.transform.rotation = Quaternion.Euler(originRotation_[0]);
            return;
        }
        for (int i = 0; i < beUltimateEnemy.Count; i++)
        {
            Debug.Log(FinalDestination[i].position);
            beUltimateEnemy[i].GetComponent<NavMeshAgent>().Warp(originPos_[i]);
            //beUltimateEnemy[i].transform.position =;
            beUltimateEnemy[i].transform.rotation = Quaternion.Euler(originRotation_[i]);
        }
    }
}
