using Cysharp.Threading.Tasks;
using Gamemanager;
using UnityEngine;

public class PunishmentMachine : MonoBehaviour
{
    [SerializeField] GameObject outerHint_;
    [SerializeField] GameObject centerHint_;
    [SerializeField] GameObject outerHitBox_;
    [SerializeField] GameObject centerHitBox_;
    [SerializeField] GameObject innerJudgement_;
    [SerializeField] GameObject outerJudgement_;

    [SerializeField] int centerHintSec_;
    [SerializeField] int outerHintSec_;

    void Start()
    {
        punishmentAttack();
    }

    async void punishmentAttack()
    {
        centerHint_.SetActive(true);
        innerJudgement_.SetActive(true);
        await UniTask.Delay(centerHintSec_);
        centerHint_.SetActive(false);
        centerHitBox_.SetActive(true);
        GameManager.Instance.MainGameEvent.Send(new GameCallSoundEffectGenerate() { SoundEffectID = 213 });
        await UniTask.DelayFrame(3);
        Destroy(centerHitBox_);
        outerHint_.SetActive(true);
        outerJudgement_.SetActive(true);
        await UniTask.Delay(outerHintSec_);
        outerHint_.SetActive(false);
        outerHitBox_.SetActive(true);
        GameManager.Instance.MainGameEvent.Send(new GameCallSoundEffectGenerate() { SoundEffectID = 213 });
        await UniTask.DelayFrame(3);
        Destroy(outerHitBox_);
        GameManager.Instance.HellDogGameEvent.Send(new BossPunishmentAttackEndCommand());
        GameManager.Instance.HellDogGameEvent.Send(new BossCallUltCamTransfer() { trigger = false });
        await UniTask.DelayFrame(50);
        Destroy(gameObject);
    }
}
