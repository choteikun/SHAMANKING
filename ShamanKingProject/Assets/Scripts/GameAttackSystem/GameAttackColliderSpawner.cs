using Datamanager;
using Gamemanager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAttackColliderSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] attackAnimationUsedTargerPoints_;
    [SerializeField] AttackColliderType colliderType_;
    void Start()
    {
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnAnimationCallAttackEffect, cmd => { spawnAttackEffect(cmd); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnAnimationCallRepeatShootAttack, cmd => { spawnRandomShootAttackEffect(cmd); });
    }

    void spawnAttackEffect(AnimationCallAttackEffectCommand command)
    {
        if (command.CommandSender == this.gameObject && command.AttackColliderType == colliderType_)
        {
            var prefab = GameContainer.Get<DataManager>().GetDataByID<GameEffectTemplete>(command.SpawnEffectId).PrefabPath;
            var transform = attackAnimationUsedTargerPoints_[command.ColliderId].transform;
            var effectObj = Instantiate(prefab, transform.position, transform.rotation);
        }
    }
    void spawnRandomShootAttackEffect(AnimationCallRepeatShootAttackCommand command)
    {
        if (command.CommandSender == this.gameObject && command.AttackColliderType == colliderType_)
        {
            var prefab = GameContainer.Get<DataManager>().GetDataByID<GameEffectTemplete>(command.SpawnEffectId).PrefabPath;
            var transform = attackAnimationUsedTargerPoints_[command.ColliderId].transform;
            // 在球體上取得一個隨機點
            Vector3 randomPoint = Random.onUnitSphere;

            // 將Y-Z平面上的點投影到物件A所在的平面上
            Vector3 randomPointOnPlane = new Vector3(0, randomPoint.y, randomPoint.z);

            // 根據距離R進行縮放
           Vector3 finalPoint = transform.position + randomPointOnPlane * 0.35f;
            var effectObj = Instantiate(prefab, finalPoint, transform.rotation);
        }
    }
}
