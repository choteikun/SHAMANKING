using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;
using UnityEngine;
using static Thry.AnimationParser;
using TooltipAttribute = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

[TaskCategory("Snake")]
public class SideMove : Action
{
    [Tooltip("The speed of the agent")]
    public SharedFloat speed;
    [Tooltip("Should the agent be looking at the target position?")]
    public SharedBool lookAtTarget = true;
    [Tooltip("Max rotation delta if lookAtTarget is enabled")]
    public SharedFloat maxLookAtRotationDelta;
    [Tooltip("面向目標")]
    public SharedGameObject targetObj;
    [Tooltip("side Pos Value")]
    public Vector3 SidePosValue;


    //public override void OnStart()
    //{
    //    var position = Target();
    //    if (position != null)
    //    {
    //        // 計算方向向量
    //        Vector3 direction = position - transform.position;
    //        direction.y = 0f; // 避免傾斜

    //        // 計算目標旋轉角度
    //        Quaternion toRotation = Quaternion.LookRotation(direction);

    //        // 使用DoTween實現在0.75秒內完成的旋轉動畫
    //        transform.DORotateQuaternion(toRotation, maxLookAtRotationDelta.Value);
    //    }

    //}
    public override TaskStatus OnUpdate()
    {
        var position = Target();

        //面向目標並進行移動
        transform.position = Vector3.MoveTowards(transform.position, transform.position + SidePosValue, speed.Value * Time.deltaTime);
        if (lookAtTarget.Value && (position - transform.position).sqrMagnitude > 0.01f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(new Vector3(position.x, 0, position.z) - new Vector3(transform.position.x, 0, transform.position.z)), maxLookAtRotationDelta.Value);
        }
        return TaskStatus.Running;
    }
    // Return targetPosition if targetTransform is null
    private Vector3 Target()
    {
        if (targetObj == null || targetObj.Value == null)
        {
            //如果對象為null則設定目標位置為(0,0,0)並傳回錯誤訊息
            Debug.LogError("目標為null，請設定目標");
            return Vector3.zero;
        }
        return targetObj.Value.transform.position;
    }
    // Reset the public variables
    public override void OnReset()
    {
        lookAtTarget = true;
        SidePosValue = Vector3.zero;
    }
}
