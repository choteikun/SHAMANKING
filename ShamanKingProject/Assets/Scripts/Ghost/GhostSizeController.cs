using UnityEngine;

public class GhostSizeController : MonoBehaviour
{
    [SerializeField] GameObject ghostPrefab_;

    void Start()
    {
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.UIGameEvent.OnSoulGageUpdate, cmd => { updateGhostSize(); });
    }

    void updateGhostSize()
    {
        ghostPrefab_.transform.localScale = new Vector3(1, 1, 1) * (1 + 0.25f * GameManager.Instance.MainGameMediator.RealTimePlayerData.GhostNowGageBlockAmount);
    }
}
