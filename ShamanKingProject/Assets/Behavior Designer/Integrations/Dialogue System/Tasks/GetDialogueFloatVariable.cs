using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using PixelCrushers.DialogueSystem;

namespace BehaviorDesigner.Runtime.Tasks.DialugeSystem
{
    [TaskDescription("Gets the value of a Dialogue System variable as a float.")]
    [TaskCategory("Dialogue System")]
    [HelpURL("https://www.opsive.com/support/documentation/behavior-designer/integrations/dialogue-system/")]
    [TaskIcon("Assets/Behavior Designer/Integrations/Dialogue System/Editor/DialogueSystemIcon.png")]
    public class GetDialogueFloatVariable: Action
    {
        [VariablePopup]
        public SharedString variableName;
        [Tooltip("Store the result in a Float variable")]
        public SharedFloat storeResult;

        public override TaskStatus OnUpdate()
        {
            if ((variableName == null) || (string.IsNullOrEmpty(variableName.Value))) {
                Debug.LogWarning("GetDialogueFloatVariable Task: Variable Name is null or empty");
                return TaskStatus.Failure;
            }
            if (storeResult != null) {
                storeResult.Value = DialogueLua.GetVariable(variableName.Value).asFloat;
            }
            return TaskStatus.Success;
        }


        public override void OnReset()
        {
            variableName = "";
            storeResult = 0;
        }
    }
}
