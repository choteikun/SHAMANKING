using DG.Tweening;
using Gamemanager;
using UnityEngine;
using UnityEngine.UI;

public class HellDogHealthBarController : MonoBehaviour
{
    [SerializeField] Image redHealthBar_;
    [SerializeField] Image healthBar_;
    [SerializeField] float nowPercentage_ = 1;
    [SerializeField] GameObject attackTarget_;
    Tweener redHealthBarTweener_;
    Tweener healthBarTweener_;
    void Start()
    {
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerAttackSuccessForData, cmd => { bossHealthChangeAnimation(cmd); });
    }


    void bossHealthChangeAnimation(PlayerAttackSuccessResponse response)
    {
        if (response.AttackTarget != attackTarget_) return;

        if (redHealthBarTweener_ != null)
        {
            redHealthBarTweener_.Kill();
        }
        if (healthBarTweener_ != null)
        {
            healthBarTweener_.Kill();
        }
        nowPercentage_ = response.EnemyHealthPercentage;
        redHealthBarTweener_ = DOTween.To(() => redHealthBar_.fillAmount, // 起始值的获取方法
              x => redHealthBar_.fillAmount = x, // 赋值方法
              nowPercentage_, // 目标值
              0.4f); // 动画持续时间
        healthBarTweener_ = DOTween.To(() => healthBar_.fillAmount, // 起始值的获取方法
              x => healthBar_.fillAmount = x, // 赋值方法
              nowPercentage_, // 目标值
              0.1f); // 动画持续时间
    }
}
