using Datamanager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectileBehavior : MonoBehaviour
{
    public float speed = 15f;
    void Start()
    {
        Destroy(gameObject, 5);
    }

    // Update is called once per frame
    void Update()
    {
        if (speed != 0)
        {
            //rb.velocity = transform.forward * speed;
            transform.position += transform.forward * (speed * Time.deltaTime);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Object"))
        {
            //Ray ray = new Ray(transform.position, transform.forward);

            //RaycastHit hit;

            //// Check if the ray hits any collider
            //if (Physics.Raycast(ray, out hit))
            //{
            //    if (hit.transform.CompareTag("Player") || hit.transform.CompareTag("Object"))
            //    {
            //        var explodeEffect = GameContainer.Get<DataManager>().GetDataByID<GameEffectTemplete>(10).PrefabPath;
            //        var explodeObject = Instantiate(explodeEffect, hit.point, Quaternion.identity);
            //    }
            //}
            var explodeEffect = GameContainer.Get<DataManager>().GetDataByID<GameEffectTemplete>(14).PrefabPath;
            var explodeObject = Instantiate(explodeEffect, other.ClosestPoint(this.gameObject.transform.position), Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
