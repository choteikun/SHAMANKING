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
    float positionSmoothSpeed_ = 0.5f;
    [SerializeField]
    float rotationSmoothSpeed_ = 0.5f;
    private Vector3 positionOffset_;
    private Quaternion rotationOffset_;

    void Start()
    {
        setOffSet(target_);
        //GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnAimingButtonTrigger,x=> setOffSet(aimingGhostPoint_));
        //var onAimingEnterEvent = GameManager.Instance.MainGameEvent.OnAimingButtonTrigger.Where(cmd => cmd.AimingButtonIsPressed).Subscribe(cmd => setOffSet(aimingGhostPoint_));
        //GameManager.Instance.MainGameMediator.AddToDisposables(onAimingEnterEvent);


    }

    void setOffSet(Transform target)
    {
        positionOffset_ = transform.position - target.position;
        rotationOffset_ = Quaternion.identity; // 初始時沒有旋轉偏移
    }

    void Update()
    {
        Vector3 targetPosition = target_.position + positionOffset_;
        transform.position = Vector3.Lerp(transform.position, targetPosition, positionSmoothSpeed_ * Time.deltaTime);

        Quaternion targetRotation = target_.rotation * rotationOffset_;
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSmoothSpeed_ * Time.deltaTime);
    }
}
