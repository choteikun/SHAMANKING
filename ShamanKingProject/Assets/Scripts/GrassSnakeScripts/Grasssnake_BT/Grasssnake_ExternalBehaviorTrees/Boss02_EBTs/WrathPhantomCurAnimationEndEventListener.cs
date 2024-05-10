using BehaviorDesigner.Runtime.Tasks;
using Gamemanager;

[TaskCategory("Snake")]
public class WrathPhantomCurAnimationEndEventListener : Action
{
    private System.IDisposable onNextSubscription;
    bool success = false;
    public override void OnStart()
    {
        success = false;
        onNextSubscription = GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnBossCurAnimationEnd, cmd => { success = true; });
    }
    public override TaskStatus OnUpdate()
    {
        if (success)
        {
            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }
    public void OnNext(PlayerAnimationEventsCommand cmd)
    {
        success = true;
    }
    public override void OnEnd()
    {
        //在任務結束時，取消事件訂閱以避免記憶體洩漏
        if (onNextSubscription != null)
        {
            onNextSubscription.Dispose();
        }
    }
}
