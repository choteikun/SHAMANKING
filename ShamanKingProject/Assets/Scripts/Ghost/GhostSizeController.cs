using UnityEngine;
using UnityEngine.VFX;

public class GhostSizeController : MonoBehaviour
{
    [SerializeField] GameObject ghostPrefab_;
    [SerializeField] VisualEffect fire_;

    void Start()
    {
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.UIGameEvent.OnSoulGageUpdate, cmd => { updateGhostSize(); });
        updateGhostSize();
    }

    void updateGhostSize()
    {
        ghostPrefab_.transform.localScale = new Vector3(2, 2, 2) * (1 + 0.5f * GameManager.Instance.MainGameMediator.RealTimePlayerData.GhostNowGageBlockAmount);
        var amount = GameManager.Instance.MainGameMediator.RealTimePlayerData.GhostSoulGageCurrentAmount / GameManager.Instance.MainGameMediator.RealTimePlayerData.GhostSoulGageMaxAmount;
        if (fire_ != null)
        {
            fire_.SetFloat("Level", amount * 10);
        }
    }
}
