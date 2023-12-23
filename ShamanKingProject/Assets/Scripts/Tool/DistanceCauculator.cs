using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceCauculator : MonoBehaviour
{
    [SerializeField] GameObject aObject_;
    [SerializeField] GameObject bObject_;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var distance = aObject_.transform.position - bObject_.transform.position;
        Debug.Log(distance.magnitude);
    }
}
