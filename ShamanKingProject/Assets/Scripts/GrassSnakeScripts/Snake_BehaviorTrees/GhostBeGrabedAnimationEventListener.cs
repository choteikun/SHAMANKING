using BehaviorDesigner.Runtime.Tasks;
using Gamemanager;

[TaskCategory("Snake")]
public class GhostBeGrabedAnimationEventListener : Action
{
    private System.IDisposable onNextSubscription;
    bool success = false;
    public override void OnStart()
    {
        success = false;
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerGrabSuccessForPlayer, cmd => { success = true; });
    }
    public override TaskStatus OnUpdate()
    {
        if (success)
        {
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
}
