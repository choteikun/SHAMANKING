using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;


[TaskCategory("Snake")]
public class GhostBeRootedAnimationEventListener : Action
{
    private System.IDisposable onNextSubscription;
    bool success = false;


    public SharedGameObject targetGameObject;

    public SharedFloat speed;

    private Animator animator;
    private GameObject prevGameObject;
    public override void OnStart()
    {
        success = false;
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerRootSuccess, cmd =>
        {
            success = true;
        });
        var currentGameObject = GetDefaultGameObject(targetGameObject.Value);
        if (currentGameObject != prevGameObject)
        {
            animator = currentGameObject.GetComponent<Animator>();
            prevGameObject = currentGameObject;
        }
    }
    public override TaskStatus OnUpdate()
    {
        if (success)
        {
            UnityEngine.Debug.Log("Help");
            animator.speed = speed.Value;

            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }
    public override void OnEnd()
    {
        // 在任务结束时，取消事件订阅以避免内存泄漏
        if (onNextSubscription != null)
        {
            onNextSubscription.Dispose();
        }
    }
    public override void OnReset()
    {
        targetGameObject = null;
        speed = 0;
    }
}
