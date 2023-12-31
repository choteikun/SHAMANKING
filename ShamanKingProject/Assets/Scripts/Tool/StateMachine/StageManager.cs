public class StageManager
{
    public StateBase CurrentState;
    /// <summary>
    /// switch State
    /// </summary>
    public virtual void TransitionState(string stateName, StageData stageData)
    {
        if (CurrentState != null)
        {
            CurrentState.OnExit();
        }

        changeAndNewState(stateName, stageData);


        CurrentState.OnEnter();
    }
    public virtual void TransitionState(string stateName)
    {
        var stagedata = new StageData();
        if (CurrentState != null)
        {
            CurrentState.OnExit();
        }

        changeAndNewState(stateName, stagedata);


        CurrentState.OnEnter();



    }


    protected virtual void changeAndNewState(string stateName, StageData stageData)
    {


    }
    public virtual void StageManagerInit()
    {

    }
    public void StageManagerUpdate()
    {
        if (CurrentState != null)
        {
            CurrentState.OnUpdate();
        }
    }
}
public struct StageData
{
    public bool GameResult { get; set; }

    public static StageData GetGameOverStageData(bool result)
    {
        return new StageData { GameResult = result };
    }
}

