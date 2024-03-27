using UnityEngine;

public class GhostSizeController : MonoBehaviour
{
    [SerializeField] GameObject ghostPrefab_;

    void Start()
    {
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.UIGameEvent.OnSoulGageUpdate, cmd => { updateGhostSize(); });
        updateGhostSize();
    }

    void updateGhostSize()
    {
        ghostPrefab_.transform.localScale = new Vector3(2,2,2) * (1+ 0.5f * GameManager.Instance.MainGameMediator.RealTimePlayerData.GhostNowGageBlockAmount);
    }
}
