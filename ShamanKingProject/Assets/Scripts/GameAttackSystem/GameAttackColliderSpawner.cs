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
}
