using UnityEngine;
using UnityEngine.UI;

public class VolumeUI : MonoBehaviour
{
    [SerializeField] GameObject volumeUIObject_;
    [SerializeField] Image volumeBar_;

    private void Start()
    {
        GameManager.Instance.UIGameEvent.SetSubscribe(GameManager.Instance.UIGameEvent.OnPlayerSwitchControlUI, cmd => { volumeUIObject_.SetActive(cmd.Switch); });
        GameManager.Instance.UIGameEvent.SetSubscribe(GameManager.Instance.UIGameEvent.OnVolumeUIUpdate, cmd => { volumeUIUpdate(); });
    }

    void volumeUIUpdate()
    {
        var realTimePlayerData = GameManager.Instance.MainGameMediator.RealTimePlayerData;
        volumeBar_.fillAmount = realTimePlayerData.GameVolume * 0.1f;
    }
}
