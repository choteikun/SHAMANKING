
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(AudioSource))]
public class FireTrackBall : MonoBehaviour
{
    [SerializeField, Tooltip("最大轉彎速度")]
    private float MaximumRotationSpeed = 120.0f;

    [SerializeField, Tooltip("加速度")]
    private float AcceleratedVeocity = 12.8f;

    [SerializeField, Tooltip("最高速度")]
    private float MaximumVelocity = 30.0f;

    [SerializeField, Tooltip("生命週期")]
    private float MaximumLifeTime = 8.0f;

    [SerializeField, Tooltip("上升期時間")]
    private float AccelerationPeriod = 0.5f;

    [SerializeField, Tooltip("爆炸特效預製體")]
    private GameObject[] ExplosionPrefabs = null;

    [SerializeField, Tooltip("飛彈渲染體元件")]
    private Renderer FireBallRenderer = null;

    [SerializeField, Tooltip("尾焰及煙霧粒子特效")]
    private ParticleSystem[] FireBallEffects = null;

    //[HideInInspector]
    public Transform Target = null; // 目標
    //[HideInInspector]
    public float CurrentVelocity = 0.0f; // 目前速度

    private AudioSource audioSource = null; // 音效組件
    private float lifeTime = 0.0f; // 生命期

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        if (!audioSource.isPlaying)
            audioSource.Play();
    }

    // 爆炸
    private void Explode()
    {
        // 之所以爆炸時不直接刪除物體，而是先停用一系列元件，
        // 是因為飛彈產生的煙霧等效果不應該立即消失

        // 禁止所有碰撞器
        foreach (Collider col in GetComponents<Collider>())
        {
            col.enabled = false;
        }
        // 禁止所有粒子系統
        foreach (ParticleSystem ps in FireBallEffects)
        {
            ps.Stop();
        }
        // 停止播放音效
        if (audioSource.isPlaying)
            audioSource.Stop();

        // 停止渲染，停止本腳本，隨機實例化爆炸特效，刪除本物體
        FireBallRenderer.enabled = false;
        enabled = false;
        Instantiate(ExplosionPrefabs[Random.Range(0, ExplosionPrefabs.Length)], transform.position, Random.rotation);

        // 三秒後刪除飛彈物體，這時候煙霧已經散去，可以刪掉物體了
        Destroy(gameObject, 3.0f);
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;
        lifeTime += deltaTime;

        // 如果超出生命週期，則直接爆炸。
        if (lifeTime > MaximumLifeTime)
        {
            Explode();
            return;
        }

        // 計算朝向目標的方向偏移量，如果處於上升期，則忽略目標
        Vector3 offset =
            ((lifeTime < AccelerationPeriod) && (Target != null))
            ? Vector3.up
            : (Target.position - transform.position).normalized;

        // 計算目前方向與目標方向的角度差
        float angle = Vector3.Angle(transform.forward, offset);

        // 根據最大旋轉速度，計算轉向目標共計所需的時間
        float needTime = angle / (MaximumRotationSpeed * (CurrentVelocity / MaximumVelocity));

        // 如果角度很小，就直接對準目標
        if (needTime < 0.001f)
        {
            transform.forward = offset;
        }
        else
        {
            // 目前影格間隔時間除以所需的時間，取得本次應旋轉的比例。
            transform.forward = Vector3.Slerp(transform.forward, offset, deltaTime / needTime).normalized;
        }

        // 如果目前速度小於最高速度，則進行加速
        if (CurrentVelocity < MaximumVelocity)
            CurrentVelocity += deltaTime * AcceleratedVeocity;

        // 朝自己的前方位移
        transform.position += transform.forward * CurrentVelocity * deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        // 當發生碰撞，爆炸
        Explode();
    }
}
