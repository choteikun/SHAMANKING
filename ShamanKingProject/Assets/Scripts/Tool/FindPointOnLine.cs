using Language.Lua;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Gamemanager;
using Cysharp.Threading.Tasks.Triggers;

public class FindPointOnLine : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float radius;
    public float t;  // ��Ҧ]�l�A����C�I�b�uAB�W����m

    private Vector3 previousPosition;  // �O�s�W�@�V����m
    bool isAttacking_ = false;

    Vector3 startLocalPosition_;
    Quaternion startLocalRotation_;
    private void Start()
    {
        // �bStart��Ƥ���l�ƤW�@�V����m
        previousPosition = transform.position;
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnSystemAttackAllow, cmd => { isAttacking_ = true; });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerMovementInterruptionFinish, cmd => { attackFinish(); });
        startLocalPosition_ = transform.localPosition;
        startLocalRotation_ = transform.localRotation;
    }


    private void Update()
    {        
        if (isAttacking_)
        {
            attackTailFollower();
        }
        previousPosition = transform.position;
        
    }
    void attackTailFollower()
    {
        //// �����e��m�M�W�@�V����m
        //if (transform.position != previousPosition)
        //{
        //    // �p�G��m���P�A����if�y�y
        //    // �p��uAB����V�V�q
        //    Vector3 AB = pointB.position - previousPosition;

        //    // ���W�ƦV�qAB
        //    Vector3 AB_normalized = AB.normalized;

        //    // �p���IC����m
        //    Vector3 pointC = pointB.position + AB_normalized * t * radius;

        //    // �N�IC����m�]�m��Unity���������m�A�Ҧp�@�Ӳy��
        //    transform.position = pointC;
        //}
        //else
        //{
            if (pointA != null && pointB != null)
            {
                // �p��uAB����V�V�q
                Vector3 AB = pointB.position - pointA.position;

                // ���W�ƦV�qAB
                Vector3 AB_normalized = AB.normalized;

                // �p���IC����m
                Vector3 pointC = pointB.position + AB_normalized * t * radius;

                // �N�IC����m�]�m��Unity���������m�A�Ҧp�@�Ӳy��
                transform.position = pointC;
            }
        //}
        var target = pointB;
        transform.LookAt(target);
        // ��s�W�@�V����m
    }
    void attackFinish()
    {
        isAttacking_ = false;
        this.transform.localPosition = startLocalPosition_;
        this.transform.localRotation = startLocalRotation_;
    }
}
