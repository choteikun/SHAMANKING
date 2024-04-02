using UnityEngine;
using Gamemanager;
public class Shadowball : MonoBehaviour
{
    public float Speed;
    [SerializeField, Tooltip("爆炸特效預製體")]
    private GameObject[] explosionPrefabs_ = null;
    [SerializeField, Tooltip("影子球渲染體元件")]
    private Renderer shadowBallRenderer_ = null;

    Transform playerTransform_;
    private void Start()
    {
        playerTransform_ = GameObject.FindGameObjectWithTag("Player").transform;
        transform.LookAt(playerTransform_.position);
    }
    private void FixedUpdate()
    {
        if (Speed != 0)
        {
            transform.position += transform.forward * (Speed * Time.deltaTime);
        }
    }
    // 爆炸
    private void Explode()
    {
        // 禁止所有碰撞器
        foreach (Collider col in GetComponents<Collider>())
        {
            col.enabled = false;
        }

        // 停止渲染，停止本腳本，隨機實例化爆炸特效，刪除本物體
        shadowBallRenderer_.enabled = false;
        enabled = false;
        GameManager.Instance.MainGameEvent.Send(new GameCallSoundEffectGenerate() { SoundEffectID = 104 });
        Instantiate(explosionPrefabs_[Random.Range(0, explosionPrefabs_.Length)], transform.position + new Vector3(0, 0.75f, 0), Random.rotation);

        // 三秒後刪除影子球物體，這時候煙霧已經散去，可以刪掉物體了
        Destroy(gameObject, 3.0f);
    }
    private void OnTriggerEnter(Collider collider)
    {
        // 當發生碰撞，爆炸
        if (collider.CompareTag("Player") || collider.CompareTag("Wall") || collider.CompareTag("Object"))
        {
            Debug.LogError("FireBall Hit!!");
            Explode();
        }
    }
}
