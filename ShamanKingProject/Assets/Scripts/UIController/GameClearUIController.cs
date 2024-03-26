using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClearUIController : MonoBehaviour
{
    [SerializeField] GameObject gameClearUI_;

    private void Start()
    {
        GameManager.Instance.HellDogGameEvent.SetSubscribe(GameManager.Instance.HellDogGameEvent.OnBossCallDeadCommand, cmd => { gameClearUI_.SetActive(true); });
    }

}