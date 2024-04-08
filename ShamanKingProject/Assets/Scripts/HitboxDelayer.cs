using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxDelayer : MonoBehaviour
{
    [SerializeField] int spawnFrame_;
    [SerializeField] GameObject target_;
    private async void Start()
    {
        await UniTask.DelayFrame(spawnFrame_);
        target_.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
