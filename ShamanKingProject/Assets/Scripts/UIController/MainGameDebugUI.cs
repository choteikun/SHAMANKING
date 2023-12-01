using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainGameDebugUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI soulGageText_;

    private void Start()
    {
        GameManager.Instance.UIGameEvent.SetSubscribe(GameManager.Instance.UIGameEvent.OnSoulGageUpdate, cmd => { updateSoulGage(); });
    }

    void updateSoulGage()
    {
        soulGageText_.text = "SoulGage: " + GameManager.Instance.MainGameMediator.RealTimePlayerData.GhostSoulGageCurrentAmount;
    }
}
