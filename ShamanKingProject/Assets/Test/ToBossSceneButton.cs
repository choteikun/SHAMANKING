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
            SceneManager.LoadScene("0220MainScene 1");
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            SceneManager.LoadScene("0301BossPlayScene");
        }
    }
}
