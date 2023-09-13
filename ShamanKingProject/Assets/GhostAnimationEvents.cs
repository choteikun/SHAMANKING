using Gamemanager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostAnimationEvents : MonoBehaviour
{
    public void Ghost_Back_Start()
    {
        GameManager.Instance.MainGameEvent.Send(new GhostAnimationEventsCommand() { AnimationEventName = "GhostMat_Dissolve" });
    }
    public void Ghost_Back_End()
    {
        GameManager.Instance.MainGameEvent.Send(new GhostAnimationEventsCommand() { AnimationEventName = "GhostMat_Revert" });
    }

}
