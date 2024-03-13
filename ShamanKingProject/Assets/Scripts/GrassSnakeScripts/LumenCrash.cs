using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LumenCrash : MonoBehaviour
{
    [SerializeField]
    GameObject LumenCube;

    void Start()
    {
        UniTask.Delay(2000);
        LumenCube.SetActive(true);
    }
}
