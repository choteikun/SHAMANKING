using Cysharp.Threading.Tasks;
using Gamemanager;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDataModel
{
    public void PlayerDataModelInit()
    {
        GameManager.Instance.MainGameEvent.OnPlayerLaunchActionFinish.Where(cmd => cmd.HitObjecctTag == HitObjecctTag.Biteable).Subscribe(cmd => { SendBiteFinishResponse(cmd); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerSuccessParry, cmd => { parryAddSoul(); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerAttackSuccessForData, async cmd =>
        {
            PlayerStatCalculator.PlayerAddOrMinusSpirit(cmd.AttackAddSoul);
            if (Gamepad.current == null) return;
            // 触发手柄震动
            Gamepad.current.SetMotorSpeeds(0.05f,0.05f);

            await UniTask.Delay(100);

            // 停止震动
            Gamepad.current.SetMotorSpeeds(0,0);
        });
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
