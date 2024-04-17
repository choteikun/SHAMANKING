using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSceneUltCamTransfer : MonoBehaviour
{
    [SerializeField] GameObject  mainCam_;
    [SerializeField] GameObject bossUltCam_;
    void Start()
    {
        GameManager.Instance.HellDogGameEvent.SetSubscribe(GameManager.Instance.HellDogGameEvent.OnBossCallUltCamTransfer, cmd =>{
            if (cmd.trigger)
            {
                bossCallUltCamTransfer();
            }
            else 
            {
                bossCallUltCamTransferBack();
            }
        });
    }

    void bossCallUltCamTransfer()
    {
        mainCam_.SetActive(false);
        bossUltCam_.SetActive(true);
    }
    void bossCallUltCamTransferBack()
    {
        mainCam_.SetActive(true);
        bossUltCam_.SetActive(false);
    }
}
