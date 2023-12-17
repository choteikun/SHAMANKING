using Datamanager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectileBehavior : MonoBehaviour
{
    public float speed = 15f;
    [SerializeField] int hit_;
    void Start()
    {
        Destroy(gameObject, 2);
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
            var explodeEffect = GameContainer.Get<DataManager>().GetDataByID<GameEffectTemplete>(hit_).PrefabPath;
            var explodeObject = Instantiate(explodeEffect, other.ClosestPoint(this.gameObject.transform.position), Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
