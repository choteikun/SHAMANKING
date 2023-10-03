using UnityEngine;
using UnityEngine.UI;

public class SpiritUIBehavior : MonoBehaviour
{
    [SerializeField] Sprite emptySpirit_;
    [SerializeField] Sprite fullSpirit_;
    [SerializeField] Image spiritImage_;
    public void ChangeSpiritImage(bool ifFull)
    {
        if (ifFull)
        {
            spiritImage_.sprite = fullSpirit_;
        }
        else
        {
            spiritImage_.sprite = emptySpirit_;
        }
    }
}
