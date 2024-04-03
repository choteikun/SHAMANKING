using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;


public class TimelineTester : MonoBehaviour
{
    public PlayableDirector PlayableDirector;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("TimelineFinish", (float)PlayableDirector.duration);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void TimelineFinish()
    {
        Debug.LogError("Timeline End");
    }
}
