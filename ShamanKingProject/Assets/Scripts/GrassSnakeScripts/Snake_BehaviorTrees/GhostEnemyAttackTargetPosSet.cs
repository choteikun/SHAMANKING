using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityTransform
{
    [TaskCategory("Snake")]
    [TaskDescription("Sets the local position of the Transform. Returns Success.")]
    public class GhostEnemyAttackTargetPosSet : Action
    {
        [Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        public SharedGameObject ghostGameObject;
        [Tooltip("The local position of the Transform")]
        public SharedVector3 localPosition;
        public int nearByTargetPosThresholdValue;

        private Transform ghostTransform;
        private GameObject prevGameObject;

        public override void OnStart()
        {
            var currentGameObject = GetDefaultGameObject(ghostGameObject.Value);
            if (currentGameObject != prevGameObject)
            {
                ghostTransform = currentGameObject.GetComponent<Transform>();
                prevGameObject = currentGameObject;
            }
        }

        public override TaskStatus OnUpdate()
        {
            if (ghostTransform == null)
            {
                Debug.LogWarning("Transform is null");
                return TaskStatus.Failure;
            }
            randomAPosition();
            //ghostTransform.localPosition = localPosition.Value;


            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            ghostGameObject = null;
            localPosition = Vector3.zero;
        }


        private void randomAPosition()
        {
            var randomNum = Random.Range(1, 4);
            switch (randomNum)
            {
                case 1:
                    ghostTransform.localPosition = localPosition.Value + new Vector3(-nearByTargetPosThresholdValue, 0, -nearByTargetPosThresholdValue);
                    break;
                case 2:
                    ghostTransform.localPosition = localPosition.Value + new Vector3(nearByTargetPosThresholdValue, 0, -nearByTargetPosThresholdValue);
                    break;
                case 3:
                    ghostTransform.localPosition = localPosition.Value + new Vector3(-nearByTargetPosThresholdValue, 0, nearByTargetPosThresholdValue);
                    break;
                case 4:
                    ghostTransform.localPosition = localPosition.Value + new Vector3(nearByTargetPosThresholdValue, 0, nearByTargetPosThresholdValue);
                    break;
                default:
                    break;
            }
        }
    }
}
