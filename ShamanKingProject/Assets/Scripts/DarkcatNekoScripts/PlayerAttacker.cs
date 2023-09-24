using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    [SerializeField] GameObject playerLightAttackHitBox_;

    public void Start()
    {
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerAttackCallHitBox, cmd => { activateHitBox(); });
    }
    
    async void activateHitBox()
    {
        playerLightAttackHitBox_.SetActive(true);
        await UniTask.Delay(200);
        playerLightAttackHitBox_.SetActive(false);
    }
}
