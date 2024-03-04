using Gamemanager;
using PixelCrushers.DialogueSystem;
using UnityEngine;

public class DialogueManagerLUA : MonoBehaviour
{

    void Start()
    {
        Lua.RegisterFunction("ChangeContinueButtonToAlways", this, SymbolExtensions.GetMethodInfo(() => ChangeContinueButtonToAlways()));
        Lua.RegisterFunction("ChangeContinueButtonToNever", this, SymbolExtensions.GetMethodInfo(() => ChangeContinueButtonToNever()));
        Lua.RegisterFunction("ChangeContinueButtonToOptional", this, SymbolExtensions.GetMethodInfo(() => ChangeContinueButtonToOptional()));
        Lua.RegisterFunction("CallTutorialSystem", this, SymbolExtensions.GetMethodInfo(() => CallTutorialSystem(0)));
        Lua.RegisterFunction("CallFirstSceneCameraTransfer", this, SymbolExtensions.GetMethodInfo(() => CallFirstSceneCameraTransfer(0)));
        Lua.RegisterFunction("CallFirstSceneCameraTransferBack", this, SymbolExtensions.GetMethodInfo(() => CallCameraTransferBack()));
        Lua.RegisterFunction("CallScene1Wave", this, SymbolExtensions.GetMethodInfo(() => CallScene1Wave(0)));
        Lua.RegisterFunction("CallMissionUIUpdate", this, SymbolExtensions.GetMethodInfo(() => CallMissionUIUpdate(0)));
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
    public void CallFirstSceneCameraTransfer(float firstSceneCameraTransfer)//�n�O�o�����W�O
    {
        GameManager.Instance.MainGameEvent.Send(new SystemCallFirstSceneCameraTransferCommand() { CameraId = firstSceneCameraTransfer });
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

    public void CallMissionUIUpdate(float missionID)
    {
        GameManager.Instance.UIGameEvent.Send(new SystemCallMissionUIUpdateCommand() { MissionData = GameManager.Instance.MissionBlockDatabase.Database[(int)missionID] });
    }

}
