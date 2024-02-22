using BehaviorDesigner.Runtime.Tasks;
using Gamemanager;

[TaskCategory("Snake")]
public class FireTackleUi : Action
{
    private System.IDisposable onNextSubscription;
    public override void OnStart()
    {
        GameManager.Instance.UIGameEvent.Send(new BossCallUISkillNameCommand() { SkillType = 0, Name = "可悲的入侵者啊，看你還能支撐多久！" });
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
