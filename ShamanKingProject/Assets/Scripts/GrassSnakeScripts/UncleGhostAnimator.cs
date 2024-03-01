using UnityEngine;

public class UncleGhostAnimator : MonoBehaviour
{
    #region 提前Hash進行優化
    readonly int animID_TimeOutToIdle = Animator.StringToHash("TimeOutToIdle");
    readonly int animID_IsUncleBusy = Animator.StringToHash("IsUncleBusy");
    #endregion

    Animator animator_;
    //Idle動畫計時器(跳轉至隨機動畫)
    float idleTimer_;
    [SerializeField, Tooltip("過渡到隨機Idle動畫所需要花的時間")]
    float idleTimeOut_ = 5;

    void Start()
    {
        animator_ = GetComponent<Animator>();
    }


    void Update()
    {

    }

    #region - 大叔鬼魂獲取指令 -

    #endregion

    #region - 大叔鬼魂待機動畫處理 -
    void timeoutToIdle()
    {
        bool IsUncleBusy = true;
        //如果大叔忙完了就待機
        if (!IsUncleBusy)
        {
            //開始計時
            idleTimer_ += Time.deltaTime;

            //計時超過五秒後
            if (idleTimer_ >= idleTimeOut_)
            {
                //計時器為-1
                idleTimer_ = -1f;
                animator_.SetTrigger(animID_TimeOutToIdle);
                //GameManager.Instance.MainGameEvent.Send(new PlayerMoveStatusChangeCommand() { IsMoving = false });
            }
        }
        else
        {
            idleTimer_ = 0f;
            animator_.ResetTrigger(animID_TimeOutToIdle);
        }
        animator_.SetBool(animID_IsUncleBusy, IsUncleBusy);
    }
    #endregion
}
