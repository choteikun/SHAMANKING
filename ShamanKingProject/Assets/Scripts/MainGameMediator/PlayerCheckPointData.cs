using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerCheckPointData
{
    [SerializeField] public bool[] PlayerClearedWave = new bool[6];
}
