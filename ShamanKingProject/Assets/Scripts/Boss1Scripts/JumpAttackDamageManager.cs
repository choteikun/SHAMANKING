using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAttackDamageManager : MonoBehaviour
{
    [SerializeField] GameObject jumpAttackHint_;
    [SerializeField] GameObject jumpAttackHitbox_;
    [SerializeField] int hintFrame_;
    void Start()
    {
        startJumpAttackFunction();
    }

    
    async void startJumpAttackFunction()
    {
        jumpAttackHint_.SetActive(true);
        await UniTask.DelayFrame(hintFrame_);
        jumpAttackHint_.SetActive(false);
        jumpAttackHitbox_.SetActive(true);
        await UniTask.DelayFrame(3);
        Destroy(gameObject);
    }
}
