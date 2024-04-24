using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObstacle : MonoBehaviour
{
    public GameObject firstBossCollider;

    // Update is called once per frame
    void Update()
    {
        transform.position = firstBossCollider.transform.position;
    }
}
