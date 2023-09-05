using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostFollower : MonoBehaviour
{
    [SerializeField]
    Transform target_; // 物體B
    [SerializeField]
    float positionSmoothSpeed_ = 0.5f;
    [SerializeField]
    float rotationSmoothSpeed_ = 0.5f;
    private Vector3 positionOffset_;
    private Quaternion rotationOffset_;

    void Start()
    {
        positionOffset_ = transform.position - target_.position;
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
