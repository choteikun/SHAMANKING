using Gamemanager;
using UnityEngine;

public class WrathPhantomAnimationEvents : MonoBehaviour
{
    public void BossCurAnimationEnd()
    {
        GameManager.Instance.MainGameEvent.Send(new BossCurAnimationEndCommand());
    }
}
