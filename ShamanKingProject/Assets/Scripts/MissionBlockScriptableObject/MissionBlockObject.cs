using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "MissionBlock")]
[System.Serializable]
public class MissionBlockObject : ScriptableObject
{
    public string MissionName;
    public int MissionId;

    [TextAreaAttribute]
    public string MissionDescription;
}
