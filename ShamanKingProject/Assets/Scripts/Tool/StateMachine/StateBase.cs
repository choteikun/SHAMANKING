using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateBase 
{
    protected StageManager stateManager;    
    public virtual void OnEnter()
    {
        Debug.Log("EnterFreePlay");
    }

    public virtual void OnUpdate()
    {

    }

    public virtual void OnExit()
    {

    }
}
public enum State_Enum
{
    Game_Init_State,Game_Start_State,Game_FreePlay_State,Game_Fever_State,Game_Over_State,
}
