using DG.Tweening;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityTransform
{
    [TaskCategory("Snake")]
    [TaskDescription("Sets the local position of the Transform. Returns Success.")]
    public class GhostEnemyAttackTargetPosSet : Action
    {
        [Tooltip("鬼魂本身")]
        public SharedGameObject ghostGameObject;
        [Tooltip("鬼魂的攻擊對象")]
        public SharedGameObject targetGameObject;
        public int nearByTargetPosThresholdValue;

        private Transform ghostTransform;
        private Vector3 targetPos;

        private GameObject prev_GhostGameObject;
        private GameObject prev_TargetGameObject;

        public override void OnStart()
        {
            var currentGhostGameObject = GetDefaultGameObject(ghostGameObject.Value);
            if (currentGhostGameObject != prev_GhostGameObject)
            {
                ghostTransform = currentGhostGameObject.GetComponent<Transform>();
                prev_GhostGameObject = currentGhostGameObject;
            }

            var currentTargetGameObject = GetDefaultGameObject(targetGameObject.Value);
            if (currentTargetGameObject != prev_TargetGameObject)
            {
                targetPos = currentTargetGameObject.GetComponent<Transform>().position;
                prev_TargetGameObject = currentTargetGameObject;
            }
            randomAPosition();
            transform.LookAt(targetPos);
        }

        public override TaskStatus OnUpdate()
        {
            if (ghostTransform == null)
            {
                Debug.LogWarning("Transform is null");
                return TaskStatus.Failure;
            }
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            ghostGameObject = null;
            targetPos = Vector3.zero;
        }


        void randomAPosition()
        {
            var randomNum = Random.Range(1, 4);
            switch (randomNum)
            {
                case 1:
                    ghostTransform.localPosition = targetPos + new Vector3(-nearByTargetPosThresholdValue, 0, -nearByTargetPosThresholdValue);
                    break;
                case 2:
                    ghostTransform.localPosition = targetPos + new Vector3(nearByTargetPosThresholdValue, 0, -nearByTargetPosThresholdValue);
                    break;
                case 3:
                    ghostTransform.localPosition = targetPos + new Vector3(-nearByTargetPosThresholdValue, 0, nearByTargetPosThresholdValue);
                    break;
                case 4:
                    ghostTransform.localPosition = targetPos + new Vector3(nearByTargetPosThresholdValue, 0, nearByTargetPosThresholdValue);
                    break;
                default:
                    break;
            }
        }
    }
}
