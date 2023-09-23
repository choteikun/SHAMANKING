using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCubeView : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Possessable"))
        {
            Debug.Log("Hit!!" + " " + other.name);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Possessable"))
        {
            Debug.Log("Out!!" + " " + other.name);
        }
    }
}
