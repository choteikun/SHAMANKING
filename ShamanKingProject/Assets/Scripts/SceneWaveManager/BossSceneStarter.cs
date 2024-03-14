using Gamemanager;
using UnityEngine;

public class BossSceneStarter : MonoBehaviour
{
   
    void Start()
    {
        GameManager.Instance.MainGameEvent.Send(new CallBossSceneCutSceneStart());
    }

    
}
