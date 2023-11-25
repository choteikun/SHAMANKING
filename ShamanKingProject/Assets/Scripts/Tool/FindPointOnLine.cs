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
    public float t;  // 比例因子，控制C點在線AB上的位置

    private Vector3 previousPosition;  // 保存上一幀的位置
    bool isAttacking_ = false;

    Vector3 startLocalPosition_;
    Quaternion startLocalRotation_;
    private void Start()
    {
        // 在Start函數中初始化上一幀的位置
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
        //// 比較當前位置和上一幀的位置
        //if (transform.position != previousPosition)
        //{
        //    // 如果位置不同，執行if語句
        //    // 計算線AB的方向向量
        //    Vector3 AB = pointB.position - previousPosition;

        //    // 正規化向量AB
        //    Vector3 AB_normalized = AB.normalized;

        //    // 計算點C的位置
        //    Vector3 pointC = pointB.position + AB_normalized * t * radius;

        //    // 將點C的位置設置為Unity中的物體位置，例如一個球體
        //    transform.position = pointC;
        //}
        //else
        //{
            if (pointA != null && pointB != null)
            {
                // 計算線AB的方向向量
                Vector3 AB = pointB.position - pointA.position;

                // 正規化向量AB
                Vector3 AB_normalized = AB.normalized;

                // 計算點C的位置
                Vector3 pointC = pointB.position + AB_normalized * t * radius;

                // 將點C的位置設置為Unity中的物體位置，例如一個球體
                transform.position = pointC;
            }
        //}
        var target = pointB;
        transform.LookAt(target);
        // 更新上一幀的位置
    }
    void attackFinish()
    {
        isAttacking_ = false;
        this.transform.localPosition = startLocalPosition_;
        this.transform.localRotation = startLocalRotation_;
    }
}
