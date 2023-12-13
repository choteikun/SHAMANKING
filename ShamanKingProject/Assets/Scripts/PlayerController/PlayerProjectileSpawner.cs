using Cysharp.Threading.Tasks;
using Datamanager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectileSpawner : MonoBehaviour
{
    
    async void Start()
    {
        await UniTask.DelayFrame(5);
        var TamaPrefab = GameContainer.Get<DataManager>().GetDataByID<GameEffectTemplete>(13).PrefabPath;
        var fireballObject = Instantiate(TamaPrefab, transform.position, transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
