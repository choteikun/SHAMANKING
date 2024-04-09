using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicAnimationController : MonoBehaviour
{
    public void StartCinematicConvertion(string name)
    {
        DialogueManager.instance.StartConversation(name);
    }
}
