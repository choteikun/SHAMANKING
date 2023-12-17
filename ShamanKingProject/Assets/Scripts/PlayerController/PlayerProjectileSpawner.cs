using Cysharp.Threading.Tasks;
using Datamanager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectileSpawner : MonoBehaviour
{
    [SerializeField] int shootDelay_;
    [SerializeField] int projectileID_;
    async void Start()
    {
        await UniTask.DelayFrame(shootDelay_);
        var TamaPrefab = GameContainer.Get<DataManager>().GetDataByID<GameEffectTemplete>(projectileID_).PrefabPath;
        var fireballObject = Instantiate(TamaPrefab, transform.position, transform.rotation);
    }
    
}
