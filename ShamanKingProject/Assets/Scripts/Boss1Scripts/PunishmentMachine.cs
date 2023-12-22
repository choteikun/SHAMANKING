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
    [SerializeField] int centerHintFrame_;
    [SerializeField] int outerHintFrame_;

    void Start()
    {
        punishmentAttack();
    }

    async void punishmentAttack()
    {
        centerHint_.SetActive(true);
        await UniTask.DelayFrame(centerHintFrame_);
        centerHint_.SetActive(false);
        centerHitBox_.SetActive(true);
        await UniTask.DelayFrame(3);
        Destroy(centerHitBox_);
        outerHint_.SetActive(true);
        await UniTask.DelayFrame(outerHintFrame_);
        outerHint_.SetActive(false);
        outerHitBox_.SetActive(true);
        await UniTask.DelayFrame(3);
        Destroy(outerHitBox_);
        await UniTask.DelayFrame(3);
        Destroy(gameObject);
    }
}
