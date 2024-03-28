using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerEffectManager : MonoBehaviour
{
    [SerializeField]
    GameObject playerEffectObjsRoot_;
    [SerializeField]
    VisualEffect[] playerEffects_;

    void Start()
    {
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerHeavyAttack, cmd => {  });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerHeavyAttackChargeFinish, cmd => { });
        if (playerEffectObjsRoot_ == null)
        {
            playerEffectObjsRoot_ = GameObject.Find("PlayerEffectObjs").gameObject;

            playerEffects_ = new VisualEffect[playerEffectObjsRoot_.transform.childCount];

            for (int i = 1; i <= playerEffectObjsRoot_.transform.childCount; i++)
            {
                playerEffects_[i - 1] = playerEffectObjsRoot_.transform.GetChild(i - 1).GetComponent<VisualEffect>();
                playerEffects_[i - 1].Stop();
            }
        }
    }
    void playerHeavyAttackEffectSpawn()
    {
        playerEffects_[0].Play();

    }
    void playerHeavyAttackEffectSuccess()
    {

    }
    void playerHeavyAttackEffectComplete()
    {

    }

    void Update()
    {
        
    }
}
