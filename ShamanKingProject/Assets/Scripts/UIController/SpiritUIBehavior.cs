using UnityEngine;
using UnityEngine.UI;

public class SpiritUIBehavior : MonoBehaviour
{
    [SerializeField] Sprite emptySpirit_;
    [SerializeField] Sprite fullSpirit_;
    [SerializeField] Image[] spiritImages_;

    private void Start()
    {
        SpiritUIInit();
    }
    public void SpiritUIInit()
    {
        //GameManager.Instance.UIGameEvent.SetSubscribe(GameManager.Instance.UIGameEvent.OnSpiritUpdate, cmd => { onSpiritUpdate(); });
    }

    void onSpiritUpdate()
    {
        //ChangeSpiritImage(GameManager.Instance.MainGameMediator.RealTimePlayerData.GhostNowEatAmount);
    }
    public void ChangeSpiritImage(int nowSpiritCount)
    {
        for (int i = 0; i < spiritImages_.Length; i++) 
        {
            if (i<nowSpiritCount)
            {
                spiritImages_[i].sprite = fullSpirit_;
            }
            else
            {
                spiritImages_[i].sprite = emptySpirit_;
            }
        }
    }
}
