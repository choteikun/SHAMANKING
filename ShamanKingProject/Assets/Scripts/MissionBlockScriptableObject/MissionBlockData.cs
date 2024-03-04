using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MissionBlockData")]
public class MissionBlockData : ScriptableObject
{
    public List<MissionBlockObject> Database;
}
