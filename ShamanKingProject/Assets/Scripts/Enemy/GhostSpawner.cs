using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

public class GhostSpawner : MonoBehaviour
{
    [SerializeField] GameObject ghostPrefab_;
    [SerializeField] float delaySecond_;
    [SerializeField] GameObject spawnEffect_;

    async public void SpawnGhost()
    {
        Instantiate(spawnEffect_, this.gameObject.transform.position, this.gameObject.transform.rotation);
        await UniTask.Delay((int)(delaySecond_ * 1000));
        Instantiate(ghostPrefab_, this.gameObject.transform.position, this.gameObject.transform.rotation);
    }
}
