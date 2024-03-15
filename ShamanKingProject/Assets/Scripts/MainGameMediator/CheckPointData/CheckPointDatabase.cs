using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CheckPoint/CheckPointDatabase")]
[System.Serializable]
public class CheckPointDatabase : ScriptableObject
{
    public List<CheckPointDataBlock> Database;
}
