using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToBossSceneButton : MonoBehaviour
{
    public void ToBossScene()
    {
        SceneManager.LoadScene("SnakeBossTestScene");
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            ToBossScene();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            GameManager.Instance.MainGameMediator.RealTimePlayerData.ToFloor1_ = true;
            SceneManager.LoadScene("0220MainScene 1");
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            GameManager.Instance.MainGameMediator.RealTimePlayerData.ToFloor1_ = true;
            SceneManager.LoadScene("0301BossPlayScene");
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            SceneManager.LoadScene("0229MainSceneF1");
        }
    }
}
