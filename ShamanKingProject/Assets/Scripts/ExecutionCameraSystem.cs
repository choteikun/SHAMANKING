using Cysharp.Threading.Tasks;
using Gamemanager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ExecutionCameraSystem : MonoBehaviour
{
    [SerializeField] GameObject enemyObject_;
    [SerializeField] GameObject playerObj_;
    [SerializeField] GameObject enemyPos_;
    [SerializeField] GameObject playerPos_;
    [SerializeField] GameObject mainCam_;
    [SerializeField] GameObject executionCam_;
    [SerializeField] Animator Animator_;
    [SerializeField] CharacterController cc_;
    [SerializeField] GameObject playerModel_;

    NavMeshAgent eliteGhostEnemyNavMeshAgent_;
    void Start()
    {
        eliteGhostEnemyNavMeshAgent_ = enemyObject_.GetComponent<NavMeshAgent>();
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerAttackSuccess, cmd => { changePos(cmd); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerMovementInterruptionFinish, cmd => { mainCam_.SetActive(true); executionCam_.SetActive(false);  });
       
    }

    // Update is called once per frame
    void Update()
    {
        if (cc_==null)
        {
            cc_ = GameManager.Instance.MainGameMediator.RealTimePlayerData.PlayerGameObject.GetComponent<CharacterController>();
        }
        if (playerObj_==null)
        {
            playerObj_ = GameManager.Instance.MainGameMediator.RealTimePlayerData.PlayerGameObject;
        }
    }

    async void changePos(PlayerAttackSuccessCommand cmd)
    {
        if (cmd.AttackInputType == AttackInputType.ExecutionAttack)
        {
            Animator_.CrossFadeInFixedTime("metarig_metarigAction", 0.25f);

            //enemyObject_.GetComponent<NavMeshAgent>().SetDestination(enemyPos_.transform.position);
            eliteGhostEnemyNavMeshAgent_.enabled = false;
            enemyObject_.transform.position = enemyPos_.transform.position;
            enemyObject_.transform.rotation = enemyPos_.transform.rotation;
            eliteGhostEnemyNavMeshAgent_.enabled = true;
            cc_.enabled = false;
            playerObj_.transform.position = playerPos_.transform.position;
            playerObj_.transform.rotation = playerPos_.transform.rotation;
            playerModel_.gameObject.transform.rotation = Quaternion.Euler(Vector3.zero);
            await UniTask.Delay(250);
            cc_.enabled = true;
            mainCam_.SetActive(false);           
            executionCam_.SetActive(true);
        }
    }
}
