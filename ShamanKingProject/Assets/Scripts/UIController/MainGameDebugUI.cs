using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainGameDebugUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI soulGageText_;
    [SerializeField] TextMeshProUGUI invincibleText_;

    private void Start()
    {
        GameManager.Instance.UIGameEvent.SetSubscribe(GameManager.Instance.UIGameEvent.OnSoulGageUpdate, cmd => { updateSoulGage(); });
        GameManager.Instance.UIGameEvent.SetSubscribe(GameManager.Instance.UIGameEvent.OnPlayerInvincibleUpdate, cmd => { updateInvincible (); });
    }

    void updateSoulGage()
    {
        soulGageText_.text = "SoulGage: " + GameManager.Instance.MainGameMediator.RealTimePlayerData.GhostSoulGageCurrentAmount;
    }

    void updateInvincible()
    {
        invincibleText_.text = "Invincible:"+ GameManager.Instance.MainGameMediator.RealTimePlayerData.PlayerInvincible.ToString();
    }
}
