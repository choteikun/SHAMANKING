using PixelCrushers.DialogueSystem;
using UnityEngine;
using Gamemanager;

public class DialogueManagerLUA : MonoBehaviour
{

    void Start()
    {
        Lua.RegisterFunction("ChangeContinueButtonToAlways", this, SymbolExtensions.GetMethodInfo(() => ChangeContinueButtonToAlways()));
        Lua.RegisterFunction("ChangeContinueButtonToNever", this, SymbolExtensions.GetMethodInfo(() => ChangeContinueButtonToNever()));
        Lua.RegisterFunction("ChangeContinueButtonToOptional", this, SymbolExtensions.GetMethodInfo(() => ChangeContinueButtonToOptional()));
        Lua.RegisterFunction("CallTutorialSystem", this, SymbolExtensions.GetMethodInfo(() =>CallTutorialSystem(0) ));
    }

    public void ChangeContinueButtonToAlways()
    {
        DialogueManager.instance.displaySettings.subtitleSettings.continueButton = DisplaySettings.SubtitleSettings.ContinueButtonMode.Always;
    }

    public void ChangeContinueButtonToNever()
    {
        DialogueManager.instance.displaySettings.subtitleSettings.continueButton = DisplaySettings.SubtitleSettings.ContinueButtonMode.Never;
    }

    public void ChangeContinueButtonToOptional()
    {
        DialogueManager.instance.displaySettings.subtitleSettings.continueButton = DisplaySettings.SubtitleSettings.ContinueButtonMode.Optional;
    }

    public void CallTutorialSystem(float tutorialNum)
    {
        GameManager.Instance.MainGameEvent.Send(new SystemCallTutorialCommand() { TutorialID = tutorialNum });
        DialogueManager.StopConversation();
    }
}
