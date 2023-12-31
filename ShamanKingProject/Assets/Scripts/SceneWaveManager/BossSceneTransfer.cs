using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossSceneTransfer : MonoBehaviour
{
    public void ToBossScene()
    {
        SceneManager.LoadScene("SnakeBossTestScene");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ToBossScene();
        }
    }
}
