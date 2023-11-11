using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimator;
using BehaviorDesigner.Runtime;
using UnityEngine;
using Gamemanager;


[TaskCategory("Snake")]
public class GhostBeGrabedAnimationEventListener : Action
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
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerAnimationEvents, cmd =>
        {
            if (cmd.AnimationEventName == "Player_Pull_Finish")
            {
                success = true;
            }
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
