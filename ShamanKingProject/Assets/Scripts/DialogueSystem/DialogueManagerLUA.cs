using PixelCrushers.DialogueSystem;
using UnityEngine;
using Gamemanager;
using System.Xml.Serialization;

public class DialogueManagerLUA : MonoBehaviour
{

    void Start()
    {
        Lua.RegisterFunction("ChangeContinueButtonToAlways", this, SymbolExtensions.GetMethodInfo(() => ChangeContinueButtonToAlways()));
        Lua.RegisterFunction("ChangeContinueButtonToNever", this, SymbolExtensions.GetMethodInfo(() => ChangeContinueButtonToNever()));
        Lua.RegisterFunction("ChangeContinueButtonToOptional", this, SymbolExtensions.GetMethodInfo(() => ChangeContinueButtonToOptional()));
        Lua.RegisterFunction("CallTutorialSystem", this, SymbolExtensions.GetMethodInfo(() =>CallTutorialSystem(0) ));
        Lua.RegisterFunction("CallFirstSceneCameraTransfer", this, SymbolExtensions.GetMethodInfo(() => CallFirstSceneCameraTransfer(0)));
        Lua.RegisterFunction("CallFirstSceneCameraTransferBack", this, SymbolExtensions.GetMethodInfo(() => CallCameraTransferBack()));
        Lua.RegisterFunction("CallScene1Wave", this, SymbolExtensions.GetMethodInfo(() => CallScene1Wave(0)));
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
    public void CallFirstSceneCameraTransfer(float firstSceneCameraTransfer)//要記得取消蓄力
    {
        GameManager.Instance.MainGameEvent.Send(new SystemCallFirstSceneCameraTransferCommand() {CameraId = firstSceneCameraTransfer });
        GameManager.Instance.MainGameEvent.Send(new SystemStopGuardingCommand()); 
    }
    public void CallCameraTransferBack()
    {
        GameManager.Instance.MainGameEvent.Send(new SystemCallCameraTransferBackCommand());
    }
    public void CallScene1Wave(float waveID)
    {
        GameManager.Instance.MainGameEvent.Send(new SystemCallWaveStartCommand() { SceneName = "Scene1", WaveID = (int)waveID });
    }

   
}
