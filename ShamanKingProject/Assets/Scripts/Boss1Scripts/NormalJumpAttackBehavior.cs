using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Datamanager;

public class NormalJumpAttackBehavior : MonoBehaviour
{
    void Start()
    {
        GameManager.Instance.HellDogGameEvent.SetSubscribe(GameManager.Instance.HellDogGameEvent.OnBossCallJumpAttackLocate, cmd => { spawnAttackManager(); });
    }

    

   void spawnAttackManager()
    {
        var attackManagerPrefab = GameContainer.Get<DataManager>().GetDataByID<GameEffectTemplete>(20).PrefabPath;
        //var pos = GameManager.Instance.MainGameMediator.RealTimePlayerData.PlayerGameObject.transform.position;
        var pos = transform.position;
        pos.y = 12f;
        var attackManagerObject = Instantiate(attackManagerPrefab,pos,Quaternion.identity);
    }
}
