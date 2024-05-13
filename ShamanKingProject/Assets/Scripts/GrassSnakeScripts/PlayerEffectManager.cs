using Gamemanager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerEffectManager : MonoBehaviour
{
    [SerializeField]
    GameObject playerChargeEffectObjsRoot_;
    [SerializeField]
    VisualEffect[] playerChargeEffects_;

    void Start()
    {
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerChargeSwitch, onPlayerChargeSwitch);
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerHeavyAttackChargeFinish, cmd => {playerHeavyAttackEffectComplete(); });
        
        if (playerChargeEffectObjsRoot_ == null)
        {
            try
            {
                playerChargeEffectObjsRoot_ = GameObject.Find("PlayerChargeEffectObjs").gameObject;
            }
            catch (NullReferenceException)
            {
                Debug.LogError("可能是蓄力特效遺失，請將Assets/GameEffect/Girl_VFX/Charge/PlayerChargeEffectObjs.prefab 裝在PlayerCharacter裡的forearm.L(左手腕)上");
                throw;
            }

            playerChargeEffects_ = new VisualEffect[playerChargeEffectObjsRoot_.transform.childCount];

            for (int i = 1; i <= playerChargeEffectObjsRoot_.transform.childCount; i++)
            {
                playerChargeEffects_[i - 1] = playerChargeEffectObjsRoot_.transform.GetChild(i - 1).GetComponent<VisualEffect>();
                playerChargeEffects_[i - 1].Stop();
            }
        }
    }
    void onPlayerChargeSwitch(PlayerChargeSwitchCommand playerChargeSwitchCommand)
    {
        if (playerChargeSwitchCommand.Switch)
        {
            //蓄力特效播放
            playerChargeEffects_[0].Play();
            
        }
        else
        {
            ////蓄力特效結束
            playerChargeEffects_[0].Stop();
            playerChargeEffects_[2].Stop();
        }
    }
    void playerHeavyAttackEffectComplete()
    {
        //蓄力完成瞬間的提示特效播放
        playerChargeEffects_[1].Play();
        //蓄力完成後的特效播放
        playerChargeEffects_[2].Play();
        //蓄力完成瞬間的提示音效播放
        GameManager.Instance.MainGameEvent.Send(new GameCallSoundEffectGenerate() { SoundEffectID = 5 });
    }

    void Update()
    {

    }
}
