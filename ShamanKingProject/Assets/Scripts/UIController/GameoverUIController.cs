using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameoverUIController : MonoBehaviour
{
    [SerializeField] GameObject gameoverCanvas_;

    private void Start()
    {
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnSystemCallPlayerGameover, cmd => { onGameOver(); });
    }

    void onGameOver()
    {
        gameoverCanvas_.SetActive(true);
    }
}
