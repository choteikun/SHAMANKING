using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunishmentMachine : MonoBehaviour
{
    [SerializeField] GameObject outerHint_;
    [SerializeField] GameObject centerHint_;
    [SerializeField] GameObject outerHitBox_;
    [SerializeField] GameObject centerHitBox_;

    void Start()
    {
        punishmentAttack();
    }

    async void punishmentAttack()
    {
        centerHint_.SetActive(true);
        await UniTask.DelayFrame(30);
        centerHitBox_.SetActive(true);
        await UniTask.DelayFrame(3);
        Destroy(centerHitBox_);
        centerHint_.SetActive(false);
        outerHint_.SetActive(true);
        await UniTask.DelayFrame(30);
        outerHitBox_.SetActive(true);
        await UniTask.DelayFrame(3);
        Destroy(outerHitBox_);
        await UniTask.DelayFrame(3);
        Destroy(gameObject);
    }
}
