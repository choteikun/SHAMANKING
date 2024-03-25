using Gamemanager;
using PixelCrushers.DialogueSystem;
using UnityEngine;

public class PlayerDialogueSystemSkipSystem : MonoBehaviour
{
    [SerializeField] string nowActiveString_;
    [SerializeField] bool activating_;

    void Start()
    {
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerSkipConversation, cmd => { skipConversation(); });
    }

    // Update is called once per frame
    void Update()
    {
        if (DialogueManager.instance.activeConversation != null)
        {
            nowActiveString_ = DialogueManager.instance.activeConversation.conversationTitle;
            activating_ = true;
        }
        else
        {
            activating_ = false;
        }
    }

    void skipConversation()
    {
        Debug.Log("Activated");
        if (!activating_) return;
        var stopedConversation = nowActiveString_;
        DialogueManager.StopConversation();
        switch(stopedConversation)
        {
            case "¶}§½¤¶²Ð":
                GameManager.Instance.MainGameEvent.Send(new GameConversationEndCommand());
                GameManager.Instance.MainGameEvent.Send(new SystemCallTutorialCommand() { TutorialID = 0 });
                return;
            case "chapter1-1-1":
                GameManager.Instance.MainGameEvent.Send(new SystemCallCameraTransferBackCommand());
                GameManager.Instance.MainGameEvent.Send(new SystemCallTutorialCommand() { TutorialID = 2 });
                DialogueManager.StopConversation();
                return;
            case "chapter 1_1_2":
                DialogueManager.StopConversation();
                GameManager.Instance.MainGameEvent.Send(new GameConversationEndCommand());
                return;
            case "chapter 1_2_1":
                GameManager.Instance.MainGameEvent.Send(new GameConversationEndCommand());
                GameManager.Instance.MainGameEvent.Send(new SystemCallTutorialCommand() { TutorialID = 1 });
                DialogueManager.StopConversation();
                return;
            case "chapter1_2_2":
                GameManager.Instance.MainGameEvent.Send(new GameConversationEndCommand());
                DialogueManager.StopConversation();
                return;
            case "Chapter 1_3.5_1":
                GameManager.Instance.MainGameEvent.Send(new GameConversationEndCommand());
                DialogueManager.StopConversation();
                return;
            case "Chapter 1_4_1":
                GameManager.Instance.MainGameEvent.Send(new GameConversationEndCommand());
                GameManager.Instance.MainGameEvent.Send(new SystemCallTutorialCommand() { TutorialID = 3 });
                DialogueManager.StopConversation();
                return;
            case "chapter 1_4_3":
                GameManager.Instance.MainGameEvent.Send(new GameConversationEndCommand());
                DialogueManager.StopConversation();
                return;

        }
    }
}
