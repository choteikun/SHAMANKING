using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AttackBlockDataBase")]
[System.Serializable]
public class SO_AttackBlockDatabase : ScriptableObject
{
    public List<SO_AttackBlockBase> Database;
}
