using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleDestroy : MonoBehaviour
{
    [SerializeField] float destroyDelayTimer_;
    void Start()
    {
        Destroy(gameObject,destroyDelayTimer_);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
