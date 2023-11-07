using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostSizeController : MonoBehaviour
{
    [SerializeField] GameObject ghostPrefab_;
    
    void Start()
    {
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.UIGameEvent.OnSpiritUpdate, cmd => { updateGhostSize(); });
    }

    void updateGhostSize()
    {
        ghostPrefab_.transform.localScale = new Vector3(1, 1, 1) * (1 + 0.25f * GameManager.Instance.MainGameMediator.RealTimePlayerData.GhostNowEatAmount);
    }
}
