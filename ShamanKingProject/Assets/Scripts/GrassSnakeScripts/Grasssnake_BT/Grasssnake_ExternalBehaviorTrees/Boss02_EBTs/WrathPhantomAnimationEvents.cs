using Gamemanager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrathPhantomAnimationEvents : MonoBehaviour
{
    public void BossCurAnimationEnd()
    {
        GameManager.Instance.MainGameEvent.Send(new BossCurAnimationEndCommand());
    }
}
