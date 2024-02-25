using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamemanager;

public class PlayerCutSceneWalker : MonoBehaviour
{
   public void CallBossCutScene()
    {
        GameManager.Instance.MainGameEvent.Send(new CallBossSceneCutSceneStart());
    }
}
