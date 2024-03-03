using Gamemanager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCutSceneWalker : MonoBehaviour
{
    public void CallBossCutScene()
    {
        GameManager.Instance.MainGameEvent.Send(new CallBossSceneCutSceneStart());
    }

    public void PlayerControlSwitch()
    {
        GameManager.Instance.MainGameEvent.Send(new CutSceneOverStartControlCommand());
    }

    public void SwitchToBossPlayScene()
    {
        SceneManager.LoadScene("0301BossPlayScene");
    }
}
