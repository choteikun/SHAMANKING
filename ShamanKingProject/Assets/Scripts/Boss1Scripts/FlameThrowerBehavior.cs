using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrowerBehavior : MonoBehaviour
{
    [SerializeField] GameObject flameThrowerManagerCenter_;
    [SerializeField] GameObject bossPos_;
    [SerializeField] float centerDistance_;
    [SerializeField] bool isFlameThrowing = false;
    [SerializeField] int frameCounter = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        throwerManagerPosUpdater();
    }
    private void FixedUpdate()
    {
        
    }
    void throwerManagerPosUpdater()
    {
        var dir = bossPos_.transform.forward.normalized;
        dir.y = 0;
        flameThrowerManagerCenter_.transform.position = bossPos_.transform.position + dir * centerDistance_;
        flameThrowerManagerCenter_.transform.rotation = bossPos_.transform.rotation;
    }
    void hitboxSpawner()
    {
        if (isFlameThrowing) 
        {
            frameCounter += 1;
            if (frameCounter == 4)
            {
                var hitbox = 
            }
        }
    }
}
