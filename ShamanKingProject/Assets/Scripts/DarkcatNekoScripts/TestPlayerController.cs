using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerController : MonoBehaviour
{
    [SerializeField] GameObject playerOBj_;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            playerOBj_.transform.position += Vector3.forward * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            playerOBj_.transform.position += Vector3.left * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            playerOBj_.transform.position += Vector3.back * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            playerOBj_.transform.position += Vector3.right * Time.deltaTime;
        }
    }
}
