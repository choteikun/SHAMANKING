using Gamemanager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitableItemTest : MonoBehaviour
{
    [field:SerializeField] public GameObject onHitPoint_ { get; private set; }
    [field: SerializeField] public HitObjecctTag HitTag { get; private set; }
}
