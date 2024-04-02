using Datamanager;
using Gamemanager;
using UnityEngine;

public class FireballProjectileBehavior : MonoBehaviour
{
    public float Speed = 15f;
    //private Rigidbody rb;
    void Start()
    {
        //rb = GetComponent<Rigidbody>();
        Destroy(gameObject, 5);

    }
    private void FixedUpdate()
    {
        //delayTimer_.c
        if (Speed != 0)
        {
            //rb.velocity = transform.forward * speed;
            transform.position += transform.forward * (Speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Object"))
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
            var explodeEffect = GameContainer.Get<DataManager>().GetDataByID<GameEffectTemplete>(10).PrefabPath;
            var explodeObject = Instantiate(explodeEffect, other.ClosestPoint(this.gameObject.transform.position), Quaternion.identity);
            GameManager.Instance.MainGameEvent.Send(new GameCallSoundEffectGenerate() {SoundEffectID = 202 });
        }
    }
}
