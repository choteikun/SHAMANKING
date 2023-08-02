using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using PixelCrushers.DialogueSystem;

namespace BehaviorDesigner.Runtime.Tasks.DialugeSystem
{
    [TaskDescription("Sets the value of a Dialogue System variable.")]
    [TaskCategory("Dialogue System")]
    [HelpURL("https://www.opsive.com/support/documentation/behavior-designer/integrations/dialogue-system/")]
    [TaskIcon("Assets/Behavior Designer/Integrations/Dialogue System/Editor/DialogueSystemIcon.png")]
    public class SetDialogueFloatVariable : Action
    {
        [VariablePopup]
        public SharedString variableName;
        public SharedFloat value;

        public override TaskStatus OnUpdate()
        {
            if (variableName == null || string.IsNullOrEmpty(variableName.Value)) {
                Debug.LogWarning("SetDialogueFloatVariable Task: Variable Name is null or blank");
                return TaskStatus.Failure;
            }
            DialogueLua.SetVariable(variableName.Value, value.Value);
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            variableName = "";
            value = 0;
        }
    }
}