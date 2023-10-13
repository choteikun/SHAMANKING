using UnityEngine;

namespace AI.FSM.Decisions
{
    [CreateAssetMenu(menuName = "AI/FSM/Decisions/RandomWanderTimeDecision")]
    public class RandomWanderTimeDecision : Decision
    {
        [SerializeField, Tooltip("設置<該移動了>的時間")]
        private int timeToMoveValue = 25;
        [SerializeField, Tooltip("設置<該移動了>時間的閾值")]
        private int timeToMoveThreshold = 5;

        int randomTimeToMoveValue = 25;

        float timer = 0;
        public override bool Decide(BaseStateMachine stateMachine)
        {
            //如果敵人身上有ZombieStats腳本且觸發器為true
            if (stateMachine.GetComponent<ZombieStats>() != null && stateMachine.GetComponent<ZombieStats>().randomTimeToMoveValueTrigger)
            {
                //生成一個閾值內的隨機<<醒來!!該去遊走了>>的時間
                randomTimeToMoveValue = Random.Range(timeToMoveValue - timeToMoveThreshold, timeToMoveValue + timeToMoveThreshold);
                //Debug.Log("randomTimeToMoveValue : " + randomTimeToMoveValue);
                stateMachine.GetComponent<ZombieStats>().randomTimeToMoveValueTrigger = false;
            }
            timer += Time.deltaTime;
            if (timer >= randomTimeToMoveValue)
            {
                timer = 0;
                return true;
            }
            else
            {
                return false;
            }
        }

        
    }
}
