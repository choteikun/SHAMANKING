using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Datamanager;
using Cysharp.Threading.Tasks;

public class FollowAttackBehavior : MonoBehaviour
{
    float smoothSpeed_ = 0f;
    async void Start()
    {
        DOTween.To(() => smoothSpeed_, x => smoothSpeed_ = x, 15, 15f);
        await UniTask.Delay(6000);
        spawnAttackColliderPrefab();
        Destroy(gameObject);
    }

    private void LateUpdate()
    {
        follower();
    }

    void follower()
    {
        var target = GameManager.Instance.MainGameMediator.RealTimePlayerData.PlayerGameObject;
        if (target != null)
        {

        transform.position = Vector3.Lerp(transform.position, target.transform.position, smoothSpeed_ * Time.deltaTime);
        }
    }

    void spawnAttackColliderPrefab()
    {
        var prefab = GameContainer.Get<DataManager>().GetDataByID<GameEffectTemplete>(10).PrefabPath;
        Instantiate(prefab,this.gameObject.transform.position,Quaternion.identity);
    }
         
}
