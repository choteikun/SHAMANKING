using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CheckPoint/CheckPointBlock")]
[System.Serializable]
public class CheckPointDataBlock : ScriptableObject
{
    [SerializeField] public string SceneName;
    [SerializeField] public int WaveID;
    [SerializeField] public Vector3 PlayerTransformPosition;
    [SerializeField] public Vector3 PlayerTransformRotation;
    [SerializeField] public Vector3 GhostTransformPosition;
    [SerializeField] public Vector3 GhostTransformRotation;
}
