using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBeHitTest : MonoBehaviour
{
    [SerializeField]
    GameObject onHitParticle_;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerHitbox"))
        {
            onHitParticle_.transform.position = other.ClosestPoint(transform.position);
            onHitParticle_.GetComponent<ParticleSystem>().Play();
        }
    }
}
