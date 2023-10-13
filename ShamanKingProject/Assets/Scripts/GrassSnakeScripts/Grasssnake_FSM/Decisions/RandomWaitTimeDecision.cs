using UnityEngine;

namespace AI.FSM.Decisions
{
    [CreateAssetMenu(menuName = "AI/FSM/Decisions/RandomWaitTimeDecision")]
    public class RandomWaitTimeDecision : Decision
    {
        [SerializeField, Tooltip("設置<該發呆了>的時間")]
        private int timeToDazeValue = 35;
        [SerializeField, Tooltip("設置<該發呆了>時間的閾值")]
        private int timeToDazeThreshold = 5;

        int randomTimeToDazeValue = 35;

        float timer = 0;

        public override bool Decide(BaseStateMachine stateMachine)
        {
            //如果敵人身上有ZombieStats腳本且觸發器為true
            if (stateMachine.GetComponent<ZombieStats>() != null && stateMachine.GetComponent<ZombieStats>().randomTimeToDazeValueTrigger)
            {
                //生成一個閾值內的隨機發呆時間
                randomTimeToDazeValue = Random.Range(timeToDazeValue - timeToDazeThreshold, timeToDazeValue + timeToDazeThreshold);
                //Debug.Log("randomTimeToDazeValue : " + randomTimeToDazeValue);
                stateMachine.GetComponent<ZombieStats>().randomTimeToDazeValueTrigger = false;
            }
            timer += Time.deltaTime;
            if (timer >= randomTimeToDazeValue)
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
