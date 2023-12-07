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
    void Start()
    {
        
    }

    // Update is called once per frame
    async void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            Instantiate(spawnEffect_, this.gameObject.transform.position, this.gameObject.transform.rotation);
            await UniTask.Delay((int)(delaySecond_*1000));
            Instantiate(ghostPrefab_, this.gameObject.transform.position, this.gameObject.transform.rotation);
        }
    }
}
