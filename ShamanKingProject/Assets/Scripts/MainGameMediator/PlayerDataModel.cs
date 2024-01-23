using Cysharp.Threading.Tasks;
using Gamemanager;
using UniRx;
using static Unity.Burst.Intrinsics.X86.Avx;

public class PlayerDataModel
{
    public void PlayerDataModelInit()
    {
        GameManager.Instance.MainGameEvent.OnPlayerLaunchActionFinish.Where(cmd => cmd.HitObjecctTag == HitObjecctTag.Biteable).Subscribe(cmd => { SendBiteFinishResponse(cmd); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerSuccessParry, cmd => { parryAddSoul(); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerAttackSuccessForData, cmd => { PlayerStatCalculator.PlayerAddOrMinusSpirit(cmd.AttackAddSoul); });
    }

    public async void SendBiteFinishResponse(PlayerLaunchActionFinishCommand command)
    {
        await UniTask.Delay(1000);
        PlayerStatCalculator.PlayerAddOrMinusSpirit(100);
        GameManager.Instance.MainGameEvent.Send(new PlayerBiteFinishResponse() { HitInfo = command.HitInfo, HitObject = command.HitInfo.gameObject });
    }

    void parryAddSoul()
    {
        PlayerStatCalculator.PlayerAddOrMinusSpirit(100);
    }
}
