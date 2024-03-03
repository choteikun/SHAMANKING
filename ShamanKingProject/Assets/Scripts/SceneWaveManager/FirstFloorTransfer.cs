using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstFloorTransfer : MonoBehaviour
{
    public void ToFirstFloorScene()
    {
        SceneManager.LoadScene("0220MainScene 1");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ToFirstFloorScene();
        }
    }
}
