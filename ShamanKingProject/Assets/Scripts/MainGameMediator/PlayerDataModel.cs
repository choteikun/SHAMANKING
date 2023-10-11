using Cysharp.Threading.Tasks;
using Gamemanager;
using UniRx;

public class PlayerDataModel
{
    public void PlayerDataModelInit()
    {
        GameManager.Instance.MainGameEvent.OnPlayerLaunchActionFinish.Where(cmd => cmd.HitObjecctTag == HitObjecctTag.Biteable).Subscribe(cmd => { SendBiteFinishResponse(cmd); });
        GameManager.Instance.MainGameEvent.OnPlayerAttack.Where(cmd => cmd.AttackName == "LightAttackThree").Subscribe(cmd => { PlayerStatCalculator.PlayerAddOrMinusSpirit(-1); });
    }

    public async void SendBiteFinishResponse(PlayerLaunchActionFinishCommand command)
    {
        await UniTask.Delay(1000);
        PlayerStatCalculator.PlayerAddOrMinusSpirit(1);
        GameManager.Instance.MainGameEvent.Send(new PlayerBiteFinishResponse() { HitInfo = command.HitInfo, HitObject = command.HitInfo.gameObject });
    }
}
