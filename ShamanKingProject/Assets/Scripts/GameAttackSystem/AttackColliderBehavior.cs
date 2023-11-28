using Cysharp.Threading.Tasks;
using Gamemanager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackColliderBehavior : MonoBehaviour
{
    [SerializeField] LayerMask layerMask_;

    [SerializeField] int lastFrame_;

    [SerializeField] Collider collidePoint_;
    private async void Start()
    {
        await UniTask.DelayFrame(lastFrame_);
        Destroy(this.gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        // 检查collider是否在LayerMask中
        if ((layerMask_.value & (1 << other.gameObject.layer)) > 0)
        {
            //var collidePoint = collidePoint_.ClosestPoint(other.transform.position);
            var collidePoint = other.ClosestPoint(this.gameObject.transform.position);
            GameManager.Instance.MainGameEvent.Send(new PlayerAttackSuccessCommand() { CollidePoint = collidePoint, AttackTarget = other.gameObject, AttackDamage = 20f });
        }
    }
}
