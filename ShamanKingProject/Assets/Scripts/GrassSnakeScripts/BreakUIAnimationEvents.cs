using Gamemanager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakUIAnimationEvents : MonoBehaviour
{
    public void CallHitSoundEffect()
    {
        GameManager.Instance.MainGameEvent.Send(new GameCallSoundEffectGenerate() { SoundEffectID = 11 });
    }
}
