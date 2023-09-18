using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

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
        switch (stateName)
        {
            //case State_Enum.Game_Init_State:
            //CurrentState = new GameInitState(this);
            //    return;
            //case State_Enum.Game_Start_State:
            //    CurrentState = new GameStartState(this);
            //    return;
            //case State_Enum.Game_FreePlay_State:
            //    CurrentState = new GameFreePlayState(this);
            //    return;
            //case State_Enum.Game_Fever_State:
            //    CurrentState = new GameFeverState(this);
            //    return;
            //case State_Enum.Game_Over_State:
            //    CurrentState = new GameOverState(this,stageData);
            //    return;
        }

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

