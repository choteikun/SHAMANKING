using UnityEngine;
using UniRx;
using Language.Lua;
using System;
using Gamemanager;

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
    float positionSmoothSpeed_ = 0.5f;
    [SerializeField]
    float rotationSmoothSpeed_ = 0.5f;
    private Vector3 positionOffset_;
    private Quaternion rotationOffset_;

    void Start()
    {
        setTarget(target_);
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnAimingButtonTrigger, x => setTarget(aimingGhostPoint_));
        var onAimingEnterEvent = GameManager.Instance.MainGameEvent.OnAimingButtonTrigger.Where(cmd => cmd.AimingButtonIsPressed).Subscribe(cmd => setTarget(aimingGhostPoint_));
        var onCancelAimingEnterEvent = GameManager.Instance.MainGameEvent.OnAimingButtonTrigger.Where(cmd => !cmd.AimingButtonIsPressed).Subscribe(cmd => setTarget(target_));
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerLaunchGhost, cmd => { setTarget(launchFollowTarget_); });
        var onLaunchHitPosscessableItem = GameManager.Instance.MainGameEvent.OnPlayerLaunchActionFinish.Where(cmd => cmd.Hit && cmd.HitObjecctTag == HitObjecctTag.Possessable).Subscribe(cmd => setTarget(cmd.HitInfo.onHitPoint_.transform));
        GameManager.Instance.MainGameMediator.AddToDisposables(onAimingEnterEvent);
        GameManager.Instance.MainGameMediator.AddToDisposables(onLaunchHitPosscessableItem);
        GameManager.Instance.MainGameMediator.AddToDisposables(onCancelAimingEnterEvent);

    }

    void setTarget(Transform target)
    {
        targetObject_ = target;
    }

    void Update()
    {
        //Vector3 targetPosition = target_.position + positionOffset_;
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
