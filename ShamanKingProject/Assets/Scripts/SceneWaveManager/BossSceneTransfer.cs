using Gamemanager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public override void EnviormaneMachinePossessInteract()
    {
        ToBossScene();
        onExit();
        //this.gameObject.SetActive(false);
    }
}
