using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAimer : MonoBehaviour
{
    [SerializeField]
    GameObject mainCam_;

    [SerializeField]
    GameObject aimCam_;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            mainCam_.SetActive(false);
            aimCam_.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            mainCam_.SetActive(true);
            aimCam_.SetActive(false);
        }
    }
}
