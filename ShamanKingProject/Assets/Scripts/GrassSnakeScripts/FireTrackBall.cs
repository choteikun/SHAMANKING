using Cysharp.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(AudioSource), typeof(Rigidbody))]
public class FireTrackBall : MonoBehaviour
{
    [SerializeField, Tooltip("最大轉彎速度")]
    private float maximumRotationSpeed_ = 100.0f;

    [SerializeField, Tooltip("加速度")]
    private float acceleratedVeocity_ = 5.0f;

    [SerializeField, Tooltip("最高速度")]
    private float maximumVelocity_ = 25.0f;

    [SerializeField, Tooltip("生命週期")]
    private float maximumLifeTime_ = 15.0f;

    [SerializeField, Tooltip("上升期時間")]
    private float accelerationPeriod_ = 2.3f;

    [SerializeField, Tooltip("繞著目標的旋轉速度")]
    private float rotateAroundSpeed_ = 2f;

    [SerializeField, Tooltip("BossObj")]
    private GameObject firstBossObj_;

    [SerializeField, Tooltip("爆炸特效預製體")]
    private GameObject[] explosionPrefabs_ = null;

    [SerializeField, Tooltip("飛彈渲染體元件")]
    private Renderer fireBallRenderer_ = null;

    [SerializeField, Tooltip("尾焰及煙霧粒子特效")]
    private ParticleSystem[] fireBallEffects_ = null;

    //[HideInInspector]
    public Transform Target = null; // 目標
    //[HideInInspector]
    public float CurrentVelocity = 0.0f; // 目前速度

    private AudioSource audioSource_ = null; // 音效組件
    private float lifeTime_ = 0.0f; // 生命期

    private bool rotateAroundTrigger_;

    Vector3 rotateAroundVec_;

    private void Start()
    {
        Target = GameManager.Instance.MainGameMediator.RealTimePlayerData.PlayerGameObject.transform;
        audioSource_ = GetComponent<AudioSource>();
        audioSource_.loop = true;
        rotateAroundTrigger_ = false;
        if (!firstBossObj_)
        {
            firstBossObj_ = GameObject.Find("FirstBoss");
        }

        rotateAroundVec_ = firstBossObj_.transform.position;

        if (!audioSource_.isPlaying)
            audioSource_.Play();
        
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
        //foreach (ParticleSystem ps in FireBallEffects)
        //{
        //    ps.Stop();
        //}
        // 停止播放音效
        if (audioSource_.isPlaying)
            audioSource_.Stop();

        // 停止渲染，停止本腳本，隨機實例化爆炸特效，刪除本物體
        fireBallRenderer_.enabled = false;
        enabled = false;
        Instantiate(explosionPrefabs_[Random.Range(0, explosionPrefabs_.Length)], transform.position, Random.rotation);

        // 三秒後刪除飛彈物體，這時候煙霧已經散去，可以刪掉物體了
        Destroy(gameObject, 3.0f);
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;
        lifeTime_ += deltaTime;

        // 如果超出生命週期，則直接爆炸。
        if (lifeTime_ > maximumLifeTime_)
        {
            Explode();
            return;
        }

        // 計算朝向目標的方向偏移量，如果處於上升期，則忽略目標
        Vector3 offset =
            ((lifeTime_ < accelerationPeriod_) && (Target != null))
            ? Vector3.up
            : (Target.position - transform.position).normalized;

        // 計算目前方向與目標方向的角度差
        float angle = Vector3.Angle(transform.forward, offset);

        // 根據最大旋轉速度，計算轉向目標共計所需的時間
        float needTime = angle / (maximumRotationSpeed_ * (CurrentVelocity / maximumVelocity_));

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
        if (CurrentVelocity < maximumVelocity_)
            CurrentVelocity += deltaTime * acceleratedVeocity_;


        // 朝自己的前方位移
        transform.position += transform.forward * CurrentVelocity * deltaTime;
        //繞著Boss中心旋轉的觸發器
        if (!rotateAroundTrigger_)
        {
            RotateAroundTarget();
        }
    }

    //繞著Boss中心旋轉
    async void RotateAroundTarget()
    {
        transform.RotateAround(rotateAroundVec_, Vector3.up, 360 * rotateAroundSpeed_ * Time.deltaTime);
        //遞減旋轉速度
        rotateAroundSpeed_ -= (rotateAroundSpeed_ / accelerationPeriod_) * Time.deltaTime;
        if (rotateAroundSpeed_ <= 0)
        {
            rotateAroundSpeed_ = 0;
        }
        await UniTask.Delay((int)accelerationPeriod_ * 1000);
        //關閉觸發器
        rotateAroundTrigger_ = true;
    }
    private void OnTriggerEnter(Collider collider)
    {
        // 當發生碰撞，爆炸
        if (collider.CompareTag("Player"))
        {
            Debug.LogError("FireBall Hit!!");
            Explode();
        }
    }
}
