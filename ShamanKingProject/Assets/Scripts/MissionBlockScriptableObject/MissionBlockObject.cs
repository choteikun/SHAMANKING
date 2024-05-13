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

    [SerializeField]
    public Sprite InputTutorialPicture;
    [SerializeField]
    public Sprite InputTutorialPicture_PS;
    [SerializeField]
    public Sprite InputTutorialPicture_XB;
}
