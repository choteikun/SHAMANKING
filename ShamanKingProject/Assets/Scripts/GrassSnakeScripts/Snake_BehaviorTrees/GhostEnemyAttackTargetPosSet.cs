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
        //如果我把這行改成vector3那就不會拿到新的位置訊息
        private Transform targetTransform;

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
                targetTransform = currentTargetGameObject.GetComponent<Transform>();
                prev_TargetGameObject = currentTargetGameObject;
            }
            Debug.Log("targetPos : " + targetTransform.position);
            targetTransform.position = targetGameObject.Value.transform.position;
            randomAPosition();
            transform.LookAt(targetTransform);
        }

        public override TaskStatus OnUpdate()
        {
            if (ghostTransform == null || targetGameObject == null)
            {
                Debug.LogWarning("Transform is null || targetGameObject is null");
                return TaskStatus.Failure;
            }
            
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            ghostGameObject = null;
            targetTransform.position = Vector3.zero;
        }


        void randomAPosition()
        {
            var randomNum = Random.Range(1, 4);
            switch (randomNum)
            {
                case 1:
                    ghostTransform.localPosition = targetTransform.position + new Vector3(-nearByTargetPosThresholdValue, 0, -nearByTargetPosThresholdValue);
                    break;
                case 2:
                    ghostTransform.localPosition = targetTransform.position + new Vector3(nearByTargetPosThresholdValue, 0, -nearByTargetPosThresholdValue);
                    break;
                case 3:
                    ghostTransform.localPosition = targetTransform.position + new Vector3(-nearByTargetPosThresholdValue, 0, nearByTargetPosThresholdValue);
                    break;
                case 4:
                    ghostTransform.localPosition = targetTransform.position + new Vector3(nearByTargetPosThresholdValue, 0, nearByTargetPosThresholdValue);
                    break;
                default:
                    break;
            }
        }
    }
}
