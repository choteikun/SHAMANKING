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

        void OnEnable()
        {
            randomTimeToMoveValue = Random.Range(timeToMoveValue - timeToMoveThreshold, timeToMoveValue + timeToMoveThreshold);
            Debug.Log("randomTimeToMoveValue : " + randomTimeToMoveValue);
        }
        public override bool Decide(BaseStateMachine stateMachine)
        {
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
