using Gamemanager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

public class BossSceneTransfer : EnviormentMachineBehaviorBase
{
    public void ToBossPlayScene()
    {
        SceneManager.LoadScene("SnakeBossTestScene");
    }
    public void ToBossScene()
    {
        SceneManager.LoadScene("0229MainSceneF1");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.MainGameEvent.Send(new PlayerEnterOrLeaveEnviormentObjectCommand() { EnterOrLeave = true, NowEnterEnviormentObject = this.gameObject });
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            onExit();
        }
    }

    void onExit()
    {
        GameManager.Instance.MainGameEvent.Send(new PlayerEnterOrLeaveEnviormentObjectCommand() { EnterOrLeave = false, NowEnterEnviormentObject = this.gameObject });
    }
    async public override void EnviormaneMachinePossessInteract()
    {
        GameManager.Instance.MainGameEvent.Send(new SystemCallSceneFadeOutCommand());
        await UniTask.Delay(1750);
        ToBossScene();
        onExit();
        //this.gameObject.SetActive(false);
    }
}
