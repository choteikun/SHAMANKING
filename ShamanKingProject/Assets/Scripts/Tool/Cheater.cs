using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheater : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            PlayerStatCalculator.PlayerAddOrMinusSpirit(200);
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            PlayerStatCalculator.PlayerAddOrMinusSpirit(400);
        }
    }
}
