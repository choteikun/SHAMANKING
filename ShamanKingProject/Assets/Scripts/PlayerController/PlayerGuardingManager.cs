using UnityEngine;
using Gamemanager;

public class PlayerGuardingManager
{
   
   public void Update()
    {
        if (GameManager.Instance.MainGameMediator.RealTimePlayerData.PlayerGuarding)
        {
            GameManager.Instance.MainGameMediator.RealTimePlayerData.PlayerGuardingResetTimer = 0;
        }
        else
        {
            GameManager.Instance.MainGameMediator.RealTimePlayerData.PlayerGuardingResetTimer++;
            if (GameManager.Instance.MainGameMediator.RealTimePlayerData.PlayerGuardingResetTimer>=100)
            {
                GameManager.Instance.MainGameMediator.RealTimePlayerData.PlayerGuardPoint = Mathf.Clamp(GameManager.Instance.MainGameMediator.RealTimePlayerData.PlayerGuardPoint + Time.deltaTime * 5f,0,GameManager.Instance.MainGameMediator.RealTimePlayerData.PlayerMaxGuardPoint);
                var percentage = GameManager.Instance.MainGameMediator.RealTimePlayerData.PlayerGuardPoint / GameManager.Instance.MainGameMediator.RealTimePlayerData.PlayerMaxGuardPoint;
                GameManager.Instance.UIGameEvent.Send(new SystemCallDefenceUIUpdateCommand() {Percentage = percentage });
            }
        }
    }
}
