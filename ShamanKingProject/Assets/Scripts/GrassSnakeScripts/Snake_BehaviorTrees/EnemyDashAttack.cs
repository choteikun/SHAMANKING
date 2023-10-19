using UnityEngine;
using System.Collections;
using BehaviorDesigner.Runtime.Tasks;
using Cysharp.Threading.Tasks;
using TooltipAttribute = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

[TaskCategory("Snake")]
[TaskDescription("敵人衝刺功能")]
public class EnemyDashAttack : Action
{
    [Tooltip("衝刺時間")]
    public float DashTime;
    [Tooltip("衝刺速度")]
    public float DashSpeed;
    [Tooltip("Should the value be reverted back to its original value after it has been set?")]
    public bool setOnce;

    bool isDashing;


    public override TaskStatus OnUpdate()
    {
        if (setOnce)
        {
            Dash(transform.forward);
        }
        return TaskStatus.Success;
        
    }

    //衝刺測試
    async void Dash(Vector3 targetDir)
    {
        if (isDashing) return; // 如果已經在衝刺中，則直接返回

        isDashing = true;
        float startTime = Time.time;
        Vector3 initialPosition = transform.position;

        while (Time.time < startTime + DashTime)
        {
            float elapsedTime = Time.time - startTime;
            float dashDistance = elapsedTime * DashSpeed;

            transform.position = initialPosition + (targetDir.normalized * dashDistance);
            await UniTask.Yield();
        }
        isDashing = false;
    }
}
