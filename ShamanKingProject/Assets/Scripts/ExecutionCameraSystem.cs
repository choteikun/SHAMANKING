using Cysharp.Threading.Tasks;
using Gamemanager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecutionCameraSystem : MonoBehaviour
{
    [SerializeField] GameObject enemyObject_;
    [SerializeField] GameObject enemyPos_;
    [SerializeField] GameObject playerPos_;
    [SerializeField] GameObject mainCam_;
    [SerializeField] GameObject executionCam_;
    [SerializeField] Animator Animator_;
    void Start()
    {
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerAttackSuccess, cmd => { changePos(cmd); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerMovementInterruptionFinish, cmd => { mainCam_.SetActive(true); executionCam_.SetActive(false); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    async void changePos(PlayerAttackSuccessCommand cmd)
    {
        if (cmd.AttackInputType == AttackInputType.ExecutionAttack)
        {
            Animator_.CrossFadeInFixedTime("Dolly_Rig_Dolly_RigAction_001", 0.25f);
            enemyObject_.transform.position = enemyPos_.transform.position;
            enemyObject_.transform.rotation = enemyPos_.transform.rotation;
            GameManager.Instance.MainGameMediator.RealTimePlayerData.PlayerGameObject.GetComponent<CharacterController>().enabled = false;
            GameManager.Instance.MainGameMediator.RealTimePlayerData.PlayerGameObject.transform.position = playerPos_.transform.position;
            GameManager.Instance.MainGameMediator.RealTimePlayerData.PlayerGameObject.transform.rotation = playerPos_.transform.rotation;
            GameManager.Instance.MainGameMediator.RealTimePlayerData.PlayerGameObject.GetComponent<CharacterController>().enabled = true;
            mainCam_.SetActive(false);           
            executionCam_.SetActive(true);
        }
    }
}
