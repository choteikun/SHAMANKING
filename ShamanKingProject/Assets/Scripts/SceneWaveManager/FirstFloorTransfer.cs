using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Gamemanager;
using Cysharp.Threading.Tasks;

public class FirstFloorTransfer : MonoBehaviour
{
    public void ToFirstFloorScene()
    {
        SceneManager.LoadScene("0220MainScene 1");
    }
    private async void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.MainGameEvent.Send(new SystemCallSceneFadeOutCommand());
            await UniTask.Delay(1750);
            ToFirstFloorScene();
        }
    }
}
