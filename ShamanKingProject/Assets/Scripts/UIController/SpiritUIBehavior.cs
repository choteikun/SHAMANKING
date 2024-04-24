using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SpiritUIBehavior : MonoBehaviour
{
    [SerializeField] Image[] spiritImages_;

    private void Start()
    {
        GameManager.Instance.UIGameEvent.SetSubscribe(GameManager.Instance.UIGameEvent.OnSoulGageUpdate, cmd => { onSpiritUpdate(GameManager.Instance.MainGameMediator.RealTimePlayerData.GhostSoulGageCurrentAmount); });
        onSpiritUpdate(GameManager.Instance.MainGameMediator.RealTimePlayerData.GhostSoulGageCurrentAmount);
    }
    
    void onSpiritUpdate(float gageAmount)
    {
        if (gageAmount>300)
        {
            var amount4 = (gageAmount-300)/100f;
            spiritImages_[3].DOFillAmount(amount4, 0.25f);
            spiritImages_[2].DOFillAmount(1, 0.25f);
            spiritImages_[1].DOFillAmount(1, 0.25f);
            spiritImages_[0].DOFillAmount(1, 0.25f);
        }
        else
        {
            spiritImages_[3].DOFillAmount(0, 0.25f);
            if (gageAmount > 200)
            {
                var amount3 = (gageAmount - 200) / 100f;
                spiritImages_[2].DOFillAmount(amount3, 0.25f);
                spiritImages_[1].DOFillAmount(1, 0.25f);
                spiritImages_[0].DOFillAmount(1, 0.25f);
            }
            else
            {
                spiritImages_[2].DOFillAmount(0, 0.25f);
                if (gageAmount>100)
                {
                    var amount2 = (gageAmount - 100) / 100f;
                    spiritImages_[1].DOFillAmount(amount2, 0.25f);
                    spiritImages_[0].DOFillAmount(1, 0.25f);
                }
                else
                {
                    spiritImages_[1].DOFillAmount(0, 0.25f);
                    var amount = gageAmount / 100f;
                    spiritImages_[0].DOFillAmount(amount, 0.25f);
                }
            }
        }
        
        
       
    }
   
}
