using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine;

[TaskCategory("Snake")]
[TaskDescription("受擊判斷.")]
public class HasTakenDamage : Conditional
{
    public SharedBool EnemyHurt;
    //private int damageFrame = -1;


    public override TaskStatus OnUpdate()
    {
        if (EnemyHurt.Value)
        {
            Debug.LogError("trigger!!!");
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
        //if (damageFrame >= Time.frameCount - 1) 
        //{
        //    return TaskStatus.Success;
        //}
        //return TaskStatus.Failure;
    }
    //void OnDamage()
    //{
    //    damageFrame = Time.frameCount;
    //}
}
