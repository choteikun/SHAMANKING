using BehaviorDesigner.Runtime;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class PatrolTypeEnemySupport : MonoBehaviour
{
    [Tooltip("自動生成隨機時間的移動觸發器")]
    public bool randomTimeToMoveValueTrigger;
    [Tooltip("自動生成隨機時間的發呆觸發器")]
    public bool randomTimeToDazeValueTrigger;

}
