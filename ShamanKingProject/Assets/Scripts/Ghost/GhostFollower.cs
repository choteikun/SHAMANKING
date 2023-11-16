using Gamemanager;
using UniRx;
using UnityEngine;

public class GhostFollower : MonoBehaviour
{
    [SerializeField]
    Transform target_; // 物體B
    [SerializeField]
    Transform aimingGhostPoint_;

    [SerializeField]
    Transform targetObject_;//跟隨中的物體

    [SerializeField]
    Transform launchFollowTarget_;

    [SerializeField]
    Transform attackFollowTarget_;

    [SerializeField]
    Transform throwAttackStartFollowTarget_;

    [SerializeField]
    Transform throwAttackFollowTarget_;
    [SerializeField]
    Transform worldUseFollowTarget_;

    [SerializeField]
    float positionSmoothSpeed_ = 0.5f;
    [SerializeField]
    float rotationSmoothSpeed_ = 0.5f;
    private Vector3 positionOffset_;
    private Quaternion rotationOffset_;

    private Vector3 velocity = Vector3.zero;
    [SerializeField] Rigidbody rb_;

    void Start()
    {
        setTarget(target_);
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnAimingButtonTrigger, x => { if (x.AimingButtonIsPressed) setTarget(aimingGhostPoint_); });
        var onAimingEnterEvent = GameManager.Instance.MainGameEvent.OnAimingButtonTrigger.Where(cmd => cmd.AimingButtonIsPressed).Subscribe(cmd => setTarget(aimingGhostPoint_));
        var onCancelAimingEnterEvent = GameManager.Instance.MainGameEvent.OnAimingButtonTrigger.Where(cmd => !cmd.AimingButtonIsPressed).Subscribe(cmd => setParent(target_));
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnStartRollMovementAnimation, cmd => { setParent(target_); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerLaunchGhost, cmd => { setTarget(launchFollowTarget_); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerGrabSuccessForPlayer, cmd => { worldUseFollowTarget_.transform.position = cmd.CollidePoint;setTarget(worldUseFollowTarget_);  });
        var onLaunchHitPosscessableItem = GameManager.Instance.MainGameEvent.OnPlayerLaunchActionFinish.Where(cmd => cmd.Hit && (cmd.HitObjecctTag == HitObjecctTag.Possessable || cmd.HitObjecctTag == HitObjecctTag.Biteable)).Subscribe(cmd => setTarget(cmd.HitInfo.onHitPoint_.transform));
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerAnimationEvents, cmd =>
        {
            if (cmd.AnimationEventName == "Player_Attack_Allow")
            {
                Debug.Log("AttackFollow");
                setTarget(attackFollowTarget_);
                positionSmoothSpeed_ = 75f;
            }
        });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerMovementInterruptionFinish, cmd => { Debug.Log("OnPlayerMovementInterruptionFinish"); setTarget(target_); positionSmoothSpeed_ = 15f; });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerAnimationEvents, cmd =>
        {
            if (cmd.AnimationEventName == "PlayerThrowAttackReady")
            {
                setTarget(throwAttackFollowTarget_);
            }
        });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerAnimationEvents, cmd =>
        {
            if (cmd.AnimationEventName == "PlayerThrowAnimationStart")
            {
                setTarget(throwAttackStartFollowTarget_);
            }
        });

        GameManager.Instance.MainGameMediator.AddToDisposables(onLaunchHitPosscessableItem);
        GameManager.Instance.MainGameMediator.AddToDisposables(onAimingEnterEvent);
        GameManager.Instance.MainGameMediator.AddToDisposables(onCancelAimingEnterEvent);

    }
    void setParent(Transform target)
    {
        //transform.SetParent(target);
        targetObject_ = target;
    }
    void setTarget(Transform target)
    {
        //transform.SetParent(null);
        targetObject_ = target;
    }
    //private void LateUpdate()
    //{
    //    //Vector3 targetPosition = target_.position + positionOffset_;
    //    //float smoothStep = Mathf.SmoothStep(0f, 1f, positionSmoothSpeed_ * Time.deltaTime);
    //    transform.position = Vector3.SmoothDamp(transform.position, targetObject_.position, ref velocity, positionSmoothSpeed_);

    //    //Quaternion targetRotation = target_.rotation * rotationOffset_;
    //    //transform.rotation = Quaternion.Lerp(transform.rotation, targetObject_.rotation, rotationSmoothSpeed_ * Time.deltaTime);
    //    rotationUpdate();

    //}
    void LateUpdate()
    {
        //Vector3 targetPosition = target_.position + positionOffset_;
        //float smoothStep = Mathf.SmoothStep(0f, 1f, positionSmoothSpeed_ * Time.deltaTime);

        transform.position = Vector3.Lerp(transform.position, targetObject_.position, positionSmoothSpeed_ * Time.deltaTime);

        //Quaternion targetRotation = target_.rotation * rotationOffset_;
        //transform.rotation = Quaternion.Lerp(transform.rotation, targetObject_.rotation, rotationSmoothSpeed_ * Time.deltaTime);
        rotationUpdate();
    }



    void rotationUpdate()
    {
        // 获取当前对象的旋转
        Quaternion currentRotation = transform.rotation;

        // 获取目标对象的局部旋转
        Quaternion targetLocalRotation = targetObject_.rotation;

        // 锁定局部Z轴旋转
        targetLocalRotation = Quaternion.Euler(targetLocalRotation.eulerAngles.x, targetLocalRotation.eulerAngles.y, 0f);

        // 使用Lerp插值来平滑过渡
        Quaternion newRotation = Quaternion.Lerp(currentRotation, targetLocalRotation, rotationSmoothSpeed_ * Time.deltaTime);

        // 将新的旋转应用到对象上
        transform.rotation = newRotation;
    }
}
