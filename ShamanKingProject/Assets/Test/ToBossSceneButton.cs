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
    }
}
