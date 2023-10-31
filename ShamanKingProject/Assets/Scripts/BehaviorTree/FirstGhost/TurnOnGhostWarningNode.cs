using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[TaskCategory("Darkcat")]
[TaskDescription("鬼回呼叫警告標示")]
public class TurnOnGhostWarningNode : Action
{
    [BehaviorDesigner.Runtime.Tasks.Tooltip("鬼魂警告特效")]
    public SharedGameObject GhostWarningParticle;
    [BehaviorDesigner.Runtime.Tasks.Tooltip("開或關")]
    public SharedBool BoolValue;
    GameObject prev_GhostWarningParticle_;


    public override void OnStart()
    {
        var particleObject = GetDefaultGameObject(GhostWarningParticle.Value);
        if (particleObject != prev_GhostWarningParticle_)
        {
            prev_GhostWarningParticle_ = particleObject;
        }
        Debug.LogWarning(BoolValue.Value.ToString());
        particleObject.SetActive(BoolValue.Value);
    }
}
