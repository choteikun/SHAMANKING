using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using Gamemanager;

public class PlayerDialogueSystem : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Lua.RegisterFunction("ConversationEnd", this, SymbolExtensions.GetMethodInfo(() => SendConversationEndMessage()));
    }

    public void SendConversationEndMessage()
    {
        GameManager.Instance.MainGameEvent.Send(new GameConversationEndCommand());
        DialogueManager.StopConversation();
    }
}
